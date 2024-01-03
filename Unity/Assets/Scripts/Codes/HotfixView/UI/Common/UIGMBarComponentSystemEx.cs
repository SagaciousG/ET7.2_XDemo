
using DG.Tweening;
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UIGMBarComponent))]
	public static class UIGMBarComponentSystem
	{
        public static void OnAwake(this UIGMBarComponent self)
        {
	        self.gm.OnClick(self.OnGMClick);
	        self.open.OnClick(self.OnTweenOn);
	        self.dragArea.OnDraging += self.OnDraging;
	        self.dragArea.OnEnd += self.OnDragEnd;
	        self.dragArea.OnBegin += self.OnDragBegin;
        }

        private static void OnDragBegin(this UIGMBarComponent self, Vector3 obj)
        {
	        self.BeginPoint = obj;
	        self.BeginNodePoint = self.node.anchoredPosition;
	        self.CancellationToken.Cancel();
        }
        
        private static void OnDraging(this UIGMBarComponent self, Vector3 obj)
        {
	        self.node.anchoredPosition = self.BeginNodePoint + (obj - self.BeginPoint);
        }

        private static void OnDragEnd(this UIGMBarComponent self, Vector3 obj)
        {
	        var x = self.node.anchoredPosition.x < 0
			        ? -1 * ScreenUtil.HalfScreen.x + self.node.sizeDelta.x / 2 
			        : ScreenUtil.HalfScreen.x - self.node.sizeDelta.x / 2;
	        var y = Mathf.Clamp(self.node.anchoredPosition.y, 
		        ScreenUtil.HalfScreen.y * -1 + self.node.sizeDelta.y / 2,
		        ScreenUtil.HalfScreen.y - self.node.sizeDelta.y / 2);
	        self.node.DOLocalMove(new Vector3(x, y, 0), 0.5f);
	        self.OnTweenOn();
        }
        
        private static async void OnTweenOn(this UIGMBarComponent self)
        {
	        self.content.DOLocalMoveX(0, 0.3f);
	        float openSizeDeltaX = self.open.rectTransform.sizeDelta.x;
	        float openMoveX = self.node.sizeDelta.x / 2 - openSizeDeltaX / 2;
	        if (self.node.anchoredPosition.x < 0)
	        {
		        self.open.rectTransform.DOLocalMoveX(-openMoveX + -openSizeDeltaX, 0.3f);
	        }
	        else
	        {
		        self.open.rectTransform.DOLocalMoveX(openMoveX + openSizeDeltaX, 0.3f);
	        }

	        DOTween.ToAlpha(() => self.open.color, c => self.open.color = c, 0, 0.3f);
	        self.CancellationToken = new ETCancellationToken();
	        await TimerComponent.Instance.WaitAsync(2000, self.CancellationToken);
	        if (self.CancellationToken.IsCancel())
		        return;
	        if (self.node.anchoredPosition.x < 0)
	        {
		        self.open.Flip = XImage.FlipDirection.Horizontal;
		        self.content.DOLocalMoveX(-self.content.sizeDelta.x, 0.3f);
		        self.open.rectTransform.DOLocalMoveX(openMoveX * -1, 0.3f).SetDelay(0.4f);
	        }
	        else
	        {
		        self.open.Flip = XImage.FlipDirection.None;
		        self.content.DOLocalMoveX(self.content.sizeDelta.x, 0.3f);
		        self.open.rectTransform.DOLocalMoveX(openMoveX, 0.3f).SetDelay(0.4f);
	        }
		    DOTween.ToAlpha(() => self.open.color, c => self.open.color = c, 1, 0.3f).SetDelay(0.4f);
        }

        private static void OnGMClick(this UIGMBarComponent self)
        {
	        UIHelper.Create(UIType.UIGM, self.ClientScene(), UILayer.High).Coroutine();
        }

        public static void OnCreate(this UIGMBarComponent self)
        {
            
        }
        
        public static async void SetData(this UIGMBarComponent self, params object[] args)
        {
	        await TimerComponent.Instance.WaitAsync(2000);
	        self.OnTweenOn();
        }
        
        public static void OnRemove(this UIGMBarComponent self)
        {
            
        }
	}
}
