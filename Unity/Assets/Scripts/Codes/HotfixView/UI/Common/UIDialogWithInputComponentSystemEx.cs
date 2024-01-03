
namespace ET.Client
{
    [FriendOf(typeof(UIDialogWithInputComponent))]
	public static class UIDialogWithInputComponentSystem
	{
        public static void OnAwake(this UIDialogWithInputComponent self)
        {
            self.rightBtn.OnClick(self.OnRightClick);
            self.leftBtn.OnClick(self.OnLeftClick);
        }


        public static void OnCreate(this UIDialogWithInputComponent self)
        {
            
        }
        
        public static void SetData(this UIDialogWithInputComponent self, params object[] args)
        {
        }
        
        public static void OnRemove(this UIDialogWithInputComponent self)
        {
            
        }
        
        private static void OnRightClick(this UIDialogWithInputComponent self)
        {
            self.Callback.SetResult(self.input.text);
            self.Close();
        }
        
        private static void OnLeftClick(this UIDialogWithInputComponent self)
        {
            self.Callback.SetResult(self.input.text);
            self.Close();
        }

        public static ETTask<string> Show(this UIDialogWithInputComponent self, string desc)
        {
            self.Callback = ETTask<string>.Create();
            self.desc.text = desc;
            self.title.text = "提示";
            self.leftText.text = "取 消";
            self.rightText.text = "确 定";
            return self.Callback;
        }
        
        public static ETTask<string> Show(this UIDialogWithInputComponent self, string desc, string title, string leftText, string rightText)
        {
            self.Callback = ETTask<string>.Create();
            self.desc.text = desc;
            self.title.text = title;
            self.leftText.text = leftText;
            self.rightText.text = rightText;
            return self.Callback;
        }
    }
}
