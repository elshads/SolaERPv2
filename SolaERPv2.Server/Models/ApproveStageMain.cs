namespace SolaERPv2.Server.Models;

public class ApproveStageMain:BaseModel
{
    public int ApproveStageMainId { get; set; }
    public int ProcedureId { get; set; }
    public int BusinessUnitId { get; set; }
    
    [Required(ErrorMessage = "The First name is required")]
    public string ApproveStageName { get; set; }


    public Procedure? Procedure { get; set; }
    public string? ProcedureName { get; set; }

    public BusinessUnit? BusinessUnit { get; set; }
    public string? BusinessUnitName { get; set; }

    public List<ApproveStageDetail>? ApproveStageDetails { get; set; }
}
