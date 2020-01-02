using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Model
{
    class NullDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyDiscountStrategyTo(decimal originalPrice)
        {
            return originalPrice;
        }
    }
}
