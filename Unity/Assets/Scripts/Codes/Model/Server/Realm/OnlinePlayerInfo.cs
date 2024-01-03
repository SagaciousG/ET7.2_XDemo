using System.Collections.Generic;

namespace ET.Server
{
    [ChildOf(typeof(OnlinePlayerComponent))]
    public class OnlinePlayerInfo : Entity, IAwake<string>
    {
        public string Account { get; set; }
        public long GateActorID { get; set; }
        public Session RealmSession { get; set; }
    }
}