namespace SolaERPv2.Server.Models;

public class Vendor : BaseModel
{
    public int BusinessUnitId { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
}