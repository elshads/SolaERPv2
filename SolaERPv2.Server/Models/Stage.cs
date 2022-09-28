using SolaERPv2.Server.Pages;

namespace SolaERPv2.Server.Models;

public class Stage:BaseModel
{
    public int StageId { get; set; }
    public string Name { get; set; } = "Stage";
    public int Sequence { get; set; }


    public Company? Company { get; set; }
    public int? CompanyId { get; set; }


    public List<StageRole>? StageRoles { get; set; }
}
