namespace SolaERPv2.Server.Models;

public class AttachmentSubType : BaseModel
{
    public int AttachmentSubTypeId { get; set; }
    public string? AttachmentSubTypeName { get; set; }
    public bool HasItems { get; set; }

    public static IEnumerable<AttachmentSubType> AttachmentSubTypeList { get; private set; } = new List<AttachmentSubType> {
            new AttachmentSubType() { AttachmentSubTypeId=1, AttachmentSubTypeName="Contract", HasItems=false },
            new AttachmentSubType() { AttachmentSubTypeId=2, AttachmentSubTypeName="E-Invoice", HasItems=false },
            new AttachmentSubType() { AttachmentSubTypeId=3, AttachmentSubTypeName="Delivery Notes", HasItems=false },
            new AttachmentSubType() { AttachmentSubTypeId=4, AttachmentSubTypeName="Vendor Invoice", HasItems=false },
            new AttachmentSubType() { AttachmentSubTypeId=5, AttachmentSubTypeName="Final Payment", HasItems=false },
            new AttachmentSubType() { AttachmentSubTypeId=6, AttachmentSubTypeName="Others", HasItems=false },
        };
}
