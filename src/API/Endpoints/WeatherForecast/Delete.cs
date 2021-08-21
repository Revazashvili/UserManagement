using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Commands.WeatherForecasts;
using Application.Common.Models;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.WeatherForecast
{
    [Route(ApiRoutes.WeatherForecast)]
    public class Delete : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<IResponse<int>>
    {
        private readonly IMediator _mediator;

        public Delete(IMediator mediator) => _mediator = mediator;
        
        [HttpDelete]
        [SwaggerOperation(Description = "Deletes weather forecast",
            Summary = "Deletes weather forecast and returns affected row number",
            OperationId = "WeatherForecast.Delete",
            Tags = new []{ "WeatherForecast" })]
        public override async Task<ActionResult<IResponse<int>>> HandleAsync([FromQuery]int Id,
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new DeleteWeatherForecastCommand(Id), cancellationToken);
            if (result.Succeeded) return Ok(result);
            return BadRequest(result);
        }
    }
}