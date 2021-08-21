using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace Application.Commands.WeatherForecasts
{
    public record DeleteWeatherForecastCommand(long Id) : IRequestWrapper<int>{}
    
    public class DeleteWeatherForecastCommandHandler : IHandlerWrapper<DeleteWeatherForecastCommand,int>
    {
        private readonly IApplicationDbContext _context;

        public DeleteWeatherForecastCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IResponse<int>> Handle(DeleteWeatherForecastCommand request, CancellationToken cancellationToken)
        {
            var weatherForecast = await _context.WeatherForecasts.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
            if (weatherForecast is null) return Response.Fail<int>("Can't find weather forecast with this id");
            _context.WeatherForecasts.Remove(weatherForecast);
            var deletedRowCount = await _context.SaveChangesAsync(cancellationToken);
            return deletedRowCount > 0
                ? Response.Success<int>(deletedRowCount)
                : Response.Fail<int>("Can't find weather forecast");
        }
    }
}