
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UITestMainComponent))]
	public static class UITestMainComponentSystem
	{
        public static void OnAwake(this UITestMainComponent self)
        {
	        self.enterBattle.OnClick(self.OnEnterBattle);
        }

        private static async void OnEnterBattle(this UITestMainComponent self)
        {
	        await UIHelper.Create(UIType.UITestLevelEntry, self.DomainScene());
        }

        public static void OnCreate(this UITestMainComponent self)
        {
            
        }
        
        public static void SetData(this UITestMainComponent self, params object[] args)
        {
        }
        
        public static void OnRemove(this UITestMainComponent self)
        {
            
        }
	}
}
