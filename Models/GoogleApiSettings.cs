using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleCalendarAPI.Models
{
    public class GoogleApiSettings : IGoogleApiSettings
    {
        public required string ClientId { get; set; }
        public required string ClientSecret { get; set; }
    }
}