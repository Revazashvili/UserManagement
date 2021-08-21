using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
    {
        public void Configure(EntityTypeBuilder<WeatherForecast> builder)
        {
            builder.Property(d => d.Date)
                .IsRequired();
            
            builder.Property(s => s.Summary)
                .IsRequired()
                .HasMaxLength(300);
        }
    }
}