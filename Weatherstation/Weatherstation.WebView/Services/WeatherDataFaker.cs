using Bogus;
using Weatherstation.WebView.Data;
using Weatherstation.WebView.Models;

namespace Weatherstation.WebView.Services;

public class WeatherDataFaker
{
    private readonly WeatherDbContext _context;
    private readonly string[] _weatherConditions = 
        { "Klar", "Sonnig", "Teilweise bewölkt", "Bewölkt", "Regnerisch", "Stürmisch", "Schneefall", "Neblig", "Gewitter", "Hagel", "Graupel", "Schwül" };
    private readonly string[] _alertTypes = 
        { "Sturm", "Überschwemmung", "Hitze", "Starkregen", "Schneefall", "Gewitter", "Nebel", "Frost", "Hagel", "Orkan", "Dürre", "Unwetter" };
    private readonly string[] _severityLevels = 
        { "Niedrig", "Mittel", "Hoch", "Extrem" };
    private readonly string[] _precipitationTypes = 
        { "Regen", "Schnee", "Hagel", "Graupel", "Nieselregen", "Kein Niederschlag" };
    private readonly string[] _cardinalDirections = 
        { "N", "NO", "O", "SO", "S", "SW", "W", "NW" };
    private readonly string[] _moonPhases = 
        { "Neumond", "Zunehmender Sichelmond", "Erstes Viertel", "Zunehmender Mond", 
          "Vollmond", "Abnehmender Mond", "Letztes Viertel", "Abnehmender Sichelmond" };
    private readonly string[] _sensorTypes = 
        { "Temperatur", "Feuchte", "Druck", "Wind", "Niederschlag", "UV", "Kombisensor", "Luftqualität", "Sonneneinstrahlung", "Bodenfeuchtigkeit" };
    private readonly string[] _manufacturers = 
        { "Davis", "Netatmo", "Oregon Scientific", "WeatherFlow", "Ambient Weather", "Mission-Tomorrow", "Bosch", "Siemens", "Honeywell", "Ecowitt" };
    private readonly (double Lat, double Lon, string Name, string Id)[] _stationLocations = 
    {
        (52.52, 13.38, "Berlin Mitte", "BER001"),       // Berlin
        (48.13, 11.58, "München Zentrum", "MUC001"),    // München
        (53.54, 9.99, "Hamburg Hafen", "HAM001"),       // Hamburg
        (50.94, 6.96, "Köln Dom", "KOL001"),            // Köln
        (50.11, 8.68, "Frankfurt Main", "FRA001"),      // Frankfurt
        (48.77, 9.18, "Stuttgart Marktplatz", "STU001"), // Stuttgart
        (51.34, 12.37, "Leipzig Messe", "LEI001"),      // Leipzig
        (51.05, 13.74, "Dresden Altstadt", "DRE001"),   // Dresden
        (49.45, 11.08, "Nürnberg Altstadt", "NUE001"),  // Nürnberg
        (51.96, 7.63, "Münster Aasee", "MUN001"),       // Münster
        (47.66, 9.18, "Konstanz Bodensee", "KON001"),   // Konstanz
        (52.37, 9.73, "Hannover Maschsee", "HAN001")    // Hannover
    };
    
    public WeatherDataFaker(WeatherDbContext context)
    {
        _context = context;
    }
    
