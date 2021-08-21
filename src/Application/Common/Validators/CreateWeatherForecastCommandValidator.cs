using System;
using Application.Commands.WeatherForecasts;
using FluentValidation;

namespace Application.Common.Validators
{
    public class CreateWeatherForecastCommandValidator : AbstractValidator<CreateWeatherForecastCommand>
    {
        public CreateWeatherForecastCommandValidator()
        {
            RuleFor(x => x.TemperatureC)
                .NotNull().NotEmpty().WithMessage("TemperatureC must be provided");

            RuleFor(x => x.Date)
                .NotEmpty().NotNull().WithMessage("Date must be provided")
                .GreaterThanOrEqualTo(DateTime.Today).WithMessage("Date must be greater that today");

            RuleFor(x => x.Summary)
                .NotNull().NotEmpty().WithMessage("Summary must be provided");
        }
    }
}