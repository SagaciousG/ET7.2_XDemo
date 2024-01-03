using UnityEngine.SceneManagement;

namespace ET.Client
{
	[Event(SceneType.Client)]
	public class UILoginEvent_Finish: AEvent<LoginFinish>
	{
		protected override async ETTask Run(Scene scene, LoginFinish args)
		{
			await UIHelper.Remove(UIType.UILogin, scene);
			await UIHelper.Create(UIType.UILobby, scene, UILayer.Mid);
		}
	}
	
	[Event(SceneType.Client)]
	public class UILoginEvent_ToTestFinish: AEvent<LoginToTestFinish>
	{
		protected override async ETTask Run(Scene scene, LoginToTestFinish args)
		{
			var roleList = await SessionHelper.Call<RoleListResponse>(scene, new RoleListRequest() { });
			await UIHelper.Remove(UIType.UITestLogin, scene);
			if (roleList.Units == null || roleList.Units.Count == 0)
			{
				var ui = await UIHelper.Create(UIType.UICreateRole, scene);
				await ui.WaitForClose();
				roleList = await SessionHelper.Call<RoleListResponse>(scene, new RoleListRequest() { });
			}

			await LobbyHelper.SelectRole(scene, roleList.Units[0].UnitId);
			await UIHelper.Create(UIType.UITestMain, scene.CurrentScene(), UILayer.Mid);
			if (Init.Instance.GlobalConfig.EnterDummy)
			{
				await SessionHelper.Call<TestFightWithDummyALResponse>(scene, new TestFightWithDummyALRequest());
			}
		}
	}
	
	[Event(SceneType.Client)]
	public class UILoginEvent_AppInitFinish: AEvent<AppStartInitFinish>
	{
		protected override async ETTask Run(Scene scene, AppStartInitFinish args)
		{
			if (Init.Instance.GlobalConfig.SkillTestMode)
			{
				await UIHelper.Create(UIType.UITestLogin, scene, UILayer.Mid);
			}
			else
			{
				await UIHelper.Create(UIType.UILogin, scene, UILayer.Mid);
			}
			UIHelper.Create(UIType.UIGMBar, scene, UILayer.High).Coroutine();
		}
	}
}
