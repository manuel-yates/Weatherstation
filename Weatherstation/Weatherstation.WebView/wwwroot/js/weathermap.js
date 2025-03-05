// Leaflet.js-Karten-Implementierung für die Wetterstation-Anwendung

let weatherMap;
let weatherMarkers = {};
let weatherLayers = {
    temperature: L.layerGroup(),
    precipitation: L.layerGroup(),
    wind: L.layerGroup(),
    pressure: L.layerGroup()
};
let activeLayer = 'temperature';

// Karte initialisieren
function initializeWeatherMap(elementId) {
    // Falls die Karte bereits initialisiert wurde, nicht erneut initialisieren
    if (weatherMap) return;

    // OpenStreetMap als Basiskarte verwenden
    const osmLayer = L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        maxZoom: 19
    });

    // Alternativer Kartenstil (optional)
    const cartoDBLayer = L.tileLayer('https://{s}.basemaps.cartocdn.com/light_all/{z}/{x}/{y}{r}.png', {
        attribution: '&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors',
        maxZoom: 19
    });

    // Karte erstellen und mit Standard-Ansicht initialisieren
    weatherMap = L.map(elementId, {
        center: [51.1657, 10.4515], // Deutschland Mitte
        zoom: 6,
        layers: [osmLayer] // Standard-Layer
    });

    // Layer-Kontrolle hinzufügen
    const baseMaps = {
        "OpenStreetMap": osmLayer,
        "Heller Stil": cartoDBLayer
    };

    L.control.layers(baseMaps).addTo(weatherMap);

    // Alle Layer zur Karte hinzufügen (aber nur aktiver Layer wird angezeigt)
    weatherLayers.temperature.addTo(weatherMap);
}

// Karte zentrieren
function centerMap(lat, lon, zoom) {
    if (!weatherMap) return;
    weatherMap.setView([lat, lon], zoom);
}

// Wetterstation zur Karte hinzufügen
function addWeatherStation(id, lat, lon, name, temperature, condition, iconName, color) {
    if (!weatherMap) return;

    // Icon basierend auf Wetterbedingung erstellen
    const markerHtml = `
        <div class="weather-marker-icon" style="background-color: ${color}; text-align: center; border-radius: 50%; width: 40px; height: 40px; display: flex; flex-direction: column; justify-content: center; color: white; border: 2px solid white; box-shadow: 0 0 5px rgba(0,0,0,0.5);">
            <div style="font-weight: bold; font-size: 12px;">${temperature}</div>
        </div>
    `;

    const weatherIcon = L.divIcon({
        html: markerHtml,
        className: 'weather-marker',
        iconSize: [40, 40],
        iconAnchor: [20, 20],
        popupAnchor: [0, -20]
    });

    // Marker erstellen
    const marker = L.marker([lat, lon], { icon: weatherIcon });

    // Popup mit Stationsinformationen
    marker.bindPopup(`
        <div style="text-align: center;">
            <h3>${name}</h3>
            <p>${temperature} | ${condition}</p>
            <button onclick="DotNet.invokeMethodAsync('Weatherstation.WebView', 'SelectStation', '${id}')" 
                    style="background-color: #1E88E5; color: white; border: none; padding: 5px 10px; border-radius: 4px; cursor: pointer;">
                Details
            </button>
        </div>
    `);

    // Marker zum entsprechenden Layer hinzufügen
    weatherMarkers[id] = {
        marker: marker,
        data: {
            id: id,
            name: name,
            temperature: temperature,
            condition: condition,
            lat: lat,
            lon: lon
        }
    };

    // Marker zu allen Layern hinzufügen
    weatherLayers.temperature.addLayer(marker);

    // Spezielle Marker für andere Layer (können später implementiert werden)
    // Hier nur Platzhalter
    weatherLayers.precipitation.addLayer(L.marker([lat, lon], { icon: weatherIcon }));
    weatherLayers.wind.addLayer(L.marker([lat, lon], { icon: weatherIcon }));
    weatherLayers.pressure.addLayer(L.marker([lat, lon], { icon: weatherIcon }));
}

// Layer wechseln
function switchLayer(layer) {
    if (!weatherMap || !weatherLayers[layer]) return;

    // Aktuellen Layer entfernen
    weatherMap.removeLayer(weatherLayers[activeLayer]);

    // Neuen Layer hinzufügen
    activeLayer = layer;
    weatherLayers[activeLayer].addTo(weatherMap);
}

// Station auswählen
function selectStation(id) {
    if (!weatherMarkers[id]) return;

    // Marker öffnen
    weatherMarkers[id].marker.openPopup();

    // Zum Blazor zurückkommunizieren
    return DotNet.invokeMethodAsync('Weatherstation.WebView', 'SelectStationAsync', id);
}