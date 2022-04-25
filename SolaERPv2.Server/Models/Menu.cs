namespace SolaERPv2.Server.Models;

public class Menu : BaseModel
{
    public int MenuId { get; set; }
    public string? MenuName { get; set; }
    public string? MenuCode { get; set; }
    public int? ParentId { get; set; }
    public string? URL { get; set; }
    public string? Icon { get; set; }
    public int UserId { get; set; }
    public bool CreateAccess { get; set; }
    public bool EditAccess { get; set; }
    public bool DeleteAccess { get; set; }
    public bool ExportAccess { get; set; }
    public bool HasChildren { get; set; }
    public IEnumerable<Menu>? Children { get; set; }
}