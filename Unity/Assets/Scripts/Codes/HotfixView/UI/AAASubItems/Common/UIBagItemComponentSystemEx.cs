
namespace ET.Client
{
	[FriendOf(typeof(UIBagItemComponent))]
	public static class UIBagItemComponentSystem
	{
		public static void OnAwake(this UIBagItemComponent self)
        {
	        self.GetParent<UI>().GameObject.OnClick(self.OnClick);
        }

		private static void OnClick(this UIBagItemComponent self)
		{
			if (self.ShowData.Id == 0)
				return;
			switch (self.ShowData.ShowState)
			{
				case UIBagItemShowState.IsTips:
					return;
			}
			UIHelper.Create(UIType.UIItemTips, self.DomainScene(), UILayer.Mid, self.ShowData).Coroutine();
		}
		
        public static void OnCreate(this UIBagItemComponent self)
        {
        }

        private static void OnBagItemChanged(this UIBagItemComponent self, int id, long num)
        {
	        self.num.text = self.ShowData.Num == 0 ? "" : self.ShowData.Num.ToString();
        }
        
        public static void SetData(this UIBagItemComponent self, params object[] args)
        {
	        var showData = (UIBagItemArgs)args[0];
	        self.ShowData = showData;
	        
	        var cfg = ItemConfigCategory.Instance.Get(self.ShowData.Id);
	        self.icon.Skin = cfg.Icon;
	        self.num.text = self.ShowData.Num == 0 ? "" : self.ShowData.Num.ToString();
        }
        
        public static void OnRemove(this UIBagItemComponent self)
        {
        }
	}
}
