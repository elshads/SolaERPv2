namespace SolaERPv2.Server.Models;

public class RequestDetails
{
    public int RequestId { get; set; }
    public int RequestLine { get; set; }
    public string? RequestNumber { get; set; }
    public string? ItemCode { get; set; }
    public string? ItemName { get; set; }
    public string? LineComment { get; set; }
    public decimal QTYOrdered { get; set; }
    public string? UOM { get; set; }
    public DateTime DueDate { get; set; }
    public string? Warehouse { get; set; }
}
