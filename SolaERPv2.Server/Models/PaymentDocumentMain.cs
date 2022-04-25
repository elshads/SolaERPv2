namespace SolaERPv2.Server.Models;

public class PaymentDocumentMain : BaseModel
{
    public int PaymentDocumentMainId { get; set; }
    public int BusinessUnitId { get; set; }
    public string? BusinessUnitCode { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
    public string? Reference { get; set; }
    public string? Comment { get; set; }
    public int PaymentDocumentTypeId { get; set; }
    public string? PaymentDocumentTypeName { get; set; }
    public int PaymentDocumentPriorityId { get; set; } = 2;
    public string? PaymentDocumentPriorityName { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal Amount { get; set; }
    public decimal VAT { get; set; }
    public int StatusId { get; set; }
    public string? StatusName { get; set; }
    public int PaymentStatusId { get; set; }
    public string? PaymentStatusName { get; set; }
    public int ApproveStatusId { get; set; }
    public int Sequence { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public string? BankCode { get; set; }
    public string? VATBankCode { get; set; }
    public string? ExpenseCode { get; set; }
    public string? GroupProject { get; set; }
    public string? Project { get; set; }
    public string? OrNumber { get; set; }
    public string? JobNumber { get; set; }
    public string? FrameContract { get; set; }
    public string? CreatedUser { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime SentToApproveDate { get; set; }
    public decimal VendorBalance { get; set; }
    public decimal BankCharge { get; set; }
    public bool HasRequestAttach { get; set; }
    public bool HasBidAttach { get; set; }
    public bool HasOrderAttach { get; set; }
    public bool HasGRNAttach { get; set; }
    public bool HasInvoieAttach { get; set; }

    public List<Attachment>? AttachmentList { get; set; } = new();
    public List<PaymentDocumentDetail>? PaymentDocumentDetailList { get; set; } = new();
}