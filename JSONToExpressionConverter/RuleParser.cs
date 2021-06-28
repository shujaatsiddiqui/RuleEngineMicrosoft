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

            if (rules == null)
                rules = new Rules();

            rules.rules = root != null ? root.rules : rules.rules;
            rules.condition = root != null ? root.condition : rules.condition;
            if (rules.rules == null || rules.rules.Count == 0) //get the condition
            {
                var condition = root != null ? root.condition : rules.condition;
                var value = int.TryParse(rules.value, out int n) ? rules.value : $"'{rules.value}'";
                s = $" {rules.field} {rules.@operator} {value} {condition} "; //change this to your real getCondition function.
                Console.WriteLine(s);
                return s;

            }
            else
            {
                for (int i = 0; i < rules.rules?.Count; i++)
                {
                    if (rules.rules[i] is Rules)
                    {
                        if (i == 0) // first node doesn't need the operator 
                        {
                            s += GetRuleInLambaFormat(null, rules.rules[i] as Rules);
                        }
                        else // only needs operator in between
                        {
                            //s += (string.IsNullOrEmpty(GetRuleInLambaFormat(null, rules.rules[i] as Rules).Trim()) ? "" : (" " + rules.condition + " " + GetRuleInLambaFormat(null, rules.rules[i] as Rules))); // only get real treeviewitem, not the one with two buttons and an empty header; change t.Header to your real getOperator function.
                            s += " " + rules.condition + " " + GetRuleInLambaFormat(null, rules.rules[i] as Rules);
                        }
                    }
                }
                return string.Format("({0})", s); //group sub conditions
            }
        }
    }
}
