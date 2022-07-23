namespace SolaERPv2.Server.Models;

public class FormCategory : BaseModel
{
    public int FormCategoryId { get; set; }
    public int AppFunctionId { get; set; }
    public string? AppFunctionName { get; set; }
    public int Sequence { get; set; }
    public string? Text { get; set; }
    public List<FormSubcategory>? FormSubcategoryList { get; set; }
}
