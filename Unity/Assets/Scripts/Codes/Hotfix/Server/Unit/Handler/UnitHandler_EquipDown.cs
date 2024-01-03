using System;
using System.Collections.Generic;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class UnitHandler_EquipDown : AMActorLocationRpcHandler<Unit, EquipDownArmsALRequest, EquipDownArmsALResponse>
    {
        protected override async ETTask Run(Unit unit, EquipDownArmsALRequest request, EquipDownArmsALResponse response, Action reply)
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
            armsComponent.EquipDown(request.UID, true);
            
            reply();
            unit.SaveAsync();
            await ETTask.CompletedTask;
        }
    }
}