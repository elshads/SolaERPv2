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
    public string? CountryCode { get; set; }
    public Analysis? PaymentTerms { get; set; }
    public List<int>? ProvidedProducts { get; set; }
    public List<Bank>? BankList { get; set; }
    public string? OtherProducts { get; set; }
    public string? RepresentedProducts { get; set; }
    public string? RepresentedCompanies { get; set; }
    public bool AgreeWithDefaultDays { get; set; } = true;
}