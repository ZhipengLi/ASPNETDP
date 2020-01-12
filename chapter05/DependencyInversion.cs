using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.DependencyInversion.Model
{
    interface IProductDiscountStrategy
    {
    }
    class ChristmasProductDiscount:IProductDiscountStrategy
    {
    }
    interface IProductRepository
    {
        IEnumerable<Product> FindAll();
    }
    class LinqProductRepository : IProductDiscountStrategy
    {
        public IEnumerable<Product> FindAll()
        {
            return new List<Product>();
        }
    }

    class Product
    {
        public void AdjustPriceWith(IProductDiscountStrategy strategy)
        {

        }
    }

    class ProductService
    {
        private IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            this._repository = repository;
        }
        public IEnumerable<Product> GetProductsAndApplyDiscount(IProductDiscountStrategy strategy)
        {
            IEnumerable<Product> products = this._repository.FindAll();
            foreach (var p in products)
            {
                p.AdjustPriceWith(strategy);
            }
            return products;
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            


            Console.ReadLine();
        }
    }
}
