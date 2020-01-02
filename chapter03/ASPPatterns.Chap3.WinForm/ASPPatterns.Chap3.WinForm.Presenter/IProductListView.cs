using ASPPatterns.Chap3.WinForm.AppService;
using ASPPatterns.Chap3.WinForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Presenter
{
    public interface IProductListView
    {
        CustomerType CustomerType { get;}
        void SetErrorMessage(string msg);
        void Display(IList<ProductViewModel> viewModels);
    }
}
