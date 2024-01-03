using UnityEngine;

namespace ET.Client
{
[FriendOf(typeof(UI))]
    public static class UISystem
    {
        [ObjectSystem]
        public class UIAwakeSystem : AwakeSystem<UI, string, GameObject>
        {
            protected override void Awake(UI self, string name, GameObject gameObject)
            {
                gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
                self.UIType = name;
                self.GameObject = gameObject;
            }
        }
		
        [ObjectSystem]
        public class UIDestroySystem : DestroySystem<UI>
        {
            protected override void Destroy(UI self)
            {
                UnityEngine.Object.Destroy(self.GameObject);
                if (self.UICloseTasks != null)
                {
                    foreach (var task in self.UICloseTasks)
                    {
                        task.SetResult();
                    }

                    self.UICloseTasks = null;
                }
                if (self.UICloseTasksWithParam != null)
                {
                    foreach (var task in self.UICloseTasksWithParam)
                    {
                        task.SetResult(self.TaskParam);
                    }

                    self.UICloseTasksWithParam = null;
                }
                self.TaskParam = null;
            }
        }

        public static void SetCloseTaskParam(this UI self, object param)
        {
            self.TaskParam = param;
        }
        public static ETTask WaitForClose(this UI self)
        {
            self.UICloseTasks ??= new();
            var task = ETTask.Create();
            self.UICloseTasks.Add(task);
            return task;
        }
        
        public static ETTask<object> WaitForCloseWithParam(this UI self)
        {
            self.UICloseTasksWithParam ??= new();
            var task = ETTask<object>.Create();
            self.UICloseTasksWithParam.Add(task);
            return task;
        }

        public static void SetAsFirstSibling(this UI self)
        {
            self.GameObject.transform.SetAsFirstSibling();
        }
        
        public static void SetData(this UI self, params object[] args)
        {
            UIEventComponent.Instance.SetData(self, self.UIType, args);
        }

        public static T FindInParent<T>(this IUIComponent self, string uiType) where T : Entity, IUIComponent
        {
            var p = ((Entity)self).GetParent<Entity>();
            while (p != null)
            {
                switch (p)
                {
                    case UI ui:
                    {
                        if (ui.UIType == uiType)
                        {
                            return ui.GetComponent<T>();
                        }
                        p = p.GetParent<Entity>();
                        break;
                    }
                    case IUIEntity:
                    {
                        p = p.GetParent<Entity>();
                        break;
                    }
                    default:
                        p = null;
                        break;
                }
            }

            return null;
        }
        
    }
}