namespace SolaERPv2.Server.Models;

public class StageRole: BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "Role";
    public double Amount { get; set; }
    public double AmountTo { get; set; }

    public Stage? Stage { get; set; }
    public int? StageId { get; set; }
}
