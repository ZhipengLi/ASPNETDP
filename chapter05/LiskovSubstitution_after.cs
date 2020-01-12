using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.LiscovSubstitue.Model
{

    enum PaymentType
    {
        Paypal = 1,
        WorldPay = 2
    }

    class RefundRequest
    {
        public PaymentType Payment { get; set; }
        public string PaymentTransactionId { get; set; }
        public decimal RefundAmount { get; set; }
    }

    class RefundResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    class MockPaypalWebService
    {
        public string ObtainToken(string username, string password)
        {
            return "this is the token";
        }
        public string MakeRefund(decimal amount, string transactionId, string token)
        {
            return "your paypal refund is successful";
        }
    }

    class MockWorldPayWebService
    {
        public string MakeRefund(decimal amount, string transactionId, string username, string password, string productId)
        {
            return "your worldpay refund is successful";
        }
    }

    abstract class PaymentServiceBase
    {
        public abstract RefundResponse Refund(decimal amount, string transactionId);
    }

    class PaypalPayment : PaymentServiceBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public PaypalPayment(string name, string pass)
        {
            Username = name;
            Password = pass;
        }
        public override RefundResponse Refund(decimal amount, string transactionId)
        {
            MockPaypalWebService paypal = new MockPaypalWebService();
            var token = paypal.ObtainToken(Username, Password);
            var message =  paypal.MakeRefund(amount, transactionId, token);
            return new RefundResponse() { Message = message, Success = message.Contains("successful") ? true : false };
        }
    }

    class WorldPayPayment : PaymentServiceBase
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ProductId { get; set; }
        public WorldPayPayment(string name, string pass, string id)
        {
            Username = name;
            Password = pass;
            ProductId = id;
        }
        public override RefundResponse Refund(decimal amount, string transactionId)
        {
            MockWorldPayWebService worldPay = new MockWorldPayWebService();
            var message =  worldPay.MakeRefund(amount, transactionId, Username, Password, ProductId);
            return new RefundResponse() { Message = message, Success = message.Contains("successful")?true:false };
        }
    }

    class PaymentServiceFactory
    {
        public static PaymentServiceBase GetPaymentServiceFrom(PaymentType type)
        {
            switch (type)
            {
                case PaymentType.Paypal:
                    return new PaypalPayment("testname", "test pass");
                case PaymentType.WorldPay:
                    return new WorldPayPayment("testname", "test pass", "test id");
                default:
                    throw new ApplicationException("no such payment type");
            }
        }
    }

    class ReturnService
    {
        public RefundResponse Refund(RefundRequest request)
        {
            RefundResponse response = new RefundResponse();
            PaymentServiceBase service = PaymentServiceFactory.GetPaymentServiceFrom(request.Payment);
            //if (service is PaypalPayment)
            //{
            //    ((PaypalPayment)service).Username = "testuser";
            //    ((PaypalPayment)service).Password = "test password";
            //}
            //else if (service is WorldPayPayment)
            //{
            //    ((WorldPayPayment)service).Username = "testuser";
            //    ((WorldPayPayment)service).Password = "test password";
            //    ((WorldPayPayment)service).ProductId = "test product id";
            //}

            response = service.Refund(request.RefundAmount, request.PaymentTransactionId); 
            return response;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            RefundRequest request = new RefundRequest();
            request.Payment = PaymentType.Paypal;
            request.PaymentTransactionId = "test id";
            request.RefundAmount = 0.5m;
            var response = new ReturnService().Refund(request);
            Console.WriteLine(response.Message);
            Console.WriteLine("is successful:"+response.Success);
            Console.ReadLine();
        }
    }
}
