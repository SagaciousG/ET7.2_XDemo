using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using MongoDB.Bson.Serialization;
using OfficeOpenXml;
using ProtoBuf;
using LicenseContext = OfficeOpenXml.LicenseContext;

namespace ET
{
    public enum ConfigType
    {
        s = 1,
        cs = 2,
    }

    class HeadInfo
    {
        // public string FieldCS;
        public string FieldDesc;
        public string FieldName;
        public string FieldType;
        public int FieldIndex;

        public HeadInfo(string cs, string desc, string name, string type, int index)
        {
            // this.FieldCS = cs;
            this.FieldDesc = desc;
            this.FieldName = name;
            this.FieldType = type;
            this.FieldIndex = index;
        }
    }

    // 这里加个标签是为了防止编译时裁剪掉protobuf，因为整个tool工程没有用到protobuf，编译会去掉引用，然后动态编译就会出错
    [ProtoContract]
    class Table
    {
        public bool IsMainSheet;
        public bool ServerOnly;
        public int Index;
        public Dictionary<string, HeadInfo> HeadInfos = new Dictionary<string, HeadInfo>();
    }

    class LogInfo
    {
        public string ExcelName;
        public long LastBuild;
        public bool Exist;
        public List<string> Sheets = new();
    }
    
    public static class ExcelExporter
    {
        private static string template;

        private const string ServerClassDir = "../Unity/Assets/Scripts/Codes/Model/Generate/Server/Config";

        private const string CSClassDir = "../Unity/Assets/Scripts/Codes/Model/Generate/ClientServer/Config";

        private const string excelDir = "../公共/Excel/";
        private const string buildLog = "../公共/Excel/buildLog.txt";

        private const string jsonDir = "../Unity/Assets/Config/Json";

        private const string clientProtoDir = "../Unity/Assets/Bundles/Config";
        private const string serverProtoDir = "../Config/Excel/{0}/{1}";
        private static Assembly[] configAssemblies = new Assembly[3];

        private static Dictionary<string, Table> _tables = new Dictionary<string, Table>();
        private static Dictionary<string, ExcelPackage> _packages = new Dictionary<string, ExcelPackage>();
        private static Dictionary<string, LogInfo> _buildLogs = new();
        private static HashSet<string> _allSheet = new();

        private static Table GetTable(string protoName)
        {
            if (!_tables.TryGetValue(protoName, out var table))
            {
                table = new Table();
                _tables[protoName] = table;
            }

            return table;
        }

        public static ExcelPackage GetPackage(string filePath)
        {
            if (!_packages.TryGetValue(filePath, out var package))
            {
                using Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                package = new ExcelPackage(stream);
                _packages[filePath] = package;
            }

            return package;
        }

