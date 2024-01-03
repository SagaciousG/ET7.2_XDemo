using System;
using System.Collections.Generic;

namespace ET
{
    public static class SkillConfigEx
    {
        public static SkillBaseConfig GetBase(this SkillConfig self, int lv)
        {
            return SkillBaseConfigCategory.Instance.GetByGroupLv(self.Group, lv);
        }
        
        public static int LvConsume(this SkillConfig self, int from, int to)
        {
            var val = 0;
            for (int i = from; i < to; i++)
            {
                var cfg = SkillBaseConfigCategory.Instance.GetByGroupLv(self.Group, i);
                if (cfg == null)
                    throw new Exception($"SkillBase 缺少配置 group={self.Group} lv = {i}");
                val += cfg.TakeSP;
            }

            return val;
        }
    }

    public partial class SkillConfig
    {
        public SkillViewConfig ViewConfig => SkillViewConfigCategory.Instance.Get(ViewID);
    }
    public partial class SkillConfigCategory
    {
        public SkillViewConfig GetView(int id)
        {
            return SkillViewConfigCategory.Instance.Get(Get(id).ViewID);
        }

        public SkillBaseConfig GetBase(int id, int lv)
        {
            return SkillBaseConfigCategory.Instance.GetByGroupLv(id, lv);
        }
    }
}