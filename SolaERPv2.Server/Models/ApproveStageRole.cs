namespace SolaERPv2.Server.Models;

public class ApproveStageRole : BaseModel
{
    public int ApproveStageRoleId { get; set; }
    public int ApproveStageDetailId { get; set; }
    public int ApproveRoleId { get; set; }
    public decimal AmountFrom { get; set; }
    public decimal AmountTo { get; set; }


    public ApproveStageDetail ApproveStageDetail { get; set; }

    public ApproveRole ApproveRole { get; set; }
    public List<ApproveRole> ApproveRoles { get; set; }
}
