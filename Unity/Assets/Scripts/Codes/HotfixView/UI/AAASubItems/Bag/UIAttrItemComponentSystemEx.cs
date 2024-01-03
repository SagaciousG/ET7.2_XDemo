
namespace ET.Client
{
    [FriendOf(typeof(UIAttrItemComponent))]
	public static class UIAttrItemComponentSystem
	{
        public static void OnAwake(this UIAttrItemComponent self)
        {
        }

        public static void OnCreate(this UIAttrItemComponent self)
        {
            
        }
        
        public static async void SetData(this UIAttrItemComponent self, params object[] args)
        {
	        var key = (int)args[0];
	        var propertyConfig = PropertyConfigCategory.Instance.GetByKey(key);
	        if (args.Length == 1)
	        {
		        var unit = await UnitHelper.GetMyUnitFromCurrentScene(self.DomainScene());
		        self.key.text = $"{propertyConfig.Name}  {unit.GetComponent<NumericComponent>().GetByKey(key).FormatNum()}";
	        }
        }
        
        public static void OnRemove(this UIAttrItemComponent self)
        {
            
        }
	}
}
