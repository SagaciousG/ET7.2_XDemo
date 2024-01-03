using ET.EventType;

namespace ET
{
    [NumericWatcher(SceneType.None, NumericType.Intellect)]
    public class OnIntellectChange : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumbericChange args)
        {
            var change = args.New - args.Old;
            var changeMp = change * 20;
            var changeMAtk = change * 12; 
            var changeMS = change * 10; //魔法护盾
            
            // var mp = numericComponent.GetAsInt(NumericType.MagicPowerMaxBase);
            var mAtk = numericComponent.GetAsInt(NumericType.MagicAttackBase);
            var ms = numericComponent.GetAsInt(NumericType.MagicShieldBase);
            
            // numericComponent.Set(NumericType.MagicPowerMaxBase, mp + changeMp);
            numericComponent.Set(NumericType.MagicAttackBase, mAtk + changeMAtk);
            numericComponent.Set(NumericType.MagicShieldBase, ms + changeMS);
        }
    }
}