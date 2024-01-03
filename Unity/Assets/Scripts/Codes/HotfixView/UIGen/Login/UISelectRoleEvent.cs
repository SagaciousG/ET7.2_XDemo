using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
    [UIEvent(UIType.UISelectRole)]
    [FriendOf(typeof(UISelectRoleComponent))]
    public class UISelectRoleEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UISelectRoleComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.role_3 = rc.Get<XSubUI>("role_3");
			self.role_1 = rc.Get<XSubUI>("role_1");
			self.role_2 = rc.Get<XSubUI>("role_2");
			self.enterMap = rc.Get<XImage>("enterMap");
			self.role.Add(3, self.role_3);
			self.role.Add(1, self.role_1);
			self.role.Add(2, self.role_2);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UISelectRoleComponent self)
        {
			self.role_3UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.role_3.uiPrefab.name, self.role_3.transform);
			self.role_1UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.role_1.uiPrefab.name, self.role_1.transform);
			self.role_2UI = await UIHelper.CreateSingleUI(self.GetParent<UI>(), self.role_2.uiPrefab.name, self.role_2.transform);
			self.roleUI.Add(3, self.role_3UI);
			self.roleUI.Add(1, self.role_1UI);
			self.roleUI.Add(2, self.role_2UI);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UISelectRoleComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UISelectRoleComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UISelectRoleComponent)ui.Component).OnRemove();
        }
    }
}