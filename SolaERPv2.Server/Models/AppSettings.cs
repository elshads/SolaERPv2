namespace SolaERPv2.Server.Models;

public class AppSettings : BaseModel
{
    public AppSettings() { }
    public AppSettings(AppSettings appSettings)
    {
        AppUrl = appSettings.AppUrl;
        DefaultTheme = appSettings.DefaultTheme;
    }

    public string? AppUrl { get; set; } = "";
    public int MaxFileSize { get; set; } = 1024 * 1024 * 10;
    public List<string>? AllowedFileExtensions { get; set; }
    public int MaxNumberOfFiles { get; set; } = 20;
    public int MaxImageSize { get; set; } = 1024 * 1024 * 10;
    public string DefaultTheme { get; set; } = "light";
}