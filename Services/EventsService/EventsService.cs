using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using GoogleCalendarAPI.Dtos;
using GoogleCalendarAPI.Dtos.Event;
using GoogleCalendarAPI.Models;
using Microsoft.AspNetCore.Http.Features;
using GoogleCalendarAPI.Services.AuthCalendarService;
using System.Data;
using System.Globalization;
using System.ComponentModel;

namespace GoogleCalendarAPI.Services.EventsService
{
    public class EventsService : IEventsService
    {
        private readonly IAuthCalendarService _calendarService;

        public EventsService(IAuthCalendarService calendarService)
        {
            _calendarService = calendarService;

        }

        [Obsolete]
        public async Task<ServiceResponse<GetEventDto>> CreateEvent(CreateEventDto calendarEvent)
        {
            ServiceResponse<GetEventDto> serviceResponse = new ServiceResponse<GetEventDto>();

            try
            {

                if (calendarEvent == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Event data is missing.";
                    return serviceResponse;
                }

                if (calendarEvent.Start < DateTime.Now || calendarEvent.Start > calendarEvent.End)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Event start date cannot be in the past or after the end date.";
                    return serviceResponse;
                }

                if (calendarEvent.Start.DayOfWeek == DayOfWeek.Friday || calendarEvent.Start.DayOfWeek == DayOfWeek.Saturday)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Events cannot be created on Fridays or Saturdays.";
                    return serviceResponse;
                }

                CalendarService service = await _calendarService.CreateCalendarService();


                Event googleEvent = new Event()
                {
                    Summary = calendarEvent.Summary,
                    Location = calendarEvent.Location,
                    Description = calendarEvent.Description,
                    Start = new EventDateTime { DateTimeDateTimeOffset = new DateTimeOffset(calendarEvent.Start), TimeZone = "Africa/Cairo" },
                    End = new EventDateTime { DateTimeDateTimeOffset = new DateTimeOffset(calendarEvent.End), TimeZone = "Africa/Cairo" }
                };

                Event request = service.Events.Insert(googleEvent, "primary").Execute();


                GetEventDto eventItem = new()
                {
                    Id = request.Id,
                    Creator = request.Creator.Email,
                    Summary = request.Summary,
                    Location = request.Location,
                    Description = request.Description,
                    Start = request.Start.DateTime,
                    End = request.End.DateTime
                };

                string createdEvent = service.Serializer.Serialize(request);

                serviceResponse.Data = eventItem;
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }

        [Obsolete]
        public async Task<ServiceResponse<GetEventsDto>> GetEvents(DateTime? startDate, DateTime? endDate, string? searchQuery, string? pageToken)
        {
            ServiceResponse<GetEventsDto> serviceResponse = new ServiceResponse<GetEventsDto>();

            try
            {
                CalendarService service = await _calendarService.CreateCalendarService();

                EventsResource.ListRequest request = service.Events.List("primary");

                if (startDate.HasValue)
                {
                    request.TimeMin = startDate.Value.ToUniversalTime();
                }

                if (endDate.HasValue)
                {
                    request.TimeMax = endDate.Value.ToUniversalTime();
                }

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    request.Q = searchQuery;
                }

                if (!string.IsNullOrEmpty(pageToken))
                {
                    request.PageToken = pageToken;
                }

                Events events = request.Execute();

                IEnumerable<GetEventDto> eventList = events.Items.Select(eventItem =>
                {
                    return new GetEventDto()
                    {
                        Id = eventItem.Id,
                        Creator = eventItem.Creator.Email,
                        Description = eventItem.Description,
                        Location = eventItem.Location,
                        Summary = eventItem.Summary,
                        End = eventItem.End.DateTime,
                        Start = eventItem.Start.DateTime
                    };
                });

                serviceResponse.Data = new GetEventsDto()
                {
                    Events = eventList,
                    NextPageToken = events.NextPageToken
                };


            }
            catch (Exception e)
            {

                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }
        public async Task<ServiceResponse<string>> DeleteEvent(string eventId)
        {
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            try
            {

                CalendarService service = await _calendarService.CreateCalendarService();


                Event retrievedEvent = await service.Events.Get("primary", eventId).ExecuteAsync();

                if (retrievedEvent != null)
                {
                    await service.Events.Delete("primary", eventId).ExecuteAsync();
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Event not found.";

                }
            }
            catch (Exception e)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = e.Message;
            }

            return serviceResponse;
        }
    }
}