    public async Task GenerateFakeDataAsync()
    {
        // Lösche alle vorhandenen Daten
        _context.WeatherAlerts.RemoveRange(_context.WeatherAlerts);
        _context.HourlyForecasts.RemoveRange(_context.HourlyForecasts);
        _context.DailyForecasts.RemoveRange(_context.DailyForecasts);
        _context.WeatherSensorInfo.RemoveRange(_context.WeatherSensorInfo);
        _context.WeatherData.RemoveRange(_context.WeatherData);
        _context.HistoricalWeatherData.RemoveRange(_context.HistoricalWeatherData);
        _context.WeatherDisplaySettings.RemoveRange(_context.WeatherDisplaySettings);
        
        await _context.SaveChangesAsync();
        
        // Generiere Wetterstationen
        var weatherStations = new List<WeatherData>();
        
        for (int i = 0; i < _stationLocations.Length; i++)
        {
            var station = GenerateWeatherDataForStation(i);
            weatherStations.Add(station);
            _context.WeatherData.Add(station);
        }
        
        await _context.SaveChangesAsync();
        
        // Generiere Warnungen, Vorhersagen und Sensorinformationen für jede Station
        foreach (var station in weatherStations)
        {
            // Warnungen
            var alertsCount = new Random().Next(0, 5); // 0-4 Warnungen pro Station
            for (int i = 0; i < alertsCount; i++)
            {
                _context.WeatherAlerts.Add(GenerateWeatherAlert(station.Id));
            }
            
            // Stündliche Vorhersagen für 72 Stunden
            for (int i = 0; i < 72; i++)
            {
                _context.HourlyForecasts.Add(GenerateHourlyForecast(station.Id, i));
            }
            
            // Tägliche Vorhersagen für 14 Tage
            for (int i = 0; i < 14; i++)
            {
                _context.DailyForecasts.Add(GenerateDailyForecast(station.Id, i));
            }
            
            // Sensorinformationen - mehrere Sensoren pro Station
            var sensorCount = new Random().Next(1, 4);
            for (int i = 0; i < sensorCount; i++)
            {
                _context.WeatherSensorInfo.Add(GenerateWeatherSensorInfo(station.Id, i));
            }
        }
        
        // Generiere historische Daten für die letzten 180 Tage
        for (int i = 1; i <= 180; i++)
        {
            foreach (var station in weatherStations)
            {
                _context.HistoricalWeatherData.Add(GenerateHistoricalWeatherData(
                    DateTime.Now.AddDays(-i), station.StationId));
            }
        }
        
        // Generiere mehrere Anzeigeeinstellungen für verschiedene Benutzer
        var userIds = new[] { "default-user", "admin-user", "guest-user" };
        foreach (var userId in userIds)
        {
            _context.WeatherDisplaySettings.Add(new WeatherDisplaySettings
            {
                UserId = userId,
                UseMetricSystem = userId != "guest-user", // Ein Nutzer verwendet imperiales System
                ShowFeelsLike = true,
                ShowUvIndex = userId != "guest-user",
                ShowHumidity = true,
                ShowPressure = userId != "guest-user",
                ShowWindInfo = true,
                ShowPrecipitation = true,
                ShowSunriseSunset = true,
                EnableNotifications = userId == "admin-user",
                NotifyOnAlerts = true,
                NotifyOnRain = userId == "admin-user",
                NotifyOnTemperatureChange = userId == "admin-user",
                TemperatureChangeThreshold = 5.0,
                PreferredChartType = userId == "admin-user" ? "Bar" : "Line",
                ThemePreference = userId == "guest-user" ? "Light" : "Auto",
                DefaultForecastDays = userId == "admin-user" ? 14 : 7,
                FavoriteLocations = userId == "default-user" 
                    ? new List<string> { "Berlin Mitte", "München Zentrum" }
                    : userId == "admin-user"
                        ? new List<string> { "Hamburg Hafen", "Köln Dom", "Frankfurt Main" }
                        : new List<string> { "Stuttgart Marktplatz" }
            });
        }
        
        await _context.SaveChangesAsync();
    }
    
    private WeatherData GenerateWeatherDataForStation(int index)
    {
        var faker = new Faker("de");
        var now = DateTime.Now;
        var sunrise = DateTime.Today.AddHours(6).AddMinutes(faker.Random.Int(0, 59));
        var sunset = DateTime.Today.AddHours(19).AddMinutes(faker.Random.Int(0, 59));
        
        // Nutze die vordefinierten Standorte
        var location = _stationLocations[index];
        
        // Jahreszeitabhängige Temperaturen
        var month = now.Month;
        double baseTemp;
        
        // Grobe Temperaturspannen je nach Jahreszeit und Region
        if (month >= 6 && month <= 8) // Sommer
        {
            baseTemp = faker.Random.Double(18, 35);
        }
        else if (month >= 3 && month <= 5) // Frühling
        {
            baseTemp = faker.Random.Double(8, 25);
        }
        else if (month >= 9 && month <= 11) // Herbst
        {
            baseTemp = faker.Random.Double(5, 20);
        }
        else // Winter
        {
            baseTemp = faker.Random.Double(-10, 10);
        }
        
        // Regionale Unterschiede (Norden etwas kühler, Süden etwas wärmer)
        if (location.Lat > 52) // Norddeutschland
        {
            baseTemp -= faker.Random.Double(0, 3);
        }
        else if (location.Lat < 49) // Süddeutschland
        {
            baseTemp += faker.Random.Double(0, 3);
        }
        
        var temperatureC = baseTemp;
        var windSpeed = faker.Random.Double(0, 30);
        
        // Höhere Windgeschwindigkeiten an der Küste
        if (location.Name.Contains("Hamburg") || location.Name.Contains("Konstanz"))
        {
            windSpeed += faker.Random.Double(5, 15);
        }
        
        var humidity = faker.Random.Double(30, 95);
        
        // Regionale Unterschiede bei Luftfeuchtigkeit
        if (location.Name.Contains("München") || location.Name.Contains("Dresden"))
        {
            humidity -= faker.Random.Double(5, 15); // Trockener
        }
        else if (location.Name.Contains("Hamburg") || location.Name.Contains("Köln"))
        {
            humidity += faker.Random.Double(5, 15); // Feuchter
        }
        
        // Wetter-Bedingung basierend auf Temperatur und Jahreszeit
        string weatherCondition;
        if (temperatureC > 28)
        {
            weatherCondition = faker.PickRandom("Sonnig", "Klar", "Schwül");
        }
        else if (temperatureC < 0)
        {
            weatherCondition = faker.PickRandom("Schneefall", "Klar", "Bewölkt", "Neblig");
        }
        else if (humidity > 80)
        {
            weatherCondition = faker.PickRandom("Regnerisch", "Bewölkt", "Neblig", "Stürmisch");
        }
        else
        {
            weatherCondition = faker.PickRandom(_weatherConditions);
        }
        
        return new WeatherData
        {
            Timestamp = now,
            Latitude = location.Lat,
            Longitude = location.Lon,
            StationName = location.Name,
            StationId = location.Id,
            TemperatureCelsius = temperatureC,
            FeelsLikeCelsius = CalculateFeelsLike(temperatureC, windSpeed, humidity),
            TemperatureMin = temperatureC - faker.Random.Double(2, 8),
            TemperatureMax = temperatureC + faker.Random.Double(2, 8),
            HumidityPercentage = humidity,
            DewPointCelsius = CalculateDewPoint(temperatureC, humidity),
            PressureHPa = faker.Random.Double(990, 1030),
            SeaLevelPressureHPa = faker.Random.Double(990, 1030),
            PressureTrend = faker.Random.Double(-3, 3),
            WindSpeedKmh = windSpeed,
            WindDirectionDegrees = faker.Random.Double(0, 359),
            WindDirectionCardinal = _cardinalDirections[faker.Random.Int(0, _cardinalDirections.Length - 1)],
            WindGustKmh = windSpeed + faker.Random.Double(5, 20),
            PrecipitationMm = weatherCondition.Contains("Regen") || weatherCondition.Contains("Schnee") 
                ? faker.Random.Double(1, 20) 
                : faker.Random.Double(0, 1),
            PrecipitationProbability = weatherCondition.Contains("Regen") || weatherCondition.Contains("Schnee") 
                ? faker.Random.Double(50, 100) 
                : faker.Random.Double(0, 30),
            PrecipitationType = GetPrecipitationType(weatherCondition),
            SnowDepthCm = weatherCondition.Contains("Schnee") ? faker.Random.Double(1, 15) : 0,
            CloudCoverPercentage = GetCloudCoverPercentage(weatherCondition),
            VisibilityKm = GetVisibilityKm(weatherCondition),
            UvIndex = GetUvIndex(weatherCondition, month),
            Sunrise = sunrise,
            Sunset = sunset,
            DaylightHours = (sunset - sunrise).TotalHours,
            WeatherCondition = weatherCondition,
            WeatherIcon = GetWeatherIcon(weatherCondition),
            Alerts = new List<WeatherAlert>(),
            HourlyForecasts = new List<HourlyForecast>(),
            DailyForecasts = new List<DailyForecast>()
        };
    }
    
