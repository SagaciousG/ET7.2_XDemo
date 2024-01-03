
using UnityEngine;
using ET;

namespace ET.Client
{
	[FriendOf(typeof(UISkillComponent))]
	public static class UISkillComponentSystem
	{
        public static void OnAwake(this UISkillComponent self)
        {
	        self.skillList.OnData += self.OnListData;
	        self.skillList.OnSelectedViewRefresh += self.OnListSelected;
        }

        private static void OnListSelected(this UISkillComponent self, RectTransform cell, bool selected, object data,
	        int index)
        {
	        // var rc = cell.GetComponent<UIReferenceCollector>();
	        // var select = rc.Get<XImage>("select");
	        // select.Display(selected);
	        // if (selected)
	        // {
		       //  self.RefreshShow((Skill)data);
	        // }
        }
        
        private static void OnListData(this UISkillComponent self, int index, RectTransform cell, object data)
        {
	        // var rc = cell.GetComponent<UIReferenceCollector>();
	        // var title = rc.Get<XText>("title");
	        // var skill = (Skill)data;
	        // title.text = $"{skill.ViewConfig.Name}";
        }

        public static void OnCreate(this UISkillComponent self)
        {
            SignalHelper.Add<int>(self.DomainScene(), SignalKey.UI_SkillLevelChanged_ID, self, self.OnSkillChange);
        }

        private static async void OnSkillChange(this UISkillComponent self, int id)
        {
	        // var myUnit = await UnitHelper.GetMyUnitFromCurrentScene(self.DomainScene());
	        // var skillComponent = myUnit.GetComponent<SkillComponent>();
	        // self.skillList.SetData(skillComponent.AllSkill());
        }
        
        public static async void SetData(this UISkillComponent self, params object[] args)
        {
	        // var myUnit = await UnitHelper.GetMyUnitFromCurrentScene(self.DomainScene());
	        // var skillComponent = myUnit.GetComponent<SkillComponent>();
	        // self.skillList.SetData(skillComponent.AllSkill());
	        // self.skillList.SetSelectIndex(0);
	        // UIHelper.CreateUITopBack(self.GetParent<UI>(), "技能").Coroutine();
        }

        // private static void RefreshShow(this UISkillComponent self, Skill skill)
        // {
	       //  self.name.text = $"{skill.ViewConfig.Name} LV.{skill.Level}";
	       //  self.upperNum.text = skill.Config.MaxLevel.ToString();
	       //  self.desc.text = skill.ViewConfig.Desc;
	       //  if (skill.Level + 1 <= skill.Config.MaxLevel)
	       //  {
		      //   self.descNext.text = skill.ViewConfig.Desc;
	       //  }
	       //  else
	       //  {
		      //   self.descNext.text = "已达到最大等级";
	       //  }
        // }
        
        public static void OnRemove(this UISkillComponent self)
        {
	        SignalHelper.Remove<int>(self.DomainScene(), SignalKey.UI_SkillLevelChanged_ID, self, self.OnSkillChange);
        }
	}
}
