namespace SolaERPv2.Server.Models;

public class ApproveStageRole : BaseModel
{
    public int ApproveStageRoleId { get; set; }
    public int ApproveStageDetailId { get; set; }
    public int ApproveRoleId { get; set; }
    public decimal AmountFrom { get; set; } = 99;
    public decimal AmountTo { get; set; } = 999999999;


    public ApproveStageDetail ApproveStageDetail { get; set; }

    public ApproveRole ApproveRole { get; set; }
    public string? ApproveRoleName { get; set; } = "Role";
    public List<ApproveRole> ApproveRoles { get; set; }
}
