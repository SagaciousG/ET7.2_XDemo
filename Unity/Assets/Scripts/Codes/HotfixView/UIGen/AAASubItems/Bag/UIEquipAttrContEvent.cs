using UnityEngine.UI;
using UnityEngine;
using ET;
using System.Collections.Generic;


namespace ET.Client
{
    [UIEvent(UIType.UIEquipAttrCont)]
    [FriendOf(typeof(UIEquipAttrContComponent))]
    public class UIEquipAttrContEvent: AUIEvent
    {
        public override async ETTask OnAwake(UI ui)
        {
            var self = (UIEquipAttrContComponent)ui.Component;
            var rc = ui.GameObject.GetComponent<UIReferenceCollector>();
			self.baseProperty = rc.Get<RectTransform>("baseProperty");
			self.line1 = rc.Get<RectTransform>("line1");
			self.p1 = rc.Get<XText>("p1");
			self.p2 = rc.Get<XText>("p2");
			self.line2 = rc.Get<RectTransform>("line2");
			self.p3 = rc.Get<XText>("p3");
			self.p4 = rc.Get<XText>("p4");
			self.specialProperty = rc.Get<RectTransform>("specialProperty");
			self.sp1 = rc.Get<XText>("sp1");
			self.sp2 = rc.Get<XText>("sp2");
			self.sp3 = rc.Get<XText>("sp3");
			self.line.Add(1, self.line1);
			self.line.Add(2, self.line2);
			self.p.Add(1, self.p1);
			self.p.Add(2, self.p2);
			self.p.Add(3, self.p3);
			self.p.Add(4, self.p4);
			self.sp.Add(1, self.sp1);
			self.sp.Add(2, self.sp2);
			self.sp.Add(3, self.sp3);
  
            await SubAwake(self);
            self.OnAwake();
        }
     
        private async ETTask SubAwake(UIEquipAttrContComponent self)
        {

        }
     			
        public override void OnCreate(UI ui)
        {
            ((UIEquipAttrContComponent)ui.Component).OnCreate();
        }

        public override void SetData(UI ui, params object[] args)
        {
            ((UIEquipAttrContComponent)ui.Component).SetData(args);
        }

        public override void OnRemove(UI ui)
        {
            ((UIEquipAttrContComponent)ui.Component).OnRemove();
        }
    }
}