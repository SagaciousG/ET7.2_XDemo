
namespace ET.Client
{
	[FriendOf(typeof(UIEquipItemComponent))]
	public static class UIEquipItemComponentSystem
	{
		public static void OnAwake(this UIEquipItemComponent self)
        {
        }

        public static void OnCreate(this UIEquipItemComponent self)
        {
            
        }
        
        public static async void SetData(this UIEquipItemComponent self, params object[] args)
        {
	        var hole = (int)args[0];
	        var unit = await UnitHelper.GetMyUnitFromCurrentScene(self.DomainScene());
		    self.icon.Skin = StringMapConfigCategory.Instance.GetVal(StringMapType.EquipHole, hole);
	        var armsComponent = unit.GetProfession().GetComponent<ArmsComponent>();
	        if (armsComponent.HoleItem.TryGetValue((EquipmentHole)hole, out var armsItem))
	        {
		        self.bagItemUI.SetData(new UIBagItemArgs()
		        {
			        Id = armsItem.GetBagItem().ItemID,
			        Num = 1,
			        ShowState = UIBagItemShowState.IsEquipped,
			        UID = armsItem.Id,
		        });
		        self.bagItem.gameObject.Display(true);
	        }
	        else
	        {
		        self.bagItem.gameObject.Display(false);
	        }
        }
        
        public static void OnRemove(this UIEquipItemComponent self)
        {
            
        }
	}
}
