using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using ET;
using YooAsset;

namespace ET
{
    public enum Package
    {
        Default,
        Art,
    }
    
    public static partial class YooAssetHelper
    {
        public static void UnloadAssets(Package packageName = Package.Default)
        {
            var package = YooAssets.GetPackage(packageName.ToString());
            package.UnloadUnusedAssets();
        }
        
        public static ETTask<RawFileOperationHandle> GetRawFileAsync(string path, Package packageName = Package.Default)
        {
            var etTask = ETTask<RawFileOperationHandle>.Create();
            var package = YooAssets.GetPackage(packageName.ToString());
            var operationHandle = package.LoadRawFileAsync(path);
            operationHandle.Completed += handle => etTask.SetResult(handle);
            return etTask;
        }
        
        public static ETTask<T> LoadAssetAsync<T>(string nameOrPath, Package packageName = Package.Default) where T : UnityEngine.Object
        {
            if (typeof (T) == typeof (GameObject))
            {
                throw new NotSupportedException("加载GameObject请用LoadGameObjectAsync");
            }
            var package = YooAssets.GetPackage(packageName.ToString());
            AssetOperationHandle assetOperationHandle = package.LoadAssetAsync<T>(nameOrPath);
            var etTask = ETTask<T>.Create();
            assetOperationHandle.Completed += handle => etTask.SetResult(handle.GetAssetObject<T>());
            return etTask;
        }
        
        public static T LoadAssetSync<T>(string nameOrPath, Package packageName = Package.Default) where T : UnityEngine.Object
        {
            if (typeof (T) == typeof (GameObject))
            {
                throw new NotSupportedException("加载GameObject请用LoadGameObjectAsync");
            }            
            var package = YooAssets.GetPackage(packageName.ToString());
            AssetOperationHandle assetOperationHandle = package.LoadAssetSync<T>(nameOrPath);
            return assetOperationHandle.GetAssetObject<T>();
        }
        
        public static ETTask<GameObject> LoadGameObjectAsync(string nameOrPath, Package packageName = Package.Default)
        {
            var etTask = ETTask<GameObject>.Create();
            var package = YooAssets.GetPackage(packageName.ToString());
            AssetOperationHandle assetOperationHandle = package.LoadAssetAsync<GameObject>(nameOrPath);
            assetOperationHandle.Completed += handle =>
            {
                etTask.SetResult(handle.InstantiateSync());
            };
            return etTask;
        }

        public static ETTask<SubAssetsOperationHandle> LoadSubAssetsAsync<T>(string mainAssetPath, Package packageName = Package.Default)
            where T : UnityEngine.Object
        {
            var etTask = ETTask<SubAssetsOperationHandle>.Create();
            var package = YooAssets.GetPackage(packageName.ToString());
            SubAssetsOperationHandle subAssetsOperationHandle = package.LoadSubAssetsAsync<T>(mainAssetPath);
            subAssetsOperationHandle.Completed += handle => etTask.SetResult(handle);
            return etTask;
        }

        public static ETTask<SceneOperationHandle> LoadSceneAsync(string scenePath, Package packageName = Package.Default,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            var etTask = ETTask<SceneOperationHandle>.Create();
            var package = YooAssets.GetPackage(packageName.ToString());
            SceneOperationHandle sceneOperationHandle = package.LoadSceneAsync(scenePath, loadSceneMode, false);
            sceneOperationHandle.Completed += handle => etTask.SetResult(handle);
            return etTask;
        }
        
        public static List<string> GetAssetPathsByTag(string tag, Package packageName = Package.Default)
        {
            var package = YooAssets.GetPackage(packageName.ToString());
            AssetInfo[] assetInfos = package.GetAssetInfos(tag);
            List<string> result = new List<string>(16);
            foreach (var assetInfo in assetInfos)
            {
                result.Add(assetInfo.Address);
            }

            return result;
        }

        public static async ETTask<DownloaderOperation> GetDownloader(Package packageName = Package.Default)
        {
            var package = YooAssets.GetPackage(packageName.ToString());
            
            var versionOperation = package.UpdatePackageVersionAsync();
            var etTask = ETTask.Create();
            versionOperation.Completed += (a) => etTask.SetResult();
            await etTask;
            if (versionOperation.Status == EOperationStatus.Failed)
            {
                Log.Error($"[{packageName}]获取版本号失败, Url = {GetHostServerURL(packageName.ToString())}");
                return null;
                //更新失败
            }
            // 更新成功后自动保存版本号，作为下次初始化的版本。
            // 也可以通过operation.SavePackageVersion()方法保存。
            var etTask2 = ETTask.Create();
            var updatePackage = package.UpdatePackageManifestAsync(versionOperation.PackageVersion, true);
            updatePackage.Completed += (a) => etTask2.SetResult();
            await etTask2;
            Log.Error($"[{packageName}]当前版本：{package.GetPackageVersion()}，已获取到最新版本号：{versionOperation.PackageVersion}");
            
            int downloadingMaxNum = 10;
            int failedTryAgain = 3;
            var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);
            if (downloader.TotalDownloadCount == 0)
            {
                Log.Error($"[{packageName}]没有更新文件 !");
            }

