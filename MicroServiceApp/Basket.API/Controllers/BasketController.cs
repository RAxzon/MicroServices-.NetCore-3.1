using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Models;
using Basket.API.Repositories.Interfaces;
using EventDrivenRabbitMQ.Common;
using EventDrivenRabbitMQ.Events;
using EventDrivenRabbitMQ.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMQProducer _eventBus;

        public BasketController(IBasketRepository repository, ILogger<BasketController> logger, IMapper mapper, EventBusRabbitMQProducer eventBus)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus)); 
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            try
            {
                var basket = await _repository.GetBasketByUserName(userName);

                if (basket == null)
                {
                    _logger.LogError($"{DateTime.Now}, basket returned null");
                    return NotFound($"Basket with {userName} was not found");
                }

                return Ok(basket);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message,$"{DateTime.Now}, Error getting basket");
                throw new Exception($"{DateTime.Now}, Error getting basket", ex);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BasketCart), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basket)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(await _repository.Update(basket));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, $"{DateTime.Now}, Error updating basket");
                    throw new Exception($"{DateTime.Now} Error updating basket");
                }
            }

            return BadRequest();
        }

        [HttpDelete("{username}")]
        [ProducesResponseType(typeof(void), (int) HttpStatusCode.OK)]
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            try
            {
                return Ok(await _repository.Delete(userName));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{DateTime.Now}, Error deleting basket");
                throw new Exception($"{DateTime.Now} Error deleting basket");
            }
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CheckOut([FromBody] BasketCheckout basketCheckout)
        {
            try
            {
                var basket = await _repository.GetBasketByUserName(basketCheckout.UserName);

                if (basket == null)
                {
                    _logger.LogInformation($"{DateTime.Now}, Basket with username: {basketCheckout.UserName}, was not found");
                    return NotFound("Basket was not found");
                }

                var removeBasket = await _repository.Delete(basketCheckout.UserName);

                if (!removeBasket)
                {
                    _logger.LogError($"{DateTime.Now}, Error deleting basket with username: {basketCheckout.UserName}");
                    return BadRequest($"Error deleting basket with username: {basketCheckout.UserName}");
                }

                // Send request to event bus

                var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
                eventMessage.RequestId = Guid.NewGuid();
                eventMessage.TotalPrice = basket.TotalPrice;

                try
                {
                    _eventBus.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{DateTime.Now}, Error publishing message to basket queue", ex.Message);
                    throw;
                }

                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"{DateTime.Now}, Error checking out");
                throw new Exception($"{DateTime.Now}, Error Publishing checkout", ex);
            }
        }
    }
}
