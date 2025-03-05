using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weatherstation.WebView.Models;

/// <summary>
/// Tägliche Wettervorhersage
/// </summary>
public class DailyForecast
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int WeatherDataId { get; set; }

    [ForeignKey("WeatherDataId")]
    public virtual WeatherData WeatherData { get; set; }

    [Required]
    public DateTime ForecastDate { get; set; }

    public double TemperatureMinCelsius { get; set; }
    public double TemperatureMaxCelsius { get; set; }
    public double FeelsLikeMinCelsius { get; set; }
    public double FeelsLikeMaxCelsius { get; set; }
    public double HumidityPercentage { get; set; }
    public double WindSpeedKmh { get; set; }
    public double WindDirectionDegrees { get; set; }
    public double PrecipitationProbability { get; set; }
    public double PrecipitationMm { get; set; }
    public double SnowDepthCm { get; set; }
    public string WeatherCondition { get; set; }
    public string WeatherIcon { get; set; }
    public DateTime Sunrise { get; set; }
    public DateTime Sunset { get; set; }
    public double UvIndex { get; set; }
    public double MoonPhase { get; set; } // 0-1 (Neumond bis Vollmond)
    public string MoonPhaseDescription { get; set; }
}