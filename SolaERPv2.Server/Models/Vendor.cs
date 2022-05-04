namespace SolaERPv2.Server.Models;

public class Vendor : BaseModel
{
    public int BusinessUnitId { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
    public string? TaxId { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public byte[]? Logo { get; set; }
    public int CreditDays { get; set; } = 60;
    public string? PaymentTermsCode { get; set; }
    public Analysis? PaymentTerms { get; set; }
    public List<string>? ProvidedProducts { get; set; }
    public string? RepresentedProducts { get; set; }
    public string? RepresentedCompanies { get; set; }
    public bool AgreeWithDefaultDays { get; set; } = true;
}