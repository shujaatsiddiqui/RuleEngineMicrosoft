using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter
{
    public class RuleParser
    {
        public static string GetRuleInLambaFormat(List<Rules> rules = null, string condition = "", string expression = "")
        {
            Root r = JsonConvert.DeserializeObject<Root>(File.ReadAllText(Program.GetFilePath("ComplexJson.json")));
            foreach (var item in r.rules)
            {
                if (item.condition == null)
                    expression += $"{item.field} {item.@operator} {item.value} {item.condition} ";
                else
                {
                    expression += GetRuleInLambaFormat(item.rules, item.condition);
                }
            }
            return expression;
        }

        public static string GetRuleInLambaFormat(Root root = null, Rules rules = null)
        {
            string s = "";
            if (rules.rules == null || rules.rules.Count == 0) //get the condition
            {
                return $"{rules.field} {rules.@operator} {rules.value} {rules.condition} "; //change this to your real getCondition function.
            }
            else
            {
                for (int i = 0; i < rules.rules?.Count; i++)
                {
                    if (rules.rules[i] is Rules) //Edit: only use treeviewitems not the button...
                    {
                        if (i == 0) // first note doesn't need the operator 
                        {
                            s += GetRuleInLambaFormat(null, rules.rules[i] as Rules);
                        }
                        else // only needs operator in between
                        {
                            s += (string.IsNullOrEmpty(GetRuleInLambaFormat(null, rules.rules[i] as Rules).Trim()) ? "" : (" " + rules.condition + " " + GetRuleInLambaFormat(null, rules.rules[i] as Rules))); // only get real treeviewitem, not the one with two buttons and an empty header; change t.Header to your real getOperator function.

                        }
                    }
                }
                return string.Format("({0})", s); //group sub conditions
            }
        }
    }
}