    private WeatherAlert GenerateWeatherAlert(int weatherDataId)
    {
        var faker = new Faker("de");
        var now = DateTime.Now;
        
        // Wähle zufällig einen Warnungstyp und Schweregrad
        var alertType = faker.PickRandom(_alertTypes);
        
        // Schweregrad mit Gewichtung: Niedrig häufiger als Extrem
        var severity = faker.Random.Int(1, 10) switch
        {
            <= 5 => "Niedrig",
            <= 8 => "Mittel", 
            <= 9 => "Hoch",
            _ => "Extrem"
        };
        
        // Expirieren in 6-48 Stunden, aber mit 20% Chance schon in 1-6 Stunden
        int expiryHours = faker.Random.Bool(0.2f) 
            ? faker.Random.Int(1, 6) 
            : faker.Random.Int(6, 48);
        
        // Erstellt vor 1-24 Stunden
        int issuedHoursAgo = faker.Random.Int(1, 24);
        
        // Beschreibungen passend zum Alerttyp
        string description = GetAlertDescription(alertType, severity);
        
        return new WeatherAlert
        {
            WeatherDataId = weatherDataId,
            AlertType = alertType,
            Severity = severity,
            Title = $"{severity} Warnung: {alertType}",
            Description = description,
            IssuedAt = now.AddHours(-issuedHoursAgo),
            ValidFrom = now.AddHours(-issuedHoursAgo/2), // Beginnt nach der Hälfte der Erstellungszeit
            ValidUntil = now.AddHours(expiryHours),
            SourceName = faker.Random.Bool(0.7f) ? "DWD" : faker.PickRandom("ZAMG", "MeteoSchweiz", "Warnwetter", "AccuWeather"),
            AreaAffected = faker.Address.City()
        };
    }
    
