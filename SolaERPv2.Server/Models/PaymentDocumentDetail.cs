namespace SolaERPv2.Server.Models;

public class PaymentDocumentDetail : BaseModel
{
    public int PaymentDocumentDetailId { get; set; }
    public int BusinessUnitId { get; set; }
    public string? BusinessUnitCode { get; set; }
    public string? VendorCode { get; set; }
    public string? VendorName { get; set; }
    public int PaymentDocumentTypeId { get; set; }
    public string? PaymentDocumentTypeName { get; set; }
    public string? PONum { get; set; }
    public int Voucher { get; set; }
    public string? CurrencyCode { get; set; }
    public decimal Amount { get; set; }
    public decimal VAT { get; set; }
    public decimal POAmount { get; set; }
    public decimal PO_VAT { get; set; }
    public decimal VoucherAmount { get; set; }
    public decimal VoucherVAT { get; set; }
    public decimal AdvanceAmount { get; set; }
    public decimal AdvanceVAT { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal PaidVAT { get; set; }
    public decimal PaymentDocumentAmount { get; set; }
    public decimal PaymentDocumentVAT { get; set; }
    public decimal PaidPaymentDocumentAmount { get; set; }
    public decimal PaidPaymentDocumentVAT { get; set; }
    public decimal AmountToPay { get; set; }
    public decimal VATToPay { get; set; }
    public decimal RemainingAmount { get; set; }
    public decimal RemainingVAT { get; set; }
    public decimal TotalToPay { get; set; }
    public decimal IsVAT { get; set; }
}