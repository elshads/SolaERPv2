namespace SolaERPv2.Server.Models;

public class MessageDefinition
{
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public int UserId { get; set; }
    public string? UserEmail { get; set; }
    public string? Subject { get; set; }
    public string? MessageText { get; set; }
    public string? MessageSource { get; set; }
    public DateTime MessageDateTime { get; set; } = DateTime.Now;
}
