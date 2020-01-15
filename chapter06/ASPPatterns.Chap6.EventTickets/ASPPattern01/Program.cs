using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASPPatterns.Chap6.EventTickets.Model
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Allocation { get; set; }

        private List<TicketPurchase> purchasedTickets = new List<TicketPurchase>();
        private List<TicketReservation> reservedTickets = new List<TicketReservation>();
        public Event(string name, int alloc)
        {
            this.Id = new Guid();
            this.Name = name;
            this.Allocation = alloc;
        }
        public int AvailableAllocation()
        {
            int count = 0;
            purchasedTickets.ForEach(x => count += x.TicketQuantity);
            reservedTickets.FindAll(x => x.StillActive()).ForEach(x => count += x.TicketQuantity);
            return Allocation - count;
        }

        public bool CanPurchaseTicketWith(Guid reservationId)
        {
            return this.HasReservationWith(reservationId) && GetReservationWith(reservationId).StillActive();
        }
        public bool CanReserveTicket(int ticketQuantity)
        {
            return AvailableAllocation() >= ticketQuantity;
        }
        public TicketReservation GetReservationWith(Guid reservationId)
        {
            if (!HasReservationWith(reservationId))
            {
                throw new ApplicationException("cannto find the reservation");
            }
            return this.reservedTickets.FirstOrDefault(x => x.Id == reservationId);
        }
        public bool HasReservationWith(Guid reservationId)
        {
            return this.reservedTickets.Exists(x => x.Id == reservationId);
        }
        public TicketPurchase PurchaseTicketWith(Guid reservationId)
        {
            if (!CanPurchaseTicketWith(reservationId))
            {
                throw new ApplicationException("cannot purchase with this id");
            }
            var reservation = GetReservationWith(reservationId);
            TicketPurchase purchase = TicketPurchaseFactory.CreateTicket(this,reservation.TicketQuantity);
            reservation.HasBeenRedeemed = true;
            this.purchasedTickets.Add(purchase);
            return purchase;
        }
        public TicketReservation ReserveTicket(int ticketQty)
        {
            if (!CanReserveTicket(ticketQty))
            {
                throw new ApplicationException("no enough ticket to reserve");
            }
            TicketReservation reservation = TicketReservationFactory.CreateReservation(this, ticketQty);
            //reservation.Event = this;
            //reservation.ExpiryTime = DateTime.Now.AddMinutes(1);
            //reservation.HasBeenRedeemed = false;
            //reservation.Id = new Guid();
            //reservation.TicketQuantity = ticketQty;
            this.reservedTickets.Add(reservation);
            return reservation;
        }
    }
    public class TicketReservation
    {
        public Event Event { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool HasBeenRedeemed { get; set; }
        public Guid Id { get; set; }
        public int TicketQuantity { get; set; }
        public bool HasExpired()
        {
            return ExpiryTime < DateTime.Now;
        }
        public bool StillActive()
        {
            return (!HasExpired()) && (!HasBeenRedeemed);
        }
    }

    public class TicketReservationFactory
    {
        public static TicketReservation CreateReservation(Event e, int qty)
        {
            TicketReservation reservation = new TicketReservation()
            {
                Id = Guid.NewGuid(),
                Event = e,
                ExpiryTime = DateTime.Now.AddMinutes(1),
                HasBeenRedeemed = false,
                TicketQuantity = qty
            };
            return reservation;
        }
    }
    public class TicketPurchaseFactory
    {
        public static TicketPurchase CreateTicket(Event e, int qty)
        {
            return new TicketPurchase {
                Event = e,
                Id = new Guid(),
                TicketQuantity = qty
            };
        }
    }

    public class TicketPurchase
    {
        public Event Event { get; set; }
        public Guid Id { get; set; }
        public int TicketQuantity { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Event event1 = new Event("test event", 100);
            Console.WriteLine("available tickets:"+event1.AvailableAllocation());
            var reservation = event1.ReserveTicket(10);
            Console.WriteLine("available tickets after reservation:" + event1.AvailableAllocation());
            Console.WriteLine("Can purchase the reservation:" + event1.CanPurchaseTicketWith(reservation.Id));
            Console.WriteLine("Can reserve 99 tickets:" + event1.CanReserveTicket(99));




            Console.ReadLine();
        }
    }
}

namespace ASPPatterns.Chap6.EventTickets.Repository
{
    public interface IEventRepository
    {
        Model.Event FindBy(Guid id);
        void Save(Model.Event eventEntity);
    }
    public class EventRepository : IEventRepository
    {
        public EventRepository()
        {
            if (events == null)
            {
                events = new List<Model.Event>();
                Model.Event ev = new Model.Event("sports game", 100);
                ev.Id = new Guid("2de874d0-00b7-4c86-9925-c7f2c243151c");
                events.Add(ev);
            }
        }
        private static List<Model.Event> events; 
        public Model.Event FindBy(Guid id)
        {
            return events.FirstOrDefault(x => x.Id == id);
        }
        public void Save(Model.Event eventEntity)
        {
            if (events.Exists(x => x.Id == eventEntity.Id))
            {
                events.RemoveAt(events.FindIndex(x => x.Id == eventEntity.Id));
            }
            events.Add(eventEntity);
        }
    }
}

namespace ASPPatterns.Chap6.EventTickets.DataContract
{
    using System.Runtime.Serialization;
    using System.ServiceModel;
    [DataContract]
    public abstract class Response
    {
        [DataMember]
        public bool Success { get; set; }
        [DataMember]
        public string Message { get; set; }
    }

