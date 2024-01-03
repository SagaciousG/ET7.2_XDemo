using System;
using System.Collections.Generic;
using System.Net;


namespace ET.Server
{
    [MessageHandler(SceneType.Realm)]
    [FriendOf(typeof(AccountInfo))]
    public class RealmHandler_Login : AMRpcHandler<LoginRequest, LoginResponse>
    {
        protected override async ETTask Run(Session session, LoginRequest request, LoginResponse response, Action reply)
        {
            //验证账号
            var dbComponent = session.DomainScene().GetComponent<DBComponent>();
            var result = await dbComponent.Query<AccountInfo>(
                info => info.account == request.Account && info.password == request.Password
            );
            if (result.Count == 0)
            {
                response.Error = ErrorCode.ERR_AccountOrPwNotExist;
                reply();
                return;
            }
            session.RemoveComponent<SessionAcceptTimeoutComponent>();
            //检测账号登录情况
            var onlinePlayerComponent = session.DomainScene().GetComponent<OnlinePlayerComponent>();
            var onlinePlayer = onlinePlayerComponent.Get(request.Account);
            if (onlinePlayer != null)
            {
                if (onlinePlayer.RealmSession is { IsDisposed: false })
                {
                    onlinePlayer.RealmSession.Send(new KickOutMessage(){Code = ErrorCode.ERR_OtherUserLogin});
                    await TimerComponent.Instance.WaitFrameAsync();
                    onlinePlayer.RealmSession.Dispose();
                    MessageHelper.SendActor(onlinePlayer.GateActorID, new R2G_KickOutPlayerAMessage(){Account = request.Account});
                }
                onlinePlayer.Dispose();
            }

            onlinePlayer = onlinePlayerComponent.Add(request.Account);
            onlinePlayer.RealmSession = session;
            var realmPlayer = session.AddComponent<RealmPlayer>();
            realmPlayer.Account = request.Account;
            
            var zoneState = await MessageHelper.CallCenter<R2C_ServerZoneStateAResponse>(new R2C_ServerZoneStateARequest());
            
            var list = new List<ET.ZoneInfo>();
            for (int i = 0; i < zoneState.OnlineZones.Count; i++)
            {
                int zone = zoneState.OnlineZones[i];
                int count = zoneState.PlayerCount[i];
              
                list.Add(new ET.ZoneInfo()
                {
                    PlayerCount = count, Zone = zone,
                });
            }
            session.Send(new ZoneListMessage()
            {
                LatestEnterZones = result[0].LatestEnterZones,
                OnlineZones = list
            });
            Log.Console($"User Login {request.Account}");
            reply();
        }
    }
}
