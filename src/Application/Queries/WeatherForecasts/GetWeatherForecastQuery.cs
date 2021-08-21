using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.DTOs.WeatherForecast;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Queries.WeatherForecasts
{
    public record GetWeatherForecastQuery(Expression<Func<WeatherForecast,bool>> Predicate) : IRequestWrapper<WeatherForecastDto>{}
    
    public class GetWeatherForecastQueryHandler : IHandlerWrapper<GetWeatherForecastQuery,WeatherForecastDto>
    {
        private readonly IApplicationDbContext _context;

        public GetWeatherForecastQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponse<WeatherForecastDto>> Handle(GetWeatherForecastQuery request, CancellationToken cancellationToken)
        {
            var weatherForecast =
                await _context.WeatherForecasts
                    .FirstOrDefaultAsync(request.Predicate, cancellationToken);
            return weatherForecast is not null
                ? Response.Success(new WeatherForecastDto(weatherForecast!.Id, weatherForecast.Date,
                    weatherForecast.TemperatureC, weatherForecast.Summary))
                : Response.Fail<WeatherForecastDto>("Can't find any weather forecast");
        }
    }
}