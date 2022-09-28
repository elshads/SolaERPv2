using SolaERPv2.Server.Pages;

namespace SolaERPv2.Server.Models;

public class Stage:BaseModel
{
    public int StageId { get; set; }
    public string Name { get; set; } = "Stage";
    public int Sequence { get; set; }
    public Customer Customer { get; set; }
    public int CustomerId { get; set; }
    public List<StageRole> StageRoles { get; set; }
}
