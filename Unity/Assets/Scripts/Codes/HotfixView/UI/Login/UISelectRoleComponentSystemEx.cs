
using TMPro;
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UISelectRoleComponent))]
	public static class UISelectRoleComponentSystem
	{
        public static void OnAwake(this UISelectRoleComponent self)
        {
	        self.enterMap.OnClick(self.EnterMap);
        }
        
        private static async void EnterMap(this UISelectRoleComponent self)
        {
	        if (self.Units == null || self.SelectedUnit == null)
	        {
		        UIHelper.PopTips(self.DomainScene(), "尚未选择角色");
		        return;
	        }
	        await LobbyHelper.SelectRole(self.ClientScene(), self.SelectedUnit.UnitId);
	        await UIHelper.Remove(UIType.UISelectRole, self.DomainScene());
        }

        public static void OnCreate(this UISelectRoleComponent self)
        {
	        UIHelper.CreateUITopBack(self.GetParent<UI>(), "选择角色").Coroutine();
	   
        }

        public static void RefreshRoles(this UISelectRoleComponent self)
        {
	        for (int i = 0; i < 3; i++)
	        {
		        if (self.Units?.Count > i)
			        self.roleUI[i + 1].SetData(self.Units[i]);
		        else
			        self.roleUI[i + 1].SetData(null);
	        }
        }
        
        public static async void SetData(this UISelectRoleComponent self, params object[] args)
        {
	        var roleList = await SessionHelper.Call<RoleListResponse>(self.ClientScene(), new RoleListRequest() { });
	        self.Units = roleList.Units;
			self.RefreshRoles();
        }
        
        public static void OnRemove(this UISelectRoleComponent self)
        {
            
        }
	}
}
