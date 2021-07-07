using Newtonsoft.Json;
using RuleEngineUnitTest.Model;
using RulesEngine.Extensions;
using RulesEngine.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace RuleEngineUnitTest
{

    public class RuleEngineTest
    {
        //[Fact]
        //public async void SCCRCheck()
        //{
        //    var input = new ProductFamily();

        //    RulesEngine.RulesEngine re = GetRuleEngineObj();
        //    var ruleParameter = new RuleParameter("PumpStationConfiguration", input);

        //    var resultList = await re.ExecuteAllRulesAsync("SCCRCheck", ruleParameter);

        //    //input.PumpConfiguration.ForEach(x=>x.QTY = 5);

        //    Assert.True(resultList.All(r => r.IsSuccess == false));

        //}

        //[Fact]
        //public async void CheckforNonRegulatingPumpsonVFDs()
        //{
        //    var input = new List<PumpConfigurations>();

        //    input = new List<PumpConfigurations>()
        //    { new PumpConfigurations() { PumpDescription = "Water Feature", VFD = "None" },
        //          new PumpConfigurations() { PumpDescription = "Transfer", VFD = "None" }
        //    };

        //    RulesEngine.RulesEngine re = GetRuleEngineObj();
        //    List<List<RuleResultTree>> lstRuleResultTree = new List<List<RuleResultTree>>();
        //    foreach (var item in input)
        //    {

        //        var ruleParameter = new RuleParameter("PumpConfiguration", item);

        //        lstRuleResultTree.Add(await re.ExecuteAllRulesAsync("Check for Non Regulating Pumps on VFDs", ruleParameter));
        //    }
        //    Assert.Contains(lstRuleResultTree, r => r.Any(r => r.IsSuccess));

        //}

        //[Fact]
        //public async void GenerateWarningReport()
        //{
        //    var input = new ProductFamily();

        //    RulesEngine.RulesEngine re = GetRuleEngineObj("\\RuleWithSingleWorkflow.json");
        //    var ruleParameter = new RuleParameter("PumpStationConfiguration", input);
        //    var resultList = await re.ExecuteAllRulesAsync("GenerateWarningReport", ruleParameter);

        //    foreach (var ruleResult in resultList)
        //    {
        //        if (ruleResult.ActionResult != null)
        //        {
        //            Console.WriteLine(ruleResult.ActionResult.Output); //ActionResult.Output contains the evaluated value of the action
        //        }
        //    }

        //    resultList.OnSuccess((Context) =>
        //    {
        //        var a = $"Discount offered is {Context} % over MRP.";
        //    });

        //    foreach (var result in resultList.Where(r => r.IsSuccess).ToList())
        //    {
        //        Console.WriteLine($"{result.Rule.ErrorMessage}");
        //    }

        //    // all validation rules failed mean no error in configuration.
        //    Assert.True(resultList[0].IsSuccess == false);

        //}

        //[Fact]
        //public async void CheckforPumpRatchets190()
        //{
        //    var input = new ProductFamily();
        //    // var a = PumpStationConfiguration.OptionsLists.Where(r => r.OptionNumber.StartsWith("005")).Select(r => r).Sum(r => r.QTY);

        //    //var isTrue = (input.StationType == "Vertical Turbine"
        //    //                || input.StationType == "Vertical Turbine Quick Ship")
        //    //                && input.QTY > 0
        //    //                && input.PumpConfiguration.Select(r=>r.)
        //    //                && (input.PumpConfiguration.Any(r => r.PumpDescription == "Water Feature" && r.VFD == "None"));


        //    RulesEngine.RulesEngine re = GetRuleEngineObj();
        //    var ruleParameter = new RuleParameter("PumpStationConfiguration", input);

        //    Rule rule = new Rule();
        //    rule.Operator = "AND";
        //    rule.Rules = new List<Rule>() { new Rule() { } };

        //    var resultList = await re.ExecuteAllRulesAsync("Check for Pump Ratchets 190", ruleParameter);


        //    Assert.True(resultList.All(r => r.IsSuccess));

        //}

        #region Moderate Cases
        [Fact]
        public async void CheckforAgencyType005()
        {
            var input = new ProductFamily();
            input.OptionsLists = new List<OptionsList>()
            {
                new OptionsList(){ OptionNumber = "005", QTY = 0 },
                 new OptionsList(){ OptionNumber = "005", QTY = 0 }
            };

            RulesEngine.RulesEngine re = GetRuleEngineObj("ModerateScenarios.json");
            var ruleParameter = new RuleParameter[] { new RuleParameter("ProductFamily", input) };

            var resultList = await re.ExecuteActionWorkflowAsync("ModerateCases", "CheckforAgencyType005", ruleParameter);

            Assert.True(resultList.Results.All(r => r.IsSuccess));

        }

        [Fact]
        public async void CheckforVTDischargeHeadErrors140()
        {
            var input = new ProductFamily();
            input.VTPumpsInOrder = 1;

            input.OptionsLists = new List<OptionsList>()
            {
                new OptionsList(){ OptionNumber = "140", QTY = 0 },
                 new OptionsList(){ OptionNumber = "140", QTY = 0 }
            };

            RulesEngine.RulesEngine re = GetRuleEngineObj("ModerateScenarios.json");
            var ruleParameter = new RuleParameter[] { new RuleParameter("ProductFamily", input) };

            var resultList = await re.ExecuteActionWorkflowAsync("ModerateCases", "CheckforVTDischargeHeadErrors140", ruleParameter);

            Assert.True(resultList.Results.All(r => r.IsSuccess));

        }
        #endregion

        #region Complex
        [Fact]
        public async void CheckforVTPumpSelectionErrors12101()
        {
            var ProductFamily = new ProductFamily() { Hz = 50 };

            ProductFamily.OptionsLists = new List<OptionsList>()
            {
                new OptionsList(){ Category = "150", OptionNumber = "00032", QTY = 1 },
                 new OptionsList(){ Category = "150", OptionNumber = "00045", QTY = 1 }
            };

            ProductFamily.PumpConfigurations = new List<PumpConfiguration>()
            {
                new PumpConfiguration(){HP = 60,PumpDescription = "Water Feature" },
                new PumpConfiguration(){HP = 70,PumpDescription = "Transfer" }
            };

            ProductFamily.VTPumpsInGrid = ProductFamily.VTPumpsInOrder = 2;

            //List<int> intlst = input.OptionsLists.Select(r.OptionNumber.Substring(r.OptionNumber.Length - 2, 2)

            //var a = ProductFamily.OptionsLists.Where(r => r.Category == "150").Where(r => int.Parse(r.OptionNumber.Substring(r.OptionNumber.Length - 2, 2)) > 24).Select(r => r).Sum(r => r.QTY);
            //var b = ProductFamily.PumpConfigurations.Any(r => r.PumpDescription == "50Hz");

            RulesEngine.RulesEngine re = GetRuleEngineObj("ComplexScenarios.json");
            var ruleParameter = new RuleParameter[] { new RuleParameter("ProductFamily", ProductFamily) };

            var resultList = await re.ExecuteAllRulesAsync("CheckforVTPumpColumnErrors150", ruleParameter);
            List<string> lsterr = new List<string>();

            //foreach (var item in resultList)
            //{
            //    MatchCollection matchCollection = Regex.Matches(item.ExceptionMessage, @"\[(.*?)\]");
            //    //2
            //    int count = matchCollection.Count;
            //    for (int i = 0; i < matchCollection.Count; i++)
            //    {
            //        string[] values = matchCollection[i].Value.Split(',');
            //        for (int a = 0; a < values.Length; a++)
            //        {
            //            string str = Regex.Matches(item.ExceptionMessage, @"\{(.*?)\}")[0].Value.ToString();
            //            item.ExceptionMessage = item.ExceptionMessage.Replace(str, values[a]);
            //        }
            //        //var str = Regex.Replace(item.ExceptionMessage, @"\[(.*?)\]",)
            //    }
            //}

            Assert.True(resultList.All(r => r.IsSuccess));

        }


        [Fact]
        public async void CheckforPumpRatchets190()
        {
            var input = new ProductFamily();
            input.OptionsLists = new List<OptionsList>()
            {
                new OptionsList(){ OptionNumber = "005-0000002", QTY = 1 },
                 new OptionsList(){ OptionNumber = "190-0000001", QTY = 2 },
                 new OptionsList(){ OptionNumber = "190-0000002", QTY = 3 },
                 new OptionsList(){ OptionNumber = "190-0000003", QTY = 4 }
            };

            input.PumpConfigurations = new List<PumpConfiguration>()
            {
                new PumpConfiguration(){HP = 75,PumpDescription = "Transfer", Qty = 1 },
                 new PumpConfiguration(){HP = 100,PumpDescription = "Transfer", Qty = 2 },
                  new PumpConfiguration(){HP = 125,PumpDescription = "Transfer", Qty = 3 }
            };

            // input.OptionsLists.Where(r => r.OptionNumber.StartsWith("005")).Sum(r => r.QTY);

            //input.PumpConfigurations.Where(r => r.HP >= 75).Sum(r => r.Qty);

            RulesEngine.RulesEngine re = GetRuleEngineObj("CheckforPumpRatchets190.json");
            var ruleParameter = new RuleParameter[] { new RuleParameter("ProductFamily", input) };
            var resultList = await re.ExecuteAllRulesAsync("CheckforPumpRatchets190", ruleParameter);
            Assert.True(true);

        }
        #endregion

        #region Private Functions
        private RulesEngine.RulesEngine GetRuleEngineObj(string fileName = "\\Rule.json")
        {
            var lstWorkflows = JsonConvert.DeserializeObject<WorkflowRules[]>(File.
                                ReadAllText(Directory.GetParent(Environment.CurrentDirectory).
                                Parent.Parent.FullName + "\\RuleEngineJson\\" + fileName));

            var re = new RulesEngine.RulesEngine(lstWorkflows, null);
            return re;

        }
        #endregion
    }
}
