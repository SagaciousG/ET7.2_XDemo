
namespace ET.Client
{
    [FriendOf(typeof(SelectRoleCellComponent))]
	public static class SelectRoleCellComponentSystem
	{
        public static void OnAwake(this SelectRoleCellComponent self)
        {
	        self.clickArea.OnClick(self.OnClick);
        }

        public static async void OnClick(this SelectRoleCellComponent self)
        {
	        if (self.Unit == null)
	        {
		        var ui = await UIHelper.Create(UIType.UICreateRole, self.DomainScene(), UILayer.Mid);
		        await ui.WaitForClose();
		        self.GetParent<UI>().GetParent<UI>().SetData();
	        }
	        else
	        {
		        var selectRoleComponent = self.GetParent<UI>().GetParent<UI>().GetComponent<UISelectRoleComponent>();
		        selectRoleComponent.SelectedUnit = self.Unit;
		        selectRoleComponent.RefreshRoles();
	        }
        }
        
        public static void OnCreate(this SelectRoleCellComponent self)
        {
	        self.name.text = "点击此处创建角色";
        }
        
        public static void SetData(this SelectRoleCellComponent self, params object[] args)
        {
	        if (args is { Length: > 0 })
	        {
		        var data = (SimpleUnit) args[0];
		        self.name.text = $"{data.Name} Lv.{data.Level}";
		        self.role.LoadModel(UnitShowConfigCategory.Instance.Get(data.UnitShow)).Coroutine();
		        self.Unit = data;
	        }
	        else
	        {
		        self.name.text = "点击此处创建角色";
	        }
	        var selectRoleComponent = self.GetParent<UI>().GetParent<UI>().GetComponent<UISelectRoleComponent>();
	        self.select.Display(self.Unit == selectRoleComponent.SelectedUnit);
        }
        
        public static void OnRemove(this SelectRoleCellComponent self)
        {
            
        }
	}
}
