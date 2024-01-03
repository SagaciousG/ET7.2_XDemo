using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HybridCLR;
using UnityEngine;
using YooAsset;

namespace ET
{
	public class CodeLoader: Singleton<CodeLoader>
	{
		private Assembly assembly;

		public async void Start()
		{
			if (!Init.Instance.GlobalConfig.UseLocalCodes &&
			    Init.Instance.GlobalConfig.EPlayMode is EPlayMode.HostPlayMode or EPlayMode.OfflinePlayMode)
			{
				await this.LoadCode();
				var modelDLL = (await YooAssetHelper.GetRawFileAsync("Model.dll")).GetRawFileData();
				var modelPDB = (await YooAssetHelper.GetRawFileAsync("Model.pdb")).GetRawFileData();
				var hotfixDLL = (await YooAssetHelper.GetRawFileAsync("Hotfix.dll")).GetRawFileData();
				var hotfixPDB = (await YooAssetHelper.GetRawFileAsync("Hotfix.pdb")).GetRawFileData();

				this.assembly = Assembly.Load(modelDLL, modelPDB);
				
				Assembly hotfixAssembly = Assembly.Load(hotfixDLL, hotfixPDB);
				Dictionary<string, Type> hotfixTypes = AssemblyHelper.GetAssemblyTypes(typeof (Game).Assembly, this.assembly, hotfixAssembly);
				EventSystem.Instance.Add(hotfixTypes);
				
				IStaticMethod start = new StaticMethod(assembly, "ET.Entry", "Start");
				start.Run();
			}
			else
			{
				this.LocalStart();
			}
		}

		private void LocalStart()
		{		
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			Dictionary<string, Type> types = AssemblyHelper.GetAssemblyTypes(assemblies);
			EventSystem.Instance.Add(types);
			foreach (Assembly ass in assemblies)
			{
				string name = ass.GetName().Name;
				if (name == "Unity.Model.Codes")
				{
					this.assembly = ass;
				}
			}
				
			IStaticMethod start = new StaticMethod(assembly, "ET.Entry", "Start");
			start.Run();
		}
		
		private async ETTask LoadCode()
		{
			// {{ AOT assemblies
			var patchedAOTAssemblyList = new List<string>
			{
				"Main.dll", "System.Core.dll", "UnityEngine.CoreModule.dll", "mscorlib.dll",
			};
			var tasks = new List<ETTask<RawFileOperationHandle>>();
			foreach (var aotDll in patchedAOTAssemblyList)
			{
				tasks.Add(YooAssetHelper.GetRawFileAsync(aotDll));
			}
			
			await ETTaskHelper.WaitAll(tasks);

			foreach (var task in tasks)
			{
				var dllBytes = task.GetResult().GetRawFileData();
				var err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.Consistent);
				Log.Debug("LoadMetadataForAOTAssembly. ret:" + err);
			}

			Log.Info("Dll加载完毕，正式进入游戏流程");
		}

		// 热重载调用该方法
		public void LoadHotfix()
		{
			byte[] assBytes;
			byte[] pdbBytes;
			if (!Define.IsEditor)
			{
				Dictionary<string, UnityEngine.Object> dictionary = AssetsBundleHelper.LoadBundle("code.unity3d");
				assBytes = ((TextAsset)dictionary["Hotfix.dll"]).bytes;
				pdbBytes = ((TextAsset)dictionary["Hotfix.pdb"]).bytes;
			}
			else
			{
				// 傻屌Unity在这里搞了个傻逼优化，认为同一个路径的dll，返回的程序集就一样。所以这里每次编译都要随机名字
				string[] logicFiles = Directory.GetFiles(Define.BuildOutputDir, "Hotfix_*.dll");
				if (logicFiles.Length != 1)
				{
					throw new Exception("Logic dll count != 1");
				}
				string logicName = Path.GetFileNameWithoutExtension(logicFiles[0]);
				assBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.dll"));
				pdbBytes = File.ReadAllBytes(Path.Combine(Define.BuildOutputDir, $"{logicName}.pdb"));
			}

			Assembly hotfixAssembly = Assembly.Load(assBytes, pdbBytes);
			
			Dictionary<string, Type> types = AssemblyHelper.GetAssemblyTypes(typeof (Game).Assembly, this.assembly, hotfixAssembly);
			
			EventSystem.Instance.Add(types);
		}
		
	}
}