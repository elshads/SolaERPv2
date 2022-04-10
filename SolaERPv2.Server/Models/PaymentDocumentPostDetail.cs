namespace SolaERPv2.Server.Models;

public class PaymentDocumentPostDetail : BaseModel
{
    public int PaymentDocumentDetailId { get; set; }
    public string? PONum { get; set; }
    public int Voucher { get; set; }
    public decimal BankAmount { get; set; }
    public decimal VendorAmount { get; set; }
    public decimal BankRate { get; set; }
    public decimal VendorRate { get; set; }
    public decimal VAT { get; set; }
    public decimal VATBankAmount { get; set; }
    public decimal AmountToPay { get; set; }
    public decimal VATToPay { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal PaidVAT { get; set; }
    public decimal POAmount { get; set; }
    public decimal PO_VAT { get; set; }
    public decimal VoucherAmount { get; set; }
    public decimal VoucherVAT { get; set; }
    public decimal AdvanceAmount { get; set; }
    public decimal AdvanceVAT { get; set; }
    public decimal PaymentDocumentAmount { get; set; }
    public decimal PaymentDocumentVAT { get; set; }
    public decimal TotalToPay { get; set; }
    public decimal IsVAT { get; set; }
}