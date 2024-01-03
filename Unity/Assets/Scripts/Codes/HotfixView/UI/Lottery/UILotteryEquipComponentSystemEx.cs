
namespace ET.Client
{
    [FriendOf(typeof(UILotteryEquipComponent))]
	public static class UILotteryEquipComponentSystem
	{
        public static void OnAwake(this UILotteryEquipComponent self)
        {
	        self.one.OnClick(self.OnClick, 1);
	        self.ten.OnClick(self.OnClick, 10);
        }
        private static async void OnClick(this UILotteryEquipComponent self, int count)
        {
	        var result = await SessionHelper.Call<LotteryALResponse>(self.ClientScene(), new LotteryALRequest()
	        {
		        Count = count,
		        Type = (int)LotteryType.Arms,
	        });
        }
        
        public static void OnCreate(this UILotteryEquipComponent self)
        {
            
        }
        
        public static void SetData(this UILotteryEquipComponent self, params object[] args)
        {
	        UIHelper.CreateUITopBack(self.GetParent<UI>(), "抽卡").Coroutine();
	        UIHelper.CreateUIMoney(self.GetParent<UI>(), MoneyType.Copper).Coroutine();
        }
        
        public static void OnRemove(this UILotteryEquipComponent self)
        {
            
        }
	}
}
