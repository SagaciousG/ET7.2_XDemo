using System.Collections.Generic;

namespace ET.Client
{
    [MessageHandler(SceneType.Client)]
    public class ZoneListHandler : AMHandler<ZoneListMessage>
    {
        protected override async ETTask Run(Session session, ZoneListMessage message)
        {
            var playerComponent = session.DomainScene().GetComponent<PlayerComponent>();
            var zoneListComponent = playerComponent.AddComponent<PlayerZoneListComponent>();
            zoneListComponent.ZoneInfos = message.OnlineZones;
            zoneListComponent.LatestEnterZone = message.LatestEnterZones;
            await ETTask.CompletedTask;
        }
    }
}