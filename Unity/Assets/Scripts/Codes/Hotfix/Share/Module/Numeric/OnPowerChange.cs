using ET.EventType;

namespace ET
{
    [NumericWatcher(SceneType.None, NumericType.Power)]
    public class OnPowerChange : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumbericChange args)
        {
            var change = args.New - args.Old;
            var changeAtk = change * 10;
            var changeDef = change * 5;
            var changeHp = change * 50;
            
            var atk = numericComponent.GetAsInt(NumericType.AttackBase);
            var def = numericComponent.GetAsInt(NumericType.DefenseBase);
            var hp = numericComponent.GetAsInt(NumericType.HeathMaxBase);
            
            numericComponent.Set(NumericType.AttackBase, atk + changeAtk);
            numericComponent.Set(NumericType.DefenseBase, def + changeDef);
            numericComponent.Set(NumericType.HeathMaxBase, hp + changeHp);
        }
    }
}