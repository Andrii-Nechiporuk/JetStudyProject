using AutoMapper;
using Azure;
using JetStudyProject.Core.Entities;
using JetStudyProject.Core.Specifications;
using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.BasketDTOs;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using JetStudyProject.Infrastracture.Resources;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Services
{
    public class BasketService : IBasketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        public BasketService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }
        public async Task<ActionResult<BasketDto>> AddBasketItem(int eventId, string userId)
        {
            var basket = await RetrieveBasket(userId);
            if (basket == null) basket = await CreateBasket(userId);

            var eventToAdd = _unitOfWork.EventRepository.GetById(eventId);

            var basketItemCheck = basket.BasketItems.FirstOrDefault(x => x.EventId == eventId);
            
            if (basketItemCheck != null)
                throw new HttpException("Подія вже у вашому кошику", HttpStatusCode.BadRequest);

            if (eventToAdd == null)
                throw new HttpException("Invalid event id", HttpStatusCode.BadRequest);

            await _unitOfWork.BasketItemRepository.Insert(new BasketItem 
            {
            BasketId = basket.Id,
            EventId = eventId
            });

            await _unitOfWork.SaveAsync();

            var basketFromDb = await RetrieveBasket(userId);

            var basketToSend = _mapper.Map<BasketDto>(basketFromDb);

            return basketToSend;
        }
        public async Task RemoveBasketItem(int eventId, string userId)
        {
            var basket = await RetrieveBasket(userId);

            if (basket == null)
                throw new HttpException("Issue with basket", HttpStatusCode.BadRequest);

            var basketItem = basket.BasketItems.FirstOrDefault(x => x.EventId == eventId);
            
            if (basketItem == null)
                throw new HttpException("You don`t have this event in basket", HttpStatusCode.BadRequest);

            _unitOfWork.BasketItemRepository.Delete(basketItem);

            await _unitOfWork.SaveAsync();
        }
        public async Task<ActionResult<BasketDto>> GetBasket(string userId)
        {
            var basketFromDb = await RetrieveBasket(userId);
            if (basketFromDb == null)
            return null;

            return _mapper.Map<BasketDto>(basketFromDb);

        }
        private async Task<Basket> CreateBasket(string userId)
        {
            var basket = new Basket { UserId = userId };
            await _unitOfWork.BasketRepository.Insert(basket);
            await _unitOfWork.SaveAsync();

            var basketFromDb = _unitOfWork.BasketRepository.GetFirstBySpec(new Baskets.ByUserId(userId));
            var userFromDb = _unitOfWork.UserRepository.GetById(userId);
            userFromDb.BasketId = basketFromDb.Id;

            _unitOfWork.UserRepository.Update(userFromDb);
            await _unitOfWork.SaveAsync();

            return basketFromDb;
        }
        private async Task<Basket> RetrieveBasket(string userId)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            var basket = _unitOfWork.BasketRepository.GetFirstBySpec(new Baskets.ByBasketIdWithBasketItems(user.BasketId));

            return basket;
        }
    }
}
