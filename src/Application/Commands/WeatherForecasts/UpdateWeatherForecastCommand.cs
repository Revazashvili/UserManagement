using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.WeatherForecasts
{
    public record UpdateWeatherForecastCommand(long Id,DateTime Date,int TemperatureC,string Summary) : IRequestWrapper<int>{}
    
    public class UpdateWeatherForecastCommandHandler : IHandlerWrapper<UpdateWeatherForecastCommand,int>
    {
        private readonly IApplicationDbContext _context;

        public UpdateWeatherForecastCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponse<int>> Handle(UpdateWeatherForecastCommand request, CancellationToken cancellationToken)
        {
            var weatherForecast =
                await _context.WeatherForecasts.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (weatherForecast is null) return Response.Fail<int>("Can't be found weather forecast with this id");
            weatherForecast.Date = request.Date;
            weatherForecast.TemperatureC = request.TemperatureC;
            weatherForecast.Summary = request.Summary;
            var updateRowCount = await _context.SaveChangesAsync(cancellationToken);
            return updateRowCount > 0
                ? Response.Success<int>(updateRowCount)
                : Response.Fail<int>("Can't update weather forecast");
        }
    }
}