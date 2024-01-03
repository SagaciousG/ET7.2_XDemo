
namespace ET.Client
{
	[FriendOf(typeof(UITopCoinItemComponent))]
	public static class UITopCoinItemComponentSystem
	{
        public static void OnAwake(this UITopCoinItemComponent self)
        {
        }

        public static void OnCreate(this UITopCoinItemComponent self)
        {
	        SignalHelper.Add<int, long>(self.ClientScene(), SignalKey.UI_BagItemChanged_ID_Num, self, self.OnBagItemChanged);
        }
        
        private static async void OnBagItemChanged(this UITopCoinItemComponent self, int id, long num)
        {
	        if (id != self.MoneyId)
		        return;
	        self.num.text = (await UnitBagHelper.GetNum(self.ClientScene(), self.MoneyId)).ToString();
        }
        
        public static async void SetData(this UITopCoinItemComponent self, params object[] args)
        {
	        if ((MoneyType)args[0] == MoneyType.None)
		        return;
	        var item = ItemConfigCategory.Instance.Get((int)(MoneyType)args[0]);
	        self.MoneyId = item.Id;
	        self.coin.Skin = item.Icon;
	        self.num.text = (await UnitBagHelper.GetNum(self.ClientScene(), item.Id)).ToString();
        }
        
        public static void OnRemove(this UITopCoinItemComponent self)
        {
	        SignalHelper.Remove<int, long>(self.ClientScene(), SignalKey.UI_BagItemChanged_ID_Num, self, self.OnBagItemChanged);
        }
	}
}
