
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UIClickBgCloseComponent))]
	public static class UIClickBgCloseComponentSystem
	{
		public static void OnAwake(this UIClickBgCloseComponent self)
		{
			self.bg.OnClick<UIClickBgCloseComponent>(OnBack, self);
		}

		private static void OnBack(UIClickBgCloseComponent self)
		{
			UIHelper.Remove(self.GetParent<UI>().GetParent<UI>().UIType, self.DomainScene()).Coroutine();
		}
        public static void OnCreate(this UIClickBgCloseComponent self)
        {
            
        }
        
        public static void SetData(this UIClickBgCloseComponent self, params object[] args)
        {
	        self.GetParent<UI>().GameObject.GetComponent<RectTransform>().SetAsFirstSibling();
        }
        
        public static void OnRemove(this UIClickBgCloseComponent self)
        {
            
        }
	}
}
