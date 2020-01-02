using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap3.WinForm.Model
{
    public class Price
    {
        private IDiscountStrategy _strategy = new NullDiscountStrategy();
        private decimal _sellingPrice;
        public Price(decimal rrp, decimal sellingPrice)
        {
            this.RRP = rrp;
            this._sellingPrice = sellingPrice;
        }
        public void SetDiscountStrategy(IDiscountStrategy strategy) {
            this._strategy = strategy;
        }
        public decimal RRP
        {
            get;
            set;
        }
        public decimal SellingPrice
        {
            get
            {
                return this._strategy.ApplyDiscountStrategyTo(this._sellingPrice);
            }
            set
            {
                this._sellingPrice = value;
            }
        }
    }
}
