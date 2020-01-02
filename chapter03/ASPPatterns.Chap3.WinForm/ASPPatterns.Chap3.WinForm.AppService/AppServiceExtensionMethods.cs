using ASPPatterns.Chap3.WinForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.AppService
{
    public static class AppServiceExtensionMethods
    {
        public static ProductViewModel ConvertToProductViewModel(this Product prod)
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Id = prod.Id;
            viewModel.Name = prod.Name;
            viewModel.RRP = $"{prod.Price.RRP.ToString("C")}";
            viewModel.SellingPrice = $"{prod.Price.SellingPrice.ToString("C")}";
            viewModel.Savings = $"{(prod.Price.RRP - prod.Price.SellingPrice).ToString("C")}";
            viewModel.Discount = $"{((prod.Price.RRP - prod.Price.SellingPrice)/prod.Price.RRP).ToString("#%")}";
            return viewModel;
        }
        public static IList<ProductViewModel> ConvertToProductViewModels(this IList<Product> products)
        {
            IList<ProductViewModel> list = new List<ProductViewModel>();
            foreach(Product prod in products)
            {
                list.Add(prod.ConvertToProductViewModel());
            }
            return list;
        }
    }
}
