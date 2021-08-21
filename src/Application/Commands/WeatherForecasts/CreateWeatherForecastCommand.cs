using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Domain.Entities;

namespace Application.Commands.WeatherForecasts
{
    public record CreateWeatherForecastCommand(DateTime Date,int TemperatureC,string Summary) : IRequestWrapper<int>{}
    
    public class CreateWeatherForecastCommandHandler : IHandlerWrapper<CreateWeatherForecastCommand,int>
    {
        private readonly IApplicationDbContext _context;

        public CreateWeatherForecastCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponse<int>> Handle(CreateWeatherForecastCommand request, CancellationToken cancellationToken)
        {
            var weatherForecast = new WeatherForecast(request.Date, request.TemperatureC, request.Summary);
            await _context.WeatherForecasts.AddAsync(weatherForecast, cancellationToken);
            var insertedRowCount = await _context.SaveChangesAsync(cancellationToken);
            return insertedRowCount > 0
                ? Response.Success(insertedRowCount)
                : Response.Fail<int>("Can't insert record");
        }
    }
    
}