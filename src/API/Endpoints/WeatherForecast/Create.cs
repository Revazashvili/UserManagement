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
    public class Create : BaseAsyncEndpoint
        .WithRequest<CreateWeatherForecastCommand>
        .WithResponse<IResponse<int>>
    {
        private readonly IMediator _mediator;

        public Create(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        [SwaggerOperation(Description = "Creates new WeatherForecast",
            Summary = "Creates new WeatherForecast and returns affected row number",
            OperationId = "WeatherForecast.Create",
            Tags = new []{ "WeatherForecast" })]
        public override async Task<ActionResult<IResponse<int>>> HandleAsync([FromBody]CreateWeatherForecastCommand request, 
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Succeeded) return Ok(result);
            return BadRequest(result);
        }
    }
}