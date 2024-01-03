
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UIGMComponent))]
	public static partial class UIGMComponentSystem
	{
        public static void OnAwake(this UIGMComponent self)
        {
	        self.allList.OnSelectedViewRefresh += self.AllList_OnSelected;
	        self.commonList.OnSelectedViewRefresh += self.CommonList_OnSelected;
	        self.popList.OnSelectedViewRefresh += self.PopList_OnSelected;
	        
	        self.allList.OnData += self.AllList_OnData;
	        self.commonList.OnData += self.CommonList_OnData;
	        self.popList.OnData += self.PopList_OnData;
	        
	        self.p1.AddSelectListener(self.OnSelectInputField, 1);
	        self.p2.AddSelectListener(self.OnSelectInputField, 2);
	        self.p3.AddSelectListener(self.OnSelectInputField, 3);
	        
	        self.p1.AddDeselectListener(self.OnDeselectInputField, 1);
	        self.p2.AddDeselectListener(self.OnDeselectInputField, 2);
	        self.p3.AddDeselectListener(self.OnDeselectInputField, 3);
	        
	        self.p1.AddValueChangeListener(self.OnValueChangeInputField, 1);
	        self.p2.AddValueChangeListener(self.OnValueChangeInputField, 2);
	        self.p3.AddValueChangeListener(self.OnValueChangeInputField, 3);
	        
	        self.submit.OnClick(self.OnSubmit);
	        self.close.OnClick(self.OnClose);
        }

        private static void PopList_OnData(this UIGMComponent self, int arg1, RectTransform arg2, object arg3)
        {
	        var rc = arg2.GetComponent<UIReferenceCollector>();
	        var title = rc.Get<TextMeshProUGUI>("title");
	        title.text = ((UIGMComponent.GMTipsItem)arg3).Name;
        }

        private static void CommonList_OnData(this UIGMComponent self, int arg1, RectTransform arg2, object arg3)
        {
	        var cfg = GMConfigCategory.Instance.Get(Convert.ToInt32(arg3));
	        var referenceCollector = arg2.GetComponent<UIReferenceCollector>();
	        var bg = referenceCollector.Get<XImage>("bg");
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        title.text = cfg.title;
        }

        private static void AllList_OnData(this UIGMComponent self, int arg1, RectTransform arg2, object arg3)
        {
	        var cfg = (GMConfig)arg3;
	        var referenceCollector = arg2.GetComponent<UIReferenceCollector>();
	        var bg = referenceCollector.Get<XImage>("bg");
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        title.text = cfg.title;
        }

        private static async void OnSubmit(this UIGMComponent self)
        {
	        self.CommonUseList.Remove(self.SelectedId.ToString());
	        self.CommonUseList.Insert(0, self.SelectedId.ToString());
	        var cfg = GMConfigCategory.Instance.Get(self.SelectedId);
	        PlayerPrefsHelper.Save(PlayerPrefsKey.GMCommon, self.CommonUseList);
	        self.commonList.SetData(self.CommonUseList);
	        self.commonList.SetSelectIndex(self.CommonUseList.IndexOf(self.SelectedId.ToString()));
	        var gmResponse = await SessionHelper.Call<GMALResponse>(self.ClientScene(),
		        new GMALRequest() { Code = cfg.Code, P1 = self.p1.text, P2 = self.p2.text, P3 = self.p3.text });
	        if (gmResponse.Error > 0)
	        {
		        return;
	        }
	        UIHelper.PopTips(self.ClientScene(), "GM执行成功");
        }

        private static void OnClose(this UIGMComponent self)
        {
	        UIHelper.Remove((UI) self.Parent, self.ClientScene()).Coroutine();
        }
        
        private static void PopList_OnSelected(this UIGMComponent self, RectTransform arg1, bool arg2, object arg3, int arg4)
        {
	        self.OnSelectPopListItem((UIGMComponent.GMTipsItem)arg3);
	        self.popList.gameObject.Display(false);
        }

        private static void CommonList_OnSelected(this UIGMComponent self, RectTransform arg1, bool arg2, object arg3, int arg4)
        {
	        var referenceCollector = arg1.GetComponent<UIReferenceCollector>();
	        var bg = referenceCollector.Get<XImage>("bg");
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        if (arg2)
	        {
		        self.allList.SetSelectIndex(-1);
		        var cfg = GMConfigCategory.Instance.Get(Convert.ToInt32(arg3));
		        self.SelectedId = cfg.Id;
		        self.desc.text = cfg.Desc;
		        self.p1.Display(!string.IsNullOrEmpty(cfg.ParamTitle1));
		        self.p2.Display(!string.IsNullOrEmpty(cfg.ParamTitle2));
		        self.p3.Display(!string.IsNullOrEmpty(cfg.ParamTitle3));
		        self.pt1.text = cfg.ParamTitle1;
		        self.pt2.text = cfg.ParamTitle2;
		        self.pt3.text = cfg.ParamTitle3;
	        }
	        bg.color = !arg2 ? new Color(0, 0.4f, 0.44f) : new Color(0.02f, 0.6f, 0.2f);
        }

        private static void AllList_OnSelected(this UIGMComponent self, RectTransform arg1, bool arg2, object arg3, int arg4)
        {
	        var referenceCollector = arg1.GetComponent<UIReferenceCollector>();
	        var bg = referenceCollector.Get<XImage>("bg");
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        if (arg2)
	        {
		        self.commonList.SetSelectIndex(-1);
		        var cfg = (GMConfig)arg3;
		        self.SelectedId = cfg.Id;
		        self.desc.text = cfg.Desc;
		        self.p1.Display(!string.IsNullOrEmpty(cfg.ParamTitle1));
		        self.p2.Display(!string.IsNullOrEmpty(cfg.ParamTitle2));
		        self.p3.Display(!string.IsNullOrEmpty(cfg.ParamTitle3));
		        self.pt1.text = cfg.ParamTitle1;
		        self.pt2.text = cfg.ParamTitle2;
		        self.pt3.text = cfg.ParamTitle3;
	        }
	        bg.color = !arg2 ? new Color(0, 0.4f, 0.44f) : new Color(0.02f, 0.6f, 0.2f);
        }

        private static void OnValueChangeInputField(this UIGMComponent self, XInputField p, int index)
        {
	        self.OnValueChanged(p, index);
        }
        private static void OnDeselectInputField(this UIGMComponent self, XInputField p, int index)
        {
        }
        
        private static void OnSelectInputField(this UIGMComponent self, XInputField p, int index)
        {
	        var rectTransform = self.popList.GetComponent<RectTransform>();
	        var pRect = p.GetComponent<RectTransform>();
	        var localPos = RectTransformHelper.TransferLocalPos(pRect, (RectTransform)rectTransform.parent);
	        rectTransform.localPosition = localPos - new Vector2(0, pRect.sizeDelta.y / 2 + rectTransform.sizeDelta.y / 2);
	        self.OnInputSelected(p, index);
	        self.ActiveInput = p;
        }
        
        public static void OnCreate(this UIGMComponent self)
        {
            
        }
        
        public static void SetData(this UIGMComponent self, params object[] args)
        {
	        var all = GMConfigCategory.Instance.GetAll();
	        self.allList.SetData(all.Values.ToArray());
	        self.allList.SetSelectIndex(0);

	        var ss = PlayerPrefsHelper.Get(PlayerPrefsKey.GMCommon);
	        self.CommonUseList.AddRange(ss);
	        self.commonList.SetData(self.CommonUseList);
        }
        
        public static void OnRemove(this UIGMComponent self)
        {
            
        }

 
	}
}
