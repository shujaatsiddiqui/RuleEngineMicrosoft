using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineUnitTest.Model
{
    public class PumpStationConfiguration
    {
        public int SCCR = 30;
        public int FLA = 600;
        public int QTY = 0;
        public string StationType = "Vertical Turbine";
        public List<PumpConfigurations> PumpConfiguration { get; set; }
            = new List<PumpConfigurations>()
            { new PumpConfigurations() { PumpDescription = "Water Feature", VFD = "None" },
                  new PumpConfigurations() { PumpDescription = "Transfer", VFD = "None" }
            };
    }

    public class PumpConfigurations
    {
        public string PumpDescription { get; internal set; }
        public string VFD { get; internal set; }
    }



}
