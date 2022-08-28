namespace SolaERPv2.Server.Models;

public class GridValueModel : BaseModel
{
    public int DueDiligenceGridDataId { get; set; }
    public int DueDiligenceDesignId { get; set; }
    public int PrequalificationGridDataId { get; set; }
    public int PrequalificationDesignId { get; set; }
    public string? Column1 { get; set; }
    public string? Column2 { get; set; }
    public string? Column3 { get; set; }
    public string? Column4 { get; set; }
    public string? Column5 { get; set; }
}
