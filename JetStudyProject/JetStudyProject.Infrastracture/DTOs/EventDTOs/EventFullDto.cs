using JetStudyProject.Infrastracture.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.DTOs.EventDTOs
{
    public class EventFullDto
    {
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
        public string? Location { get; set; }
        public string? AdditionalRecources { get; set; }
        public bool? IsOnline { get; set; }
        public UserDto? Creator { get; set; }
        public string? EventType { get; set; }
        public bool? WaitingForConfirmation { get; set; }
        public bool? IsAllowedToWatchAllContent { get; set; }
        public List<UserDto>? Lecturers { get; set; }
    }
}
