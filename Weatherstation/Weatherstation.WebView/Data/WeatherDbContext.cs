using Microsoft.EntityFrameworkCore;
using Weatherstation.WebView.Models;

namespace Weatherstation.WebView.Data;

public class WeatherDbContext : DbContext
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) 
        : base(options)
    {
    }

    public DbSet<WeatherData> WeatherData { get; set; }
    public DbSet<WeatherAlert> WeatherAlerts { get; set; }
    public DbSet<DailyForecast> DailyForecasts { get; set; }
    public DbSet<HourlyForecast> HourlyForecasts { get; set; }
    public DbSet<HistoricalWeatherData> HistoricalWeatherData { get; set; }
    public DbSet<WeatherSensorInfo> WeatherSensorInfo { get; set; }
    public DbSet<WeatherDisplaySettings> WeatherDisplaySettings { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Konfiguriere One-to-Many Beziehung zwischen WeatherData und WeatherAlert
        modelBuilder.Entity<WeatherAlert>()
            .HasOne(a => a.WeatherData)
            .WithMany(w => w.Alerts)
            .HasForeignKey(a => a.WeatherDataId);
        
        // Konfiguriere One-to-Many Beziehung zwischen WeatherData und HourlyForecast
        modelBuilder.Entity<HourlyForecast>()
            .HasOne(h => h.WeatherData)
            .WithMany(w => w.HourlyForecasts)
            .HasForeignKey(h => h.WeatherDataId);
        
        // Konfiguriere One-to-Many Beziehung zwischen WeatherData und DailyForecast
        modelBuilder.Entity<DailyForecast>()
            .HasOne(d => d.WeatherData)
            .WithMany(w => w.DailyForecasts)
            .HasForeignKey(d => d.WeatherDataId);
        
        // Konfiguriere One-to-One Beziehung zwischen WeatherData und WeatherSensorInfo
        modelBuilder.Entity<WeatherSensorInfo>()
            .HasOne(s => s.WeatherData)
            .WithOne(w => w.SensorInfo)
            .HasForeignKey<WeatherSensorInfo>(s => s.WeatherDataId);
    }
}