namespace SolaERPv2.Server.Models;

public class Menu : BaseModel
{
    public int MenuId { get; set; }
    public string? MenuName { get; set; }
    public string? MenuCode { get; set; }
    public int? ParentId { get; set; }
    public string? URL { get; set; }
    public string? Icon { get; set; }
    public IEnumerable<Menu>? Children { get; set; }
}