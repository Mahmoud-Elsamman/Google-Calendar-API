using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using GoogleCalendarAPI.Models;
using Microsoft.Extensions.Options;

namespace GoogleCalendarAPI.Services.AuthCalendarService
{
    public class AuthCalendarService : IAuthCalendarService
    {
        private readonly GoogleApiSettings _settings;

        public AuthCalendarService(IOptions<GoogleApiSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<CalendarService> CreateCalendarService()
        {
            UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = _settings.ClientId,
                    ClientSecret = _settings.ClientSecret
                },
                new[] { CalendarService.Scope.Calendar },
                "user",
                CancellationToken.None
            );

            CalendarService service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Calendar API"
            });

            return service;

        }
    }
}