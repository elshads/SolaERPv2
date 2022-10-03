namespace SolaERPv2.Server.ViewModels;

public  class  FullApproveStageMain
{
    public int ApproveStageMainId { get; set; }
    public string? ApproveStageName { get; set; } = "MainName";
    public int BusinessUnitId { get; set; }

    public int ProcedureId { get; set; }
    public string? ProcedureKey { get; set; }
    public string? ProcedureName { get; set; }


}
