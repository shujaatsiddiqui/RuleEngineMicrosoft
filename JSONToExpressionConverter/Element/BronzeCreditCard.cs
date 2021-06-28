using JSONToExpressionConverter.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter.Element
{
    public class BronzeCreditCard : ICreditCard
    {
        public void accept(IOfferVisitor visitor)
        {
            visitor.VisitBronzeCreditCard(this);
        }
    }
}
