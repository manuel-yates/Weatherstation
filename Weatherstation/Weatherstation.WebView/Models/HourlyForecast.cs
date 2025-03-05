using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weatherstation.WebView.Models;

/// <summary>
/// Stündliche Wettervorhersage
/// </summary>
public class HourlyForecast
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int WeatherDataId { get; set; }

    [ForeignKey("WeatherDataId")]
    public virtual WeatherData WeatherData { get; set; }

    [Required]
    public DateTime ForecastTime { get; set; }

    public double TemperatureCelsius { get; set; }
    public double FeelsLikeCelsius { get; set; }
    public double HumidityPercentage { get; set; }
    public double WindSpeedKmh { get; set; }
    public double WindDirectionDegrees { get; set; }
    public double PrecipitationProbability { get; set; }
    public double PrecipitationMm { get; set; }
    public string WeatherCondition { get; set; }
    public string WeatherIcon { get; set; }
    public double PressureHPa { get; set; }
    public int CloudCoverPercentage { get; set; }
    public double VisibilityKm { get; set; }
}