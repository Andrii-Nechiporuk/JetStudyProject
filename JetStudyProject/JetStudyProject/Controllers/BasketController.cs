using JetStudyProject.Infrastracture.DTOs.BasketDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JetStudyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IUserService _userService;
        public BasketController(IBasketService basketService, IUserService userService)
        {
            _basketService = basketService;
            _userService = userService;
        }
        /// <summary>
        /// Returns basket if exist
        /// </summary>
        /// <returns></returns>
        [HttpGet("get-basket")]
        [Authorize]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var userId = await _userService.GetUserId(User);
            var basket = await _basketService.GetBasket(userId);
            if (basket == null)
            {
                return NotFound();
            }
            return Ok(basket);
        }
        /// <summary>
        /// Adds event into basket
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost("add-item")]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int eventId)
        {
            var userId = await _userService.GetUserId(User);
            var basket = await _basketService.AddBasketItem(eventId, userId);

            return Ok(basket);
        }
        /// <summary>
        /// Deletes event from basket
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpDelete("delete-item")]
        public async Task<ActionResult<BasketDto>> DeleteItemFromBasket(int eventId)
        {
            var userId = await _userService.GetUserId(User);
            await _basketService.RemoveBasketItem(eventId, userId);

            return Ok("Подію успішно видалено з кошика");
        }

    }
}
