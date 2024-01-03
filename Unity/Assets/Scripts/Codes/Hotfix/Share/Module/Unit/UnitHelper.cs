using System.Collections.Generic;
using Unity.Mathematics;

namespace ET
{
    [FriendOf(typeof(MoveComponent))]
    [FriendOf(typeof(NumericComponent))]
    public static class UnitHelper
    {
        public static UnitProto CreateUnitInfo(Unit unit)
        {
            UnitProto unitProto = new UnitProto();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitProto.Position = unit.Position;
            unitProto.Forward = unit.Forward;
            unitProto.Map = unit.Map;
            unitProto.MoveSpeed = unit.MoveSpeed;
            unitProto.SimpleUnit = new()
            {
                UnitId = unit.Id,
                UnitShow = unit.UnitShow,
                UnitType = (int)unit.Type,
                Level = unit.Level,
                Name = unit.Name,
            };

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent != null)
            {
                if (!moveComponent.IsArrived())
                {
                    unitProto.MoveInfo = new MoveInfo(){Points = new()};
                    for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                    {
                        float3 pos = moveComponent.Targets[i];
                        unitProto.MoveInfo.Points.Add(pos);
                    }
                }
            }
            
            if (unit.Type == UnitType.Player)
            {
                unitProto.GateWayId = unit.GetComponent<UnitInfoComponent>().BeInGateWay;
            }else if (unit.Type == UnitType.NPC)
            {
                unitProto.NPCID = unit.GetComponent<UnitNPCComponent>().NPCID;
            }
            return unitProto;
        }
        
        public static void SetUnitLevel(Unit unit, int lv)
        {
            var numericComponent = unit.GetComponent<NumericComponent>();
            var lvCfg = LevelUpConfigCategory.Instance.Get(lv);
            numericComponent.SetNoEvent(NumericType.HeathMaxBase, lvCfg.Hp);
        }

        public static void RefreshLevel(Unit unit)
        {
            var exp = UnitBagHelper.GetNum(unit, (int)MoneyType.Exp);
            var all = LevelUpConfigCategory.Instance.GetAll();
            foreach (var kv in all)
            {

                exp -= kv.Value.Exp;
                unit.Level = kv.Value.Level;
                if (exp <= 0)
                {
                    break;
                }
            }
        }
    }
}