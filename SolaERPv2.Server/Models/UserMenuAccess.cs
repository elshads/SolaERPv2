namespace SolaERPv2.Server.Models;

public class UserMenuAccess : BaseModel
{
    public int UserId { get; set; }
    public int MenuId { get; set; }
    public string? MenuName { get; set; }
    public string? MenuCode { get; set; }
    public bool ReadAccess { get; set; }
    public bool ExportAccess { get; set; }
    public bool CreateAccess { get; set; }
    public bool UpdateAccess { get; set; }
    public bool DeleteAccess { get; set; }
}