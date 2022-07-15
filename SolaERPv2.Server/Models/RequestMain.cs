namespace SolaERPv2.Server.Models;

public class RequestMain : BaseModel
{
    public int RequestId { get; set; }
    public int BusinessUnitId { get; set; }
    public string? RequestNumber { get; set; }
    public DateTime RequestDate { get; set; }
    public string? Approver { get; set; }
    public string? Requester { get; set; }
    public string? Comment { get; set; }
    public List<RequestDetails>? RequestDetailsList { get; set; }
}