        public static void Export()
        {
            try
            {
                if (File.Exists(buildLog))
                {
                    var allLines = File.ReadAllLines(buildLog);
                    var curLog = new LogInfo();
                    foreach (string line in allLines)
                    {
                        if (line.StartsWith("--"))
                        {
                            var sheetName = line.Replace("--", "").Trim();
                            curLog.Sheets.Add(sheetName);
                        }
                        else
                        {
                            curLog = new();
                            var ss = line.Split(' ');
                            curLog.ExcelName = ss[0].Trim();
                            curLog.LastBuild = System.Convert.ToInt64(ss[1]);
                            _buildLogs.Add(ss[0], curLog);
                        }
                    }
                }
                
                
                //防止编译时裁剪掉protobuf
                ProtoBuf.WireType.Fixed64.ToString();
                
                template = File.ReadAllText("Template.txt");
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                
                var changeFiles = new List<string>();
                List<string> files = FileHelper.GetAllFiles(excelDir);
                foreach (string path in files)
                {
                    string fileName = Path.GetFileName(path);
                    if (!fileName.EndsWith(".xlsx") || fileName.StartsWith("~$") || fileName.Contains("#"))
                    {
                        continue;
                    }

                    ExcelPackage p = GetPackage(Path.GetFullPath(path));
                    foreach (var sWorksheet in p.Workbook.Worksheets)
                    {
                        var sheetName = sWorksheet.Name.Substring(sWorksheet.Name.IndexOf('@') + 1);
                        _allSheet.Add(sheetName);
                    }
                    var fileInfo = new FileInfo(path);
                    if (_buildLogs.TryGetValue(Path.GetFileNameWithoutExtension(fileInfo.Name), out var log))
                    {
                        if (log.LastBuild >= fileInfo.LastWriteTime.Ticks)
                        {
                            continue;
                        }
                        log.Sheets.Clear();
                    }
                    else
                    {
                        log = new LogInfo()
                        {
                            ExcelName = Path.GetFileNameWithoutExtension(fileInfo.Name),
                            LastBuild = fileInfo.LastWriteTime.Ticks,
                            Exist = true,
                            Sheets = new()
                        };
                        _buildLogs.Add(log.ExcelName, log);
                    }
                    foreach (var sWorksheet in p.Workbook.Worksheets)
                    {
                        var sheetName = sWorksheet.Name.Substring(sWorksheet.Name.IndexOf('@') + 1);
                        log.Sheets.Add(sheetName);
                    }
                    changeFiles.Add(path);


                    ExportExcelClass(p);
                }

                foreach (var kv in _tables)
                {
                    if (kv.Value.IsMainSheet)
                        ExportClass(kv.Key, kv.Value.HeadInfos, kv.Value.ServerOnly? ConfigType.s : ConfigType.cs);
                }

                // 动态编译生成的配置代码
                configAssemblies[(int) ConfigType.s] = DynamicBuild(ConfigType.s);
                configAssemblies[(int) ConfigType.cs] = DynamicBuild(ConfigType.cs);

                foreach (string path in changeFiles)
                {
                    ExportExcel(path);
                }
                
                if (Directory.Exists(clientProtoDir))
                {
                    Directory.Delete(clientProtoDir, true);
                }

                var allPublicCS = FileHelper.GetAllFiles(CSClassDir, "*.cs");
                foreach (string s in allPublicCS)
                {
                    if (!_allSheet.Contains(Path.GetFileNameWithoutExtension(s)))
                    {
                        File.Delete(s);
                        Console.Write($"Remove CS {s}");
                    }
                }
                var allServerCS = FileHelper.GetAllFiles(ServerClassDir, "*.cs");
                foreach (string s in allServerCS)
                {
                    if (!_allSheet.Contains(Path.GetFileNameWithoutExtension(s)))
                    {
                        File.Delete(s);
                        Console.Write($"Remove CS {s}");
                    }
                }
                
                var allJson = FileHelper.GetAllFiles(jsonDir, "*.txt");
                foreach (string s in allJson)
                {
                    if (!_allSheet.Contains(Path.GetFileNameWithoutExtension(s)))
                    {
                        File.Delete(s);
                        Console.Write($"Remove json {s}");
                    }
                }
                
                var allConfig = FileHelper.GetAllFiles("../Config/Excel", "*.bytes");
                foreach (string s in allConfig)
                {
                    if (!_allSheet.Contains(Path.GetFileNameWithoutExtension(s).Replace("Category", "")))
                    {
                        File.Delete(s);
                        Console.Write($"Remove json {s}");
                    }
                }
                
                FileHelper.CopyDirectory("../Config/Excel", clientProtoDir);

                var sb = new StringBuilder();
                foreach (var log in _buildLogs.Values)
                {
                    if (log.Exist)
                    {
                        sb.AppendLine($"{log.ExcelName} {log.LastBuild}");
                        foreach (string sheet in log.Sheets)
                        {
                            sb.AppendLine($"--{sheet}");
                        }
                    }
                }
                FileHelper.WriteAllText(buildLog, sb.ToString());
                Console.Write("Export Excel Sucess!");
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
            finally
            {
                _tables.Clear();
                foreach (var kv in _packages)
                {
                    kv.Value.Dispose();
                }

                _packages.Clear();
            }
        }

        private static void ExportExcel(string path)
        {
            string dir = Path.GetDirectoryName(path);
            string relativePath = Path.GetRelativePath(excelDir, dir.Replace("\\", "/"));
            string fileName = Path.GetFileName(path);
            if (!fileName.EndsWith(".xlsx") || fileName.StartsWith("~$") || fileName.Contains("#"))
            {
                return;
            }

         
            ExcelPackage p = GetPackage(Path.GetFullPath(path));
            
            ExportExcelJson(p, relativePath);
       
        }

        private static string GetProtoDir(ConfigType configType, string relativeDir)
        {
            return string.Format(serverProtoDir, configType.ToString(), relativeDir);
        }

        private static Assembly GetAssembly(ConfigType configType)
        {
            return configAssemblies[(int) configType];
        }

        private static string GetClassDir(ConfigType configType)
        {
            return configType switch
            {
                ConfigType.s => ServerClassDir,
                _ => CSClassDir
            };
        }
        
        // 动态编译生成的cs代码
        private static Assembly DynamicBuild(ConfigType configType)
        {
            string classPath = GetClassDir(configType);
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            List<string> protoNames = new List<string>();
            foreach (string classFile in Directory.GetFiles(classPath, "*.cs"))
            {
                protoNames.Add(Path.GetFileNameWithoutExtension(classFile));
                syntaxTrees.Add(CSharpSyntaxTree.ParseText(File.ReadAllText(classFile)));
            }

            List<PortableExecutableReference> references = new List<PortableExecutableReference>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    if (assembly.IsDynamic)
                    {
                        continue;
                    }

                    if (assembly.Location == "")
                    {
                        continue;
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    throw;
                }

                PortableExecutableReference reference = MetadataReference.CreateFromFile(assembly.Location);
                references.Add(reference);
            }

            CSharpCompilation compilation = CSharpCompilation.Create(null,
                syntaxTrees.ToArray(),
                references.ToArray(),
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using MemoryStream memSteam = new MemoryStream();

            EmitResult emitResult = compilation.Emit(memSteam);
            if (!emitResult.Success)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (Diagnostic t in emitResult.Diagnostics)
                {
                    stringBuilder.Append($"{t.GetMessage()}\n");
                }

                throw new Exception($"动态编译失败:\n{stringBuilder}");
            }

            memSteam.Seek(0, SeekOrigin.Begin);

            Assembly ass = Assembly.Load(memSteam.ToArray());
            return ass;
        }


