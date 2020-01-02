using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.AppService
{
    public class ProductService : IProductService
    {
        private Model.ProductService _service;
        public ProductService(Model.ProductService service)
        {
            this._service = service;
        }
        public ProductResponse GetAllProducts(ProductRequest request)
        {
            ProductResponse response = new ProductResponse();
            try
            {
                response.ProductViewModels = this._service.GetAllProducts(request.type).ConvertToProductViewModels();
                response.Success = true;
            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;
            }
            return response;
        }
    }
}
