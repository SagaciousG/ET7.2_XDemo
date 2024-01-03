using UnityEngine;

namespace ET.Client
{
    public static class UIHelper
    {
        public static async ETTask<UI> Create(string uiType, Scene scene, UILayer uiLayer = UILayer.Mid, params object[] args)
        {
            return await scene.GetComponent<UIComponent>().Create(uiType, uiLayer, args);
        }

        public static UI UI(IUIComponent self)
        {
            var com = (Entity)self;
            var ui = com.GetParent<UI>();
            return ui;
        }
        public static async ETTask Remove(IUIComponent self)
        {
            var com = (Entity)self;
            var ui = com.GetParent<UI>();
            ui.DomainScene().GetComponent<UIComponent>().Remove(ui.UIType);
            await ETTask.CompletedTask;
        }
        
        public static async ETTask Remove(string uiType, Scene scene)
        {
            scene.GetComponent<UIComponent>().Remove(uiType);
            await ETTask.CompletedTask;
        }
        
        public static async ETTask Remove(UI ui, Scene scene)
        {
            scene.GetComponent<UIComponent>().Remove(ui.UIType);
            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 此处创建的UI不被UIManager记录管理
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="uiType"></param>
        /// <param name="parentTrans"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async ETTask<UI> CreateSingleUI(UI parent, string uiType, Transform parentTrans = null)
        {
            GameObject gameObject = await YooAssetHelper.LoadGameObjectAsync($"{uiType}");
            gameObject.transform.SetParent(parentTrans == null ? parent.GameObject.transform : parentTrans, false);
            var ui = parent.AddChild<UI, string, GameObject>(uiType, gameObject);
            var comType = typeof (UIComponent).Assembly.GetType($"ET.Client.{uiType}Component");
            ui.Component = ui.AddComponent(comType);
            ui.IsSingleUI = true;
            await UIEventComponent.Instance.OnAwake(ui, uiType);
            UIEventComponent.Instance.OnCreate(ui, uiType);
            return ui;
        }

        public static async ETTask<UI> BindSingleUI(UI parent, string uiType, GameObject uiObj)
        {
            var ui = parent.AddChild<UI, string, GameObject>(uiType, uiObj);
            var comType = typeof (UIComponent).Assembly.GetType($"ET.Client.{uiType}Component");
            if (comType == null)
            {
                Log.Error($"未获取到UIComponent类型，UIType = {uiType}");
            }
            ui.Component = ui.AddComponent(comType);
            ui.IsSingleUI = true;
            await UIEventComponent.Instance.OnAwake(ui, uiType);
            UIEventComponent.Instance.OnCreate(ui, uiType);
            return ui;
        }
        public static UI GetUI(Scene scene, string uiType)
        {
            var uiComponent = scene.GetComponent<UIComponent>();
            return uiComponent.Get(uiType);
        }
        
        public static T GetUIComponent<T>(Scene scene, string uiType) where T : Entity
        {
            var uiComponent = scene.GetComponent<UIComponent>();
            return uiComponent.Get(uiType)?.GetComponent<T>();
        }
        
        public static async ETTask<UITopBackComponent> CreateUITopBack(UI parent, string title)
        {
            var ui = await CreateSingleUI(parent, UIType.UITopBack, null);
            ui.SetData(title);
            return ui.GetComponent<UITopBackComponent>();
        }
        
        public static async ETTask<UIClickBgCloseComponent> CreateUIBgClose(UI parent)
        {
            var ui = await CreateSingleUI(parent, UIType.UIClickBgClose);
            ui.SetData();
            return ui.GetComponent<UIClickBgCloseComponent>();
        }
        
        public static async ETTask<UICoinBannerComponent> CreateUIMoney(UI parent, MoneyType m1 = default, MoneyType m2 = default, MoneyType m3 = default)
        {
            var ui = await CreateSingleUI(parent, UIType.UICoinBanner);
            ui.SetData(m1, m2, m3);
            return ui.GetComponent<UICoinBannerComponent>();
        }

        
        public static async void PopTips(Scene scene, string tips)
        {
            var uiComponent = scene.GetComponent<UIComponent>();
            var ui = uiComponent.Get(UIType.UIPopTips);
            if (ui == null)
            {
                ui = await Create(UIType.UIPopTips, scene, UILayer.High);
            }
            
            var popTipsComponent = ui.GetComponent<UIPopTipsComponent>();
            popTipsComponent.ShowTips(tips);
        }
        public static void PopError(Scene scene, int errorCode, string message = null)
        {
            var tip = ErrorCodeHelper.GetCodeTips(errorCode);
            
            switch (errorCode)
            {
                case ErrorCode.ERR_ItemNotEnough:
                {
                    var item = ItemConfigCategory.Instance.Get(message.ToInt32());
                    tip = string.Format(tip, item.Name);
                    break;
                }
            }
            PopTips(scene, tip);
        }

        public static async ETTask<string> ShowDialogWithInput(Scene scene, string desc, string title, string leftTxt, string rightTxt)
        {
            var uiComponent = scene.GetComponent<UIComponent>();
            var ui = uiComponent.Get(UIType.UIPopTips);
            if (ui == null)
            {
                ui = await Create(UIType.UIDialogWithInput, scene, UILayer.High);
            }
            var component = ui.GetComponent<UIDialogWithInputComponent>();
            return await component.Show(desc, title, leftTxt, rightTxt);
        }
        
        public static async ETTask<string> ShowDialogWithInput(Scene scene, string desc)
        {
            var uiComponent = scene.GetComponent<UIComponent>();
            var ui = uiComponent.Get(UIType.UIPopTips);
            if (ui == null)
            {
                ui = await Create(UIType.UIDialogWithInput, scene, UILayer.High);
            }
            var component = ui.GetComponent<UIDialogWithInputComponent>();
            return await component.Show(desc);
        }
    }
}