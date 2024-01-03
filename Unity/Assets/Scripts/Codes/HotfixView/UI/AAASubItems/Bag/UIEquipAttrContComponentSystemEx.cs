
namespace ET.Client
{
    [FriendOf(typeof(UIEquipAttrContComponent))]
	public static class UIEquipAttrContComponentSystem
	{
        public static void OnAwake(this UIEquipAttrContComponent self)
        {
        }

        public static void OnCreate(this UIEquipAttrContComponent self)
        {
            
        }
        
        public static void SetData(this UIEquipAttrContComponent self, params object[] args)
        {
	        var cfg = (ItemConfig)args[0];
	        var armConfig = ItemArmConfigCategory.Instance.Get(cfg.Index);
	        var words = armConfig.BaseWord.ToIntArray(',');
	        var vals = armConfig.BaseWordVal.ToIntArray(',');
	        for (int i = 0; i < 4; i++)
	        {
		        if (words.Length > i)
		        {
			        var entryWord = EntryWordConfigCategory.Instance.Get(words[i]);
			        var propertyConfig = PropertyConfigCategory.Instance.Get(entryWord.Param1.ToInt32());
					self.p[i + 1].text = $"{propertyConfig.Name}  {vals[i]}";
					self.p[i + 1].gameObject.Display(true);
		        }
		        else
		        {
					self.p[i + 1].gameObject.Display(false);
		        }
	        }

	        var special = armConfig.SpecialWord.ToIntArray(',');
	        var specialVals = armConfig.SpecialWordArgs.ToIntArray(',');
	        for (int i = 0; i < 3; i++)
	        {
		        if (special.Length > i)
		        {
			        var skillViewConfig = SkillConfigCategory.Instance.GetView(special[i]);
			        self.sp[i + 1].text = DescHelper.GetSkillDesc(skillViewConfig.Desc, special[i], specialVals[i]);
			        self.sp[i + 1].gameObject.Display(true);
		        }
		        else
		        {
			        self.sp[i + 1].gameObject.Display(false);
		        }
	        }   
        }
        
        public static void OnRemove(this UIEquipAttrContComponent self)
        {
            
        }
	}
}
