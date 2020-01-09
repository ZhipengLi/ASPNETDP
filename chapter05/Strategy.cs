using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.StrategyPattern.Model
{
    enum DiscountType
    {
        NoDiscount = 1,
        MoneyOff = 2,
        PercentageOff = 3
    }
    class BasketDiscountFactory
    {
        public static IBasketDiscountStrategy GetDiscount(DiscountType type)
        {
            IBasketDiscountStrategy s;
            switch (type)
            {
                case DiscountType.MoneyOff:
                    s = new BasketDiscountMoneyOff();
                    break;
                case DiscountType.PercentageOff:
                    s =  new BasketDiscountPercentageOff();
                    break;
                default:
                    
                    s = new  NoBasketDiscount();
                    break;
            }
            return s;
        }
    }
    class Basket
    {
        private IBasketDiscountStrategy _strategy;
        public decimal TotalCost { get; set; }
        public Basket(DiscountType type)
        {
            this._strategy = BasketDiscountFactory.GetDiscount(type);
        }
        public decimal GetTotalCostAfterDiscount()
        {
            return this._strategy.GetTotalCostAfterApplyingDiscountTo(this);
        }
    }
    interface IBasketDiscountStrategy
    {
        decimal GetTotalCostAfterApplyingDiscountTo(Basket basket);
    }
    class NoBasketDiscount: IBasketDiscountStrategy
    {
        public decimal GetTotalCostAfterApplyingDiscountTo(Basket basket)
        {
            return basket.TotalCost;
        }
    }

    class BasketDiscountPercentageOff: IBasketDiscountStrategy
    {
        public decimal GetTotalCostAfterApplyingDiscountTo(Basket basket)
        {
            return basket.TotalCost*0.8m;
        }
    }
    class BasketDiscountMoneyOff: IBasketDiscountStrategy
    {
        public decimal GetTotalCostAfterApplyingDiscountTo(Basket basket)
        {
            if (basket.TotalCost > 100)
                return basket.TotalCost - 100;
            return basket.TotalCost;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Basket basket = new Basket(DiscountType.PercentageOff);
            basket.TotalCost = 100;
            Console.WriteLine(basket.GetTotalCostAfterDiscount());
            Console.ReadLine();
        }
    }
}
