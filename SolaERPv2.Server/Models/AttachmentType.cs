namespace SolaERPv2.Server.Models;

public class AttachmentType : BaseModel
{
    public int AttachmentTypeId { get; private set; }
    public string? AttachmentTypeName { get; private set; }

    public IEnumerable<AttachmentSubType>? AttachmentSubTypeList { get; set; }

    public static IEnumerable<AttachmentType> AttachmentTypeList { get; private set; } = new List<AttachmentType> {
            new AttachmentType() { AttachmentTypeId = 1, AttachmentTypeName = "Advance", AttachmentSubTypeList = AttachmentSubType.AttachmentSubTypeList.Where(e => new[] {1,2,4,6 }.Contains(e.AttachmentSubTypeId)) },
            new AttachmentType() { AttachmentTypeId = 2, AttachmentTypeName = "Internal PO", AttachmentSubTypeList = AttachmentSubType.AttachmentSubTypeList.Where(e => new[] {1,2,3,4,5,6 }.Contains(e.AttachmentSubTypeId)) },
            new AttachmentType() { AttachmentTypeId = 3, AttachmentTypeName = "Production PO", AttachmentSubTypeList = AttachmentSubType.AttachmentSubTypeList.Where(e => new[] {1,2,3,4,5,6 }.Contains(e.AttachmentSubTypeId)) },
            new AttachmentType() { AttachmentTypeId = 4, AttachmentTypeName = "Service", AttachmentSubTypeList = AttachmentSubType.AttachmentSubTypeList.Where(e => new[] {1,2,3,4,5,6 }.Contains(e.AttachmentSubTypeId)) },
            new AttachmentType() { AttachmentTypeId = 5, AttachmentTypeName = "Landed Cost", AttachmentSubTypeList = AttachmentSubType.AttachmentSubTypeList.Where(e => new[] {1,2,4,5,6 }.Contains(e.AttachmentSubTypeId)) },
        };
}
