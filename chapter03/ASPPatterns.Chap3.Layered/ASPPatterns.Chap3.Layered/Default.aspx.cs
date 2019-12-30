using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASPPatterns.Chap3.Layered.Model;
using ASPPatterns.Chap3.Layered.Service;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace ASPPatterns.Chap3.Layered.Model
{
    public interface IDiscountStrategy
    {
        decimal ApplyExtraDiscountsTo(decimal OriginalSalePrice);
    }
    public interface IRepository
    {
        IList<Product> FindAll();
    }
    public class NullDiscountStrategy : IDiscountStrategy
    {
        public decimal ApplyExtraDiscountsTo(decimal originalSalePrice)
        {
            return originalSalePrice;
        }
    }
    public class RegularStrategy : IDiscountStrategy
    {
        public decimal ApplyExtraDiscountsTo(decimal originalSalePrice)
        {
            return originalSalePrice * 0.95m;
        }
    }
    public enum CustomerType
    {
        Standard = 0,
        Trade = 1
    }
    public class DiscountFactory
    {
        public static IDiscountStrategy GetDiscountStrategy(CustomerType type)
        {
            switch (type) {
                case CustomerType.Standard:
                    return new NullDiscountStrategy();
                default:
                    return new RegularStrategy();
            } 
        }
    }
    public class Price
    {
        private IDiscountStrategy _discountStrategy = new NullDiscountStrategy();
        private decimal _rrp;
        private decimal _sellingPrice;
        public Price(decimal RRP, decimal SellingPrice)
        {
            _rrp = RRP;
            _sellingPrice = SellingPrice;
        }
        public void SetDiscountStrategyTo(IDiscountStrategy DiscountStrategy)
        {
            _discountStrategy = DiscountStrategy;
        }
        public decimal SellingPrice
        {
            get { return _discountStrategy.ApplyExtraDiscountsTo(_sellingPrice); }
        }
        public decimal RRP
        {
            get { return _rrp; }
        }
        public decimal Discount
        {
            get
            {
                if (RRP > SellingPrice)
                    return (RRP - SellingPrice);
                else
                    return 0;
            }
        }
        public decimal Savings
        {
            get
            {
                if (RRP > SellingPrice)
                    return 1 - (SellingPrice / RRP);
                else
                    return 0;
            }
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Price Price { get; set; }
    }
    public static class ProductServiceExtensionMethods
    {
        public static void ApplyDiscountStrategy(this IList<Product> products, IDiscountStrategy strategy)
        {
            foreach (Product p in products)
            {
                p.Price.SetDiscountStrategyTo(strategy);
            }
        }
    }
    public class ProductService
    {
        private IRepository _repository;
        public ProductService(IRepository repository)
        {
            _repository = repository;
        }
        public IList<Product> GetProductsFor(CustomerType type)
        {
            IDiscountStrategy strategy = DiscountFactory.GetDiscountStrategy(type);
            IList<Product> products = _repository.FindAll();
            products.ApplyDiscountStrategy(strategy);
            return products;
        }
    }
}

namespace ASPPatterns.Chap3.Layered.Service
{
    public class ProductViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string RRP { get; set; }
        public string SellingPrice { get; set; }
        public string Discount { get; set; }
        public string Savings { get; set; }
    }
    public class ProductRequest
    {
        public Model.CustomerType type { get; set; }
    }
    public class ProductResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IList<ProductViewModel> Products { get; set; }
    }
    public static class ProductMapperExtensionMethods
    {
        public static ProductViewModel ConvertToProductViewModel(this Model.Product product)
        {
            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.ProductId = product.Id;
            productViewModel.Name = product.Name;
            productViewModel.RRP = String.Format("{0:C}", product.Price.RRP);
            productViewModel.SellingPrice =
            String.Format("{0:C}", product.Price.SellingPrice);
            if (product.Price.Discount > 0)
                productViewModel.Discount =
                String.Format("{0:C}", product.Price.Discount);
            if (product.Price.Savings < 1 && product.Price.Savings > 0)
                productViewModel.Savings = product.Price.Savings.ToString("#%");
            return productViewModel;
        }
        public static IList<ProductViewModel> ConvertToProductiewModelList(this IList<Model.Product> products)
        {
            IList<ProductViewModel> result = new List<ProductViewModel>();
            foreach (Model.Product p in products)
            {
                result.Add(p.ConvertToProductViewModel());
            }
            return result;
        }
    }

    public class ProductService
    {
        private Model.ProductService _productService;
        public ProductService(Model.ProductService service)
        {
            this._productService = service;
        }
        public ProductResponse GetProducts(ProductRequest request) {
            ProductResponse response = new ProductResponse();
            try
            {
                IList<ProductViewModel> list = this._productService.GetProductsFor(request.type).ConvertToProductiewModelList();
                response.Success = true;
                response.Products = list;
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

namespace ASPPatterns.Chap3.Layered.Repository
{
    public class ProductRepository : Model.IRepository
    {
        public IList<Product> FindAll()
        {
            IList<Product> result = new List<Product>();
            result.Add(new Product() { Id = 1, Price = new Price(100.0m, 99.5m), Name = "Shoe" });
            result.Add(new Product() { Id = 2, Price = new Price(100.0m, 99.5m), Name = "TV" });
            result.Add(new Product() { Id = 3, Price = new Price(100.0m, 99.5m), Name = "Stereo" });
            return result;
        }
    }
}
namespace ASPPatterns.Chap3.Layered.Presentation
{
    public interface IProductListView
    {
        void Display(IList<Service.ProductViewModel> products);
        CustomerType type { get; }
        string ErrorMessage { set; }
    }

    public class ProductListPresenter
    {
        private IProductListView _productListview;
        private Service.ProductService _productService;
        public ProductListPresenter(IProductListView view, Service.ProductService service)
        {
            _productListview = view;
            _productService = service;
        }
        public void Display() {
            Service.ProductRequest request = new Service.ProductRequest();
            request.type = _productListview.type;
            try
            {
                Service.ProductResponse response = _productService.GetProducts(request);
                if (response.Success)
                    _productListview.Display(response.Products);
                else
                {
                    _productListview.ErrorMessage = response.Message;
                }
            }
            catch (Exception e)
            {
                _productListview.ErrorMessage = e.Message;
            }
        }
    }
}

namespace ASPPatterns.Chap3.Layered.IoC
{
    public class BootStrapper
    {
        public static void ConfigureStructureMap()
        {
            ObjectFactory.Initialize(x =>
            {
                x.AddRegistry<ModelRegistry>();
            });
        }
        public class ModelRegistry : Registry
        {
            public ModelRegistry()
            {
                For<IRepository>().Use<Repository.ProductRepository>();
            }
        }
    }
}

namespace ASPPatterns.Chap3.Layered
{
    public partial class Default : System.Web.UI.Page, Presentation.IProductListView
    {
        private Presentation.ProductListPresenter _presenter;

        protected void Page_Init(object sender, EventArgs e)
        {
            _presenter = new Presentation.ProductListPresenter(this, 
                ObjectFactory.GetInstance<Service.ProductService>());
            this.ddlCustomerType.SelectedIndexChanged +=
                delegate { _presenter.Display(); };
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack != true)
                _presenter.Display();
        }
        public string ErrorMessage
        {
            set
            {
                this.lblErrorMessage.Text = value;
            }
        }

        public CustomerType type
        {
            get
            {
                return (CustomerType)Enum.ToObject(typeof(CustomerType),int.Parse(this.ddlCustomerType.SelectedValue));
            }
        }

        public void Display(IList<ProductViewModel> products)
        {
            this.rptProducts.DataSource = products;
            this.rptProducts.DataBind();
        }
    }
}