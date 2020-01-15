using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASPPatterns.Chap6.EventTickets;
using System.ServiceModel;

namespace ASPPatterns.Chap6.EventTickets.ServiceProxy
{
    public class TicketServiceClientProxy: ClientBase<Contracts.ITicketService>, Contracts.ITicketService
    {
        public DataContract.ReserveTicketResponse ReserveTicket(DataContract.ReserveTicketRequest request)
        {
            return base.Channel.ReserveTicket(request);
        }
        public DataContract.PurchaseTicketResponse PurchaseTicket(DataContract.PurchaseTicketRequest request)
        {
            return base.Channel.PurchaseTicket(request);
        }
    }

    public class TicketPresentation
    {
        public string TicketId { get; set; }
        public string EventId { get; set; }
        public string Description { get; set; }
        public bool WasAbleToPurchaseTicket { get; set; }
    }
    public class TicketReservationPresentation
    {
        public string EventId { get; set; }
        public string ReservationId { get; set; }
        public string Description { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool TicketWasSuccessfullyReserved { get; set; }
    }

    public class TicketServiceFacade
    {
        Contracts.ITicketService _ticketService;
        public TicketServiceFacade(Contracts.ITicketService ticketService)
        {
            this._ticketService = ticketService;
        }
        public TicketReservationPresentation ReserveTicketFor(string eventId, int noOfTkt)
        {
            TicketReservationPresentation reservationPresentation = new TicketReservationPresentation();
            DataContract.ReserveTicketRequest request = new DataContract.ReserveTicketRequest();
            request.EventId = eventId;
            request.TicketQty = noOfTkt;
            try
            {
                var response = this._ticketService.ReserveTicket(request);
                if (response.Success)
                {
                    reservationPresentation.Description = string.Format("Reservation succeeded");
                    reservationPresentation.EventId = response.EventId;
                    reservationPresentation.ExpiryDate = response.ExpirationTime;
                    reservationPresentation.ReservationId = response.ReservationNumber;
                    reservationPresentation.TicketWasSuccessfullyReserved = true;
                }
                else
                {
                    reservationPresentation.TicketWasSuccessfullyReserved = false;
                    reservationPresentation.Description = response.Message;
                }
            }
            catch (Exception ex)
            {
                reservationPresentation.TicketWasSuccessfullyReserved = false;
                reservationPresentation.Description = ex.Message;
            }
            return reservationPresentation;
        }

        public TicketPresentation PurchaseTicketWithReservation(string eventId, string reservationId)
        {
            TicketPresentation ticketPresentation = new TicketPresentation();
            DataContract.PurchaseTicketRequest request = new DataContract.PurchaseTicketRequest();
            request.EventId = eventId;
            request.ReservationId = reservationId;
            request.CorrelationId = reservationId;
            try
            {
                var response = this._ticketService.PurchaseTicket(request);
                if (response.Success)
                {
                    ticketPresentation.Description = string.Format("Reservation succeeded");
                    ticketPresentation.EventId = response.EventId;
                    ticketPresentation.TicketId = response.TicketId;
                    ticketPresentation.WasAbleToPurchaseTicket = true;
                }
                else
                {
                    ticketPresentation.WasAbleToPurchaseTicket = false;
                    ticketPresentation.Description = response.Message;
                }
            }
            catch (Exception ex)
            {
                ticketPresentation.WasAbleToPurchaseTicket = false;
                ticketPresentation.Description = ex.Message;
            }
            return ticketPresentation;
        }
    }
}
