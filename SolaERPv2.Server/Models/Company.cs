namespace SolaERPv2.Server.Models;

public class Company:BaseModel
{
    public int CompanyId { get; set; }
    public string Name { get; set; } = "Company";
    public string Description { get; set; } = "Description";
    public BaseCompany? BaseCompany { get; set; }
    public int? BaseCompanyId { get; set; }

    public List<Stage>? Stages { get; set; }
}
