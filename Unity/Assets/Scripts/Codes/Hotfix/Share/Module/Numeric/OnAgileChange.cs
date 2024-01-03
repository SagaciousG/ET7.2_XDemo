using ET.EventType;

namespace ET
{
    [NumericWatcher(SceneType.None, NumericType.Agile)]
    public class OnAgileChange : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumbericChange args)
        {
            var change = args.New - args.Old;
            var changeAtk = change * 7;
            var changeAs = change * 10; //出手速度
            var changeDodge = change * 1; //闪避
            
            var atk = numericComponent.GetAsInt(NumericType.AttackBase);
            var atkSpeed = numericComponent.GetAsInt(NumericType.AttackSpeedBase);
            var dodge = numericComponent.GetAsInt(NumericType.DodgeBase);
            
            numericComponent.Set(NumericType.AttackBase, atk + changeAtk);
            numericComponent.Set(NumericType.AttackSpeedBase, atkSpeed + changeAs);
            numericComponent.Set(NumericType.DodgeBase, dodge + changeDodge);
        }
    }
}