namespace SolaERPv2.Server.Models;

public class DueDiligenceDesign : BaseModel
{
    public int DueDiligenceDesignId { get; set; }
    public int LineNo { get; set; }
    public string? Label { get; set; }
    public bool HasTextbox { get; set; }
    public bool HasTextarea { get; set; }
    public bool HasCheckbox { get; set; }
    public bool HasRadiobox { get; set; }
    public bool HasInt { get; set; }
    public bool HasDecimal { get; set; }
    public bool HasDateTime { get; set; }
    public bool HasAttachment { get; set; }
    public bool HasBankList { get; set; }  
    public string? Title { get; set; }
    public int Weight { get; set; }

}
