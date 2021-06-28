using JSONToExpressionConverter.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter.Visitor
{
    class HotelCashBackOffer : IOfferVisitor
    {
        public void VisitBronzeCreditCard(BronzeCreditCard bronze)
        {
            throw new NotImplementedException();
        }

        public void VisitGoldCreditCard(GoldCreditCard gold)
        {
            Console.WriteLine("10 % discount gold credit card");
        }
    }
}
