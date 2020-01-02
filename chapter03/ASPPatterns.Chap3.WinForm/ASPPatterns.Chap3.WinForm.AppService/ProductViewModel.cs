using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.AppService
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RRP { get; set; }
        public string SellingPrice { get; set; }
        public string Savings { get; set; }
        public string Discount { get; set; }
    }
}
