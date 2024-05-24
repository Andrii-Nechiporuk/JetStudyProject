using JetStudyProject.Infrastracture.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface IBasketService
    {
        Task<ActionResult<BasketDto>> AddBasketItem(int eventId, string userId);
        Task RemoveBasketItem(int eventId, string userId);
        Task<ActionResult<BasketDto>> GetBasket(string userId);
    }
}
