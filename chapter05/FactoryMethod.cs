namespace ASPPatterns.Chap5.FactoryPattern.Model
{
    class Address
    {
        public string CountryCode { get; set; }
    }
    class Order
    {
        public string CourierTrackingId { get; set; }
        public decimal TotalCost { get; set; }
        public decimal WeightInKG { get; set; }

        public Address DispatchAddress { get; set; }
    }

    interface IShippingCourier
    {
        string GenerateConsignmentLabelFor(Address addr);
    }

    class RoyalMail : IShippingCourier
    {
        public string GenerateConsignmentLabelFor(Address addr)
        {
            return "royal mail: " + addr.CountryCode;
        }
    }

    class DHL : IShippingCourier
    {
        public string GenerateConsignmentLabelFor(Address addr)
        {
            return "DHL mail: " + addr.CountryCode;
        }
    }

    class UKShippingCourierFactory
    {
        public static IShippingCourier CreateShippingCourier(Order order)
        {
            if (order.TotalCost>100)
                return new DHL();
            else
                return new RoyalMail();
        }
    }

    class OrderService
    {
        public void Dispatch(Order order)
        {
            IShippingCourier courier = UKShippingCourierFactory.CreateShippingCourier(order);
            order.CourierTrackingId = courier.GenerateConsignmentLabelFor(order.DispatchAddress);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var os = new OrderService();
            Order order = new Order{ DispatchAddress = new Address { CountryCode = "001" } };
            order.TotalCost = 101;
            os.Dispatch(order);
            Console.WriteLine(order.CourierTrackingId);
            Console.ReadLine();
        }
    }
}
