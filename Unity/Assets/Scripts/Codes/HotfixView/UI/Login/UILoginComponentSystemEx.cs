using TMPro;
using UnityEngine.UI;
using ET;

namespace ET.Client
{
    [FriendOf(typeof(UILoginComponent))]
	public static class UILoginComponentSystem
    {
        public static void OnAwake(this UILoginComponent self)
        {
            self.loginBtn.GetComponent<XImage>().OnClick(self.OnLogin);
            self.registerBtn.GetComponent<XImage>().OnClick(self.OnRegister);
            self.tabBtn.GetComponent<XImage>().OnClick(self.OnTab);
        }

        public static void OnCreate(this UILoginComponent self)
        {
            
        }
        
        public static void SetData(this UILoginComponent self, params object[] args)
        {
            self.account.text = "123";
            self.password.text = "123";
        }
        
        public static void OnRemove(this UILoginComponent self)
        {
        }
        
        
        public static async void OnLogin(this UILoginComponent self)
        {
            self.tips.GetComponent<TextMeshProUGUI>().text = "";
            var result = await LoginHelper.Login(
                self.DomainScene(), 
                self.account.text, 
                self.password.text);
            if (result == ErrorCode.ERR_AccountOrPwNotExist)
            {
                self.tips.GetComponent<TextMeshProUGUI>().text = "账号或密码不正确";
                return;
            } 
            await EventSystem.Instance.PublishAsync(self.ClientScene(), new LoginFinish());
        }

        public static async void OnTab(this UILoginComponent self)
        {
            UIHelper.Create(UIType.UITabDemo, self.DomainScene()).Coroutine();
        }

        public static async void OnRegister(this UILoginComponent self)
        {
            self.tips.GetComponent<TextMeshProUGUI>().text = "";
            var result = await LoginHelper.Register(
                self.DomainScene(), 
                self.account.GetComponent<TMP_InputField>().text, 
                self.password.GetComponent<TMP_InputField>().text);
            if (result == ErrorCode.ERR_AccountIsExist)
            {
                self.tips.GetComponent<TextMeshProUGUI>().text = "账号已存在";
            }
            else
            {
                self.tips.GetComponent<TextMeshProUGUI>().text = "注册成功";
            }
        }
    }
}