using System.ComponentModel.DataAnnotations;

namespace Weatherstation.WebView.Models;

/// <summary>
/// Historische Wetterdaten für Analysen und Trends
/// </summary>
public class HistoricalWeatherData
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string StationId { get; set; }

    public double AvgTemperatureCelsius { get; set; }
    public double MinTemperatureCelsius { get; set; }
    public double MaxTemperatureCelsius { get; set; }
    public double TotalPrecipitationMm { get; set; }
    public double AvgHumidityPercentage { get; set; }
    public double AvgPressureHPa { get; set; }
    public double AvgWindSpeedKmh { get; set; }
    public double PeakWindSpeedKmh { get; set; }
    public double SunshineHours { get; set; }
    public string PrimaryWeatherCondition { get; set; }
}