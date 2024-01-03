
using System.Linq;
using TMPro;
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UICreateRoleComponent))]
	public static class UICreateRoleComponentSystem
	{
        public static void OnAwake(this UICreateRoleComponent self)
        {
	        self.armList.OnData += self.OnArmsData;
	        self.armList.OnSelectedViewRefresh += self.OnArmsSelected;
	        self.opList.OnData += self.OnOpData;
	        self.opList.OnSelectedViewRefresh += self.OnOpSelected;
	        self.colorList.OnData += self.OnColorData;
	        self.colorList.OnSelectedViewRefresh += self.OnColorSelected;
	        self.modelList.OnData += self.OnModelData;
	        self.modelList.OnSelectedViewRefresh += self.OnModelSelect;
	        
	        self.create.OnClick(self.OnCreateRole);
	        self.getName.OnClick(self.RequireName);
	        self.inputName.onSelect.AddListener(self.OnInputNameSelect);
        }
        
        private static void OnModelSelect(this UICreateRoleComponent self, RectTransform arg1, bool arg2, object arg3, int arg4)
        {
	        var referenceCollector = arg1.GetComponent<UIReferenceCollector>();
	        var select = referenceCollector.Get<XImage>("select");
	        select.Display(arg2);
	        var data = (UnitShowConfig)arg3;
	        if (arg2)
	        {
		        self.role.LoadModel(data).Coroutine();
	        }
        }

        private static void OnModelData(this UICreateRoleComponent self, int arg1, RectTransform arg2, object arg3)
        {
	        var referenceCollector = arg2.GetComponent<UIReferenceCollector>();
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        var data = (UnitShowConfig)arg3;
	        title.text = data.Name;
	        var have = CreateRoleConfigCategory.Instance.TryGetByGroup(data.Group, out var all);
	        if (have)
	        {
		        self.ArmsGroup = new();
		        foreach (var item in all)
		        {
			        self.ArmsGroup.Add(item.ArmType, item);
		        }
		        self.armList.SetData(self.ArmsGroup.Keys.ToArray());
		        self.armList.gameObject.Display(true);
	        }
	        else
	        {
		        self.armList.gameObject.Display(false);
		        self.opList.gameObject.Display(false);
		        self.colorList.gameObject.Display(false);
	        }
        }

        private static void OnInputNameSelect(this UICreateRoleComponent self, string val)
        {
	        self.errorText.text = null;
        }
        private static async void OnCreateRole(this UICreateRoleComponent self)
        {
	        var data = (UnitShowConfig) self.modelList.SelectedData;
	        var error = await LobbyHelper.CreateRole(self.ClientScene(), data.Id, self.inputName.text);
	        if (error > 0)
	        {
		        if (error == ErrorCode.ERR_UnitNameUsed)
					self.errorText.text = "名称已存在";
		        return;
	        }
	        UIHelper.Remove(UIType.UICreateRole, self.DomainScene()).Coroutine();
        }

        private static void OnColorSelected(this UICreateRoleComponent self, RectTransform arg1, bool arg2, object arg3,
	        int arg4)
        {
	        
        }

        private static void OnOpSelected(this UICreateRoleComponent self, RectTransform arg1, bool arg2, object arg3,
	        int arg4)
        {
	        if (arg2)
	        {
		        var type = (string)arg3;
		        var list = self.OpGroup[type];
		        self.colorList.SetData(list);
		        self.colorList.SetSelectIndex(0);
	        }	
        }
        
        private static void OnColorData(this UICreateRoleComponent self, int arg1, RectTransform arg2, object arg3)
        {
	        var referenceCollector = arg2.GetComponent<UIReferenceCollector>();
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        var cfg = (CreateRoleConfig)arg3;
	        title.text = cfg.ColorName;
        }

        private static void OnOpData(this UICreateRoleComponent self, int arg1, RectTransform arg2, object arg3)
        {
	        var referenceCollector = arg2.GetComponent<UIReferenceCollector>();
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        title.text = $"造型{arg1}";
        }

        private static void OnArmsSelected(this UICreateRoleComponent self, RectTransform arg1, bool arg2, object arg3, int arg4)
        {
	        var referenceCollector = arg1.GetComponent<UIReferenceCollector>();
	        var select = referenceCollector.Get<XImage>("select");
	        select.Display(arg2);
	        if (!arg2)
	        {
		        return;
	        }
	        var list = self.ArmsGroup[(int)arg3];
	        self.OpGroup = new();
	        var colorOnly = false;
	        foreach (var config in list)
	        {
		        if (config.OpType.IsNullOrEmpty())
		        {
			        colorOnly = true;
		        }
		        else
		        {
			        self.OpGroup.Add(config.OpType, config);
		        }
	        }

	        if (colorOnly && self.OpGroup.Count > 0)
	        {
		        Log.Error($"CreateRoleConfig 结构配置错误");
	        }

	        if (colorOnly)
	        {
		        self.opList.gameObject.Display(false);
		        self.colorList.SetData(list);
		        self.colorList.SetSelectIndex(0);
	        }
	        else
	        {
		        self.opList.SetData(self.OpGroup.Keys.ToArray());
		        self.opList.SetSelectIndex(0);
	        }
        }

        private static void OnArmsData(this UICreateRoleComponent self, int arg1, RectTransform arg2, object arg3)
        {
	        var referenceCollector = arg2.GetComponent<UIReferenceCollector>();
	        var bg = referenceCollector.Get<XImage>("bg");
	        var title = referenceCollector.Get<TextMeshProUGUI>("title");
	        var type = (UnitPartType)(int)arg3;
	        title.text = type.GetNameAttribute().Name;
        }

        private static async void RequireName(this UICreateRoleComponent self)
        {
	        self.errorText.text = "";
	        self.inputName.text = await LobbyHelper.RandomName(self.ClientScene());
        }
        
        public static void OnCreate(this UICreateRoleComponent self)
        {
	        UIHelper.CreateUITopBack(self.GetParent<UI>(), "创建角色").Coroutine();
        }
        
        public static void SetData(this UICreateRoleComponent self, params object[] args)
        {
	        var allRole = UnitShowConfigCategory.Instance.GetByGroup(1);
	        self.modelList.SetData(allRole);
	        self.modelList.SetSelectIndex(0);
	        
	        self.RequireName();
        }	
        
        public static void OnRemove(this UICreateRoleComponent self)
        {
            
        }
	}
}