    private HourlyForecast GenerateHourlyForecast(int weatherDataId, int hourOffset)
    {
        var faker = new Faker("de");
        var forecastTime = DateTime.Now.AddHours(hourOffset);
        
        // Basistemperatur abhängig von Tageszeit und Jahreszeit
        var hour = forecastTime.Hour;
        var month = forecastTime.Month;
        double baseTemp;
        
        // Tagestemperaturkurve
        if (hour >= 22 || hour <= 5) // Nacht
        {
            baseTemp = faker.Random.Double(-5, 15);
        }
        else if (hour >= 10 && hour <= 16) // Nachmittag
        {
            baseTemp = faker.Random.Double(5, 30);
        }
        else // Morgen & Abend
        {
            baseTemp = faker.Random.Double(0, 25);
        }
        
        // Jahreszeitliche Anpassung
        if (month >= 6 && month <= 8) // Sommer
        {
            baseTemp += faker.Random.Double(5, 15);
        }
        else if (month >= 3 && month <= 5 || month >= 9 && month <= 11) // Frühling/Herbst
        {
            baseTemp += faker.Random.Double(-5, 5);
        }
        else // Winter
        {
            baseTemp += faker.Random.Double(-15, -5);
        }
        
        // Varianz für realistischere Vorhersagen
        // Je weiter in der Zukunft, desto mehr Varianz
        double variance = Math.Min(hourOffset / 12.0, 1.0) * 5.0;
        double temperatureC = baseTemp + faker.Random.Double(-variance, variance);
        
        // Wetterbedingung basierend auf Temperatur und Stunde
        string weatherCondition = GenerateWeatherCondition(temperatureC, hour, month);
        
        return new HourlyForecast
        {
            WeatherDataId = weatherDataId,
            ForecastTime = forecastTime,
            TemperatureCelsius = temperatureC,
            FeelsLikeCelsius = CalculateFeelsLike(temperatureC, faker.Random.Double(0, 30), faker.Random.Double(30, 95)),
            HumidityPercentage = faker.Random.Double(30, 95),
            WindSpeedKmh = faker.Random.Double(0, 30) * (1 + hourOffset/48.0), // Mehr Varianz in der Zukunft
            WindDirectionDegrees = faker.Random.Double(0, 359),
            PrecipitationProbability = weatherCondition.Contains("Regen") || weatherCondition.Contains("Schnee") 
                ? faker.Random.Double(50, 100) 
                : faker.Random.Double(0, 30),
            PrecipitationMm = weatherCondition.Contains("Regen") || weatherCondition.Contains("Schnee") 
                ? faker.Random.Double(0.5, 15) 
                : 0,
            WeatherCondition = weatherCondition,
            WeatherIcon = GetWeatherIcon(weatherCondition),
            PressureHPa = faker.Random.Double(990, 1030),
            CloudCoverPercentage = GetCloudCoverPercentage(weatherCondition),
            VisibilityKm = GetVisibilityKm(weatherCondition)
        };
    }
    
    private DailyForecast GenerateDailyForecast(int weatherDataId, int dayOffset)
    {
        var faker = new Faker("de");
        var forecastDate = DateTime.Today.AddDays(dayOffset);
        var sunrise = forecastDate.AddHours(6).AddMinutes(faker.Random.Int(0, 59));
        var sunset = forecastDate.AddHours(19).AddMinutes(faker.Random.Int(0, 59));
        
        // Jahreszeit-basierte Temperaturspannen
        var month = forecastDate.Month;
        double minTemp, maxTemp;
        
        // Basistemperaturen je nach Jahreszeit
        if (month >= 6 && month <= 8) // Sommer
        {
            minTemp = faker.Random.Double(12, 20);
            maxTemp = faker.Random.Double(22, 35);
        }
        else if (month >= 3 && month <= 5) // Frühling
        {
            minTemp = faker.Random.Double(3, 15);
            maxTemp = faker.Random.Double(15, 25);
        }
        else if (month >= 9 && month <= 11) // Herbst
        {
            minTemp = faker.Random.Double(2, 12);
            maxTemp = faker.Random.Double(10, 22);
        }
        else // Winter
        {
            minTemp = faker.Random.Double(-10, 5);
            maxTemp = faker.Random.Double(0, 10);
        }
        
        // Varianz steigt mit dem Abstand zum aktuellen Tag
        double variance = Math.Min(dayOffset / 7.0, 1.0) * 8.0;
        minTemp += faker.Random.Double(-variance, variance);
        maxTemp += faker.Random.Double(-variance, variance);
        
        // Stelle sicher, dass Min < Max
        if (minTemp > maxTemp)
        {
            (minTemp, maxTemp) = (maxTemp, minTemp);
        }
        
        var humidity = faker.Random.Double(30, 95);
        double moonPhaseValue = ((forecastDate.Day % 28) / 28.0); // Einfache Mondphasenberechnung
        
        // Berechne Mondphasen-Beschreibung basierend auf dem Wert
        string moonPhaseDesc = _moonPhases[0];
        if (moonPhaseValue < 0.125) moonPhaseDesc = _moonPhases[0];
        else if (moonPhaseValue < 0.25) moonPhaseDesc = _moonPhases[1];
        else if (moonPhaseValue < 0.375) moonPhaseDesc = _moonPhases[2];
        else if (moonPhaseValue < 0.5) moonPhaseDesc = _moonPhases[3];
        else if (moonPhaseValue < 0.625) moonPhaseDesc = _moonPhases[4];
        else if (moonPhaseValue < 0.75) moonPhaseDesc = _moonPhases[5];
        else if (moonPhaseValue < 0.875) moonPhaseDesc = _moonPhases[6];
        else moonPhaseDesc = _moonPhases[7];
        
        // Wetterbedingung basierend auf Temperaturen und Jahreszeit
        string weatherCondition = GenerateWeatherCondition((minTemp + maxTemp)/2, 12, month);
        
        return new DailyForecast
        {
            WeatherDataId = weatherDataId,
            ForecastDate = forecastDate,
            TemperatureMinCelsius = minTemp,
            TemperatureMaxCelsius = maxTemp,
            FeelsLikeMinCelsius = CalculateFeelsLike(minTemp, faker.Random.Double(0, 10), humidity),
            FeelsLikeMaxCelsius = CalculateFeelsLike(maxTemp, faker.Random.Double(5, 20), humidity),
            HumidityPercentage = humidity,
            WindSpeedKmh = faker.Random.Double(5, 40),
            WindDirectionDegrees = faker.Random.Double(0, 359),
            PrecipitationProbability = weatherCondition.Contains("Regen") || weatherCondition.Contains("Schnee") 
                ? faker.Random.Double(50, 100) 
                : faker.Random.Double(0, 30),
            PrecipitationMm = weatherCondition.Contains("Regen") 
                ? faker.Random.Double(1, 30) 
                : weatherCondition.Contains("Schnee") 
                    ? faker.Random.Double(1, 15) 
                    : faker.Random.Double(0, 0.5),
            SnowDepthCm = weatherCondition.Contains("Schnee") ? faker.Random.Double(1, 20) : 0,
            WeatherCondition = weatherCondition,
            WeatherIcon = GetWeatherIcon(weatherCondition),
            Sunrise = sunrise,
            Sunset = sunset,
            UvIndex = GetUvIndex(weatherCondition, month),
            MoonPhase = moonPhaseValue,
            MoonPhaseDescription = moonPhaseDesc
        };
    }
    
