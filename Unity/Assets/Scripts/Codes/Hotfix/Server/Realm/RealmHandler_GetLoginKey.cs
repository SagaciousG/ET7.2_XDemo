using System;
using System.Collections.Generic;

namespace ET.Server
{

    [MessageHandler(SceneType.Realm)]
    [FriendOf(typeof(AccountInfo))]
    public class RealmHandler_GetLoginKey : AMRpcHandler<GetLoginGateKeyRequest, GetLoginGateKeyResponse>
    {
        protected override async ETTask Run(Session session, GetLoginGateKeyRequest request, GetLoginGateKeyResponse response, Action reply)
        {
            var gate = StartSceneConfigCategory.Instance.GetGate(request.Zone);
            var loginKey = (R2G_GetLoginKeyAResponse) await MessageHelper.CallActor(gate.InstanceId, new R2G_GetLoginKeyARequest(){Account = request.Account});
            response.Address = gate.OuterIPPort.ToString();
            response.Key = loginKey.Key;
            
            var onlinePlayerComponent = session.DomainScene().GetComponent<OnlinePlayerComponent>();
            var onlinePlayer = onlinePlayerComponent.Get(request.Account);
            onlinePlayer.GateActorID = gate.InstanceId;
            DBComponent dbComponent = session.DomainScene().GetComponent<DBComponent>();
            var accountInfo = await dbComponent.QueryFirst<AccountInfo>(a => a.account == request.Account);
            accountInfo.LatestEnterZones ??= new List<int>();
            accountInfo.LatestEnterZones.Remove(request.Zone);
            accountInfo.LatestEnterZones.Insert(0, request.Zone);
            dbComponent.Save(accountInfo).Coroutine();
            reply();
        }
    }

}