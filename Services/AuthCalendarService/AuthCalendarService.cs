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
        private UserCredential? _userCredential;


        public AuthCalendarService(IOptions<GoogleApiSettings> settings)
        {
            _settings = settings.Value;
        }


        private async Task<UserCredential> GetUserCredential()
        {
            if (_userCredential == null)
            {
                _userCredential = await AuthorizeUserAsync();
            }
            else if (_userCredential.Token.ExpiresInSeconds < 60)
            {
                _userCredential = await RefreshAccessToken(_userCredential);
            }

            return _userCredential;
        }

        private async Task<UserCredential> AuthorizeUserAsync()
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

            return credential;

        }



        private static async Task<UserCredential> RefreshAccessToken(UserCredential userCredential)
        {
            await userCredential.RefreshTokenAsync(CancellationToken.None);

            return userCredential;
        }

        public async Task<CalendarService> CreateCalendarService()
        {

            UserCredential userCredential = await GetUserCredential();

            CalendarService service = new CalendarService(new BaseClientService.Initializer
            {
                HttpClientInitializer = userCredential,
                ApplicationName = "Calendar API"
            });

            return service;

        }
    }
}