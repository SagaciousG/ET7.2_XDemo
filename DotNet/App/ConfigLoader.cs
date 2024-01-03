using System;
using System.Collections.Generic;
using System.IO;

namespace ET.Server
{
    [Invoke]
    public class GetAllConfigBytes: AInvokeHandler<ConfigComponent.GetAllConfigBytes, Dictionary<Type, byte[]>>
    {
        public override Dictionary<Type, byte[]> Handle(ConfigComponent.GetAllConfigBytes args)
        {
            Dictionary<Type, byte[]> output = new Dictionary<Type, byte[]>();
            var allFiles = FileHelper.GetAllFiles("./Config/Excel", "*.bytes");
            HashSet<Type> configTypes = EventSystem.Instance.GetTypes(typeof (ConfigAttribute));
            var map = new Dictionary<string, Type>();
            foreach (Type configType in configTypes)
            {
                map.Add(configType.Name, configType);
            }
            foreach (var file in allFiles)
            {
                output[map[Path.GetFileNameWithoutExtension(file)]] = File.ReadAllBytes(file);
            }

            return output;
        }
    }
    
    [Invoke]
    public class GetOneConfigBytes: AInvokeHandler<ConfigComponent.GetOneConfigBytes, byte[]>
    {
        public override byte[] Handle(ConfigComponent.GetOneConfigBytes args)
        {
            var allFiles = Directory.GetFiles("./Config/Excel", "*.bytes");
            foreach (string file in allFiles)
            {
                if (Path.GetFileNameWithoutExtension(file) == args.ConfigName)
                    return File.ReadAllBytes(file);
            }

            return null;
        }
    }
}