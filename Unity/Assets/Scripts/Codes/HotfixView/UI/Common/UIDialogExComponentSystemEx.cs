
namespace ET.Client
{
    [FriendOf(typeof(UIDialogExComponent))]
	public static class UIDialogExComponentSystem
	{
        public static void OnAwake(this UIDialogExComponent self)
        {
            self.rightBtn.OnClick(self.OnRightClick);
            self.leftBtn.OnClick(self.OnLeftClick);
            self.centerBtn.OnClick(self.OnCenterClick);
        }


        public static void OnCreate(this UIDialogExComponent self)
        {
            
        }
        
        public static void SetData(this UIDialogExComponent self, params object[] args)
        {
        }
        
        public static void OnRemove(this UIDialogExComponent self)
        {
            
        }
        
        private static void OnRightClick(this UIDialogExComponent self)
        {
            self.Callback.SetResult(UIDialog.ClickedBtn.Right);
            self.Close();
        }

        private static void OnCenterClick(this UIDialogExComponent self)
        {
            self.Callback.SetResult(UIDialog.ClickedBtn.Center);
            self.Close();
        }

        private static void OnLeftClick(this UIDialogExComponent self)
        {
            self.Callback.SetResult(UIDialog.ClickedBtn.Left);
            self.Close();
        }

        public static ETTask<UIDialog.ClickedBtn> Show(this UIDialogExComponent self, string desc)
        {
            self.Callback = ETTask<UIDialog.ClickedBtn>.Create();
            self.SetShow(UIDialog.ShowType.Two);
            self.desc.text = desc;
            self.title.text = "提示";
            self.leftText.text = "取 消";
            self.rightText.text = "确 定";
            return self.Callback;
        }
        
        public static ETTask<UIDialog.ClickedBtn> Show(this UIDialogExComponent self, string desc, string title, string leftText, string rightText)
        {
            self.Callback = ETTask<UIDialog.ClickedBtn>.Create();
            self.SetShow(UIDialog.ShowType.Two);
            self.desc.text = desc;
            self.title.text = title;
            self.leftText.text = leftText;
            self.rightText.text = rightText;
            return self.Callback;
        }
        
        public static async ETTask<UIDialog.ClickedBtn> Show(this UIDialogExComponent self, string desc, string title, string centerText, string leftText, string rightText)
        {
            self.Callback = ETTask<UIDialog.ClickedBtn>.Create();
            self.SetShow(UIDialog.ShowType.Three);
            self.desc.text = desc;
            self.title.text = title;
            self.centerText.text = centerText;
            self.leftText.text = leftText;
            self.rightText.text = rightText;
            return await self.Callback;
        }

        private static void SetShow(this UIDialogExComponent self, UIDialog.ShowType showType)
        {
            switch (showType)
            {
                case UIDialog.ShowType.One:
                    self.leftBtn.gameObject.SetActive(false);
                    self.rightBtn.gameObject.SetActive(false);
                    self.centerBtn.gameObject.SetActive(true);
                    break;
                case UIDialog.ShowType.Two:
                    self.leftBtn.gameObject.SetActive(true);
                    self.rightBtn.gameObject.SetActive(true);
                    self.centerBtn.gameObject.SetActive(false);
                    break;
                case UIDialog.ShowType.Three:
                    self.leftBtn.gameObject.SetActive(true);
                    self.rightBtn.gameObject.SetActive(true);
                    self.centerBtn.gameObject.SetActive(true);
                    break;
            }
        }
	}
}