        #region 导出class

        static void ExportExcelClass(ExcelPackage p)
        {
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                var sheetName = worksheet.Name;
                if (sheetName.StartsWith('#'))
                    continue;
                
                var frontName = sheetName.Substring(0, sheetName.IndexOf('@'));
                sheetName = sheetName.Substring(sheetName.IndexOf('@') + 1);
                Table table = GetTable(sheetName);
                if (frontName.Contains("_s"))
                {
                    table.ServerOnly = true;
                }

                table.IsMainSheet = !Regex.IsMatch(sheetName, @"(\w+)_.+");

                try
                {
                    ExportSheetClass(worksheet, table);
                }
                catch (Exception e)
                {
                    Console.Write(e);
                    throw;
                }
            }
        }

        static void ExportSheetClass(ExcelWorksheet worksheet, Table table)
        {
            const int row = 2;
            for (int col = 3; col <= worksheet.Dimension.End.Column; ++col)
            {
                if (worksheet.Name.StartsWith("#"))
                {
                    continue;
                }

                string fieldName = worksheet.Cells[row + 2, col].Text.Trim(); //第四行，取字段类型
                if (fieldName == "")
                {
                    continue;
                }

                if (table.HeadInfos.ContainsKey(fieldName))
                {
                    continue;
                }

                string fieldCS = worksheet.Cells[row, col].Text.Trim().ToLower(); //第二行，字段归属或忽略列
                if (fieldCS.Contains("#"))
                {
                    table.HeadInfos[fieldName] = null;
                    continue;
                }
                
                // if (fieldCS == "")
                // {
                //     fieldCS = "cs";
                // }

                // if (table.HeadInfos.TryGetValue(fieldName, out var oldClassField))
                // {
                //     if (oldClassField.FieldCS != fieldCS)
                //     {
                //         Console.Write($"field cs not same: {worksheet.Name} {fieldName} oldcs: {oldClassField.FieldCS} {fieldCS}");
                //     }
                //
                //     continue;
                // }

                string fieldDesc = worksheet.Cells[row + 1, col].Text.Trim(); //第三行，描述
                string fieldType = worksheet.Cells[row + 3, col].Text.Trim(); //第五行，字段类型

                table.HeadInfos[fieldName] = new HeadInfo(fieldCS, fieldDesc, fieldName, fieldType, ++table.Index);
            }
        }

        static void ExportClass(string protoName, Dictionary<string, HeadInfo> classField, ConfigType configType)
        {
            string dir = GetClassDir(configType);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            string exportPath = Path.Combine(dir, $"{protoName}.cs");

            using FileStream txt = new FileStream(exportPath, FileMode.Create);
            using StreamWriter sw = new StreamWriter(txt);

            StringBuilder sb = new StringBuilder();
            foreach ((string _, HeadInfo headInfo) in classField)
            {
                if (headInfo == null)
                {
                    continue;
                }
                
                sb.Append($"\t\t/// <summary>{headInfo.FieldDesc}</summary>\n");
                sb.Append($"\t\t[ProtoMember({headInfo.FieldIndex})]\n");
                string fieldType = headInfo.FieldType;
                sb.Append($"\t\tpublic {fieldType} {headInfo.FieldName} {{ get; set; }}\n");
            }

            string content = template.Replace("[ConfigName]", protoName);
            content = content.Replace(("[Fields]"), sb.ToString());
            
            sw.Write(content);
        }

        #endregion

        #region 导出json


