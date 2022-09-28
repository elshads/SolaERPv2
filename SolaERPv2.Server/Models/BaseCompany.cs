namespace SolaERPv2.Server.Models;

public class BaseCompany:BaseModel
{
    public int Id { get; set; }
    public string Name { get; set; } = "Company";
    public List<Company>? Companies { get; set; }
}
