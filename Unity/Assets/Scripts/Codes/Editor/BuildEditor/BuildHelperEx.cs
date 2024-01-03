using System;
using ET;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using YooAsset.Editor;

public static class BuildHelper
{
    [MenuItem("Build/Build Codes")]
    public static void BuildCodes()
    {
        if (Define.EnableCodes)
        {
            throw new Exception("now in ENABLE_CODES mode, do not need Build!");
        }
        BuildAssembliesHelper.BuildModel(CodeOptimization.Debug, CodeMode.Client);
        BuildAssembliesHelper.BuildHotfix(CodeOptimization.Debug, CodeMode.Client);
    }
    
    public static void Build()
    {
        var args = Environment.GetCommandLineArgs();
        BuildTarget platform = BuildTarget.Android;
        var build = EBuildMode.IncrementalBuild;
        var version = "1";
        foreach (var s in args)
        {
            if (s.Contains("--platform"))
            {
                var p = s.Substring(s.IndexOf(':') + 1);
                switch (p)
                {
                    case "StandaloneWindows64":
                        platform = BuildTarget.StandaloneWindows64;
                        break;
                    case "Android":
                        platform = BuildTarget.Android;
                        break;
                }
            }
            else if (s.Contains("--version"))
            {
                version = s.Substring(s.IndexOf(':') + 1);
            }
            else if (s.Contains("--rebuild"))
            {
                build = EBuildMode.ForceRebuild;
            }
        }

        BuildInternal(platform, version, build);
    }
    
    //收集shader
    private static void CollectSVC(Action onComplete)
    {
        var setting = ShaderVariantCollectorSettingData.Setting;  
        System.Action completedCallback = () =>
        {
            ShaderVariantCollection collection =
                AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(setting.SavePath);
            if (collection != null)
            {
                Debug.Log($"ShaderCount : {collection.shaderCount}");
                Debug.Log($"VariantCount : {collection.variantCount}");
            }
            else
            {
                throw new Exception("Failed to Collect shader Variants.");
            }
        
            EditorTools.CloseUnityGameWindow();
            EditorApplication.Exit(0);
            onComplete?.Invoke();
        };
        ShaderVariantCollector.Run(setting.SavePath, setting.CollectPackage, setting.ProcessCapacity, completedCallback);
    }
    
    private static void BuildInternal(BuildTarget buildTarget, string version, EBuildMode buildMode)
    {
        Debug.Log($"开始构建 : {buildTarget}");

        // 构建参数
        string defaultOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        BuildParameters buildParameters = new BuildParameters();
        buildParameters.StreamingAssetsRoot = AssetBundleBuilderHelper.GetDefaultStreamingAssetsRoot();
        buildParameters.BuildOutputRoot = defaultOutputRoot;
        buildParameters.BuildTarget = buildTarget;
        buildParameters.BuildPipeline = EBuildPipeline.BuiltinBuildPipeline;
        buildParameters.BuildMode = buildMode;
        buildParameters.PackageName = "Default";
        buildParameters.PackageVersion = version;
        buildParameters.VerifyBuildingResult = true;
        buildParameters.SharedPackRule = new ZeroRedundancySharedPackRule();
        buildParameters.CompressOption = ECompressOption.LZ4;
        buildParameters.OutputNameStyle = EOutputNameStyle.HashName;
        buildParameters.CopyBuildinFileOption = ECopyBuildinFileOption.None;

        // 执行构建
        AssetBundleBuilder builder = new AssetBundleBuilder();
        var buildResult = builder.Run(buildParameters);
        if (buildResult.Success)
        {
            Debug.Log($"构建成功 : {buildResult.OutputPackageDirectory}");
        }
        else
        {
            Debug.LogError($"构建失败 : {buildResult.ErrorInfo}");
        }
    }

// 从构建命令里获取参数示例
    private static string GetBuildPackageName()
    {
        foreach (string arg in System.Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith("buildPackage"))
                return arg.Split("="[0])[1];
        }
        return string.Empty;
    }
}

