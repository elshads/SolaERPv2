namespace SolaERPv2.Server.Models;

public class PaymentDocumentPostMain : BaseModel
{
    public int PaymentDocumentMainId { get; set; }
    public string? BusinessUnitCode { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
    public string? Reference { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Comment { get; set; }
    public int PaymentDocumentPriorityId { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public string? BankCode { get; set; }
    public string? VATBankCode { get; set; }

    [Required(ErrorMessage = "This field is mandatory")]
    public string? ExpenseCode { get; set; }

    [Required(ErrorMessage = "This field is mandatory")]
    public string? GroupProject { get; set; }

    [Required(ErrorMessage = "This field is mandatory")]
    public string? Project { get; set; }
    public decimal BankCharge { get; set; }
    public List<PaymentDocumentPostDetail>? PaymentDocumentPostDetailList { get; set; } = new();
}