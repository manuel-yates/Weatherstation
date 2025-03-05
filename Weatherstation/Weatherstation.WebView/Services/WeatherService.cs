using Microsoft.EntityFrameworkCore;
using Weatherstation.WebView.Data;
using Weatherstation.WebView.Models;

namespace Weatherstation.WebView.Services;

public class WeatherService
{
    private readonly WeatherDbContext _context;

    public WeatherService(WeatherDbContext context)
    {
        _context = context;
    }

    // Aktuelle Wetterdaten für alle Stationen abrufen
    public async Task<List<WeatherData>> GetAllCurrentWeatherDataAsync()
    {
        return await _context.WeatherData
            .Include(w => w.Alerts)
            .Include(w => w.SensorInfo)
            .ToListAsync();
    }

    // Aktuelle Wetterdaten für eine bestimmte Station abrufen
    public async Task<WeatherData?> GetCurrentWeatherDataAsync(string stationId)
    {
        return await _context.WeatherData
            .Include(w => w.Alerts)
            .Include(w => w.SensorInfo)
            .FirstOrDefaultAsync(w => w.StationId == stationId);
    }

    // Stündliche Vorhersage für eine Station abrufen
    public async Task<List<HourlyForecast>> GetHourlyForecastAsync(string stationId, int hours = 24)
    {
        var weatherData = await _context.WeatherData
            .FirstOrDefaultAsync(w => w.StationId == stationId);

        if (weatherData == null)
            return new List<HourlyForecast>();

        return await _context.HourlyForecasts
            .Where(h => h.WeatherDataId == weatherData.Id && 
                   h.ForecastTime <= DateTime.Now.AddHours(hours))
            .OrderBy(h => h.ForecastTime)
            .ToListAsync();
    }

    // Tägliche Vorhersage für eine Station abrufen
    public async Task<List<DailyForecast>> GetDailyForecastAsync(string stationId, int days = 7)
    {
        var weatherData = await _context.WeatherData
            .FirstOrDefaultAsync(w => w.StationId == stationId);

        if (weatherData == null)
            return new List<DailyForecast>();

        return await _context.DailyForecasts
            .Where(d => d.WeatherDataId == weatherData.Id && 
                   d.ForecastDate <= DateTime.Today.AddDays(days))
            .OrderBy(d => d.ForecastDate)
            .ToListAsync();
    }

    // Aktive Wetterwarnungen abrufen
    public async Task<List<WeatherAlert>> GetActiveAlertsAsync()
    {
        return await _context.WeatherAlerts
            .Include(a => a.WeatherData)
            .Where(a => a.ValidFrom <= DateTime.Now && a.ValidUntil >= DateTime.Now)
            .OrderByDescending(a => a.IssuedAt)
            .ToListAsync();
    }

    // Historische Wetterdaten für eine Station abrufen
    public async Task<List<HistoricalWeatherData>> GetHistoricalDataAsync(
        string stationId, DateTime startDate, DateTime endDate)
    {
        return await _context.HistoricalWeatherData
            .Where(h => h.StationId == stationId && 
                   h.Date >= startDate && h.Date <= endDate)
            .OrderBy(h => h.Date)
            .ToListAsync();
    }

    // Temperaturgrafik-Daten für eine Station abrufen (für die letzten X Tage)
    public async Task<List<HistoricalWeatherData>> GetTemperatureChartDataAsync(
        string stationId, int days = 30)
    {
        return await _context.HistoricalWeatherData
            .Where(h => h.StationId == stationId && 
                   h.Date >= DateTime.Today.AddDays(-days))
            .OrderBy(h => h.Date)
            .ToListAsync();
    }

    // Niederschlagsdaten für eine Station abrufen (für die letzten X Tage)
    public async Task<List<HistoricalWeatherData>> GetPrecipitationChartDataAsync(
        string stationId, int days = 30)
    {
        return await _context.HistoricalWeatherData
            .Where(h => h.StationId == stationId && 
                   h.Date >= DateTime.Today.AddDays(-days))
            .OrderBy(h => h.Date)
            .ToListAsync();
    }

    // Anzeigeeinstellungen eines Benutzers abrufen
    public async Task<WeatherDisplaySettings?> GetUserDisplaySettingsAsync(string userId)
    {
        return await _context.WeatherDisplaySettings
            .FirstOrDefaultAsync(s => s.UserId == userId);
    }

    // Anzeigeeinstellungen aktualisieren
    public async Task UpdateDisplaySettingsAsync(WeatherDisplaySettings settings)
    {
        var existingSettings = await _context.WeatherDisplaySettings
            .FirstOrDefaultAsync(s => s.UserId == settings.UserId);

        if (existingSettings != null)
        {
            _context.Entry(existingSettings).CurrentValues.SetValues(settings);
        }
        else
        {
            _context.WeatherDisplaySettings.Add(settings);
        }

        await _context.SaveChangesAsync();
    }

// Wetterstationsinformationen abrufen
    public async Task<List<(string Id, string Name)>> GetWeatherStationsAsync()
    {
        var stations = await _context.WeatherData
            .Select(w => new { w.StationId, w.StationName })
            .Distinct()
            .OrderBy(w => w.StationName)
            .ToListAsync();
        
        return stations.Select(s => (s.StationId, s.StationName)).ToList();
    }
}