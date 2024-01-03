using UnityEngine;
using ET;

namespace ET.Client
{
    public static class ETUIListSystem 
    {
        public class ETUIListAwakeSystem : AwakeSystem<ETUIList, UIList>
        {
            protected override void Awake(ETUIList self, UIList a)
            {
                self.UIList = a;
                if (a.IsPrefabAsset)
                {
                    a.OnCellFetch += self.OnCellFetch;
                    a.OnCellCollect += self.OnCellCollect;
                    a.OnData += self.OnData;
                }
            }
        }
        
        public class ETUIListDestroySystem : DestroySystem<ETUIList>
        {
            protected override void Destroy(ETUIList self)
            {
                if (self.UIList.IsPrefabAsset)
                {
                    self.UIList.OnCellFetch -= self.OnCellFetch;
                    self.UIList.OnCellCollect -= self.OnCellCollect;
                    self.UIList.OnData -= self.OnData;
                }
                self.UIList = null;
            }
        }

        private static void OnData(this ETUIList self, int index, RectTransform cell, object data)
        {
            UIEventComponent.Instance.SetData(self.IndexUIs[index], self.UIList.RenderCell.name, data);
        }
        
        private static void OnCellCollect(this ETUIList self, int index)
        {
            self.UnUsedUIs.Push(self.IndexUIs[index]);
            self.IndexUIs.Remove(index);
        }
        
        private static async void OnCellFetch(this ETUIList self, RectTransform cell, int index)
        {
            var uiType = self.UIList.RenderCell.name;
            if (self.UnUsedUIs.Count > 0)
            {
                self.IndexUIs[index] = self.UnUsedUIs.Pop();
                return;
            }
            var ui = self.AddChild<UI, string, GameObject>(uiType, cell.gameObject);
            var comType = typeof (UIComponent).Assembly.GetType($"ET.Client.{uiType}Component");
            ui.Component = ui.AddComponent(comType);
            self.IndexUIs[index] = ui;
            await UIEventComponent.Instance.OnAwake(ui, uiType);
            UIEventComponent.Instance.OnCreate(ui, uiType);
        }
    }
}