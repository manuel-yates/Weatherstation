using System.ComponentModel.DataAnnotations;

namespace Weatherstation.WebView.Models
{
    /// <summary>
    /// Konfiguration für Darstellung und Präferenzen
    /// </summary>
    public class WeatherDisplaySettings
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; }
        
        public bool UseMetricSystem { get; set; } = true;
        public bool ShowFeelsLike { get; set; } = true;
        public bool ShowUvIndex { get; set; } = true;
        public bool ShowHumidity { get; set; } = true;
        public bool ShowPressure { get; set; } = true;
        public bool ShowWindInfo { get; set; } = true;
        public bool ShowPrecipitation { get; set; } = true;
        public bool ShowSunriseSunset { get; set; } = true;
        public bool EnableNotifications { get; set; } = false;
        public bool NotifyOnAlerts { get; set; } = true;
        public bool NotifyOnRain { get; set; } = false;
        public bool NotifyOnTemperatureChange { get; set; } = false;
        public double TemperatureChangeThreshold { get; set; } = 5.0;
        public string PreferredChartType { get; set; } = "Line";
        public string ThemePreference { get; set; } = "Auto";
        public int DefaultForecastDays { get; set; } = 5;
        public List<string> FavoriteLocations { get; set; }
    }
}