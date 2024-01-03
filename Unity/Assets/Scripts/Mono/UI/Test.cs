using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ET;
using UnityEngine;
using UnityEngine.AI;

namespace ETEditor
{
    [ExecuteAlways]
    public class Test : MonoBehaviour
    {
        private void OnEnable()
        {
            GetSkillDesc("{{[[lv]] * [[lv]] + 1}}", 1);
        }

        [StaticField]
        private static Regex _formula = new Regex(@"\{\{([\[\]a-z0-9\(\)\ \+\-\*\/\%]+)\}\}");

        [StaticField]
        private static Regex _keyWord = new Regex(@"\[\[([a-z0-9]+)\]\]");
        
        public static string GetSkillDesc(string sentence, int skillId)
        {
            if (_formula.IsMatch(sentence))
            {
                var formula = _formula.Matches(sentence);
                foreach (Match m in formula)
                {
                    for (int i = 1; i < m.Groups.Count; i++)
                    {
                        var group = m.Groups[i];
                        var key = _keyWord.Matches(group.Value);
                        foreach (Match m2 in key)
                        {
                            for (var j = 1; j < m2.Groups.Count; j++)
                            {
                                var keyGroup = m2.Groups[j];
                                switch (keyGroup.Value)
                                {
                                    case "lv":
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return "";
        }
        
    }
}