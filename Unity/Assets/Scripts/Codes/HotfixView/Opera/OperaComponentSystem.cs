using System;
using UnityEngine;
using ET;

namespace ET.Client
{
    [FriendOf(typeof(OperaComponent))]
    public static class OperaComponentSystem
    {
        [ObjectSystem]
        public class OperaComponentAwakeSystem : AwakeSystem<OperaComponent>
        {
            protected override void Awake(OperaComponent self)
            {
                self.listenerId = InputComponent.Instance.AddBlockUIListener(OnInput, self); 
            }

            private void OnInput(InputData obj, object arg)
            {
                var self = (OperaComponent)arg;
                if (!self.ActiveSelf)
                    return;
                switch (obj.eventType)
                {
                    case InputEventType.Click:
                        var hit = RayUtil.RayCast(Camera.main, obj.position, LayerMask.GetMask("Ground"));
                        if (!hit.HasValue)
                            return;
                        FindPathALMessage c2MPathfindingResult = new FindPathALMessage();
                        c2MPathfindingResult.Position = hit.HitInfo.point;
                        SessionHelper.Send(self.ClientScene(), c2MPathfindingResult);
                        break;
                }
            }
        }
        
        public class OperaComponentDestroySystem : DestroySystem<OperaComponent>
        {
            protected override void Destroy(OperaComponent self)
            {
                InputComponent.Instance.RemoveListener(self.listenerId);
            }
        }
        
    }
}