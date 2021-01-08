using System;
using System.Net;
using System.Threading.Tasks;
using Basket.API.Models;
using Basket.API.Repositories.Interfaces;
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

        public BasketController(IBasketRepository repository, ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
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

        [HttpPut]
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
    }
}
