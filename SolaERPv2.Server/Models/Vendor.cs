namespace SolaERPv2.Server.Models;

public class Vendor : BaseModel
{
    public int VendorId { get; set; }
    public int BusinessUnitId { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
    public string? TaxId { get; set; }
    public string? CompanyLocation { get; set; }
    public string? CompanyWebsite { get; set; }
    public Attachment? CompanyLogo { get; set; }
    public int CompanyLogoId { get; set; }
    public string? CompanyLogoName { get; set; }
    public Attachment? OfficialLetter { get; set; }
    public int OfficialLetterId { get; set; }
    public string? OfficialLetterName { get; set; }
    public int CreditDays { get; set; } = 60;
    public string? PaymentTermsCode { get; set; }
    public string? CountryCode { get; set; }
    public Analysis? PaymentTerms { get; set; }
    public List<int>? ProvidedProductIdList { get; set; }
    public List<Product>? ProvidedProductList { get; set; }
    public List<Bank>? BankList { get; set; }
    public List<AppUser>? CompanyUsers { get; set; }
    public List<ApproveStage>? ApproveStageList { get; set; }
    public int NumberOfUsers { get; set; }
    public EvaluationForm? EvaluationForm { get; set; }
    public string? OtherProducts { get; set; }
    public string? RepresentedProducts { get; set; }
    public string? RepresentedCompanies { get; set; }
    public bool AgreeWithDefaultDays { get; set; } = true;
    public bool Exists { get; set; }
    public int StatusId { get; set; }
    public string? StatusName { get; set; }
    public int Sequence { get; set; }
}