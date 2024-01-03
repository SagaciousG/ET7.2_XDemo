namespace ET
{
    public enum ProfessionNum
    {
        One,
        Two,
    }
    
    
    [ChildOf()] //BattleUnit Unit
    public class Profession : Entity, IAwake<ProfessionNum>, ISerializeToEntity
    {
        public ProfessionNum Num { get; set; }
    }
}