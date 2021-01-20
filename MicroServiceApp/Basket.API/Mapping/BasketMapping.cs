using AutoMapper;
using Basket.API.Models;
using EventDrivenRabbitMQ.Events;

namespace Basket.API.Mapping
{
    public class BasketMapping : Profile
    {
        public BasketMapping()
        {
            // Both way out mapping
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
