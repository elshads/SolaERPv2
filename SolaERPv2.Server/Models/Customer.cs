namespace SolaERPv2.Server.Models;

public class Customer:BaseModel
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = "Company";
    public string Description { get; set; } = "Description";
    public string UnitCode { get; set; }

    public MainCompany MainCompany { get; set; }
    public int MainCompanyId { get; set; }
    public List<Stage> Stages { get; set; }
}
