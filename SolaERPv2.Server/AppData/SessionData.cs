namespace SolaERPv2.Server.AppData;

public class SessionData
{
    public SessionData(AppUserService appUserService)
    {
        CurrentUser = appUserService.GetCurrentUser();
    }
    public AppUser? CurrentUser { get; set; } = new();
    public PaymentDocumentMain? PaymentDocumentMain { get; set; }
    public List<RequestMain>? RequestMainList { get; set; }
}
