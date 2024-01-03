namespace ET.Server
{
    [ComponentOf(typeof(Unit))]
    public class MeetMonsterComponent : Entity, IAwake
    {
        //上次战斗时间
        public long LastFightTime { get; set; }
        public long MeetDetectionTime { get; set; }
        public bool IsInBattle { get; set; }
    }
}