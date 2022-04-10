namespace SolaERPv2.Server.Models;

public class ApproveStage : BaseModel
{
    public int PaymentDocumentApprovalId { get; set; }
    public string? ApproveStageDetailsName { get; set; }
    public int Sequence { get; set; }
    public string? FullName { get; set; }
    public string? ApproveStatusName { get; set; }
    public DateTime ApproveDate { get; set; }
    public string? Comment { get; set; }
}
