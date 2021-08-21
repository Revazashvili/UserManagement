using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.WeatherForecast;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.WeatherForecasts
{
    public record GetWeatherForecastsQuery : IRequestWrapper<IReadOnlyList<WeatherForecastDto>>{}
    
    public class GetWeatherForecastsQueryHandler : IHandlerWrapper<GetWeatherForecastsQuery,IReadOnlyList<WeatherForecastDto>>
    {
        private readonly IApplicationDbContext _context;
        public GetWeatherForecastsQueryHandler(IApplicationDbContext context) { _context = context; }

        public async Task<IResponse<IReadOnlyList<WeatherForecastDto>>> Handle(GetWeatherForecastsQuery request, CancellationToken cancellationToken)
        {
            IReadOnlyList<WeatherForecastDto> weatherForecasts = await _context.WeatherForecasts
                .Select(w => new WeatherForecastDto(w.Id, w.Date, w.TemperatureC, w.Summary))
                .ToListAsync(cancellationToken);
            
            return weatherForecasts.Any()
                ? Response.Success(weatherForecasts)
                : Response.Fail<IReadOnlyList<WeatherForecastDto>>("Can't find any record");
        }
    }
}