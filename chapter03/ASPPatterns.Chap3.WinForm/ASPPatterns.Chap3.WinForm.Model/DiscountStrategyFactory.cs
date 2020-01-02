using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Model
{
    public class DiscountStrategyFactory
    {
        public static IDiscountStrategy CreateStrategyByType(CustomerType type)
        {
            switch (type)
            {
                case CustomerType.Trade:
                    return new RegularDiscountStrategy();
                default:
                    return new NullDiscountStrategy();
            }
        }
    }
}