    private WeatherSensorInfo GenerateWeatherSensorInfo(int weatherDataId, int sensorIndex)
    {
        var faker = new Faker("de");
        
        // Verschiedene Sensortypen nach Index
        string sensorType = sensorIndex == 0 
            ? "Kombisensor" 
            : faker.PickRandom(_sensorTypes.Where(s => s != "Kombisensor").ToArray());
        
        var manufacturer = faker.PickRandom(_manufacturers);
        var model = $"Model {faker.Random.AlphaNumeric(4)}";
        
        // Genauigkeiten je nach Hersteller und Modell
        double accuracyTemp = manufacturer.Contains("Mission-Tomorrow") || manufacturer.Contains("Bosch")
            ? faker.Random.Double(0.1, 0.5)  // Bessere Genauigkeit bei Premiummarken
            : faker.Random.Double(0.5, 1.5); // Schlechtere Genauigkeit bei günstigeren Marken
            
        double accuracyHumidity = manufacturer.Contains("Mission-Tomorrow") || manufacturer.Contains("Davis")
            ? faker.Random.Double(1, 3)
            : faker.Random.Double(3, 8);
            
        double accuracyPressure = manufacturer.Contains("Mission-Tomorrow") || manufacturer.Contains("Oregon")
            ? faker.Random.Double(0.1, 1.0)
            : faker.Random.Double(1.0, 3.0);
        
        // Batteriestatus und Online-Status
        double batteryLevel = faker.Random.Double(20, 100);
        bool isOnline = faker.Random.Bool(0.95f); // 95% der Sensoren sind online
        
        // Kalibrierungs- und Wartungsdaten
        DateTime lastCalibration = DateTime.Now.AddDays(-faker.Random.Int(0, 365));
        DateTime lastMaintenance = DateTime.Now.AddDays(-faker.Random.Int(0, 180));
        
        return new WeatherSensorInfo
        {
            WeatherDataId = weatherDataId,
            SensorType = sensorType,
            Manufacturer = manufacturer,
            Model = model,
            LastCalibration = lastCalibration,
            AccuracyTemperature = accuracyTemp,
            AccuracyHumidity = accuracyHumidity,
            AccuracyPressure = accuracyPressure,
            BatteryLevel = batteryLevel,
            IsOnline = isOnline,
            LastMaintenance = lastMaintenance
        };
    }
    
