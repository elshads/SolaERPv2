namespace SolaERPv2.Server.Models;

public class Group : BaseModel
{
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public List<AppUser>? Users { get; set; } = new();
    public List<BusinessUnit>? BusinessUnits { get; set; } = new();
    public List<Menu>? Menus { get; set; } = new();
    public List<ApproveRole>? ApproveRoles { get; set; } = new();
    public List<Unit>? Units { get; set; } = new();


    public int GroupAdditionalPrivilegeId { get; set; }
    public bool VendorDraft { get; set; }
}