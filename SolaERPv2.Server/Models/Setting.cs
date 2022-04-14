namespace SolaERPv2.Server.Models;

public class Setting : BaseModel
{
    public int MaxFileSize { get; set; } = 1024 * 1024 * 10;
    public List<string>? AllowedFileExtensions { get; set; }
    public int MaxImageSize { get; set; } = 1024 * 1024 * 10;
    public List<string>? AllowedImageExtensions { get; set; }
    public int MaxNumberOfFiles { get; set; }
    public int BaseCurrencyId { get; set; }
    public string DefaultTheme { get; set; } = "light";
}