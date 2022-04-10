namespace SolaERPv2.Server.Models;

public class BusinessUnit : BaseModel
{
    public int BusinessUnitId { get; set; }
    public string? BusinessUnitCode { get; set; }
    public string? BusinessUnitName { get; set; }
}