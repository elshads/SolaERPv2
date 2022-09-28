namespace SolaERPv2.Server.Models;

public class MainCompany:BaseModel
{
    public int MainCompanyId { get; set; }
    public string UnitName { get; set; }
    public string UnitCode { get; set; }
    public List<Customer> Customers { get; set; }
}
