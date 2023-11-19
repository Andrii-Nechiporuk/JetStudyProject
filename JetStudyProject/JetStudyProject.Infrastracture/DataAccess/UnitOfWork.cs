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
