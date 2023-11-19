using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.DataAccess
{
    public interface IUnitOfWork
    {
        IGenericRepository<Event> EventRepository { get; }
        IGenericRepository<ApplicationToEvent> ApplicationToEventRepository { get; }
        Task SaveAsync();
    }
}
