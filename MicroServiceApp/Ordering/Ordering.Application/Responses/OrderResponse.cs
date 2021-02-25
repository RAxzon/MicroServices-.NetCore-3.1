using System;
using System.Collections.Generic;
using System.Text;

namespace Ordering.Application.Responses
{
    public class OrderResponse
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // Billing information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        // Payment info
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }
    }
}
