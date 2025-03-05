using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weatherstation.WebView.Models;

/// <summary>
/// Informationen über die Wettersensoren
/// </summary>
public class WeatherSensorInfo
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int WeatherDataId { get; set; }

    [ForeignKey("WeatherDataId")]
    public virtual WeatherData WeatherData { get; set; }

    public string SensorType { get; set; }
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public DateTime LastCalibration { get; set; }
    public double AccuracyTemperature { get; set; } // ±°C
    public double AccuracyHumidity { get; set; } // ±%
    public double AccuracyPressure { get; set; } // ±hPa
    public double BatteryLevel { get; set; } // Prozent
    public bool IsOnline { get; set; }
    public DateTime LastMaintenance { get; set; }
}