    private HistoricalWeatherData GenerateHistoricalWeatherData(DateTime date, string stationId)
    {
        var faker = new Faker("de");
        var month = date.Month;
        
        // Temperatur basierend auf Monat (Jahreszeit)
        double baseTemp;
        if (month >= 6 && month <= 8) // Sommer
        {
            baseTemp = faker.Random.Double(18, 32);
        }
        else if (month >= 3 && month <= 5) // Frühling
        {
            baseTemp = faker.Random.Double(8, 22);
        }
        else if (month >= 9 && month <= 11) // Herbst
        {
            baseTemp = faker.Random.Double(5, 18);
        }
        else // Winter
        {
            baseTemp = faker.Random.Double(-8, 8);
        }
        
        // Zufällige Variation innerhalb des Monats
        double avgTemp = baseTemp + faker.Random.Double(-5, 5);
        double minTemp = avgTemp - faker.Random.Double(3, 10);
        double maxTemp = avgTemp + faker.Random.Double(3, 10);
        
        // Regionale Temperaturanpassungen
        if (stationId.StartsWith("HAM")) // Hamburg (nördlicher)
        {
            avgTemp -= faker.Random.Double(0, 3);
        }
        else if (stationId.StartsWith("MUC")) // München (südlicher)
        {
            avgTemp += faker.Random.Double(0, 3);
        }
        
        // Niederschlag ist stärker in regenreichen Monaten
        double rainMultiplier = 1.0;
        if (month >= 4 && month <= 9) // April bis September: mehr Regen
        {
            rainMultiplier = 1.5;
        }
        
        // Mehr Niederschlag in bestimmten Regionen
        if (stationId.StartsWith("HAM") || stationId.StartsWith("KOL"))
        {
            rainMultiplier *= 1.3; // Mehr Regen in Hamburg und Köln
        }
        
        double precipitation = faker.Random.Double(0, 20) * rainMultiplier;
        
        // Sonnenstunden variieren mit Jahreszeit und Niederschlag
        double maxSunshineHours = month >= 5 && month <= 8 ? 16.0 : 10.0; // Längere Tage im Sommer
        double sunshineHours = faker.Random.Double(0, maxSunshineHours);
        
        // Bei viel Niederschlag weniger Sonnenschein
        if (precipitation > 10)
        {
            sunshineHours *= 0.3; // Stark reduziert bei viel Regen
        }
        else if (precipitation > 5)
        {
            sunshineHours *= 0.7; // Leicht reduziert bei etwas Regen
        }
        
        // Wähle Wetterbedingung basierend auf Niederschlag und Temperatur
        string condition;
        if (precipitation > 15)
        {
            condition = "Starkregen";
        }
        else if (precipitation > 5)
        {
            condition = "Regnerisch";
        }
        else if (sunshineHours > maxSunshineHours * 0.7)
        {
            condition = avgTemp > 20 ? "Sonnig" : "Klar";
            }
        else if (sunshineHours > maxSunshineHours * 0.3)
        {
            condition = "Teilweise bewölkt";
        }
        else
        {
            condition = "Bewölkt";
        }
        
        // Im Winter bei niedrigen Temperaturen Schnee statt Regen
        if (precipitation > 0 && avgTemp < 2)
        {
            condition = precipitation > 10 ? "Starker Schneefall" : "Schneefall";
        }
        
        return new HistoricalWeatherData
        {
            Date = date,
            StationId = stationId,
            AvgTemperatureCelsius = avgTemp,
            MinTemperatureCelsius = minTemp,
            MaxTemperatureCelsius = maxTemp,
            TotalPrecipitationMm = precipitation,
            AvgHumidityPercentage = faker.Random.Double(30, 95),
            AvgPressureHPa = faker.Random.Double(990, 1030),
            AvgWindSpeedKmh = faker.Random.Double(0, 30),
            PeakWindSpeedKmh = faker.Random.Double(5, 60),
            SunshineHours = sunshineHours,
            PrimaryWeatherCondition = condition
        };
    }
    
    // Hilfsmethoden

    private double CalculateFeelsLike(double tempC, double windSpeedKmh, double humidity)
    {
        // Einfache Implementierung des Windchill und Hitzeindex
        if (tempC < 10) // Kalt - Windchill
        {
            // Vereinfachte Windchill-Formel
            return tempC - (windSpeedKmh * 0.15);
        }
        else if (tempC > 25) // Warm - Hitzeindex
        {
            // Vereinfachte Hitzeindex-Formel
            return tempC + ((humidity - 50) / 5);
        }
        else // Moderate Temperatur - nahe der tatsächlichen Temperatur
        {
            return tempC + new Random().NextDouble() * 2 - 1; // ±1 Grad Variation
        }
    }
    
    private double CalculateDewPoint(double tempC, double humidity)
    {
        // Approximation der Magnus-Formel
        double a = 17.27;
        double b = 237.7;
        double alpha = ((a * tempC) / (b + tempC)) + Math.Log(humidity / 100.0);
        return (b * alpha) / (a - alpha);
    }
    
    private string GetPrecipitationType(string weatherCondition)
    {
        return weatherCondition switch
        {
            "Regnerisch" or "Gewitter" or "Starkregen" => "Regen",
            "Schneefall" => "Schnee",
            "Hagel" => "Hagel",
            "Graupel" => "Graupel",
            "Neblig" => "Nieselregen",
            _ => "Kein Niederschlag"
        };
    }
    
    private int GetCloudCoverPercentage(string weatherCondition)
    {
        return weatherCondition switch
        {
            "Klar" => new Random().Next(0, 10),
            "Sonnig" => new Random().Next(0, 20),
            "Teilweise bewölkt" => new Random().Next(30, 70),
            "Bewölkt" => new Random().Next(70, 100),
            "Regnerisch" or "Stürmisch" or "Schneefall" or "Gewitter" => new Random().Next(80, 100),
            "Neblig" => new Random().Next(50, 100),
            _ => new Random().Next(0, 100)
        };
    }
    
