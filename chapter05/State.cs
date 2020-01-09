using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap5.StatePattern.Model
{
    enum OrderStatus
    {
        New = 1,
        Shipped = 2,
        Canceled = 3
    }
    interface IOrderState
    {
        bool CanCancel(Order order);
        void Cancel(Order order);
        bool CanShip(Order order);
        void Ship(Order order);
        OrderStatus Status();
    }

    class Order
    {
        public IOrderState _orderState;
        public Order(IOrderState state)
        {
            this._orderState = state;
        }
        public String Customer { get; set; }
        public string Id { get; set; }
        public DateTime OrderedDate { get; set; }
        public OrderStatus Status()
        {
            return this._orderState.Status();
        }
        public bool CanCancel()
        {
            return this._orderState.CanCancel(this);
        }
        public void Cancel()
        {
            if(CanCancel())
                this._orderState.Cancel(this);
        }
        public bool CanShip()
        {
            return this._orderState.CanShip(this);
        }
        public void Ship()
        {
            if(CanShip())
                this._orderState.Ship(this);
        }

        public void Change(IOrderState state)
        {
            this._orderState = state;
        }
    }

    class OrderShippedState : IOrderState
    {
        public bool CanCancel(Order order)
        {
            return false;
        }
        public void Cancel(Order order)
        {
            throw new NotImplementedException("Shipped order cannot be canceled");
        }

        public bool CanShip(Order order)
        {
            return false;
        }
        public void Ship(Order order)
        {
            throw new NotImplementedException("Shipped order cannot be shipped again");
        }
        public OrderStatus Status()
        {
            return OrderStatus.Shipped;
        }
    }

    class OrderCanceledState : IOrderState
    {
        public bool CanCancel(Order order)
        {
            return false;
        }
        public void Cancel(Order order)
        {
            throw new NotImplementedException("Canceled order cannot be canceled again");
        }

        public bool CanShip(Order order)
        {
            return false;
        }
        public void Ship(Order order)
        {
            throw new NotImplementedException("Canceled order cannot be shipped");
        }
        public OrderStatus Status()
        {
            return OrderStatus.Canceled;
        }
    }

    class OrderNewState : IOrderState
    {
        public bool CanCancel(Order order)
        {
            return true;
        }
        public void Cancel(Order order)
        {
            Console.WriteLine("order canceled");
            order.Change(new OrderCanceledState());
        }

        public bool CanShip(Order order)
        {
            return true;
        }
        public void Ship(Order order)
        {
            Console.WriteLine("order shipped");
            order.Change(new OrderShippedState());
        }
        public OrderStatus Status()
        {
            return OrderStatus.New;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Order o = new Order(new OrderNewState());
            Console.WriteLine("order status:" + o.Status());
            Console.WriteLine("can ship?" + (o.CanShip() ? "true":"false"));
            Console.WriteLine("can cancel?" + (o.CanCancel() ? "true" : "false"));

            o.Ship();
            Console.WriteLine("order status:" + o.Status());
            Console.WriteLine("can ship?" + (o.CanShip() ? "true" : "false"));
            Console.WriteLine("can cancel?" + (o.CanCancel() ? "true" : "false"));
            Console.ReadLine();
        }
    }
}
