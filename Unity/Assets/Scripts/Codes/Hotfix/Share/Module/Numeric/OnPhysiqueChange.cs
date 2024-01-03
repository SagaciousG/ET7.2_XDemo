using ET.EventType;

namespace ET
{
    [NumericWatcher(SceneType.None, NumericType.Physique)]
    public class OnPhysiqueChange : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumbericChange args)
        {
            var change = args.New - args.Old;
            var changeMDef = change * 3;
            var changeDef = change * 5;
            var changeHp = change * 100;
            var changeParry = change * 1;
            
            var mDef = numericComponent.GetAsInt(NumericType.MagicDefenseBase);
            var def = numericComponent.GetAsInt(NumericType.DefenseBase);
            var hp = numericComponent.GetAsInt(NumericType.HeathMaxBase);
            var parry = numericComponent.GetAsInt(NumericType.ParryBase);
            
            numericComponent.Set(NumericType.MagicDefenseBase, mDef + changeMDef);
            numericComponent.Set(NumericType.DefenseBase, def + changeDef);
            numericComponent.Set(NumericType.HeathMaxBase, hp + changeHp);
            numericComponent.Set(NumericType.ParryBase, parry + changeParry);
        }
    }
}