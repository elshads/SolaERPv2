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
            Error = "#d51923",
        },

        PaletteDark = new Palette() {
            Background = "#1e1e1e",
            DrawerBackground = "#2d2d2d",
            TextPrimary = "#fafafa",
            Surface = "#303030",
            Error = "#d51923",
        }
    };
    
}
