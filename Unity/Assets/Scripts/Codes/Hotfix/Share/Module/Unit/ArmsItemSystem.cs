namespace ET
{
    public static class ArmsItemSystem
    {
        public class ArmsItemAwakeSystem : AwakeSystem<ArmsItem, EquipmentHole>
        {
            protected override void Awake(ArmsItem self, EquipmentHole a)
            {
                self.Hole = a;
            }
        }

        public static BagItem GetBagItem(this ArmsItem self)
        {
            var unit = self.GetParentDepth<Unit>();
            var bagComponent = unit.GetComponent<BagComponent>();
            return bagComponent.GetByUID(self.Id);
        }
    }
}