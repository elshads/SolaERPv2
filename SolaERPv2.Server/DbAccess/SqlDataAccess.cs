namespace SolaERPv2.Server.DbAccess;

public class SqlDataAccess
{
    public string? ConnectionString { get; private set; }
    public SqlDataAccess(string connectionString) => ConnectionString = connectionString;

    public async Task<IEnumerable<T>?> QueryAll<T>(string sql, DynamicParameters? p, CommandType commandType = CommandType.StoredProcedure) where T : IBaseModel, new()
    {
        IEnumerable<T>? result = new List<T>();
        try
        {
            using IDbConnection cn = new SqlConnection(ConnectionString);
            result = await cn.QueryAsync<T>(sql, p, commandType: commandType);
        }
        catch (Exception e)
        {
            var message = e.Message;
        }
        return result;
    }

    public async Task<T?> QuerySingle<T>(string sql, DynamicParameters? p, CommandType commandType = CommandType.StoredProcedure) where T : IBaseModel, new()
    {
        T? result = new();
        try
        {
            using IDbConnection cn = new SqlConnection(ConnectionString);
            IEnumerable<T>? _result = await cn.QueryAsync<T>(sql, p, commandType: commandType);
            return _result.FirstOrDefault();
        }
        catch (Exception e)
        {
            result.ReturnMessage = e.Message;
        }
        return result;
    }

    public async Task<SqlResult?> ExecuteSql(string sql, DynamicParameters? p, CommandType commandType = CommandType.StoredProcedure)
    {
        SqlResult? result = new();
        try
        {
            using IDbConnection cn = new SqlConnection(ConnectionString);
            result.QueryResult = await cn.ExecuteAsync(sql, p, commandType: commandType);
        }
        catch (Exception e)
        {
            result.QueryResultMessage = e.Message;
        }
        return result;
    }
}