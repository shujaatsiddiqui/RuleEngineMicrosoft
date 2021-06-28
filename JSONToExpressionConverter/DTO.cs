using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Order
    {
        public DateTime TheDate { get; set; }
        public string Customer { get; set; }
        public decimal Amount { get; set; }
        public bool Discount { get; set; }
    }

    public class Root
    {
        public List<Rules> rules { get; set; }
        public string condition { get; set; }
    }

    public class Rules
    {

        public string field { get; set; }
        public string @operator { get; set; }

        public string value { get; set; }

        public string condition { get; set; }

        public List<Rules> rules { get; set; }
    }


    //public class Condition
    //{
    //    public string Field { get; set; }
    //    public string Operator { get; set; }
    //    public string Value { get; set; }
    //}
    //public class AND
    //{
    //    public List<Condition> Conditions { get; set; }
    //    public AND ANDOBJ { get; set; }

    //    public OR OROBJ { get; set; }
    //}

    //public class OR
    //{
    //    public List<Condition> Conditions { get; set; }
    //    public AND ANDOBJ { get; set; }

    //    public OR OROBJ { get; set; }
    //}

    //public class Root
    //{
    //    public AND AND { get; set; }

    //    public OR OR { get; set; }


    //}




}
