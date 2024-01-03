namespace ET
{
    public static class BagItemSystem
    {
        public class BagItemAwakeSystem : AwakeSystem<BagItem, int>
        {
            protected override void Awake(BagItem self, int a)
            {
                self.ItemID = a;
                switch ((BagItemType) self.Config.Type)
                {
                    case BagItemType.Arms:
                        self.AddComponent<BagItemArmComponent>();
                        break;
                }
            }
        }
    }
}