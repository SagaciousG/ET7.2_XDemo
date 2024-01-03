namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class TeamComponent : Entity, IAwake
    {
        public bool IsTeaming { get; set; }
        public bool IsLeader => this.Id == this.LeaderId;
        public long LeaderId { get; set; }
        public long[] Partners { get; set; } = new long[5];
    }
}