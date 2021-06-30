using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter
{
    public class LambdaToJsonConverter
    {

        public static void Final(Root r, List<Criteria> lstCriteria, string v)
        {
            bool criteriaEnd = false;
            lstCriteria = r != null ? r.criteria : lstCriteria;
            string[] ss = v.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ss.Length; i++)
            {
                int a = 0;
                while (ss[i][a] == '(')
                {
                    a++;
                    if (lstCriteria.Count <= 0)
                    {
                        lstCriteria.Add(new Criteria()); // new List<Criteria>();
                        lstCriteria[0].criteria = new List<Criteria>();
                    }
                    lstCriteria[0].condition = "||";
                    Final(null, lstCriteria[0].criteria, v.Substring(a, v.Length - a));
                    i++;
                    break;
                }
                string[] conditons = ss[i].Split("=");
                Criteria c = new Criteria() { field = conditons[0], @operator = "=", value = conditons[1].Replace(")", "") };
                lstCriteria.Add(c);
                if (r != null)
                    r.condition = "||";
                else
                    lstCriteria[0].condition = "||";
                criteriaEnd = ss[i].Contains(")");
                if (criteriaEnd)
                    break;
            }
        }
        public static Criteria Convert(Criteria c, string s)
        {
            s = s.Trim();
            int i = 0;
            while (s[i] == '(')
            {
                i++;
            }
            c = c ?? new Criteria();
            string[] ss = s.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            c.condition = "||";
            GetCriteriaObject(c.criteria, s.Substring(i, s.Length - i));
            return c;
        }

        private static void GetCriteriaObject(List<Criteria> critera, string v)
        {
            bool criteriaEnd = false;
            critera = critera ?? new List<Criteria>();
            string[] ss = v.Split(new string[] { "||" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ss.Length; i++)
            {
                if (criteriaEnd)
                {
                    //critera[0].criteria = new List<Criteria>();
                    //critera[0].condition = "||";
                    string s = "";
                    for (int a = i; a <= ss.Length - 1; a++)
                    {
                        s += ss[a] + " || ";
                    }
                    if (s.Trim() != "||")
                    {
                        GetCriteriaObject(critera, s);
                    }
                    criteriaEnd = false;
                }
                else
                {
                    int a = 0;
                    while (ss[i][a] == '(')
                    {
                        a++;
                        // iterate until char != (
                        critera[0].criteria = new List<Criteria>();
                        critera[0].condition = "||";
                        GetCriteriaObject(critera[0].criteria, v.Substring(a, v.Length - a));
                    }
                    string[] conditons = ss[i].Split("=");
                    Criteria c = new Criteria() { field = conditons[0], @operator = "=", value = conditons[1].Replace(")", "") };
                    critera.Add(c);
                    criteriaEnd = ss[i].Contains(")");
                }
            }

        }
    }
}
