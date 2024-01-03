using System;
using System.Collections.Generic;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class GMHandler : AMActorLocationRpcHandler<Unit, GMALRequest, GMALResponse>
    {
        protected override async ETTask Run(Unit unit, GMALRequest request, GMALResponse response, Action reply)
        {
            switch (request.Code)
            {
                case GMCode.AddRobot:
                {
                    break;
                }
                case GMCode.ClearDB:
                {
                    this.ClearDB(unit);
                    break;
                }
                case GMCode.ReloadSkill:
                {
                    this.ReloadSkill(unit, request);
                    break;
                }
                case GMCode.ReloadBuff:
                {
                    this.ReloadBuff(unit, request);
                    break;
                }
                case GMCode.GetItem:
                {
                    response.Error = this.GetItem(unit, request);
                    break;
                }
                case GMCode.ClearBag:
                {
                    this.ClearBag(unit, request);
                    break;
                }
            }
            reply();
            
            await ETTask.CompletedTask;
        }

        private void ClearDB(Unit unit)
        {
            
        }

        private void ReloadSkill(Unit unit, GMALRequest request)
        {
           
        }

        private void ReloadBuff(Unit unit, GMALRequest request)
        {
       
        }

        private int GetItem(Unit unit, GMALRequest request)
        {
            var cfg = ItemConfigCategory.Instance.Get(Convert.ToInt32(request.P1));
            if (cfg == null)
                return ErrorCode.ERR_ItemNotExist;
            var bagComponent = unit.GetComponent<BagComponent>();
      
            bagComponent.Add(Convert.ToInt32(request.P1), Convert.ToInt32(request.P2));
            unit.SaveAsync();
            return 0;
        }

        private void ClearBag(Unit unit, GMALRequest request)
        {
            var bagComponent = unit.GetComponent<BagComponent>();
            int type = 0;
            switch (request.P1)
            {
                case "全部":
                    break;
                case "道具":
                    type = 1;
                    break;
                case "材料":
                    type = 3;
                    break;
                case "装备":
                    type = 2;
                    break;
                case "货币":
                    type = 100;
                    break;
            }

            var removed = new List<BagItemProto>();
            foreach (var bagItem in bagComponent.GetChildren<BagItem>())
            {    
                if (bagItem.ItemID == 2 || bagItem.ItemID == 3)
                {
                    continue;
                }
                if (type == 0)
                {
                    removed.Add(new BagItemProto()
                    {
                        ID = bagItem.ItemID,
                        Num = bagItem.Num,
                        UID = bagItem.Id
                    });
                    bagComponent.Remove(bagItem.Id, bagItem.Num, false);
                }
                else if (bagItem.Config.Type == type)
                {
                    removed.Add(new BagItemProto()
                    {
                        ID = bagItem.ItemID,
                        Num = bagItem.Num,
                        UID = bagItem.Id
                    });
                    bagComponent.Remove(bagItem.Id, bagItem.Num, false);
                }
            }
            EventSystem.Instance.Publish(unit.DomainScene(), new Event_OnBagItemChange(){Items = removed, Unit = unit});
            
        }
    }
}