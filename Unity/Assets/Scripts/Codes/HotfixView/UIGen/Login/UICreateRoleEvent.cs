using UnityEngine.UI;
using UnityEngine;
using ET;
using TMPro;


namespace ET.Client
{
    [UIEvent(UIType.UICreateRole)]
    [FriendOf(typeof(UICreateRoleComponent))]
    public class UICreateRoleEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UICreateRoleComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.getName = rc.Get<XImage>("getName");
			self.errorText = rc.Get<TextMeshProUGUI>("errorText");
			self.inputName = rc.Get<XInputField>("inputName");
			self.create = rc.Get<XImage>("create");
			self.role = rc.Get<XModelImage>("role");
			self.armList = rc.Get<UIList>("armList");
			self.opList = rc.Get<UIList>("opList");
			self.colorList = rc.Get<UIList>("colorList");
			self.random = rc.Get<XImage>("random");
			self.reset = rc.Get<XImage>("reset");
			self.modelList = rc.Get<UIList>("modelList");
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UICreateRoleComponent self)
        {
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.armList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.opList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.colorList);
			self.GetParent<UI>().AddChild<ETUIList, UIList>(self.modelList);

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UICreateRoleComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UICreateRoleComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UICreateRoleComponent)ui.Component).OnRemove();
        }
    }
}