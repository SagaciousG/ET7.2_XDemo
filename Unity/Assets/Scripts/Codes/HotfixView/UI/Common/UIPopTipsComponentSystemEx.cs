
using DG.Tweening;
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UIPopTipsComponent))]
	public static class UIPopTipsComponentSystem
	{
        public static void OnAwake(this UIPopTipsComponent self)
        {
	        self.StartPos = self.tips1.anchoredPosition;
        }

        public static void OnCreate(this UIPopTipsComponent self)
        {
            
        }
        
        public static void SetData(this UIPopTipsComponent self, params object[] args)
        {
        }
        
        public static void OnRemove(this UIPopTipsComponent self)
        {
            
        }

        public static void ShowTips(this UIPopTipsComponent self, string tip)
        {
	        if (self.Sequence[self.Index - 1] != null)
	        {
		        self.Sequence[self.Index - 1].Kill();
	        }
	        
			var cur = self.tips[self.Index];
			cur.anchoredPosition = self.StartPos;
			self.txt[self.Index].text = tip;
			cur.SetAsLastSibling();
			cur.Display(true);
			cur.Alpha(1);
			cur.localScale = new Vector3(1, 0, 1);
			self.Sequence[self.Index - 1] = DOTween.Sequence().Append(cur.DOScaleY(1, 0.5f));
			self.Sequence[self.Index - 1].Append(cur.Alpha(0, 0.5f).SetDelay(2));
			
			self.Index = self.Index + 1 > 3 ? 1 : self.Index + 1;
        }
	}
}
