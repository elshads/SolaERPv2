namespace SolaERPv2.Server.Models;

public class PaymentDocumentType : BaseModel
{
    public int PaymentDocumentTypeId { get; private set; }
    public string? PaymentDocumentTypeName { get; private set; }

    public static IEnumerable<PaymentDocumentType> PaymentDocumentTypeList { get; private set; } = new List<PaymentDocumentType> {
            new PaymentDocumentType() { PaymentDocumentTypeId = 0, PaymentDocumentTypeName="Order" },
            new PaymentDocumentType() { PaymentDocumentTypeId = 1, PaymentDocumentTypeName = "Advance" }
        };
}