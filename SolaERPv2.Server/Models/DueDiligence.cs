﻿namespace SolaERPv2.Server.Models;

public class DueDiligence : BaseModel
{
    public int VendorDueDiligenceId { get; set; }
    public int DueDiligenceDesignId { get; set; }
    public int VendorId { get; set; }
    public string? TextboxValue { get; set; }
    public string? TextareaValue { get; set; }
    public bool CheckboxValue { get; set; }
    public bool RadioboxValue { get; set; }
    public bool AgreementValue { get; set; }
    public int IntValue { get; set; }
    public decimal DecimalValue { get; set; }
    public DateTime DateTimeValue { get; set; } = new DateTime(1800,1,1);
    public List<Attachment>? AttachmentValue { get; set; }
    public List<GridValueModel>? GridValue { get; set; }
    public List<Bank>? BankList { get; set; }
    public decimal Scoring { get; set; }
    public decimal Outcome { get; set; }
}
