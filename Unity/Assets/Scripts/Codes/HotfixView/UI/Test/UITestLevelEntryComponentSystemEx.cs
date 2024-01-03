
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UITestLevelEntryComponent))]
	public static class UITestLevelEntryComponentSystem
	{
        public static void OnAwake(this UITestLevelEntryComponent self)
        {
	        self.muzhuang.OnClick(self.OnFightWithDummy);
        }

        private static async void OnFightWithDummy(this UITestLevelEntryComponent self)
        {
	        var result = await SessionHelper.Call<TestFightWithDummyALResponse>(self.ClientScene(), new TestFightWithDummyALRequest());
	        if (result.Error == 0)
		        UIHelper.Remove(UIType.UITestLevelEntry, self.DomainScene()).Coroutine();
        }

        public static void OnCreate(this UITestLevelEntryComponent self)
        {
            
        }
        
        public static void SetData(this UITestLevelEntryComponent self, params object[] args)
        {
	        UIHelper.CreateUIBgClose(self.GetParent<UI>()).Coroutine();
        }
        
        public static void OnRemove(this UITestLevelEntryComponent self)
        {
            
        }
	}
}
