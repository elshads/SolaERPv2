namespace SolaERPv2.Server.Models;

public class Vendor : BaseModel
{
    public int BusinessUnitId { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
    public string? TaxId { get; set; }
    public string? CompanyLocation { get; set; }
    public string? CompanyWebsite { get; set; }
    public byte[]? CompanyLogo { get; set; }
    public int CreditDays { get; set; } = 60;
    public string? PaymentTermsCode { get; set; }
    public Analysis? PaymentTerms { get; set; }
    public List<string>? ProvidedProducts { get; set; }
    public List<string>? RepresentedProducts { get; set; }
    public List<string>? RepresentedCompanies { get; set; }
    public bool AgreeWithDefaultDays { get; set; } = true;
}