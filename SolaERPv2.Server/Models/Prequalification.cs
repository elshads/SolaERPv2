namespace SolaERPv2.Server.Models;

public class Prequalification : BaseModel
{
    public int VendorPrequalificationId { get; set; }
    public int PrequalificationDesignId { get; set; }
    public int VendorId { get; set; }
    public string? TextboxValue { get; set; }
    public string? TextareaValue { get; set; }
    public bool CheckboxValue { get; set; }
    public bool RadioboxValue { get; set; }
    public int IntValue { get; set; }
    public decimal DecimalValue { get; set; }
    public bool AgreementValue { get; set; }
    public DateTime DateTimeValue { get; set; } = new DateTime(1800, 1, 1);
    public List<Attachment>? AttachmentValue { get; set; }
    public List<object>? ListCollection { get; set; }
    public List<Bank>? BankList { get; set; }
    public List<GridValueModel>? GridValue { get; set; }
    public object? ListValue { get; set; }
    public decimal Scoring { get; set; }
    public decimal Outcome { get; set; }
}
