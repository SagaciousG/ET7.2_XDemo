using ET;

namespace ET.Client
{
    [FriendOf(typeof(UITopBackComponent))]
	public static class UITopBackComponentSystem
    {
        public static void OnAwake(this UITopBackComponent self)
        {
            self.back.OnClick<UITopBackComponent>(OnBack, self);
        }

        private static void OnBack(UITopBackComponent self)
        {
            UIHelper.Remove(self.GetParent<UI>().GetParent<UI>().UIType, self.DomainScene()).Coroutine();
        }

        public static void OnCreate(this UITopBackComponent self)
        {
            
        }
        
        public static void SetData(this UITopBackComponent self, params object[] args)
        {
            self.title.text = (string)args[0];
        }
        
        public static void OnRemove(this UITopBackComponent self)
        {
            
        }
    }
}