using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolaERPv2.Server.Identity;

public class AppUser : IdentityUser<int>, IBaseModel
{
    public AppUser()
    {
        RowIndex = -1;
        ChangePassword = false;
        StatusId = 0;
        Theme = "light";
        ExpirationDate = new DateTime(2099, 12, 31);
        CreatedOn = DateTime.Now;
        UpdatedOn = DateTime.Now;
    }


    public int RowIndex { get; set; }
    public string? FullName { get; set; }
    public string? NotificationEmail { get; set; }
    public bool ChangePassword { get; set; }
    public int StatusId { get; set; }
    public string? Theme { get; set; }
    public string? SyteLineUserCode { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int Sessions { get; set; }
    public DateTime LastActivity { get; set; }
    public byte[]? Photo { get; set; }
    public string? ReturnMessage { get; set; }
    public DateTime CreatedOn { get; set; }
    public int CreatedBy { get; set; }
    public DateTime UpdatedOn { get; set; }
    public int UpdatedBy { get; set; }
    [NotMapped]
    public IEnumerable<Menu>? UserMenuList { get; set; } = new List<Menu>();
}
