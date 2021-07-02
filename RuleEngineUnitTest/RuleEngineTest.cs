using Newtonsoft.Json;
using RuleEngineUnitTest.Model;
using RulesEngine.Extensions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace RuleEngineUnitTest
{

    public class RuleEngineTest
    {
        [Fact]
        public async void SCCRCheck()
        {
            var input = new ProductFamily();

            RulesEngine.RulesEngine re = GetRuleEngineObj();
            var ruleParameter = new RuleParameter("PumpStationConfiguration", input);

            var resultList = await re.ExecuteAllRulesAsync("SCCRCheck", ruleParameter);

            //input.PumpConfiguration.ForEach(x=>x.QTY = 5);

            Assert.True(resultList.All(r => r.IsSuccess == false));

        }

        [Fact]
        public async void CheckforNonRegulatingPumpsonVFDs()
        {
            var input = new List<PumpConfigurations>();

            input = new List<PumpConfigurations>()
            { new PumpConfigurations() { PumpDescription = "Water Feature", VFD = "None" },
                  new PumpConfigurations() { PumpDescription = "Transfer", VFD = "None" }
            };

            RulesEngine.RulesEngine re = GetRuleEngineObj();
            List<List<RuleResultTree>> lstRuleResultTree = new List<List<RuleResultTree>>();
            foreach (var item in input)
            {

                var ruleParameter = new RuleParameter("PumpConfiguration", item);

                lstRuleResultTree.Add(await re.ExecuteAllRulesAsync("Check for Non Regulating Pumps on VFDs", ruleParameter));
            }
            Assert.Contains(lstRuleResultTree, r => r.Any(r => r.IsSuccess));

        }

        [Fact]
        public async void GenerateWarningReport()
        {
            var input = new ProductFamily();

            RulesEngine.RulesEngine re = GetRuleEngineObj("\\RuleWithSingleWorkflow.json");
            var ruleParameter = new RuleParameter("PumpStationConfiguration", input);
            var resultList = await re.ExecuteAllRulesAsync("GenerateWarningReport", ruleParameter);

            foreach (var ruleResult in resultList)
            {
                if (ruleResult.ActionResult != null)
                {
                    Console.WriteLine(ruleResult.ActionResult.Output); //ActionResult.Output contains the evaluated value of the action
                }
            }

            resultList.OnSuccess((Context) =>
            {
                var a = $"Discount offered is {Context} % over MRP.";
            });

            foreach (var result in resultList.Where(r => r.IsSuccess).ToList())
            {
                Console.WriteLine($"{result.Rule.ErrorMessage}");
            }

            // all validation rules failed mean no error in configuration.
            Assert.True(resultList[0].IsSuccess == false);

        }

        [Fact]
        public async void CheckforPumpRatchets190()
        {
            var input = new ProductFamily();

            //var isTrue = (input.StationType == "Vertical Turbine"
            //                || input.StationType == "Vertical Turbine Quick Ship")
            //                && input.QTY > 0
            //                && input.PumpConfiguration.Select(r=>r.)
            //                && (input.PumpConfiguration.Any(r => r.PumpDescription == "Water Feature" && r.VFD == "None"));


            RulesEngine.RulesEngine re = GetRuleEngineObj();
            var ruleParameter = new RuleParameter("PumpStationConfiguration", input);

            Rule rule = new Rule();
            rule.Operator = "AND";
            rule.Rules = new List<Rule>() { new Rule() { } };

            var resultList = await re.ExecuteAllRulesAsync("Check for Pump Ratchets 190", ruleParameter);


            Assert.True(resultList.All(r => r.IsSuccess));

        }

        private RulesEngine.RulesEngine GetRuleEngineObj(string fileName = "\\Rule.json")
        {
            var lstWorkflows = JsonConvert.DeserializeObject<WorkflowRules[]>(File.
                                ReadAllText(Directory.GetParent(Environment.CurrentDirectory).
                                Parent.Parent.FullName + fileName));

            var re = new RulesEngine.RulesEngine(lstWorkflows, null);
            return re;

        }
    }
}
