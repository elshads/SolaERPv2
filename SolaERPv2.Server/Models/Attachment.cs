namespace SolaERPv2.Server.Models;

public class Attachment : BaseModel
{
    public int AttachmentId { get; set; }
    public string? FileName { get; set; }
    public byte[]? FileData { get; set; }
    public int SourceId { get; set; }
    public int AttachmentTypeId { get; set; }
    public int AttachmentSubTypeId { get; set; }
    public int SourceTypeId { get; set; }
    public string? SourceTypeName { get; set; }
    public string? Reference { get; set; }
    public string? ExtensionType { get; set; }
    public long Size { get; set; }
    public DateTime UploadDateTime { get; set; }
}