    private double GetVisibilityKm(string weatherCondition)
    {
        return weatherCondition switch
        {
            "Klar" or "Sonnig" => new Random().NextDouble() * 20 + 30, // 30-50 km
            "Teilweise bewölkt" => new Random().NextDouble() * 15 + 15, // 15-30 km
            "Bewölkt" => new Random().NextDouble() * 10 + 10, // 10-20 km
            "Regnerisch" => new Random().NextDouble() * 8 + 2, // 2-10 km
            "Stürmisch" => new Random().NextDouble() * 5 + 1, // 1-6 km
            "Schneefall" => new Random().NextDouble() * 3 + 1, // 1-4 km
            "Neblig" => new Random().NextDouble() * 0.9 + 0.1, // 0.1-1 km
            "Gewitter" => new Random().NextDouble() * 5 + 1, // 1-6 km
            _ => new Random().NextDouble() * 10 + 10 // 10-20 km
        };
    }
    
    private double GetUvIndex(string weatherCondition, int month)
    {
        // Basis-UV-Index nach Monat (Jahreszeit)
        double baseUv;
        if (month >= 6 && month <= 8) // Sommer
        {
            baseUv = new Random().NextDouble() * 5 + 6; // 6-11
        }
        else if (month >= 3 && month <= 5 || month >= 9 && month <= 10) // Frühling/Frühherbst
        {
            baseUv = new Random().NextDouble() * 4 + 3; // 3-7
        }
        else // Winter/Spätherbst
        {
            baseUv = new Random().NextDouble() * 2 + 1; // 1-3
        }
        
        // Anpassung basierend auf Wetterbedingungen
        return weatherCondition switch
        {
            "Klar" or "Sonnig" => baseUv,
            "Teilweise bewölkt" => baseUv * 0.7,
            "Bewölkt" => baseUv * 0.4,
            "Regnerisch" or "Stürmisch" or "Schneefall" or "Neblig" or "Gewitter" => baseUv * 0.2,
            _ => baseUv * 0.5
        };
    }
    
    private string GetWeatherIcon(string condition)
    {
        return condition switch
        {
            "Klar" => "weather-sunny",
            "Sonnig" => "weather-sunny",
            "Teilweise bewölkt" => "weather-partly-cloudy",
            "Bewölkt" => "weather-cloudy",
            "Regnerisch" => "weather-rainy",
            "Stürmisch" => "weather-windy",
            "Schneefall" => "weather-snowy",
            "Neblig" => "weather-fog",
            "Gewitter" => "weather-lightning",
            "Hagel" => "weather-hail",
            "Graupel" => "weather-snowy-rainy",
            "Schwül" => "weather-cloudy", 
            _ => "weather-cloudy"
        };
    }
    
    private string GenerateWeatherCondition(double temperature, int hour, int month)
    {
        var random = new Random();
    
        // Wahrscheinlichkeiten je nach Tageszeit und Jahreszeit
        bool isNight = hour < 6 || hour > 21;
        bool isWinter = month == 12 || month <= 2;
        bool isSummer = month >= 6 && month <= 8;
    
        string[] conditions;
        int[] weights;
    
        // Wetterbedingung basierend auf Temperatur und anderen Faktoren
        if (temperature < 0)
        {
            conditions = new[] { "Klar", "Bewölkt", "Schneefall", "Neblig" };
            weights = new[] { 2, 3, isWinter ? 4 : 2, isNight ? 3 : 1 };
        }
        else if (temperature > 25)
        {
            conditions = new[] { "Klar", "Sonnig", "Teilweise bewölkt", "Gewitter", "Schwül" };
            weights = new[] { 3, 4, 2, isSummer ? 2 : 1, isSummer ? 3 : 1 };
        }
        else
        {
            conditions = new[] { "Klar", "Sonnig", "Teilweise bewölkt", "Bewölkt", "Regnerisch", "Stürmisch", "Neblig" };
            weights = new[] { 2, isNight ? 0 : 3, 4, 3, 2, 1, isNight ? 2 : 1 };
        }
    
        // Berechne die Summe aller Gewichte
        int totalWeight = weights.Sum();
    
        // Wähle einen zufälligen Punkt auf der Gewichtungsskala
        int randomPoint = random.Next(totalWeight);
    
        // Finde das entsprechende Element
        int currentWeight = 0;
        for (int i = 0; i < conditions.Length; i++)
        {
            currentWeight += weights[i];
            if (randomPoint < currentWeight)
            {
                return conditions[i];
            }
        }
    
        // Fallback für unwahrscheinlichen Fall
        return conditions[0];
    }
    
