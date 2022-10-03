namespace SolaERPv2.Server.Models;

public class ApproveStageMain:BaseModel
{
    public int ApproveStageMainId { get; set; }
    public int ProcedureId { get; set; }
    public int BusinessUnitId { get; set; }
    public string? ApproveStageName { get; set; } = "MainName";


    public Procedure? Procedure { get; set; }


    public BusinessUnit? BusinessUnit { get; set; }
    public string? BusinessUnitName { get; set; }

    public List<ApproveStageDetail>? ApproveStageDetails { get; set; }
}
