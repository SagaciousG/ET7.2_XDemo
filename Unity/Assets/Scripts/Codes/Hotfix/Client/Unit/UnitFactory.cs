using Unity.Mathematics;

namespace ET.Client
{
    public static class UnitFactory
    {
        public static Unit Create(Scene currentScene, UnitProto unitProto)
        {
	        UnitComponent unitComponent = currentScene.GetComponent<UnitComponent>();
	        Unit unit = unitComponent.Add(unitProto.SimpleUnit.UnitId);
	        unit.Position = unitProto.Position;
	        unit.Forward = unitProto.Forward;
	        unit.UnitShow = unitProto.SimpleUnit.UnitShow;
	        unit.Type = (UnitType) unitProto.SimpleUnit.UnitType;
	        unit.Name = unitProto.SimpleUnit.Name;
		    unit.Map = unitProto.Map;
		    unit.Level = unitProto.SimpleUnit.Level;
		    unit.MoveSpeed = unitProto.MoveSpeed;

	        
	        unit.AddComponent<NumericComponent>();
	        unit.AddComponent<MoveComponent>();
	        unit.AddComponent<ObjectWait>();
	        
	        
	        switch (unit.Type)
	        {
		        case UnitType.Player:
		        {
			        var unitInfoComponent = unit.AddComponent<UnitInfoComponent>();
			        unitInfoComponent.BeInGateWay = unitProto.GateWayId;
		        
			        unit.AddComponent<TeamComponent>();
			        unit.AddComponent<BagComponent>();
			        break;
		        }
		        case UnitType.NPC:
		        {
			        unit.AddComponent<UnitNPCComponent>().NPCID = unitProto.NPCID;
			        break;
		        }
	        }
	        EventSystem.Instance.Publish(unit.DomainScene(), new Event_AfterUnitCreate() {Unit = unit});
            return unit;
        }
    }
}
