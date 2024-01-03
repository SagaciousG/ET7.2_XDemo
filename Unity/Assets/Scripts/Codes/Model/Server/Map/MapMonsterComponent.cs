namespace ET.Server
{
    [ComponentOf(typeof(Map))]
    public class MapMonsterComponent : Entity, IAwake<int>
    {
        public int MonsterGroup;

        public int WeightTotal;
        //遭遇怪物最小间隔时间
        public int MeetMonsterTimeMin { get; set; } = 2000;
        public int MonsterGetTimeMin { get; set; } = 500;
        
    }
}