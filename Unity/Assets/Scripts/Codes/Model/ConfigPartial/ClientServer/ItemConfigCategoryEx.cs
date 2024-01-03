namespace ET
{
    public static class ItemConfigCategoryEx
    {
        public static ItemArmConfig GetArm(this ItemConfig self)
        {
            return ItemArmConfigCategory.Instance.Get(self.Index);
        }
        
    }
    public partial class ItemConfigCategory
    {
    }
}