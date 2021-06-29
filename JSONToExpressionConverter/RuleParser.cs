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
        static List<string> lstSpecialOperator = new List<string>() { "Any", "All" };

        /// <summary>
        /// https://stackoverflow.com/questions/13911450/how-to-traverse-a-tree-with-multiple-branches
        /// </summary>
        /// <param name="root"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public static string GetRuleInLambaFormat(Root root = null, Criteria rules = null, string parentCondition = "")
        {
            string s = "";

            if (rules == null)
                rules = new Criteria();

            rules.criteria = root != null ? root.criteria : rules.criteria;
            rules.condition = root != null ? root.condition : rules.condition;
            if (rules.criteria == null || rules.criteria.Count == 0) //get the condition
            {
                if (!lstSpecialOperator.Any(r => r.Equals(rules.@operator, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var condition = root != null ? root.condition : rules.condition;
                    var value = int.TryParse(rules.value, out int n) ? rules.value : $"'{rules.value}'";
                    if (parentCondition == "")
                        s = $" {rules.field} {rules.@operator} {value} {condition} "; //change this to your real getCondition function.
                    else
                        s = $" r.{rules.field} {rules.@operator} {value} {condition} ";
                    Console.WriteLine(s);
                    return s;
                }
                else
                {
                    s = LoopThroughRules(rules.values, s, "Any");
                    var prefix = parentCondition == "" ? "" : "r.";
                    return $"({prefix}{rules.field}.{rules.@operator}(r=>{s}))";
                }

            }
            else
            {
                s = LoopThroughRules(rules, s);
                return string.Format("({0})", s); //group sub conditions
            }
        }

        private static string LoopThroughRules(Criteria rules, string s = "", string parentCondition = "")
        {
            for (int i = 0; i < rules.criteria?.Count; i++)
            {
                if (rules.criteria[i] is Criteria)
                {
                    if (i == 0) // first node doesn't need the operator 
                    {
                        s += GetRuleInLambaFormat(null, rules.criteria[i] as Criteria, parentCondition);
                    }
                    else // only needs operator in between
                    {
                        //s += (string.IsNullOrEmpty(GetRuleInLambaFormat(null, rules.rules[i] as Rules).Trim()) ? "" : (" " + rules.condition + " " + GetRuleInLambaFormat(null, rules.rules[i] as Rules))); // only get real treeviewitem, not the one with two buttons and an empty header; change t.Header to your real getOperator function.
                        s += " " + rules.condition + " " + GetRuleInLambaFormat(null, rules.criteria[i] as Criteria, parentCondition);
                    }
                }
            }

            return s;
        }
    }
}
