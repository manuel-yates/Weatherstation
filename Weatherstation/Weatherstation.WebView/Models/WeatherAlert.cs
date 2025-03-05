using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Weatherstation.WebView.Models;

/// <summary>
/// Wetterwarnungen
/// </summary>
public class WeatherAlert
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int WeatherDataId { get; set; }

    [ForeignKey("WeatherDataId")]
    public virtual WeatherData WeatherData { get; set; }

    [Required]
    public string AlertType { get; set; } // Sturm, Überschwemmung, Hitze, etc.

    [Required]
    public string Severity { get; set; } // Niedrig, Mittel, Hoch, Extrem

    [Required]
    public string Title { get; set; }

    public string Description { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime ValidFrom { get; set; }
    public DateTime ValidUntil { get; set; }
    public string SourceName { get; set; } // z.B. DWD, ZAMG, etc.
    public string AreaAffected { get; set; } // Betroffenes Gebiet
}