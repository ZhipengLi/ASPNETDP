using ASPPatterns.Chap3.WinForm.Presenter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ASPPatterns.Chap3.WinForm.Model;
using ASPPatterns.Chap3.WinForm.AppService;
using StructureMap;

namespace ASPPatterns.Chap3.WinForm.View
{
    public partial class PrdForm : Form, IProductListView
    {
        public PrdForm()
        {
            InitializeComponent();
        }

        public CustomerType CustomerType
        {
            get
            {
                if (this.ddlCustomerType.SelectedIndex == -1)
                    return CustomerType.Regular;

                return (CustomerType)Enum.Parse(typeof(CustomerType), this.ddlCustomerType.SelectedItem.ToString());
            }
        }

        public void Display(IList<ProductViewModel> viewModels)
        {
            this.dataGridViewProductList.DataSource = viewModels;
        }

        public void SetErrorMessage(string msg)
        {
            this.lblErrorMessage.Text = msg;
        }

        private void ddlCustomerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.ddlCustomerType.SelectedIndex == -1)
                return;
            ProductListPresenter presenter = new ProductListPresenter(this, ObjectFactory.GetInstance<AppService.ProductService>());
            presenter.Display();
        }
    }
}
