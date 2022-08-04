namespace SolaERPv2.Server.Models;

public class BaseModel : IBaseModel
{
    public string? ReturnMessage { get; set; }
    public int RowIndex { get; set; }

    public T GetInstanceClone<T>(T originalItem) where T : new()
    {
        T newItem = new();
        var properties = typeof(T).GetProperties();
        foreach (var property in properties)
        {
            property.SetValue(newItem, property.GetValue(originalItem));
        }
        return newItem;
    }

}
