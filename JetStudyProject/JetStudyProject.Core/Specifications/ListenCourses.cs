using Ardalis.Specification;
using JetStudyProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Core.Specifications
{
    public static class ListenCourses
    {
        public class ByUserIdAndEventId : Specification<ListenCourse>
        {
            public ByUserIdAndEventId(int eventId, string userId)
            {
                Query
                    .Where(x => x.EventId == eventId && x.UserId == userId);
            }
        }
    }
}
