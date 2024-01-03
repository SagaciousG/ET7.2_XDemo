
namespace ET.Client
{
	[FriendOf(typeof(UICoinBannerComponent))]
	public static class UICoinBannerComponentSystem
	{
        public static void OnAwake(this UICoinBannerComponent self)
        {
        }

        public static void OnCreate(this UICoinBannerComponent self)
        {
            
        }
        
        public static void SetData(this UICoinBannerComponent self, params object[] args)
        {
	        for (int i = 0; i < 2; i++)
	        {
			    self.item[i + 1].gameObject.Display((MoneyType) args[i] != MoneyType.None);
			    self.itemUI[i + 1].SetData(args[i]);
	        }
        }
        
        public static void OnRemove(this UICoinBannerComponent self)
        {
            
        }
	}
}
