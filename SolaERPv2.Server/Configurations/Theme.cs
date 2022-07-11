using MudBlazor.Utilities;

namespace SolaERPv2.Server.Configurations;

public static class Theme
{
    public static MudTheme? DefaultTheme { get; set; } = new MudTheme() {
        LayoutProperties = new LayoutProperties() { },

        Palette = new Palette() {
            Background = "#ffffff",
            DrawerBackground = "#ffffff",
            TextPrimary = "#424242",
            Surface = "#ffffff",
            Secondary = "f0f0f0",
            SecondaryContrastText = "#424242",
            Error = "#d51923",
        },

        PaletteDark = new Palette() {
            Background = "#1e1e1e",
            DrawerBackground = "#2d2d2d",
            TextPrimary = "#e6e6e6",
            Surface = "#303030",
            Secondary = "424242",
            SecondaryContrastText = "#e6e6e6",
            Error = "#d51923",
        }
    };
    
}
