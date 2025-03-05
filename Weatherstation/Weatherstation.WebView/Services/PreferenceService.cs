using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using Weatherstation.WebView.Data;
using Weatherstation.WebView.Models;

namespace Weatherstation.WebView.Services
{
    public interface IPreferenceService
    {
        Task<WeatherDisplaySettings> GetUserDisplaySettingsAsync(string userId);
        Task SetDarkMode(bool isDarkMode);
        Task<bool> IsSystemInDarkMode();
    }

    public class PreferenceService : IPreferenceService
    {
        private readonly WeatherDbContext _context;
        private readonly IJSRuntime _jsRuntime;

        public PreferenceService(WeatherDbContext context, IJSRuntime jsRuntime)
        {
            _context = context;
            _jsRuntime = jsRuntime;
        }

        public async Task<WeatherDisplaySettings> GetUserDisplaySettingsAsync(string userId)
        {
            return await _context.WeatherDisplaySettings.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task SetDarkMode(bool isDarkMode)
        {
            var settings = await GetUserDisplaySettingsAsync("default-user");
            if (settings != null)
            {
                settings.ThemePreference = isDarkMode ? "Dark" : "Light";
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsSystemInDarkMode()
        {
            try
            {
                return await _jsRuntime.InvokeAsync<bool>("window.matchMedia('(prefers-color-scheme: dark)').matches");
            }
            catch
            {
                return false;
            }
        }
    }
}