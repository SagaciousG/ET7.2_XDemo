namespace ET.Server
{
    public struct Event_UnitEnterSightRange
    {
        public AOIEntity A;
        public AOIEntity B;
    }

    public struct Event_UnitLeaveSightRange
    {
        public AOIEntity A;
        public AOIEntity B;
    }

    public struct Event_OnEnterBattle
    {
        public Unit Unit;
        public int Seed;
    }

}