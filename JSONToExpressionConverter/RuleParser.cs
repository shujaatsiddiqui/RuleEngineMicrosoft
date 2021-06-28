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
            foreach (var item in r.criteria)
            {
                if (item.condition == null)
                    expression += $"{item.field} {item.@operator} {item.value} {item.condition} ";
                else
                {
                    expression += GetRuleInLambaFormat(item.criteria, item.condition);
                }
            }
            return expression;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/13911450/how-to-traverse-a-tree-with-multiple-branches
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static string GetRuleInLambaFormat(Root root = null, Rules rules = null)
        {
            string s = "";

            if (rules == null)
                rules = new Rules();

            rules.criteria = root != null ? root.criteria : rules.criteria;
            rules.condition = root != null ? root.condition : rules.condition;
            if (rules.criteria == null || rules.criteria.Count == 0) //get the condition
            {
                var condition = root != null ? root.condition : rules.condition;
                var value = int.TryParse(rules.value, out int n) ? rules.value : $"'{rules.value}'";
                s = $" {rules.field} {rules.@operator} {value} {condition} "; //change this to your real getCondition function.
                Console.WriteLine(s);
                return s;

            }
            else
            {
                for (int i = 0; i < rules.criteria?.Count; i++)
                {
                    if (rules.criteria[i] is Rules)
                    {
                        if (i == 0) // first node doesn't need the operator 
                        {
                            s += GetRuleInLambaFormat(null, rules.criteria[i] as Rules);
                        }
                        else // only needs operator in between
                        {
                            //s += (string.IsNullOrEmpty(GetRuleInLambaFormat(null, rules.rules[i] as Rules).Trim()) ? "" : (" " + rules.condition + " " + GetRuleInLambaFormat(null, rules.rules[i] as Rules))); // only get real treeviewitem, not the one with two buttons and an empty header; change t.Header to your real getOperator function.
                            s += " " + rules.condition + " " + GetRuleInLambaFormat(null, rules.criteria[i] as Rules);
                        }
                    }
                }
                return string.Format("({0})", s); //group sub conditions
            }
        }
    }
}
