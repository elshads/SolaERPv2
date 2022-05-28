namespace SolaERPv2.Server.DbAccess;

public class SqlResult
{
    public int QueryResult { get; set; }
    //public int InsertedResult { get; set; }
    //public int InsertedResultCount { get; set; }
    //public int UpdatedResult { get; set; }
    //public int DeletedResult { get; set; }
    public string? QueryResultMessage { get; set; }
    //public string? InsertedResultMessage { get; set; }
    //public string? UpdatedResultMessage { get; set; }
    //public string? DeletedResultMessage { get; set; }
    public int ReturnId { get; set; }
    public string? ReturnCode { get; set; }
    //public IBaseModel? ReturnModel { get; set; }
}
