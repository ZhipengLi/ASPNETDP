using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Model
{
    public interface IProductRepository
    {
        IList<Product> GetAll();
    }
}
