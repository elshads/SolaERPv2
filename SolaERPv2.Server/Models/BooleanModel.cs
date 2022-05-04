namespace SolaERPv2.Server.Models;

public class BooleanModel
{
    public string? Name { get; private set; }
    public bool Value { get; private set; }
    public static IEnumerable<BooleanModel>? BooleanModelList { get; private set; } = new List<BooleanModel>() { new() { Name = "Yes", Value = true }, new() { Name = "No", Value = false } };
}
