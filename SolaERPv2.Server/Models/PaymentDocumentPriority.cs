namespace SolaERPv2.Server.Models;

public class PaymentDocumentPriority : BaseModel
{
    public int PaymentDocumentPriorityId { get; private set; }
    public string? PaymentDocumentPriorityName { get; private set; }

    public static IEnumerable<PaymentDocumentPriority> PaymentDocumentPriorityList { get; private set; } = new List<PaymentDocumentPriority> {
            new PaymentDocumentPriority() { PaymentDocumentPriorityId = 1, PaymentDocumentPriorityName = "High" },
            new PaymentDocumentPriority() { PaymentDocumentPriorityId = 2, PaymentDocumentPriorityName = "Normal" },
            new PaymentDocumentPriority() { PaymentDocumentPriorityId = 3, PaymentDocumentPriorityName = "Low" },
        };
}