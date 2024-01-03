using System;
using System.Collections.Generic;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Map)]
    public class UnitHandler_Lottery : AMActorLocationRpcHandler<Unit, LotteryALRequest, LotteryALResponse>
    {
        protected override async ETTask Run(Unit unit, LotteryALRequest request, LotteryALResponse response, Action reply)
        {
            var lotteryComponent = unit.GetComponent<LotteryComponent>();
            var bagComponent = unit.GetComponent<BagComponent>();
            var list = new List<int>();
            var addItems = new List<BagItemProto>();
            for (int i = 0; i < request.Count; i++)
            {
                var res = lotteryComponent.DoLottery((LotteryType)request.Type, lotteryComponent.GetGroup((LotteryType)request.Type));
                list.Add(res);

                var dropPoolConfig = DropPoolConfigCategory.Instance.Get(res);
                var items = bagComponent.Add(dropPoolConfig.Item, dropPoolConfig.Num, false);
                addItems.AddRange(items);
            }
            bagComponent.SendBagItemChange(addItems);
            response.Result = list;
            unit.SaveAsync();
            reply();
            await ETTask.CompletedTask;
        }
    }
}