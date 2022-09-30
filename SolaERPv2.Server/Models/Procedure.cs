namespace SolaERPv2.Server.Models;

public class Procedure : BaseModel
{
    public int ProcedureId { get; set; }
    public string ProcedureKey { get; set; }
    public string ProcedureName { get; set; }
    public List<ApproveStageMain> ApproveStageMains { get; set; }
}
