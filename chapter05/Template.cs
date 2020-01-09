using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.TemplatePattern.Model
{
    enum ReturnAction
    {
        FaultyReturn = 0,
        NoQuibblesReturn = 1
    }
    class ReturnOrder
    {
        public decimal AmountToRefund { get; set; }
        public string PaymentTransactionId { get; set; }
        public decimal PostageCost { get; set; }
        public decimal PricePaid { get; set; }
        public string ProductId { get; set; }
        public int QtyBeingReturn { get; set; }

        public ReturnAction Action { get; set; }
    }

    abstract class ReturnProcessTemplate
    {
        protected abstract void CalculateRefundFor(ReturnOrder order);
        protected abstract void GenerateReturnTransactionFor(ReturnOrder order);
        public void Process(ReturnOrder order)
        {
            this.CalculateRefundFor(order);
            this.GenerateReturnTransactionFor(order);
        }
    }

    class FaultyReturnProcess: ReturnProcessTemplate
    {
        protected override void CalculateRefundFor(ReturnOrder order)
        {
            order.AmountToRefund = order.PricePaid + order.PostageCost;
        }
        protected override void GenerateReturnTransactionFor(ReturnOrder order)
        {
            Console.WriteLine("transaction generated for faulty return....");
        }
    }

    class NoQuibblesReturnProcess : ReturnProcessTemplate
    {
        protected override void CalculateRefundFor(ReturnOrder order)
        {
            order.AmountToRefund = order.PricePaid;
        }
        protected override void GenerateReturnTransactionFor(ReturnOrder order)
        {
            Console.WriteLine("transaction generated for no quibbles return....");
        }
    }

    class ReturnProcessFactory
    {
        public static ReturnProcessTemplate CreateFrom(ReturnOrder order)
        {
            ReturnProcessTemplate template;
            switch (order.Action)
            {
                
                case ReturnAction.FaultyReturn:
                    template =  new FaultyReturnProcess();
                    break;
                case ReturnAction.NoQuibblesReturn:
                    template = new NoQuibblesReturnProcess();
                    break;
                default:
                    throw new NotImplementedException("no action found for this return order");
            }

            return template;
        }
    }

    class ReturnService
    {
        public void Process(ReturnOrder order)
        {
            var process = ReturnProcessFactory.CreateFrom(order);
            process.Process(order);
            Console.WriteLine(order.AmountToRefund);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            ReturnOrder o1 = new ReturnOrder { PricePaid = 10, PostageCost = 10, Action = ReturnAction.FaultyReturn};
            ReturnOrder o2 = new ReturnOrder { PricePaid = 10, PostageCost = 10, Action = ReturnAction.NoQuibblesReturn };
            ReturnService rs = new ReturnService();
            rs.Process(o1);
            rs.Process(o2);
            Console.ReadLine();
        }
    }
}
