using ASPPatterns.Chap3.WinForm.AppService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Presenter
{
    public class ProductListPresenter
    {
        private IProductListView _view;
        private IProductService _service;
        public ProductListPresenter(IProductListView view, IProductService service)
        {
            this._view = view;
            this._service = service;
        }
        public void Display()
        {
            ProductRequest request = new ProductRequest();
            request.type = _view.CustomerType;
            ProductResponse response = this._service.GetAllProducts(request);
            if (response.Success)
            {
                this._view.Display(response.ProductViewModels);
            }
            else
            {
                this._view.SetErrorMessage(response.Message);
            }
        }
    }
}
