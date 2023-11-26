using Ardalis.Specification;
using JetStudyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace JetStudyProject.Core.Specifications
{
    public static class Events
    {
        public class ByEventIdWithUserAndEventAndLectorers : Specification<Event>
        {
            public ByEventIdWithUserAndEventAndLectorers(int eventId)
            {
                Query
                    .Where(x => x.Id == eventId)
                    .Include(x => x.Creator)
                    .Include(x => x.Lecturers)
                    .ThenInclude(x => x.User)
                    .Include(x => x.ApplicationToEvents)
                    .Include(x => x.EventType);
            }
        }
        public class WithUserAndEventAndLectorers : Specification<Event>
        {
            public WithUserAndEventAndLectorers()
            {
                Query
                    .Include(x => x.Creator)
                    .Include(x => x.Lecturers)
                    .ThenInclude(x => x.User)
                    .Include(x => x.ApplicationToEvents)
                    .Include(x => x.EventType);
            }
        }
    }
}