            return downloader;
        }

        public static async ETTask DownloadPackages(DownloaderOperation[] downloaderOperations)
        {
            int totalCount = 0;
            long totalBytes = 0;
            foreach (var op in downloaderOperations)
            {
                totalCount += op?.TotalDownloadCount ?? 0;
                totalBytes += op?.TotalDownloadBytes ?? 0;
            }
            Log.Error($"共{totalCount}个文件需要更新，大小：{MathHelper.ByteFormat(totalBytes)}");
            UILoading.Show(0, "开始下载");
            foreach (var op in downloaderOperations)
            {
                if (op == null)
                    continue;
                await DownloadPackage(op);
            }
            UILoading.Hide();
            Log.Error("下载完成");
        }

        private static async ETTask DownloadPackage(DownloaderOperation downloaderOperation)
        {
            downloaderOperation.OnDownloadProgressCallback += (count, downloadCount, bytes, downloadBytes) =>
            {
                UILoading.Set(downloadBytes * 1f / bytes, $"正在下载{MathHelper.ByteFormat(downloadBytes)}/{MathHelper.ByteFormat(bytes)}  ");
            };
            var etTask3 = ETTask.Create();
            downloaderOperation.BeginDownload();
            downloaderOperation.Completed += (a) =>
            {
                etTask3.SetResult();
            };
            await etTask3;
        }
        
        public static async void InitShader(Package packageName)
        {
            var shaderVariantCollection = await LoadAssetAsync<ShaderVariantCollection>("ShaderVariants", packageName);
            if (shaderVariantCollection == null)
            {
                Log.Error("Load ShaderVariants error");
                return;
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            Log.Info($"开始Shader Warm Up, shaderCount: {shaderVariantCollection.shaderCount} variantCount: {shaderVariantCollection.variantCount}");
            shaderVariantCollection.WarmUp();
            stopwatch.Stop();
            Log.Info($"Shader Warm Up完成, 耗时: {stopwatch.ElapsedMilliseconds}ms");
        }

        
         public static ETTask<string> StartArtAssets()
        {
            var etTask = ETTask<string>.Create();
            
            var package = YooAssets.CreatePackage("Art");

            InitializationOperation initializationOperation = null;
            string defaultHostServer = GetHostServerURL("Art");
            string fallbackHostServer = GetHostServerURL("Art");
            var createParameters = new HostPlayModeParameters();
            createParameters.SandboxRootDirectory = Application.persistentDataPath;
            createParameters.DecryptionServices = new GameDecryptionServices();
            createParameters.QueryServices = new GameQueryServices();
            createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
            initializationOperation = package.InitializeAsync(createParameters);
            initializationOperation.Completed += a=>
            {
                etTask.SetResult(a.Error);
            };

            return etTask;
        }
         
        public static ETTask<string> StartUIAssets(EPlayMode playMode)
        {
            YooAssets.SetDownloadSystemCertificateHandler(new WebReqSkipCert());
            
            var etTask = ETTask<string>.Create();
            // 初始化资源系统
            YooAssets.Initialize();
            
            // 创建默认的资源包
            var package = YooAssets.CreatePackage("Default");

            // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
            YooAssets.SetDefaultPackage(package);
            
            InitializationOperation initializationOperation = null;
            switch (playMode)
            {
                case EPlayMode.EditorSimulateMode:
                {
                    var initParameters = new EditorSimulateModeParameters();
                    initParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild("Default");
                    initializationOperation = package.InitializeAsync(initParameters);
                    break;
                }
                // 单机运行模式
                case EPlayMode.OfflinePlayMode:
                {
                    var initParameters = new OfflinePlayModeParameters();
                    initializationOperation = package.InitializeAsync(initParameters);
                    break;
                }
                // 联机运行模式
                case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL("Default");
                    string fallbackHostServer = GetHostServerURL("Default");
                    var createParameters = new HostPlayModeParameters();
                    createParameters.SandboxRootDirectory = Application.persistentDataPath;
                    createParameters.DecryptionServices = new GameDecryptionServices();
                    createParameters.QueryServices = new GameQueryServices();
                    createParameters.RemoteServices = new RemoteServices(defaultHostServer, fallbackHostServer);
                    initializationOperation = package.InitializeAsync(createParameters);
                    break;
                }
            }
            initializationOperation.Completed += a=>
            {
                etTask.SetResult(a.Error);
            };

            return etTask;
        }
    }
}