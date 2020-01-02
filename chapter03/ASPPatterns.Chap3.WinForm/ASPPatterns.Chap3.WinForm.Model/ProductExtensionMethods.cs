using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Model
{
    public static class ProductExtensionMethods
    {
        public static void ApplyDiscountStrategy(this Product product, CustomerType type)
        {
            product.Price.SetDiscountStrategy(DiscountStrategyFactory.CreateStrategyByType(type));
        }

        public static void ApplyDiscountStrategy(this IList<Product> products, CustomerType type)
        {
            foreach (Product prod in products)
            {
                prod.ApplyDiscountStrategy(type);
            }
        }
    }
}
