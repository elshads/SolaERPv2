namespace SolaERPv2.Server.ModelService;

public class AppSettingsService : BaseModelService<AppSettings>
{
    public AppSettings AppSettings { get; set; } = new();
}
