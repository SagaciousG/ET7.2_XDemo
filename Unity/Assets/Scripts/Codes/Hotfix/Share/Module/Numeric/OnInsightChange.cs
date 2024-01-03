using ET.EventType;

namespace ET
{
    [NumericWatcher(SceneType.None, NumericType.Insight)]
    public class OnInsightChange : INumericWatcher
    {
        public void Run(NumericComponent numericComponent, NumbericChange args)
        {
            var change = args.New - args.Old;
            var changeMp = change * 10;
            var changeMAtk = change * 8; 
            var changeDodge = change * 1; //闪避
            var changeInsightValue = change * 1; //洞察值
            
            // var mp = numericComponent.GetAsInt(NumericType.MagicPowerMaxBase);
            var mAtk = numericComponent.GetAsInt(NumericType.MagicAttackBase);
            var dodge = numericComponent.GetAsInt(NumericType.DodgeBase);
            var iv = numericComponent.GetAsInt(NumericType.InsightValueBase);
            
            // numericComponent.Set(NumericType.MagicPowerMaxBase, mp + changeMp);
            numericComponent.Set(NumericType.MagicAttackBase, mAtk + changeMAtk);
            numericComponent.Set(NumericType.DodgeBase, dodge + changeDodge);
            numericComponent.Set(NumericType.InsightValueBase, iv + changeInsightValue);
        }
    }
}