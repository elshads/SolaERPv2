namespace SolaERPv2.Server.Models;

public class ApproveData : BaseModel
{
    public int ModelId { get; set; }
    public int UserId { get; set; }
    public int ApproveStatusId { get; set; }
    public string? Comment { get; set; }
    public int Sequence { get; set; }
}   
