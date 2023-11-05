using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleCalendarAPI.Models
{
    public interface IGoogleApiSettings
    {
        string ClientId { get; set; }
        string ClientSecret { get; set; }
    }
}