namespace SolaERPv2.Server.Models;

public class ApproveStageMain:BaseModel
{
    public int ApproveStageMainId { get; set; }
    public int ProcedureId { get; set; }
    public int BusinessUnitId { get; set; }
    public string? ApproveStageName { get; set; } = "MainName";


    public Procedure? Procedure { get; set; }
    public string? ProcedureName { get; set; }

    public List<ApproveStageDetail>? ApproveStageDetails { get; set; }
}
