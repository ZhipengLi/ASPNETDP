using ASPPatterns.Chap3.WinForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Repository
{
    public class ProductRepository : IProductRepository
    {
        public IList<Product> GetAll()
        {
            IList<Product> products = new List<Product>();
            products.Add(new Product() { Id = 1, Name = "Soap", Price = new Price(100.0m, 95.0m) });
            products.Add(new Product() { Id = 2, Name = "Wash", Price = new Price(80.0m, 45.0m) });
            products.Add(new Product() { Id = 3, Name = "Tree", Price = new Price(50.0m, 35.0m) });
            return products;
        }
    }
}
