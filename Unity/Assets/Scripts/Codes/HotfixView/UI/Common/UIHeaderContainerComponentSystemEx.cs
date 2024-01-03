
using UnityEngine;

namespace ET.Client
{
	[FriendOf(typeof(UIHeaderContainerComponent))]
	public static class UIHeaderContainerComponentSystem
	{
		public class UIHeaderContainerComponentUpdateSystem : UpdateSystem<UIHeaderContainerComponent>
		{
			protected override async void Update(UIHeaderContainerComponent self)
			{
				var units = self.UnitComponent.Units;
				if (self.Headers.Count < units.Count)
				{
					for (int i = self.Headers.Count; i < units.Count; i++)
					{
						var ui = await UIHelper.CreateSingleUI(self.GetParent<UI>(), UIType.UIHeader);
						((UIHeaderComponent)ui.Component).Container = self.GetParent<UI>().GameObject.GetComponent<RectTransform>();
						self.Headers.Add(ui);
					}
				}
				else
				{
					for (int i = units.Count; i < self.Headers.Count; i++)
					{
						self.Headers[i].GameObject.SetActive(false);
					}
				}

				for (int i = 0; i < units.Count; i++)
				{
					self.Headers[i].GameObject.SetActive(true);
					((UIHeaderComponent)self.Headers[i].Component).Update(units[i]);
				}
				
			}
		}
		
        public static void OnAwake(this UIHeaderContainerComponent self)
        {
	        self.UnitComponent = self.DomainScene().GetComponent<UnitComponent>();
        }

        public static void OnCreate(this UIHeaderContainerComponent self)
        {
            
        }
        
        public static void SetData(this UIHeaderContainerComponent self, params object[] args)
        {
        }
        
        public static void OnRemove(this UIHeaderContainerComponent self)
        {
            
        }
	}
}
