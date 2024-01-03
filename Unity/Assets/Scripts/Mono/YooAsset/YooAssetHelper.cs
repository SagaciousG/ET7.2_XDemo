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
    public static partial class YooAssetHelper
    {
        #region Extension

        public static T GetAsset<T>(this AssetOperationHandle assetOperationHandle)
            where T : UnityEngine.Object
        {
            return assetOperationHandle.GetAssetObject<T>();
        }

        public static T GetSubAsset<T>(this SubAssetsOperationHandle assetOperationHandle, string subAssetName)
            where T : UnityEngine.Object
        {
            return assetOperationHandle.GetSubAssetObject<T>(subAssetName);
        }
        

        #endregion
        
 
        /// <summary>
        /// 资源文件查询服务类
        /// </summary>
        public class GameQueryServices : IQueryServices
        {
            public DeliveryFileInfo GetDeliveryFileInfo(string packageName, string fileName)
            {
                throw new System.NotImplementedException();
            }

            public bool QueryDeliveryFiles(string packageName, string fileName)
            {
                return false;
            }

            public bool QueryStreamingAssets(string packageName, string fileName)
            {
                return false;
            }
        }
        
        
        /// <summary>
        /// 远端资源地址查询服务类
        /// </summary>
        private class RemoteServices : IRemoteServices
        {
            private readonly string _defaultHostServer;
            private readonly string _fallbackHostServer;

            public RemoteServices(string defaultHostServer, string fallbackHostServer)
            {
                _defaultHostServer = defaultHostServer;
                _fallbackHostServer = fallbackHostServer;
            }
            string IRemoteServices.GetRemoteMainURL(string fileName)
            {
                return $"{_defaultHostServer}/{fileName}";
            }
            string IRemoteServices.GetRemoteFallbackURL(string fileName)
            {
                return $"{_fallbackHostServer}/{fileName}";
            }
        }
        
        /// <summary>
        /// 获取资源服务器地址
        /// </summary>
        private static string GetHostServerURL(string package)
        {
            //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
            // string hostServerIP = "https://61.139.65.181:62322";
            string hostServerIP = "http://127.0.0.1:8808";
            string appVersion = "1";

#if UNITY_EDITOR
            if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                return $"{hostServerIP}/Output/Android/{package}";
            else
                return $"{hostServerIP}/Output/StandaloneWindows64/{package}";
#else
		if (Application.platform == RuntimePlatform.Android)
			return $"{hostServerIP}/CDN/Android/{appVersion}";
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
			return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		else if (Application.platform == RuntimePlatform.WebGLPlayer)
			return $"{hostServerIP}/CDN/WebGL/{appVersion}";
		else
			return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
        }
        
        /// <summary>
        /// 资源文件解密服务类
        /// </summary>
        private class GameDecryptionServices : IDecryptionServices
        {
            public ulong LoadFromFileOffset(DecryptFileInfo fileInfo)
            {
                return 32;
            }

            public byte[] LoadFromMemory(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            public Stream LoadFromStream(DecryptFileInfo fileInfo)
            {
                throw new NotImplementedException();
            }

            public uint GetManagedReadBufferSize()
            {
                return 1024;
            }
        }
    }
}