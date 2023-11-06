using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using GoogleCalendarAPI.Dtos;
using GoogleCalendarAPI.Services.EventsService;
using GoogleCalendarAPI.Dtos.Event;
using GoogleCalendarAPI.Models;

namespace GoogleCalendarAPI.Controllers
{

    [Route("api/events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;

        }


        /// <summary>
        /// Create a new event in the user's Google Calendar.
        /// </summary>
        /// <param name="eventModel">Event details</param>
        /// <returns>details of the created event.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResponse<GetEventDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto eventModel)
        {
            ServiceResponse<GetEventDto> response = await _eventsService.CreateEvent(eventModel);

            if (response.Data != null)
            {
                return Created($"api/events/{response.Data.Id}", response);

            }

            return BadRequest(response);
        }

        /// <summary>
        /// Get all the events in the user's Google Calendar.
        /// </summary>
        /// <param name="startDate">date to start from</param>
        /// <param name="endDate">date to end with</param>
        /// <param name="searchQuery">text search term to find in the events</param>
        /// <param name="pageToken">Token specifying which result page to return to handle pagination.</param>
        /// <returns>list of events in the user's Google Calendar.</returns>
        [ProducesResponseType(typeof(ServiceResponse<GetEventsDto>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IActionResult> GetEvents(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? searchQuery,
            [FromQuery] string? pageToken)
        {

            return Ok(await _eventsService.GetEvents(startDate, endDate, searchQuery, pageToken));
        }

        /// <summary>
        /// Delete and event from the user's Google Calendar.
        /// </summary>
        /// <param name="id">the event Id</param>
        /// <returns>No content response if successfully delete the event.</returns>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ServiceResponse<>), StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            ServiceResponse<string> response = await _eventsService.DeleteEvent(id);

            if (response.Success == false)
            {
                return NotFound(response);
            }

            return NoContent();
        }

    }

}