using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ASPPatterns.Chap6.EventTickets.ServiceProxy;

namespace ASPPatterns.Chap6.EventTickets.WebShop
{
    public class Basket
    {
        public Guid Id { get; set; }
        public ServiceProxy.TicketReservationPresentation Reservation { get; set; }
        public static Basket GetBasket()
        {
            if (HttpContext.Current.Session["Basket"] == null)
            {
                HttpContext.Current.Session["Basket"] = new Basket { Id = Guid.NewGuid() };
            }
            return (Basket)HttpContext.Current.Session["Basket"];
        }
        public static void Clear()
        {
            HttpContext.Current.Session["Basket"] = null;
        }
    }
}