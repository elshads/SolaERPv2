namespace SolaERPv2.Server.Models;

public class ApproveStage : BaseModel
{
    public int ApprovalId { get; set; }
    public int ApproveStageMainId { get; set; }
    public string? ApproveStageName { get; set; }
    public int StagesCount { get; set; }
    public int ApproveStageDetailsId { get; set; }
    public string? ApproveStageDetailsName { get; set; }
    public int Sequence { get; set; }
    public int UserId { get; set; }
    public string? FullName { get; set; }
    public string? ApproveStatusId { get; set; }
    public string? ApproveStatusName { get; set; }
    public DateTime ApproveDate { get; set; }
    public string? Comment { get; set; }


    public int VendorId { get; set; }
}
