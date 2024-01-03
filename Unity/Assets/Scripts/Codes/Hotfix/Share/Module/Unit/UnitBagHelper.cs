namespace ET
{
    public static class UnitBagHelper
    {
        public static long GetNum(Unit unit, int id)
        {
            var bagComponent = unit.GetComponent<BagComponent>();
            return bagComponent.GetNum(id);
        }
    }
}