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
    public class Update : BaseAsyncEndpoint
        .WithRequest<UpdateWeatherForecastCommand>
        .WithResponse<IResponse<int>>
    {
        private readonly IMediator _mediator;

        public Update(IMediator mediator) => _mediator = mediator;
        
        [HttpPut]
        [SwaggerOperation(Description = "Updates weather forecast",
            Summary = "Updates weather forecast and returns affected row number",
            OperationId = "WeatherForecast.Update",
            Tags = new []{ "WeatherForecast" })]
        public override async Task<ActionResult<IResponse<int>>> HandleAsync([FromBody]UpdateWeatherForecastCommand request, 
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(request, cancellationToken);
            if (result.Succeeded) return Ok(result);
            return BadRequest(result);
        }
    }
}