using System;

namespace Application.Common.DTOs.WeatherForecast
{
    public record WeatherForecastDto(long Id, DateTime Date, int TemperatureC, string Summary)
    {
        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
    }
}