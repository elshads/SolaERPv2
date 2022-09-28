namespace SolaERPv2.Server.Models;

public class StageRole: BaseModel
{
    public int StageRoleId { get; set; }
    public string Name { get; set; } = "Role";
    public decimal Amount { get; set; } = 0.99M;
    public decimal AmountTo { get; set; } = 0.999999M; 

    public Stage Stage { get; set; }
    public int StageId { get; set; }
}
