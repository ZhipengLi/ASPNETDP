using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Model
{
    class RegularDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyDiscountStrategyTo(decimal originalPrice)
        {
            return originalPrice * 0.95m;
        }
    }
}
