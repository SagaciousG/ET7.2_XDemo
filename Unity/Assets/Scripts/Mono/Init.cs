using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using CommandLine;
using HybridCLR;
using UnityEngine;
using ET;
using YooAsset;

namespace ET
{
	public class Init: MonoBehaviour
	{
		public static HttpClient HttpClient => _httpClient ??= new HttpClient();
		private static HttpClient _httpClient;
		
		
		public static Init Instance;

		public Camera UICamera;
		public Camera MainCamera;
		public GlobalConfig GlobalConfig;
		
		private async void Awake()
		{
			Instance = this;
			
			DontDestroyOnLoad(gameObject);
			if (this.GlobalConfig.DebugOpen)
				DebugComponent.New();
			
			AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error(e.ExceptionObject.ToString());
			};
				
			Game.AddSingleton<MainThreadSynchronizationContext>();

			// 命令行参数
			string[] args = "".Split(" ");
			Parser.Default.ParseArguments<Options>(args)
				.WithNotParsed(error => throw new Exception($"命令行格式错误! {error}"))
				.WithParsed(Game.AddSingleton);
			
			Game.AddSingleton<TimeInfo>();
			Game.AddSingleton<Logger>().ILog = new UnityLogger();
			Game.AddSingleton<ObjectPool>();
			Game.AddSingleton<IdGenerater>();
			Game.AddSingleton<EventSystem>();
			Game.AddSingleton<TimerComponent>();
			Game.AddSingleton<CoroutineLockComponent>();
			
			ETTask.ExceptionHandler += Log.Error;
			
			await YooAssetHelper.StartUIAssets(this.GlobalConfig.EPlayMode);
			await YooAssetHelper.StartArtAssets();
			await YooAssetHelper.DownloadPackages(new[]
			{
				await YooAssetHelper.GetDownloader(Package.Art),
				await YooAssetHelper.GetDownloader(Package.Default)
			});
			YooAssetHelper.InitShader(Package.Art);
			if (GlobalConfig.EPlayMode == EPlayMode.HostPlayMode || GlobalConfig.EPlayMode == EPlayMode.OfflinePlayMode)
				YooAssetHelper.InitShader(Package.Default);
			
			Game.AddSingleton<CodeLoader>().Start();
		}

		private void Update()
		{
			Game.Update();
		}

		private void LateUpdate()
		{
			Game.LateUpdate();
			Game.FrameFinishUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}