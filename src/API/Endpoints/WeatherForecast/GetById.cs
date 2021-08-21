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
    public class GetById : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<IResponse<WeatherForecastDto>>
    {
        private readonly IMediator _mediator;

        public GetById(IMediator mediator) => _mediator = mediator;

        [HttpGet("{Id}")]
        [SwaggerOperation(Description = "Returns weather forecast by id",
            Summary = "Returns weather forecast by id",
            OperationId = "WeatherForecast.GetById",
            Tags = new []{ "WeatherForecast" })]
        public override async Task<ActionResult<IResponse<WeatherForecastDto>>> HandleAsync(int Id, CancellationToken cancellationToken = new CancellationToken())
        {
            var result = await _mediator.Send(new GetWeatherForecastQuery(x => x.Id == Id), cancellationToken);
            if (result.Succeeded) return Ok(result);
            return BadRequest(result);
        }
    }
}