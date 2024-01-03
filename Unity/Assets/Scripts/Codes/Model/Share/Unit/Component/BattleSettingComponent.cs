namespace ET
{
    [ComponentOf(typeof(Unit))]
    public class BattleSettingComponent : Entity, ISerializeToEntity, IAwake
    {
        public bool AutoBattle { get; set; }
        public int UseSkill { get; set; }
    }
}