using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Model
{
    public class ProductService
    {
        private IProductRepository _repo;
        public ProductService(IProductRepository repo)
        {
            this._repo = repo;
        }
        public IList<Product> GetAllProducts(CustomerType type)
        {
            IList<Product> products = _repo.GetAll();
            products.ApplyDiscountStrategy(type);
            return products;
        }
    }
}
