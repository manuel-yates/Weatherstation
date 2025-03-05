using MudBlazor;
using MudBlazor.Utilities;

namespace Weatherstation.WebView.Theme
{
    public static class WeatherStationTheme
    {
        public static MudTheme DefaultTheme => new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = new MudColor("#1E88E5"),       // Blau-Himmel
                PrimaryDarken = "#1565C0",
                PrimaryLighten = "#42A5F5",
                Secondary = new MudColor("#26A69A"),     // Türkis
                SecondaryDarken = "#00897B",
                SecondaryLighten = "#4DB6AC",
                Tertiary = new MudColor("#FFB74D"),      // Orange für Warnungen
                
                Background = new MudColor("#F5F7FA"),
                Surface = new MudColor("#FFFFFF"),
                
                AppbarBackground = new MudColor("#1565C0"),
                AppbarText = new MudColor("#FFFFFF"),
                
                DrawerBackground = new MudColor("#0F2439"),
                DrawerText = new MudColor("#FFFFFF"),
                DrawerIcon = new MudColor("#78909C"),
                
                Success = new MudColor("#4CAF50"),       // Grün
                Warning = new MudColor("#FB8C00"),       // Orange
                Error = new MudColor("#E53935"),         // Rot
                Info = new MudColor("#2196F3"),          // Info-Blau

                TextPrimary = new MudColor("#263238"),
                TextSecondary = new MudColor("#546E7A"),
                TextDisabled = new MudColor("#90A4AE"),
                
                ActionDefault = new MudColor("#546E7A"),
                ActionDisabled = new MudColor("#CFD8DC"),
                ActionDisabledBackground = new MudColor("#ECEFF1")
            },
            PaletteDark = new PaletteDark
            {
                Primary = new MudColor("#42A5F5"),       // Helleres Blau für Dark Mode
                PrimaryDarken = "#1976D2",
                PrimaryLighten = "#64B5F6",
                Secondary = new MudColor("#26A69A"),     // Türkis
                SecondaryDarken = "#00897B",
                SecondaryLighten = "#4DB6AC",
                Tertiary = new MudColor("#FFB74D"),      // Orange
                
                Background = new MudColor("#0F2439"),    // Dunkelblau
                Surface = new MudColor("#162F4D"),       // Leicht helleres Dunkelblau
                
                AppbarBackground = new MudColor("#0A1929"),
                AppbarText = new MudColor("#FFFFFF"),
                
                DrawerBackground = new MudColor("#0A1929"),
                DrawerText = new MudColor("#FFFFFF"),
                DrawerIcon = new MudColor("#78909C"),
                
                Success = new MudColor("#66BB6A"),       // Helleres Grün für Dark Mode
                Warning = new MudColor("#FFA726"),       // Helleres Orange für Dark Mode
                Error = new MudColor("#EF5350"),         // Helleres Rot für Dark Mode
                Info = new MudColor("#42A5F5"),          // Helleres Info-Blau
                
                TextPrimary = new MudColor("#ECEFF1"),
                TextSecondary = new MudColor("#B0BEC5"),
                TextDisabled = new MudColor("#546E7A"),
                
                ActionDefault = new MudColor("#B0BEC5"),
                ActionDisabled = new MudColor("#37474F"),
                ActionDisabledBackground = new MudColor("#263238")
            },
            Typography = new Typography
            {
                Default = new DefaultTypography()
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "0.9rem",
                    LineHeight = "1.5"
                },
                H1 = new H1Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "2.5rem",
                    FontWeight = "300",
                    LineHeight = "1.2"
                },
                H2 = new H2Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "2rem",
                    FontWeight = "400",
                    LineHeight = "1.3"
                },
                H3 = new H3Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "1.75rem",
                    FontWeight = "400",
                    LineHeight = "1.4"
                },
                H4 = new H4Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "1.5rem",
                    FontWeight = "400",
                    LineHeight = "1.4"
                },
                H5 = new H5Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "1.25rem",
                    FontWeight = "400",
                    LineHeight = "1.4"
                },
                H6 = new H6Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "1rem",
                    FontWeight = "500",
                    LineHeight = "1.4"
                },
                Subtitle1 = new Subtitle1Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "1rem",
                    FontWeight = "400",
                    LineHeight = "1.5"
                },
                Subtitle2 = new Subtitle2Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "0.875rem",
                    FontWeight = "500",
                    LineHeight = "1.4"
                },
                Body1 = new Body1Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "0.9rem",
                    FontWeight = "400",
                    LineHeight = "1.5"
                },
                Body2 = new Body2Typography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "0.8rem", 
                    FontWeight = "400",
                    LineHeight = "1.43"
                },
                Button = new ButtonTypography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "0.875rem",
                    FontWeight = "500",
                    LineHeight = "1.5"
                },
                Caption = new CaptionTypography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "0.75rem",
                    FontWeight = "400",
                    LineHeight = "1.4"
                },
                Overline = new OverlineTypography
                {
                    FontFamily = new[] { "Roboto", "Segoe UI", "sans-serif" },
                    FontSize = "0.75rem",
                    FontWeight = "500",
                    LineHeight = "1.3"
                }
            },
            Shadows = new Shadow(),
            LayoutProperties = new LayoutProperties
            {
                DefaultBorderRadius = "4px",
                DrawerWidthLeft = "260px",
                DrawerWidthRight = "260px",
                DrawerMiniWidthLeft = "60px",
                DrawerMiniWidthRight = "60px",
                AppbarHeight = "64px"
            }
        };

        public static MudTheme DarkTheme => new MudTheme
        {
            PaletteLight = DefaultTheme.PaletteLight,
            PaletteDark = DefaultTheme.PaletteDark,
            Typography = DefaultTheme.Typography,
            Shadows = DefaultTheme.Shadows,
            LayoutProperties = DefaultTheme.LayoutProperties
        };
    }
}