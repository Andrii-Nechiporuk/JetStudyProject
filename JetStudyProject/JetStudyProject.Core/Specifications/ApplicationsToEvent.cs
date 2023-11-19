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
    public static class ApplicationsToEvent
    {
        public class ByUserIdAndEventId : Specification<ApplicationToEvent>
        {
            public ByUserIdAndEventId(int eventId, string userId)
            {
                Query
                    .Where(x => x.EventId == eventId && x.UserId == userId);
            }
        }
    }
}
