using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Weatherstation.WebView.Components;
using Weatherstation.WebView.Data;
using Weatherstation.WebView.Services;

namespace Weatherstation.WebView;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
            
        // MudBlazor Services hinzufügen
        builder.Services.AddMudServices();
        
        // DbContext für InMemory-Datenbank
        builder.Services.AddDbContext<WeatherDbContext>(options =>
            options.UseInMemoryDatabase("WeatherDb"));
            
        // Services registrieren
        // In Program.cs
        builder.Services.AddScoped<IPreferenceService, PreferenceService>();
        builder.Services.AddScoped<WeatherDataFaker>();
        builder.Services.AddScoped<WeatherService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
            
        // Datenbank mit Testdaten initialisieren
        using (var scope = app.Services.CreateScope())
        {
            var dataFaker = scope.ServiceProvider.GetRequiredService<WeatherDataFaker>();
            dataFaker.GenerateFakeDataAsync().Wait();
        }

        app.Run();
    }
}