using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;

namespace GoogleCalendarAPI.Dtos.Event
{
    public class GetEventsDto
    {
        public required IEnumerable<GetEventDto> Events { get; set; }

        public string? NextPageToken { get; set; }
    }
}