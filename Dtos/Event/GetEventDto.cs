using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleCalendarAPI.Dtos.Event
{
    public class GetEventDto
    {
        public required string Id { get; set; }
        public required string Creator { get; set; }
        public required string Summary { get; set; }
        public required string Location { get; set; }
        public required string Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
    }
}