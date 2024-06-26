﻿using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.GenericRepository;
using JeyStudyProject.Infrastracture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _ctx;

        public UnitOfWork(DataContext _ctx)
        {
            this._ctx = _ctx;
        }

        public IGenericRepository<Event> eventRepository;

        public IGenericRepository<Event> EventRepository
        {
            get
            {
                if (this.eventRepository == null)
                {
                    this.eventRepository = new GenericRepository<Event>(_ctx);
                }
                return eventRepository;
            }
        }

        public IGenericRepository<ApplicationToEvent> applicationToEventRepository;

        public IGenericRepository<ApplicationToEvent> ApplicationToEventRepository
        {
            get
            {
                if (this.applicationToEventRepository == null)
                {
                    this.applicationToEventRepository = new GenericRepository<ApplicationToEvent>(_ctx);
                }
                return applicationToEventRepository;
            }
        }

        public IGenericRepository<ListenCourse> listenCourseRepository;

        public IGenericRepository<ListenCourse> ListenCourseRepository
        {
            get
            {
                if (this.listenCourseRepository == null)
                {
                    this.listenCourseRepository = new GenericRepository<ListenCourse>(_ctx);
                }
                return listenCourseRepository;
            }
        }

        public IGenericRepository<Category> categoryRepository;

        public IGenericRepository<Category> CategoryRepository
        {
            get
            {
                if (this.categoryRepository == null)
                {
                    this.categoryRepository = new GenericRepository<Category>(_ctx);
                }
                return categoryRepository;
            }
        }

        public IGenericRepository<EventType> eventTypeRepository;

        public IGenericRepository<EventType> EventTypeRepository
        {
            get
            {
                if (this.eventTypeRepository == null)
                {
                    this.eventTypeRepository = new GenericRepository<EventType>(_ctx);
                }
                return eventTypeRepository;
            }
        }

        public IGenericRepository<Basket> basketRepository;

        public IGenericRepository<Basket> BasketRepository
        {
            get
            {
                if (this.basketRepository == null)
                {
                    this.basketRepository = new GenericRepository<Basket>(_ctx);
                }
                return basketRepository;
            }
        }

        public IGenericRepository<BasketItem> basketItemRepository;

        public IGenericRepository<BasketItem> BasketItemRepository
        {
            get
            {
                if (this.basketItemRepository == null)
                {
                    this.basketItemRepository = new GenericRepository<BasketItem>(_ctx);
                }
                return basketItemRepository;
            }
        }

        public IGenericRepository<User> userRepository;

        public IGenericRepository<User> UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(_ctx);
                }
                return userRepository;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _ctx.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _ctx.SaveChangesAsync();
        }
    }
}
