namespace SolaERPv2.Server.Models;

public class BusinessUnit : BaseModel
{
    public int BusinessUnitId { get; set; }
    public string? BusinessUnitCode { get; set; }
    public string? BusinessUnitName { get; set; }
    public string? TaxId { get; set; }
    public string? Address { get; set; }
    public string? CountryCode { get; set; }
    public bool IsAgree { get; set; }
    public string? Position { get; set; }
    public string? FullName { get; set; }
}