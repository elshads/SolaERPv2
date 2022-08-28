namespace SolaERPv2.Server.Models;

public class PrequalificationDesign : BaseModel
{
    public int PrequalificationDesignId { get; set; }
    public int LineNo { get; set; }
    public string? Discipline { get; set; }
    public string? Label { get; set; }
    public bool HasTextbox { get; set; }
    public bool HasTextarea { get; set; }
    public bool HasCheckbox { get; set; }
    public bool HasRadiobox { get; set; }
    public bool HasInt { get; set; }
    public bool HasDecimal { get; set; }
    public bool HasDateTime { get; set; }
    public bool HasAttachment { get; set; }
    public bool HasList { get; set; }
    public bool HasBankList { get; set; }
    public bool HasGrid { get; set; }
    public bool HasAgreement { get; set; }
    public string? AgreementText { get; set; }
    public int GridRowLimit { get; set; }
    public int GridColumnCount { get; set; }
    public string? Title { get; set; }
    public string? Column1Alias { get; set; }
    public string? Column2Alias { get; set; }
    public string? Column3Alias { get; set; }
    public string? Column4Alias { get; set; }
    public string? Column5Alias { get; set; }
    public int Weight { get; set; }
}
