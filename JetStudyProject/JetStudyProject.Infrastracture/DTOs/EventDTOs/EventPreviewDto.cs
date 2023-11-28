using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.DTOs.EventDTOs
{
    public class EventPreviewDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Thumbnail { get; set; }
        public string? ImageSrc { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public UserDto? Creator { get; set; }
        public string? EventType { get; set; }
        public int EventTypeId { get; set; }
        public int CategoryId { get; set; }
        public List<UserDto>? Lecturers { get; set; }
    }
}
