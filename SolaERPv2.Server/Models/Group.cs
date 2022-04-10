namespace SolaERPv2.Server.Models;

public class Group : BaseModel
{
    public int GroupId { get; set; }
    public string? GroupName { get; set; }
    public string? Description { get; set; }
    public IEnumerable<AppUser>? Users { get; set; }
    public IEnumerable<BusinessUnit>? BusinessUnits { get; set; }
    public IEnumerable<Menu>? Menus { get; set; }
    public IEnumerable<ApproveRole>? ApproveRoles { get; set; }
    public IEnumerable<Unit>? Units { get; set; }
}