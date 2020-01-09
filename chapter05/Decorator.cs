using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.FactoryPattern.Model
{
    interface IPrice
    {
        decimal Cost { get; set; }
    }

    class Product
    {
        public IPrice Price { get; set; }

    }

    class BasePrice : IPrice
    {
        public decimal Cost
        {
            get;set;
        }
    }
    class CurrentcyPriceDecorator: IPrice
    {
        private IPrice _basePrice;
        private decimal _exchangeRate;
        public CurrentcyPriceDecorator(IPrice basePrice, decimal exchangeRate)
        {
            this._basePrice = basePrice;
            this._exchangeRate = exchangeRate;
        }
        public decimal Cost
        {
            get
            {
                return this._basePrice.Cost * _exchangeRate;
            }
            set
            {
                _basePrice.Cost = value;
            }
        }
    }

    class TradeDiscountPriceDecorator : IPrice
    {
        private IPrice _basePrice;
        private decimal _discount;
        public TradeDiscountPriceDecorator(IPrice basePrice, decimal discount)
        {
            this._basePrice = basePrice;
            this._discount = discount;
        }
        public decimal Cost
        {
            get
            {
                return _basePrice.Cost * this._discount;
            }
            set
            {
                this._basePrice.Cost = value;
            }
        }
    }

    static class ProductCollectionExtensionMethods
    {
        public static void ApplyCurrencyChange(this IEnumerable<Product> products, decimal exchangeRate)
        {
            foreach (var p in products)
            {
                p.Price = new CurrentcyPriceDecorator(p.Price, exchangeRate);
            }
        }

        public static void ApplyDiscount(this IEnumerable<Product> products, decimal discount)
        {
            foreach (var p in products)
            {
                p.Price = new TradeDiscountPriceDecorator(p.Price, discount);
            }
        }
    }

    interface IProductRepository
    {
        IEnumerable<Product> FindAll();
    }

    class ProductRepository:IProductRepository
    {
        public IEnumerable<Product> FindAll()
        {
            var p1 = new Product { Price = new BasePrice { Cost = 1.0m } };
            var p2 = new Product { Price = new BasePrice { Cost = 2.0m } };
            var p3 = new Product { Price = new BasePrice { Cost = 3.0m } };
            return new List<Product> { p1, p2, p3 };
        }
    }

    class ProductService
    {
        IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            this._repository = repository;
        }
        public IEnumerable<Product> GetAllProducts()
        {
            IEnumerable<Product> products = this._repository.FindAll();
            products.ApplyCurrencyChange(6.5m);
            products.ApplyDiscount(0.9m);
            return products;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ProductService ps = new ProductService(new ProductRepository());
            var res = ps.GetAllProducts();
            foreach (var p in res)
            {
                Console.WriteLine(p.Price.Cost);
            }
            Console.ReadLine();
        }
    }
}
