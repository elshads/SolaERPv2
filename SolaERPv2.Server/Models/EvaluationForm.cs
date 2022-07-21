namespace SolaERPv2.Server.Models;

public class EvaluationForm : BaseModel
{
    public int PageIndex { get; set; }
    public string? StepCode { get; set; }
    public string? StepName { get; set; }
    public string? StepIcon { get; set; }
    public int ChildrenCount { get; set; }
}
