
namespace ET.Client
{
	[FriendOf(typeof(UILotterySkillComponent))]
	public static class UILotterySkillComponentSystem
	{
        public static void OnAwake(this UILotterySkillComponent self)
        {
	        self.one.OnClick(self.OnClick, 1);
	        self.ten.OnClick(self.OnClick, 10);
        }

        private static async void OnClick(this UILotterySkillComponent self, int count)
        {
	        var result = await SessionHelper.Call<LotteryALResponse>(self.ClientScene(), new LotteryALRequest()
	        {
		        Count = count,
		        Type = (int)LotteryType.Skill,
	        });
        }

        public static void OnCreate(this UILotterySkillComponent self)
        {
            
        }
        
        public static void SetData(this UILotterySkillComponent self, params object[] args)
        {
	        UIHelper.CreateUITopBack(self.GetParent<UI>(), "抽卡").Coroutine();
	        UIHelper.CreateUIMoney(self.GetParent<UI>(), MoneyType.Copper).Coroutine();
        }
        
        public static void OnRemove(this UILotterySkillComponent self)
        {
            
        }
	}
}
