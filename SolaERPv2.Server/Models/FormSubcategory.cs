namespace SolaERPv2.Server.Models;

public class FormSubcategory : BaseModel
{
    public int FormSubcategoryId { get; set; }
    public int FormCategoryId { get; set; }
    public int Sequence { get; set; }
    public string? Text { get; set; }
    public bool HasRadioBox { get; set; }
    public bool HasCheckBox { get; set; }
    public bool HasTextBox { get; set; }
    public bool HasNumericBox { get; set; }
    public bool HasDecimalBox { get; set; }
    public bool HasDatePicker { get; set; }
    public bool HasAttachment { get; set; }
}
