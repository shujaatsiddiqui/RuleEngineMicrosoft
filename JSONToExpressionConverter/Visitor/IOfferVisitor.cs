using JSONToExpressionConverter.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter.Visitor
{
    public interface IOfferVisitor
    {
        public void VisitBronzeCreditCard(BronzeCreditCard bronze);
        public void VisitGoldCreditCard(GoldCreditCard gold);

    }
}
