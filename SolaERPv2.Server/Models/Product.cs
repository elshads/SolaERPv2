namespace SolaERPv2.Server.Models;

public class Product : BaseModel
{
    public int ProductServiceId { get; set; }
    public string? ProductServiceName { get; set; }
    public int Other { get; set; }
}
