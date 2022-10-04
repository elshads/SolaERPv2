namespace SolaERPv2.Server.Models;

public class ApproveStageDetail : BaseModel
{
    public int ApproveStageDetailsId { get; set; }
    public int ApproveStageMainId { get; set; }
    public string? ApproveStageDetailsName { get; set; } = "Detail";
    public int Sequence { get; set; }


    public ApproveStageMain ApproveStageMain { get; set; }
    public List<ApproveStageRole> ApproveStageRoles { get; set; }

}
