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

    public List<Attachment>? AttachmentList { get; set; } = new();
    public List<PaymentDocumentDetail>? PaymentDocumentDetailList { get; set; } = new();
}