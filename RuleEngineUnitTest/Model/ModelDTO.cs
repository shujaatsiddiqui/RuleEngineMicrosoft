using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RuleEngineUnitTest.Model
{
    /// <summary>
    /// Workflow Name: Generate Warning - ProductFamily
    /// Workflow Name: Auto Check - ProductFamily
    /// </summary>
    public class ProductFamily
    {
        public int SCCR = 30;
        public int FLA = 600;
        public int QTY = 0;
        public string StationType = "Vertical Turbine";
    }

    /// <summary>
    /// Workflow Name: Generate Warning - Pump Station
    /// Workflow Name: Auto Check - Pump Station
    /// </summary>
    public class PumpConfigurations
    {
        public PumpConfigurations()
        {
            ProductFamily = new ProductFamily();
        }
        public string PumpDescription { get; internal set; }
        public string VFD { get; internal set; }
        public ProductFamily ProductFamily { get; set; }

    }

    /// <summary>
    /// Workflow Name: Generate Warning - OptionsList
    /// Workflow Name: Auto Check - OptionsList
    /// can be a problem if we have to check pump configuration along with option list as well.
    /// </summary>
    public class OptionsList
    {
        public OptionsList()
        {
            ProductFamily = new ProductFamily();
        }
        public string OptionNumber { get; set; }
        public string Category { get; set; }
        public ProductFamily ProductFamily { get; set; }

    }

    public class OptionsList2
    {
        public OptionsList2()
        {
            ProductFamily = new ProductFamily();
        }
        public List<PumpConfigurations> PumpConfigurations { get; set; }
        public string OptionNumber { get; set; }
        public string Category { get; set; }
        public ProductFamily ProductFamily { get; set; }

    }



}
