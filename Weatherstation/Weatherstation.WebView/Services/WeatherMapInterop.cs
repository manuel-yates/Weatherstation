using Microsoft.JSInterop;

namespace Weatherstation.WebView.Services;

/// <summary>
/// Static class for JS Interop zwischen WeatherMap.razor und weathermap.js
/// </summary>
public static class WeatherMapInterop
{
    // Referenz zu einem DotNetObjectReference, das an JS übergeben wird
    private static DotNetObjectReference<WeatherMapCallbacks>? _dotNetObjectRef;
    
    // JS-Funktion zum Auswählen einer Station (wird von JS aufgerufen)
    [JSInvokable("SelectStation")]
    public static async Task SelectStation(string stationId)
    {
        if (_dotNetObjectRef != null)
        {
            await _dotNetObjectRef.Value.OnStationSelected(stationId);
        }
    }
    
    // Methode, um ein DotNetObjectReference zu registrieren
    public static void Initialize(WeatherMapCallbacks callbacks)
    {
        _dotNetObjectRef = DotNetObjectReference.Create(callbacks);
    }
    
    // Methode zum Aufräumen
    public static void Cleanup()
    {
        _dotNetObjectRef?.Dispose();
        _dotNetObjectRef = null;
    }
}

/// <summary>
/// Klasse für Callbacks von der JS-Seite
/// </summary>
public class WeatherMapCallbacks
{
    private readonly Action<string> _onStationSelected;
    
    public WeatherMapCallbacks(Action<string> onStationSelected)
    {
        _onStationSelected = onStationSelected;
    }
    
    [JSInvokable]
    public Task OnStationSelected(string stationId)
    {
        _onStationSelected(stationId);
        return Task.CompletedTask;
    }
}