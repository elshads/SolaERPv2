namespace SolaERPv2.Server.Models;

public class Person : BaseModel
{
    public int PersonId { get; set; }
    public string? UserName { get; set; }
    public string? PersonName { get; set; }
    public string? Position { get; set; }
    public string? PhoneNumber { get; set; }
}
