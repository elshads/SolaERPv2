namespace SolaERPv2.Server.ModelService;

public class BaseModelService<T> where T : IBaseModel, new()
{
    public T GetInstanceClone(T originalItem)
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
