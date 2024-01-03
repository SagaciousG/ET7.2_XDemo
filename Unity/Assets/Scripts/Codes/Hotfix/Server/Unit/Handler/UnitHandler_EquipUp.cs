using System;
using System.Collections.Generic;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class UnitHandler_EquipUp : AMActorLocationRpcHandler<Unit, EquipUpArmsALRequest, EquipUpArmsALResponse>
    {
        protected override async ETTask Run(Unit unit, EquipUpArmsALRequest request, EquipUpArmsALResponse response, Action reply)
        {
            var bag = unit.GetComponent<BagComponent>();
            var item = bag.GetByUID(request.UID);
            if (item == null)
            {
                response.Error = ErrorCode.ERR_ItemNotExist;
                reply();
                return;
            }

            var profession = unit.GetProfession((ProfessionNum)request.Profession);
            var armsComponent = profession.GetComponent<ArmsComponent>();
            armsComponent.EquipUp(item, request.Profession, (EquipmentHole)request.Hole);
            
            reply();
            unit.SaveAsync();
            await ETTask.CompletedTask;
        }
    }
}