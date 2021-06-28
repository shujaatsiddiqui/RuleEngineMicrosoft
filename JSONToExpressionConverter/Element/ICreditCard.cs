using JSONToExpressionConverter.Visitor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONToExpressionConverter.Element
{
    public interface ICreditCard
    {
        public void accept(IOfferVisitor visitor);
    }
}
