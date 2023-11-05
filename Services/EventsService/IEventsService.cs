using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3.Data;
using GoogleCalendarAPI.Dtos;
using GoogleCalendarAPI.Dtos.Event;
using GoogleCalendarAPI.Models;

namespace GoogleCalendarAPI.Services.EventsService
{
    public interface IEventsService
    {
        public Task<ServiceResponse<GetEventDto>> CreateEvent(CreateEventDto calenderEvent);

        public Task<ServiceResponse<GetEventsDto>> GetEvents(DateTime? startDate, DateTime? endDate, string? searchQuery, string? pageToken);

        public Task<ServiceResponse<string>> DeleteEvent(string eventId);

    }
}