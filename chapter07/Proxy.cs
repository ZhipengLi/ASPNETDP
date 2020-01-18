using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap7.ProxyPattern.Model
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
    }
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<Order> Orders { get; set; }
    }

    public interface ICustomerRepository
    {
        Customer FindBy(Guid id);
    }
    public interface IOrderRepository
    {
        IEnumerable<Order> FindAllBy(Guid customerId);
    }
    public class CustomerProxy : Customer
    {
        private bool _hasLoadedOrders = false;
        private IEnumerable<Order> _orders;
        public IOrderRepository OrderRepo { get; set; }
        public bool HasLoadedOrders()
        {
            return _hasLoadedOrders;
        }
        private void LoadOrders()
        {
            if (!HasLoadedOrders())
            {
                this._orders = OrderRepo.FindAllBy(base.Id);
                _hasLoadedOrders = true;
            }
        }
        public override IEnumerable<Order> Orders
        {
            get
            {
                LoadOrders();
                return _orders;
            }

            set
            {
                base.Orders = value;
            }
        }
    }
}
namespace ASPPatterns.Chap7.ProxyPattern.Repository
{
    using ASPPatterns.Chap7.ProxyPattern.Model;
    public class OrderRepository : IOrderRepository
    {
        public IEnumerable<Order> FindAllBy(Guid customerId)
        {
            Console.WriteLine("Load orders from db...");

            List<Order> orders = new List<Model.Order>();
            orders.Add(new Order { Id = Guid.NewGuid(), OrderDate = DateTime.Now.AddMinutes(-1) });
            orders.Add(new Order { Id = Guid.NewGuid(), OrderDate = DateTime.Now.AddMinutes(-2) });
            orders.Add(new Order { Id = Guid.NewGuid(), OrderDate = DateTime.Now.AddMinutes(-3) });
            return orders;
        }
    }

    public class CustomerRepository : ICustomerRepository
    {
        private IOrderRepository _repo;
        public CustomerRepository(IOrderRepository repo)
        {
            this._repo = repo;
        }
        public Customer FindBy(Guid id)
        {
            CustomerProxy customer = new CustomerProxy();
            customer.OrderRepo = _repo;
            return customer;
        }
    }
}

namespace ASPPattern07
{
    using ASPPatterns.Chap7.ProxyPattern.Model;
    using ASPPatterns.Chap7.ProxyPattern.Repository;
    class Concurrency
    {
        static void Main(string[] args)
        {
            CustomerRepository repo = new CustomerRepository(new OrderRepository());
            var customer = repo.FindBy(Guid.NewGuid());
            Console.WriteLine("access orders for the first time...");
            var orders = customer.Orders;
            foreach (var o in orders)
            {
                Console.WriteLine(o.Id + " " + o.OrderDate);
            }
            Console.WriteLine("access orders again...");
            orders = customer.Orders;
            foreach (var o in orders)
            {
                Console.WriteLine(o.Id + " " + o.OrderDate);
            }

            Console.ReadLine();
        }
    }
}
