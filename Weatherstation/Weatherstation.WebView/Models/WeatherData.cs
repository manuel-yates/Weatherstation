using System.ComponentModel.DataAnnotations;

namespace Weatherstation.WebView.Models;

/// <summary>
/// Hauptklasse für Wetterdaten
/// </summary>
public class WeatherData
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Timestamp { get; set; }

    // Geo-Location der Messstation
    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    public string StationName { get; set; }
    public string StationId { get; set; }

    // Temperaturdaten
    public double TemperatureCelsius { get; set; }
    public double FeelsLikeCelsius { get; set; }
    public double TemperatureMin { get; set; }
    public double TemperatureMax { get; set; }

    // Luftfeuchtigkeitsdaten
    public double HumidityPercentage { get; set; }
    public double DewPointCelsius { get; set; }

    // Luftdruckdaten
    public double PressureHPa { get; set; }
    public double SeaLevelPressureHPa { get; set; }
    public double PressureTrend { get; set; } // Änderungsrate über Zeit

    // Winddaten
    public double WindSpeedKmh { get; set; }
    public double WindDirectionDegrees { get; set; }
    public string WindDirectionCardinal { get; set; } // N, NO, O, usw.
    public double WindGustKmh { get; set; }

    // Niederschlagsdaten
    public double PrecipitationMm { get; set; }
    public double PrecipitationProbability { get; set; }
    public string PrecipitationType { get; set; } // Regen, Schnee, Hagel, etc.
    public double SnowDepthCm { get; set; }

    // Wolken- und Sichtdaten
    public int CloudCoverPercentage { get; set; }
    public double VisibilityKm { get; set; }
    public double UvIndex { get; set; }

    // Sonnenauf- und Untergang
    public DateTime? Sunrise { get; set; }
    public DateTime? Sunset { get; set; }
    public double DaylightHours { get; set; }

    // Warnungen und Beschreibungen
    public string WeatherCondition { get; set; } // klarer Himmel, bewölkt, Regen, etc.
    public string WeatherIcon { get; set; } // Icon-Code für UI

    // Beziehungen
    public virtual ICollection<WeatherAlert> Alerts { get; set; }
    public virtual ICollection<HourlyForecast> HourlyForecasts { get; set; }
    public virtual ICollection<DailyForecast> DailyForecasts { get; set; }
    public virtual WeatherSensorInfo SensorInfo { get; set; }
}