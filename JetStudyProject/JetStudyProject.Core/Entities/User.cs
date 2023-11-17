using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Core.Entities
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Thumbnail { get; set; }
        public virtual ICollection<ReadCourse> EventsToTeach { get; set; }
        public virtual ICollection<ListenCourse> EventsToListen { get; set; }
        public virtual ICollection<Event> CreatedEvents { get; set; }
    }
}
