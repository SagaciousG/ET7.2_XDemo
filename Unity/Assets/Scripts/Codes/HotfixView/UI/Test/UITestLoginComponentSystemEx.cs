
using UnityEngine.SceneManagement;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UITestLoginComponent))]
	public static class UITestLoginComponentSystem
	{
        public static void OnAwake(this UITestLoginComponent self)
        {
	        self.loginBtn.OnClick(self.OnLogin);
        }

        private static async void OnLogin(this UITestLoginComponent self)
        {
	        var error = await LoginHelper.LoginToTest(self.ClientScene(), self.account.text);
	        if (error > 0)
	        {
		        self.tips.text = ErrorCodeHelper.GetCodeTips(error);
		        return;
	        }
        }

        public static void OnCreate(this UITestLoginComponent self)
        {
            
        }
        
        public static void SetData(this UITestLoginComponent self, params object[] args)
        {
	        if (Init.Instance.GlobalConfig.AutoLogin)
	        {
		        self.account.text = Init.Instance.GlobalConfig.Account;
		        self.OnLogin();
	        }
        }
        
        public static void OnRemove(this UITestLoginComponent self)
        {
            
        }
	}
}
