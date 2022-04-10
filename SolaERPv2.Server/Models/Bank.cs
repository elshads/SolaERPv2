namespace SolaERPv2.Server.Models;

public class Bank : BaseModel
{
    public string? BankCode { get; set; }
    public string? BankName { get; set; }
    public string? CurrencyCode { get; set; }
    public string? Account { get; set; }
}