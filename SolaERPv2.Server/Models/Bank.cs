namespace SolaERPv2.Server.Models;

public class Bank : BaseModel
{
    public int BankId { get; set; }
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public decimal Balance { get; set; }
    public string? Account { get; set; }
    public string? CurrencyCode { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BeneficiaryFullName { get; set; }
    public string? BeneficiaryAddress { get; set; }
    public string? BeneficiaryAddress1 { get; set; }
    public string? BeneficiaryBankName { get; set; }
    public string? BeneficiaryBankAddress { get; set; }
    public string? BeneficiaryBankAddress1 { get; set; }
    public string? BeneficiaryBankCode { get; set; }
    public string? IntermediaryBankCodeNumber { get; set; }
    public string? IntermediaryBankCodeType { get; set; }
    public bool IsNewItem { get; set; } = true;
}