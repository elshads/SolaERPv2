namespace SolaERPv2.Server.AppData;

public class SessionData
{
    public SessionData(AppUserService appUserService)
    {
        CurrentUser = appUserService.GetCurrentUser();
    }
    public AppUser? CurrentUser { get; set; } = new();
    public List<UserMenuAccess>? CurrentUserMenuAccessList { get; set; }
    public PaymentDocumentMain? PaymentDocumentMain { get; set; }
}
