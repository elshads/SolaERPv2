namespace SolaERPv2.Server.Models;

public interface IBaseModel
{
    public int RowIndex { get; set; }
    public string? ReturnMessage { get; set; }
}
