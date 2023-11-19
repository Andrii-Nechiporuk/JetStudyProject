using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Core.Entities
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Thumbnail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public double? Price { get; set; }
        public string? ShortDescription { get; set; }
        public string? LongDescription { get; set; }
        public string? TargetedAudience { get; set; }
        public int? SeatsAvailable { get; set; }
        public string? Location {  get; set; }
        public string? AdditionalRecources {  get; set; }
        public bool? IsOnline { get; set; }
        public User Creator { get; set; }
        public string CreatorId {  get; set; }
        public EventType? EventType { get; set; }
        public int EventTypeId { get; set; }
        public StatusForInstructor? StatusForInstructor { get; set; }
        public int StatusForInstructorId { get; set; }
        public StatusForAdministrator? StatusForAdministrator { get; set; }
        public int StatusForAdministratorId { get; set; }
        public StatusForStudent? StatusForStudent { get; set; }
        public int StatusForStudentId { get; set; }
        public virtual ICollection<ReadCourse> Lecturers { get; set; }
        public virtual ICollection<ListenCourse> Students { get; set; }
        public virtual ICollection<ApplicationToEvent> ApplicationToEvents { get; set; }
    }
}