    [DataContract]
    public class PurchaseTicketResponse : Response
    {
        [DataMember]
        public string TicketId { get; set; }
        [DataMember]
        public string EventId { get; set; }
        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public int NumOfTickets { get; set; }
    }
    [DataContract]
    public class ReserveTicketResponse : Response
    {
        [DataMember]
        public string ReservationNumber { get; set; }
        [DataMember]
        public DateTime ExpirationTime { get; set; }
        [DataMember]
        public string EventId { get; set; }
        [DataMember]
        public string EventName { get; set; }
        [DataMember]
        public int NumOfTickets { get; set; }
    }

    [DataContract]
    public class ReserveTicketRequest
    {
        [DataMember]
        public string EventId { get; set; }
        [DataMember]
        public int TicketQty { get; set; }
    }

    [DataContract]
    public class PurchaseTicketRequest
    {
        [DataMember]
        public string CorrelationId { get; set; }
        [DataMember]
        public string ReservationId { get; set; }
        [DataMember]
        public string EventId { get; set; }
    }
}

namespace ASPPatterns.Chap6.EventTickets.Contracts
{
    using System.Runtime.Serialization;
    using System.ServiceModel;
    [ServiceContract(Namespace="ASPPatterns.Chap6.EventTickets/")]
    public interface ITicketService
    {
        [OperationContract]
        DataContract.ReserveTicketResponse ReserveTicket(DataContract.ReserveTicketRequest request);
        [OperationContract]
        DataContract.PurchaseTicketResponse PurchaseTicket(DataContract.PurchaseTicketRequest request);
    }
}

namespace ASPPatterns.Chap6.EventTickets.Service
{
    using System.ServiceModel.Activation;
    public static class TicketPurchaseExtensionMethods
    {
        public static DataContract.PurchaseTicketResponse ConvertToTicketPurchaseResponse(this Model.TicketPurchase purchase)
        {
            DataContract.PurchaseTicketResponse response = new DataContract.PurchaseTicketResponse();
            response.EventId = purchase.Event.Id.ToString();
            response.EventName = purchase.Event.Name;
            response.NumOfTickets = purchase.TicketQuantity;
            response.TicketId = purchase.Id.ToString();
            return response;
        }
    }
    public static class TicketReservationExtensionMethods
    {
        public static DataContract.ReserveTicketResponse ConvertToTicketReservationResponse(this Model.TicketReservation reservation)
        {
            DataContract.ReserveTicketResponse response = new DataContract.ReserveTicketResponse();
            response.EventId = reservation.Event.Id.ToString();
            response.EventName = reservation.Event.Name;
            response.NumOfTickets = reservation.TicketQuantity;
            response.ExpirationTime = reservation.ExpiryTime;
            response.ReservationNumber = reservation.Id.ToString();
            return response;
        }
    }

    class MessageResponseHistory<T>
    {
        private Dictionary<string, T> _responseHistory;
        public MessageResponseHistory()
        {
            this._responseHistory = new Dictionary<string, T>();
        }
        public bool IsUniqueResponse(string correlationId)
        {
            return !this._responseHistory.ContainsKey(correlationId);
        }
        public void LogResponse(string correlationId, T response)
        {
            if (this._responseHistory.ContainsKey(correlationId))
                this._responseHistory[correlationId] = response;
            else
                this._responseHistory.Add(correlationId, response);
        }
        public T RetrievePreviousResponseFor(string correlationId)
        {
            return this._responseHistory[correlationId];
        }
    }

    public class ErrorLog
    {
        public static string GenerateErrorRef(Exception exception)
        {
            return string.Format("you can use this id for error reference {0}", Guid.NewGuid().ToString());
        }
    }

    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TicketService :Contracts.ITicketService
    {
        Repository.IEventRepository _repository;
        private static MessageResponseHistory<DataContract.PurchaseTicketResponse> _purchaseHistory = new MessageResponseHistory<DataContract.PurchaseTicketResponse>();
        public TicketService()
        {
            this._repository = new Repository.EventRepository();
        }
        public DataContract.ReserveTicketResponse ReserveTicket(DataContract.ReserveTicketRequest request)
        {
            DataContract.ReserveTicketResponse response = new DataContract.ReserveTicketResponse();
            try
            {
                Model.Event ticketEvent = this._repository.FindBy(new Guid(request.EventId));
                Model.TicketReservation reservation;
                if (ticketEvent.CanReserveTicket(request.TicketQty))
                {
                    reservation = ticketEvent.ReserveTicket(request.TicketQty);
                    this._repository.Save(ticketEvent);
                    response = reservation.ConvertToTicketReservationResponse();
                    response.Success = true;
                }
                else
                {
                    response.Success = false;
                    response.Message = string.Format("There're only {0} tickets available.", ticketEvent.AvailableAllocation());
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorLog.GenerateErrorRef(ex);
            }
            return response;
        }
        public DataContract.PurchaseTicketResponse PurchaseTicket(DataContract.PurchaseTicketRequest request)
        {
            DataContract.PurchaseTicketResponse response = new DataContract.PurchaseTicketResponse();
            try
            {
                if (_purchaseHistory.IsUniqueResponse(request.CorrelationId))
                {
                    Model.Event ticketEvent = this._repository.FindBy(new Guid(request.EventId));
                    var reservation = ticketEvent.GetReservationWith(new Guid(request.ReservationId));
                    if (ticketEvent.CanPurchaseTicketWith(reservation.Id))
                    {
                        var purchase = ticketEvent.PurchaseTicketWith(reservation.Id);
                        this._repository.Save(ticketEvent);
                        response = purchase.ConvertToTicketPurchaseResponse();
                        response.Success = true;
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "can no longer purchase with this reservation";
                    }
                }
                else
                {
                    response = _purchaseHistory.RetrievePreviousResponseFor(request.CorrelationId);
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ErrorLog.GenerateErrorRef(ex);
            }

            return response;
        }
    }
}
