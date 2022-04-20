namespace SolaERPv2.Server.Configurations;

public class MenuItem
{
    public int Id { get; set; }
    public int ParentId { get; set; }
    public bool Disabled { get; set; }
    public bool HasChildren { get; set; }
    public bool Separator { get; set; }
    public string? IconClass { get; set; }
    public string? Icon { get; set; }
    public string? ImageUrl { get; set; }
    public string? Text { get; set; }
    public string? Url { get; set; }
    public List<MenuItem>? Items { get; set; }
}
