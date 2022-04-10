using Microsoft.AspNetCore.Identity;

namespace SolaERPv2.Server.Identity;

public class AppRole : IdentityRole<int>, IBaseModel
{
    public int RowIndex { get; set; }
    public string? ReturnMessage { get; set; }
}
