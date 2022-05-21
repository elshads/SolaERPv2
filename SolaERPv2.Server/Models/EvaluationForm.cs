namespace SolaERPv2.Server.Models;

public class EvaluationForm : BaseModel
{
    public int VendorEvaluationFormId { get; set; }
    public int VendorId { get; set; }
    public DateTime ExpirationDate { get; set; } = new DateTime(2022, 1, 1);
    public int ContextOfTheOrganization1 { get; set; }
    public int ContextOfTheOrganization2 { get; set; }
    public int ContextOfTheOrganization3 { get; set; }
    public int Leadership1 { get; set; }
    public int Leadership2 { get; set; }
    public int Planning1 { get; set; }
    public int Planning2 { get; set; }
    public int Planning3 { get; set; }
}
