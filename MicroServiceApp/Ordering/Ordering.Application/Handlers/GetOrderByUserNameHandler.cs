using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Mapper;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Interfaces;

namespace Ordering.Application.Handlers
{
    public class GetOrderByUserNameHandler : IRequestHandler<GetOrderByUserNameQuery, IEnumerable<OrderResponse>>
    {

        private readonly IOrderRepository _orderRepo;

        public GetOrderByUserNameHandler(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
        }

        public async Task<IEnumerable<OrderResponse>> Handle(GetOrderByUserNameQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepo.GetOrderByUserName(request.UserName);

            var orderListResponse = OrderMapper.Mapper.Map<IEnumerable<OrderResponse>>(orderList);

            return orderListResponse;
        }
    }
}
