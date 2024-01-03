using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class MapHandler_GetInteractCodes : AMActorLocationRpcHandler<Unit, GetInteractCodesALRequest, GetInteractCodesALResponse>
    {
        protected override async ETTask Run(Unit unit, GetInteractCodesALRequest request, GetInteractCodesALResponse response, Action reply)
        {
            response.Options = new();
            var target = UnitHelper.GetUnit(unit.DomainScene(), request.Unit);
            switch (target.Type)
            {
                case UnitType.Player:
                {
                    
                    break;
                }
                case UnitType.NPC:
                {
                    var npcConfig = NPCConfigCategory.Instance.Get(target.GetComponent<UnitNPCComponent>().NPCID);
                    response.Options.AddRange(npcConfig.OptionCode.ToIntArray());
                    break;
                }
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}