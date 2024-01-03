
namespace ET.Client
{
    [FriendOf(typeof(UITabDemoComponent))]
	public static class UITabDemoComponentSystem
	{
        public static void OnAwake(this UITabDemoComponent self)
        {
        }

        public static void OnCreate(this UITabDemoComponent self)
        {
            
        }
        
        public static void SetData(this UITabDemoComponent self, params object[] args)
        {
	        UIHelper.CreateUIBgClose(self.GetParent<UI>()).Coroutine();
        }
        
        public static void OnRemove(this UITabDemoComponent self)
        {
            
        }
	}
}
