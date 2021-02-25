using System;
using MediatR;
using Ordering.Application.Commands;
using Ordering.Application.Responses;
using System.Threading;
using System.Threading.Tasks;
using Ordering.Application.Mapper;
using Ordering.Core.Entities;
using Ordering.Core.Interfaces;

namespace Ordering.Application.Handlers
{
    public class CheckoutOrderHandler : IRequestHandler<CheckoutOrderCommand, OrderResponse>
    {

        private readonly IOrderRepository _orderRepo;

        public CheckoutOrderHandler(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
        }

        public async Task<OrderResponse> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = OrderMapper.Mapper.Map<Order>(request);

            if (orderEntity == null)
            {
                throw new ApplicationException("Object could not be mapped");
            }

            var order = await _orderRepo.AddAsync(orderEntity);
            var orderResponse = OrderMapper.Mapper.Map<OrderResponse>(order);

            return orderResponse;
        }
    }
}