    private string GetAlertDescription(string alertType, string severity)
    {
        var faker = new Faker("de");
        
        // Basis-Beschreibungen je nach Warnungstyp
        string baseDesc = alertType switch
        {
            "Sturm" => "Starke Winde mit Geschwindigkeiten von {0} km/h erwartet. {1}",
            "Überschwemmung" => "Erhöhte Wasserstände und Überflutungsgefahr in Flussnähe. {0}",
            "Hitze" => "Hohe Temperaturen bis zu {0}°C. {1}",
            "Starkregen" => "Starke Niederschläge von bis zu {0} mm/h. {1}",
            "Schneefall" => "Erheblicher Schneefall mit Mengen bis zu {0} cm. {1}",
            "Gewitter" => "Gewitter mit Blitzschlaggefahr und {0}. {1}",
            "Nebel" => "Dichte Nebelfelder mit Sichtweiten unter {0} m. {1}",
            "Frost" => "Temperaturen bis zu {0}°C unter dem Gefrierpunkt. {1}",
            "Hagel" => "Hagelschauer mit Korngrößen bis zu {0} cm. {1}",
            "Orkan" => "Orkanartige Windböen mit Geschwindigkeiten bis zu {0} km/h. {1}",
            "Dürre" => "Anhaltende Trockenheit mit erhöhter Waldbrandgefahr. {0}",
            "Unwetter" => "Schwere Unwetter mit {0}. {1}",
            _ => "Wetterbedingungen können zu {0} führen. {1}"
        };
        
        // Details basierend auf Schweregrad und Typ
        string details = severity switch
        {
            "Niedrig" => "Geringe Beeinträchtigungen möglich.",
            "Mittel" => "Mäßige Beeinträchtigungen im Alltag zu erwarten.",
            "Hoch" => "Erhebliche Beeinträchtigungen zu erwarten. Vorsichtsmaßnahmen sollten getroffen werden.",
            "Extrem" => "Lebensgefährliche Situation möglich. Schutzvorkehrungen dringend erforderlich.",
            _ => "Beeinträchtigungen möglich."
        };
        
        // Spezifische Werte je nach Warnungstyp
        string value = alertType switch
        {
            "Sturm" => (60 + severity switch
            {
                "Niedrig" => faker.Random.Int(0, 20),
                "Mittel" => faker.Random.Int(20, 40),
                "Hoch" => faker.Random.Int(40, 60),
                "Extrem" => faker.Random.Int(60, 80),
                _ => faker.Random.Int(0, 20)
            }).ToString(),
            
            "Hitze" => (30 + severity switch
            {
                "Niedrig" => faker.Random.Int(0, 3),
                "Mittel" => faker.Random.Int(3, 6),
                "Hoch" => faker.Random.Int(6, 9),
                "Extrem" => faker.Random.Int(9, 12),
                _ => faker.Random.Int(0, 5)
            }).ToString(),
            
            "Starkregen" => (20 + severity switch
            {
                "Niedrig" => faker.Random.Int(0, 10),
                "Mittel" => faker.Random.Int(10, 30),
                "Hoch" => faker.Random.Int(30, 50),
                "Extrem" => faker.Random.Int(50, 80),
                _ => faker.Random.Int(0, 20)
            }).ToString(),
            
            "Schneefall" => (5 + severity switch
            {
                "Niedrig" => faker.Random.Int(0, 5),
                "Mittel" => faker.Random.Int(5, 15),
                "Hoch" => faker.Random.Int(15, 30),
                "Extrem" => faker.Random.Int(30, 50),
                _ => faker.Random.Int(0, 10)
            }).ToString(),
            
            "Gewitter" => faker.PickRandom("Starkregen", "Hagel", "heftigen Windböen", "lokalen Überschwemmungen"),
            
            "Nebel" => (100 + severity switch
            {
                "Niedrig" => faker.Random.Int(0, 400),
                "Mittel" => faker.Random.Int(-50, 100),
                "Hoch" => faker.Random.Int(-80, 20),
                "Extrem" => faker.Random.Int(-90, 0),
                _ => faker.Random.Int(0, 200)
            }).ToString(),
            
            "Frost" => (severity switch
            {
                "Niedrig" => faker.Random.Int(0, 5),
                "Mittel" => faker.Random.Int(5, 10),
                "Hoch" => faker.Random.Int(10, 15),
                "Extrem" => faker.Random.Int(15, 25),
                _ => faker.Random.Int(0, 10)
            }).ToString(),
            
            "Hagel" => (0.5 + severity switch
            {
                "Niedrig" => faker.Random.Double(0, 1),
                "Mittel" => faker.Random.Double(1, 2),
                "Hoch" => faker.Random.Double(2, 4),
                "Extrem" => faker.Random.Double(4, 7),
                _ => faker.Random.Double(0, 2)
            }).ToString("0.#"),
            
            "Orkan" => (120 + severity switch
            {
                "Niedrig" => faker.Random.Int(0, 20),
                "Mittel" => faker.Random.Int(20, 40),
                "Hoch" => faker.Random.Int(40, 60),
                "Extrem" => faker.Random.Int(60, 80),
                _ => faker.Random.Int(0, 30)
            }).ToString(),
            
            "Dürre" => "Bewohner werden gebeten, Wasser zu sparen und offenes Feuer zu vermeiden.",
            
            "Unwetter" => faker.PickRandom("Starkregen, Hagel und Sturmböen", "Orkanböen und Überschwemmungen", 
                "Blitzschlag und lokalen Tornados", "Starkregen und Überflutungen"),
                
            _ => faker.PickRandom("Verkehrsbehinderungen", "Gebäudeschäden", "Umstürzenden Bäumen", "Stromausfällen")
        };
        
        // Formatierte Beschreibung
        try
        {
            return string.Format(baseDesc, value, details);
        }
        catch
        {
            // Fallback, falls die Formatierung fehlschlägt
            return $"{alertType}-Warnung: {details}";
        }
    }
}