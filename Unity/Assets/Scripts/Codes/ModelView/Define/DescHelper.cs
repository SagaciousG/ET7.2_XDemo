using System.Text.RegularExpressions;

namespace ET.Client
{
    public static class DescHelper
    {
        [StaticField]
        private static Regex _formula = new Regex(@"\{\{([\[\]a-z0-9\(\)\ \+\-\*\/\%]+)\}\}");

        [StaticField]
        private static Regex _keyWord = new Regex(@"\[\[([a-z0-9]+)\]\]");
        
        public static string GetSkillDesc(string sentence, int skillId, int lv)
        {
            var res = sentence;
            if (_formula.IsMatch(sentence))
            {
                var formula = _formula.Matches(sentence);
                foreach (Match m in formula)
                {
                    var group = m.Groups[1];
                    var key = _keyWord.Matches(group.Value);
                    var formulaWord = group.Value;
                    foreach (Match m2 in key)
                    {
                        var keyGroup = m2.Groups[1];
                        switch (keyGroup.Value)
                        {
                            case "lv":
                            {
                                formulaWord = formulaWord.Replace(m2.Groups[0].Value, lv.ToString());
                                break;
                            }
                            case "keyword":
                            {
                                var baseConfig = SkillConfigCategory.Instance.GetBase(skillId, lv);
                                var words = GetKeyWordDesc(baseConfig.EntryWord, baseConfig.EntryWordArg);
                                formulaWord = formulaWord.Replace(m2.Groups[0].Value, words.Joint());
                                break;
                            }
                        }
                    }
                    
                    res = res.Replace(m.Groups[0].Value, MathHelper.ComputeStr(formulaWord));
                }
            }

            return res;
        }

        public static string[] GetKeyWordDesc(string words, string wordArgs)
        {
            var keys = words.ToIntArray(',');
            var vals = wordArgs.ToIntArray(',');
            var res = new string[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                var k = keys[i];
                var entryWordConfig = EntryWordConfigCategory.Instance.Get(k);
                var propertyConfig = PropertyConfigCategory.Instance.Get(entryWordConfig.Param1.ToInt32());
                if (entryWordConfig.Type == 0)
                    res[i] = $"{propertyConfig.Name}  {vals[i].ToSymbolString()}";
                else if (entryWordConfig.Type == 1)
                    res[i] = $"{propertyConfig.Name}  {(vals[i] / 100f).ToSymbolString("F1")}%";
                    
            }

            return res;
        }
    }
}