        static void ExportExcelJson(ExcelPackage p, string relativeDir)
        {
            var mainSheets = new Dictionary<string, ExcelWorksheet>();
            var subSheets = new MultiMap<string, ExcelWorksheet>();
            var regex = new Regex(@"(\w+)_.+");
            foreach (ExcelWorksheet worksheet in p.Workbook.Worksheets)
            {
                if (worksheet.Name.StartsWith("#"))
                {
                    continue;
                }
                var sheetName = worksheet.Name;
                var frontName = sheetName.Substring(0, sheetName.IndexOf('@'));
                sheetName = sheetName.Substring(sheetName.IndexOf('@') + 1);
                if (regex.IsMatch(sheetName)) 
                {
                    subSheets.Add(sheetName.Substring(0, sheetName.IndexOf('_')), worksheet);
                }
                else
                {
                    mainSheets.Add(sheetName, worksheet);
                }
            }

            foreach (var kv in mainSheets)
            {
                var sheetName = kv.Key;
                StringBuilder sb = new StringBuilder();
                sb.Append("{\"list\":[\n");
                
                var table = GetTable(sheetName);
                var configType = table.ServerOnly? ConfigType.s : ConfigType.cs;
                ExportSheetJson(kv.Value, sheetName, table.HeadInfos, configType, sb);
                if (subSheets.TryGetValue(sheetName, out var subs))
                {
                    foreach (ExcelWorksheet worksheet in subs)
                    {
                        var sourceName = worksheet.Name;
                        sourceName = sourceName.Substring(sourceName.IndexOf('@') + 1);
                        var t = GetTable(sourceName);
                        ExportSheetJson(worksheet, sheetName, t.HeadInfos, configType, sb);
                    }
                }
                sb.Append("]}\n");
                string dir = $"{jsonDir}/{configType}/{relativeDir}";
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                string jsonPath = Path.Combine(dir, $"{sheetName}.txt");
                FileHelper.WriteAllText(jsonPath, sb.ToString());
                ExportExcelProtobuf(configType, sheetName, relativeDir);
            }
        }

        static void ExportSheetJson(ExcelWorksheet worksheet, string name, 
                Dictionary<string, HeadInfo> classField, ConfigType configType, StringBuilder sb)
        {
            string configTypeStr = configType.ToString();
            for (int row = 6; row <= worksheet.Dimension.End.Row; ++row)
            {
                if (worksheet.Cells[row, 3].Text.Trim() == "")
                {
                    continue;
                }

                sb.Append("{");
                sb.Append($"\"_t\":\"{name}\"");
                for (int col = 3; col <= worksheet.Dimension.End.Column; ++col)
                {
                    string fieldName = worksheet.Cells[4, col].Text.Trim();
                    if (!classField.ContainsKey(fieldName))
                    {
                        continue;
                    }

                    HeadInfo headInfo = classField[fieldName];

                    if (headInfo == null)
                    {
                        continue;
                    }

                    // if (configType != ConfigType.cs && !headInfo.FieldCS.Contains(configTypeStr))
                    // {
                    //     continue;
                    // }

                    string fieldN = headInfo.FieldName;
                    if (fieldN == "Id")
                    {
                        fieldN = "_id";
                    }

                    sb.Append($",\"{fieldN}\":{Convert(headInfo.FieldType, worksheet.Cells[row, col].Text.Trim())}");
                }

                sb.Append("},\n");
            }
        }

        private static string Convert(string type, string value)
        {
            switch (type)
            {
                case "uint[]":
                case "int[]":
                case "int32[]":
                case "long[]":
                    return $"[{value}]";
                case "string[]":
                case "int[][]":
                    return $"[{value}]";
                case "int":
                case "uint":
                case "int32":
                case "int64":
                case "long":
                case "float":
                case "double":
                    if (value == "")
                    {
                        return "0";
                    }

                    return value;
                case "string":
                    return $"\"{value}\"";
                default:
                    throw new Exception($"不支持此类型: {type}");
            }
        }

        #endregion


        // 根据生成的类，把json转成protobuf
        private static void ExportExcelProtobuf(ConfigType configType, string protoName, string relativeDir)
        {
            string dir = GetProtoDir(configType, relativeDir);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            Assembly ass = GetAssembly(configType);
            Type type = ass.GetType($"ET.{protoName}Category");
            Type subType = ass.GetType($"ET.{protoName}");

            Serializer.NonGeneric.PrepareSerializer(type);
            Serializer.NonGeneric.PrepareSerializer(subType);

            IMerge final = Activator.CreateInstance(type) as IMerge;

            string p = $"{jsonDir}/{configType}/{relativeDir}";
            string[] ss = Directory.GetFiles(p, $"{protoName}_*.txt");
            List<string> jsonPaths = ss.ToList();
            jsonPaths.Add(Path.Combine(p, $"{protoName}.txt"));

            jsonPaths.Sort();
            jsonPaths.Reverse();
            foreach (string jsonPath in jsonPaths)
            {
                string json = File.ReadAllText(jsonPath);
                object deserialize = BsonSerializer.Deserialize(json, type);
                final.Merge(deserialize);
            }

            string path = Path.Combine(dir, $"{protoName}Category.bytes");

            using FileStream file = File.Create(path);
            Serializer.Serialize(file, final);
        }
    }
}