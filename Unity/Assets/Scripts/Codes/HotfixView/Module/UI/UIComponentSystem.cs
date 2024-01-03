using UnityEngine;

namespace ET.Client
{
    /// <summary>
    /// 管理Scene上的UI
    /// </summary>
    [FriendOf(typeof(UIComponent))]
    public static class UIComponentSystem
    {
        public static async ETTask<UI> Create(this UIComponent self, string uiType, UILayer uiLayer, params object[] args)
        {
            if (self.UIs.TryGetValue(uiType, out var ui))
            {
                UIEventComponent.Instance.SetData(ui, uiType, args);
                return ui;
            }
			
            GameObject gameObject = await YooAssetHelper.LoadGameObjectAsync($"{uiType}");
            gameObject.transform.SetParent(UIEventComponent.Instance.GetLayer((int)uiLayer), false);
            ui = self.AddChild<UI, string, GameObject>(uiType, gameObject);
            if (!self.Name2Type.TryGetValue($"{uiType}", out var comType))
            {
                comType = typeof (UIComponent).Assembly.GetType($"ET.Client.{uiType}Component");
                self.Name2Type[$"{uiType}"] = comType;
            }
            ui.Component = ui.AddComponent(comType);
            await UIEventComponent.Instance.OnAwake(ui, uiType);
            UIEventComponent.Instance.OnCreate(ui, uiType);
            UIEventComponent.Instance.SetData(ui, uiType, args);
            self.UIs.Add(uiType, ui);
            return ui;
        }

        public static void Remove(this UIComponent self, string uiType)
        {
            if (!self.UIs.TryGetValue(uiType, out UI ui))
            {
                return;
            }
            YooAssetHelper.UnloadAssets();
            UIEventComponent.Instance.OnRemove(ui, uiType);
            DepthRemove(ui);
            self.UIs.Remove(uiType);
            ui.Dispose();
        }

        private static void DepthRemove(UI ui)
        {
            var children = ui.GetChildren<UI>();
            foreach (var c in children)
            {
                DepthRemove(c);
                UIEventComponent.Instance.OnRemove(c, c.UIType);
            }
        }

        public static UI Get(this UIComponent self, string name)
        {
            UI ui = null;
            self.UIs.TryGetValue(name, out ui);
            return ui;
        }

        public static void Close(this IUIComponent uiComponent)
        {
            UIHelper.Remove(uiComponent).Coroutine();
        }
    }
}