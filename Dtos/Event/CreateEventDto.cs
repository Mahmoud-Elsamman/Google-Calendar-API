using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleCalendarAPI.Dtos
{
    public class CreateEventDto
    {
        public string Summary { get; set; } = "This is the default summary.";
        public string? Location { get; set; }
        public string Description { get; set; } = "This is the default description.";
        public required DateTime Start { get; set; }
        public required DateTime End { get; set; }
    }
}
