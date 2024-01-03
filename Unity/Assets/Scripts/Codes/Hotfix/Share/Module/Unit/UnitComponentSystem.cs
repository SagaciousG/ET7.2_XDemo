namespace ET
{
    [ObjectSystem]
    public class UnitComponentAwakeSystem : AwakeSystem<UnitComponent>
    {
        protected override void Awake(UnitComponent self)
        {
        }
    }
	
    [ObjectSystem]
    public class UnitComponentDestroySystem : DestroySystem<UnitComponent>
    {
        protected override void Destroy(UnitComponent self)
        {
        }
    }
	
    [FriendOf(typeof(UnitComponent))]
    public static class UnitComponentSystem
    {
        public static void Add(this UnitComponent self, Unit unit)
        {
            self.AddChild(unit);
            self.Units.Add(unit);
            self.TypeUnits.Add(unit.Type, unit);
        }
		
        public static Unit Add(this UnitComponent self, long id)
        {
            var unit = self.AddChildWithId<Unit>(id);
            self.Units.Add(unit);
            self.TypeUnits.Add(unit.Type, unit);
            return unit;
        }

        public static Unit Get(this UnitComponent self, long id)
        {
            Unit unit = self.GetChild<Unit>(id);
            return unit;
        }

        public static void Remove(this UnitComponent self, long id)
        {
            Unit unit = self.GetChild<Unit>(id);
            if (unit == null)
                return;
            self.TypeUnits.Remove(unit.Type, unit);
            self.Units.Remove(unit);
            unit?.Dispose();
        }
    }
}