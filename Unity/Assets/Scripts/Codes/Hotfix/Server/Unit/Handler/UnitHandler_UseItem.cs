using System;
using System.Collections.Generic;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class UnitHandler_UseItem : AMActorLocationRpcHandler<Unit, UseItemALRequest, UseItemALResponse>
    {
        protected override async ETTask Run(Unit unit, UseItemALRequest request, UseItemALResponse response, Action reply)
        {
            var bag = unit.GetComponent<BagComponent>();
            var item = bag.GetByUID(request.UID);
            if (item.Num < request.Num)
            {
                response.Error = ErrorCode.ERR_ItemNotEnough;
                response.Message = item.ItemID.ToString();
                reply();
                return;
            }

            var consume = ItemConsumeConfigCategory.Instance.Get(item.Config.Index);
            switch ((ConsumeType) consume.Type)
            {
                case ConsumeType.Skill:
                {
                    // var skillComponent = unit.GetComponent<SkillComponent>();
                    // if (skillComponent.CanLevelUp(consume.Param1, (int) item.Num))
                    // {
                    //     var cur = skillComponent.GetLv(consume.Param1);
                    //     var skillCfg = SkillConfigCategory.Instance.Get(consume.Param1);
                    //     var take = skillCfg.LvConsume(cur, cur + (int)item.Num);
                    //     var have = UnitBagHelper.GetNum(unit, (int) MoneyType.Sp);
                    //     if (take > have)
                    //     {
                    //         response.Error = ErrorCode.ERR_ItemNotEnough;
                    //         response.Message = ((int)MoneyType.Sp).ToString();
                    //         reply();
                    //         return;
                    //     }
                    //     skillComponent.LevelUp(consume.Param1, (int) item.Num);
                    //     bag.Remove(request.UID, request.Num);
                    //     bag.Remove((int) MoneyType.Sp, take);
                    //     reply();
                    // }
                    // else
                    // {
                    //     response.Error = ErrorCode.ERR_SkillLevelMax;
                    //     reply();
                    //     return;
                    // }
                    break;
                }
            }
            unit.SaveAsync();
            await ETTask.CompletedTask;
        }
    }
}