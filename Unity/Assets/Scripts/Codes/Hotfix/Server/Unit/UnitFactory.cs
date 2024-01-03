using System;

namespace ET.Server
{
    public static class UnitFactory
    {
        public static Unit CreateNewUnit(Scene scene, SimpleUnit info)
        {
            var unit = scene.GetComponent<UnitComponent>().AddChildWithId<Unit>(info.UnitId);
            unit.UnitShow = info.UnitShow;
            unit.Level = 1;
            unit.Name = info.Name;
            if (scene.DomainZone() == 999)
                unit.Map = 999;
            else
                unit.Map = 1;
            unit.Type = (UnitType) info.UnitType;
            var mapConfig = MapConfigCategory.Instance.Get(unit.Map);
            unit.Position = mapConfig.BornPoint.ToFloat3();

            unit.MoveSpeed = 6;
            unit.AOI = 15000;
            
            unit.AddComponent<UnitInfoComponent>();
            unit.AddComponent<BattleSettingComponent>();

            unit.AddComponent<BagComponent>();
            unit.AddComponent<MyMapComponent>();

            var lotteryComponent = unit.AddComponent<LotteryComponent>();
            foreach (var value in Enum.GetValues(typeof(LotteryType)))
            {
                var key = (LotteryType)value;
                lotteryComponent.RandomSeeds[key] = (int) (TimeHelper.ServerNow() >> (int)value);
                lotteryComponent.Randoms[key] = new Random(lotteryComponent.RandomSeeds[key]);
                lotteryComponent.LotteryCounts[key] = 0;
            }
            
            foreach (var value in Enum.GetValues(typeof(ProfessionNum)))
            {
                var key = (ProfessionNum)value;
                var profession = unit.AddChild<Profession, ProfessionNum>(key);
                profession.AddComponent<ArmsComponent>();
            }
                
            unit.SaveAsync();
            return unit;
        }

        public static Unit CreateNPC(Scene scene, int npcID)
        {
            var npcConfig = NPCConfigCategory.Instance.Get(npcID);
            var unit = scene.GetComponent<UnitComponent>().AddChild<Unit>();
            var attr = LevelUpConfigCategory.Instance.Get(npcConfig.Attribute);
            unit.UnitShow = npcConfig.ModelShow;
            unit.Level = 1;
            unit.Name = npcConfig.Name;
            unit.Map = npcConfig.Map;
            unit.Type = UnitType.NPC;
            unit.Position = npcConfig.Pos.ToFloat3();

            unit.AddComponent<UnitNPCComponent>().NPCID = npcID;
            
            var numericComponent = unit.AddComponent<NumericComponent>();
            numericComponent.Set(NumericType.PowerBase, attr.Power);
            numericComponent.Set(NumericType.AgileBase, attr.Agile);
            numericComponent.Set(NumericType.InsightBase, attr.Insight);
            numericComponent.Set(NumericType.PhysiqueBase, attr.Physique);
            numericComponent.Set(NumericType.IntellectBase, attr.Intellect);

            unit.SaveAsync();
            return unit;
        }
    }
}