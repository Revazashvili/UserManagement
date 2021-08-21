using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using API.Routes;
using Application.Common.DTOs.WeatherForecast;
using Application.Common.Models;
using Application.Queries.WeatherForecasts;
using Ardalis.ApiEndpoints;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Endpoints.WeatherForecast
{
    [Route(ApiRoutes.WeatherForecast)]
    public class Get : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<IResponse<IReadOnlyList<WeatherForecastDto>>>
    {
        private readonly IMediator _mediator;
        public Get(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [SwaggerOperation(Description = "Returns all weather forecast",
            Summary = "Returns all weather forecast",
            OperationId = "WeatherForecast.Get",
            Tags = new []{ "WeatherForecast" })]
        public override async Task<ActionResult<IResponse<IReadOnlyList<WeatherForecastDto>>>> HandleAsync(
            CancellationToken cancellationToken = new())
        {
            var result = await _mediator.Send(new GetWeatherForecastsQuery(),cancellationToken);
            if (result.Succeeded)
                return Ok(result);
            return BadRequest(result);
        }
    }
}