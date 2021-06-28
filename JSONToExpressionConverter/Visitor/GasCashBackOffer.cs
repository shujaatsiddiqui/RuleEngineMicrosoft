using JSONToExpressionConverter.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter.Visitor
{
    public class GasCashBackOffer : IOfferVisitor
    {
        public void VisitBronzeCreditCard(BronzeCreditCard bronze)
        {
            Console.WriteLine("10 % discount bronze credit card");
        }

        public void VisitGoldCreditCard(GoldCreditCard gold)
        {
            throw new NotImplementedException();
        }
    }
}
