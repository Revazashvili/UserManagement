using System;
using Domain.Common;

namespace Domain.Entities
{
    public class WeatherForecast : Auditable
    {
        public WeatherForecast(DateTime date, int temperatureC, string summary)
        {
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public int TemperatureC { get; set; }
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
        public string Summary { get; set; }
    }
}