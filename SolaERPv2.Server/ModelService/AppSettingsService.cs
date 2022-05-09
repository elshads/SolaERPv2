namespace SolaERPv2.Server.ModelService;

public class AppSettingsService : BaseModelervice<AppSettings>
{
    public AppSettings AppSettings { get; set; } = new();
}
