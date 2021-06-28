using Newtonsoft.Json;
using RuleEngineMicrosoft.Model;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RuleEngineMicrosoft
{
    class Program
    {


        static void Main(string[] args)
        {
            RunRule().Wait();
        }

        [Obsolete]
        private static async System.Threading.Tasks.Task RunRule()
        {
            var lstWorkflows = JsonConvert.DeserializeObject<WorkflowRules[]>(File.
                                ReadAllText(Directory.GetParent(Environment.CurrentDirectory).
                                Parent.Parent.FullName + "\\Rule.json"));
            var re = new RulesEngine.RulesEngine(lstWorkflows, null);
            var input = new PumpStationConfiguration();

            var isTrue = (input.StationType == "Vertical Turbine"
                            || input.StationType == "Vertical Turbine Quick Ship")
                            && input.QTY > 0
                            && (input.PumpConfiguration.Any(r => r.PumpDescription == "Water Feature" && r.VFD == "None"));

            foreach (var workflow in lstWorkflows)
            {
                var ruleParameter = new RuleParameter("PumpStationConfiguration", input);
                var resultList = await re.ExecuteAllRulesAsync(workflow.WorkflowName, ruleParameter);

                //Check success for rule
                foreach (var result in resultList.Where(r => r.IsSuccess).ToList())
                {
                    //Console.WriteLine($"Rule - {result.Rule.RuleName}, IsSuccess - {result.IsSuccess}, {errorMessage} {Environment.NewLine}");
                    Console.WriteLine($"{result.Rule.ErrorMessage}");
                }
            }

            Console.ReadLine();
        }
    }
}
