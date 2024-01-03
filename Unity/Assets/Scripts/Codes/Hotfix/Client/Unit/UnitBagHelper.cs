namespace ET.Client
{
    public static class UnitBagHelper
    {
        public static async ETTask<long> GetNum(Scene client, int id)
        {
            var myUnit = await UnitHelper.GetMyUnitFromClientScene(client);
            var bagComponent = myUnit.GetComponent<BagComponent>();
            return bagComponent.GetNum(id);
        }
    }
}