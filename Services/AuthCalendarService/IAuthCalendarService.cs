using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Calendar.v3;


namespace GoogleCalendarAPI.Services.AuthCalendarService
{
    public interface IAuthCalendarService
    {
        public Task<CalendarService> CreateCalendarService();

    }
}