
using Unity.Mathematics;
using UnityEngine;

namespace ET.Client
{
	[FriendOf(typeof(UIHeaderComponent))]
	public static class UIHeaderComponentSystem
	{
        public static void OnAwake(this UIHeaderComponent self)
        {
	        self.ViewObj = self.GetParent<UI>().GameObject.GetComponent<RectTransform>();
        }

        public static void OnCreate(this UIHeaderComponent self)
        {
            
        }
        
        public static void SetData(this UIHeaderComponent self, params object[] args)
        {
        }
        
        public static void OnRemove(this UIHeaderComponent self)
        {
            
        }

        public static void Update(this UIHeaderComponent self, Unit unit)
        {
	        self.name.text = $"Lv.{unit.Level}  {unit.Name}";
	        var screenPos = Init.Instance.MainCamera.WorldToScreenPoint(unit.Position + new float3(0, 2, 0));
	        RectTransformUtility.ScreenPointToLocalPointInRectangle(self.Container, screenPos,
		        Init.Instance.UICamera, out var local);
	        self.ViewObj.localPosition = local;
	        self.ViewObj.localScale = Mathf.Clamp((10 - screenPos.z) / 10 + 1, 0.5f, 1) * Vector3.one;
        }
	}
}
