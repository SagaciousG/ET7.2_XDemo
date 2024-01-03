using System;

namespace ET.Server
{
    [ActorMessageHandler(SceneType.Gate)]
    public class R2G_KickOutPlayerHandler : AMActorHandler<Scene, R2G_KickOutPlayerAMessage>
    {
        protected override async ETTask Run(Scene scene, R2G_KickOutPlayerAMessage message)
        {
            var playerComponent = scene.GetComponent<PlayerComponent>();
            var player = playerComponent.GetByAccount(message.Account);
            player.Session.Send(new KickOutMessage(){Code = ErrorCode.ERR_OtherUserLogin});
            await TimerComponent.Instance.WaitFrameAsync();
            player.Session.Dispose(); 
            await ETTask.CompletedTask;
        }
    }
}