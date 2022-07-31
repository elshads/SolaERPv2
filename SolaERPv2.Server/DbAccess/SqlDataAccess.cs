namespace SolaERPv2.Server.DbAccess;

public class SqlDataAccess
{
    public string? ConnectionString { get; private set; }
    public SqlDataAccess(string connectionString) => ConnectionString = connectionString;

    public async Task<IEnumerable<T>?> QueryAll<T>(string sql, object? p, string errorMessageSource, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection cn = new SqlConnection(ConnectionString);
            return await cn.QueryAsync<T>(sql, p, commandType: commandType);
        }
        catch (Exception e)
        {
            var message = $"{errorMessageSource}: {e.Message}";
            return null;
        }
    }

    public async Task<T?> QuerySingle<T>(string sql, object? p, string errorMessageSource, CommandType commandType = CommandType.StoredProcedure)
    {
        try
        {
            using IDbConnection cn = new SqlConnection(ConnectionString);
            IEnumerable<T>? _result = await cn.QueryAsync<T>(sql, p, commandType: commandType);
            return _result.FirstOrDefault();
        }
        catch (Exception e)
        {
            var message = $"{errorMessageSource}: {e.Message}";
            return default(T);
        }
    }
    
    public async Task<SqlResult?> QueryReturnInteger(string sql, object? p, string errorMessageSource, CommandType commandType = CommandType.StoredProcedure)
    {
        SqlResult? result = new();
        try
        {
            using IDbConnection cn = new SqlConnection(ConnectionString);
            IEnumerable<int>? _result = await cn.QueryAsync<int>(sql, p, commandType: commandType);
            result.QueryResult = _result.FirstOrDefault();
        }
        catch (Exception e)
        {
            result.QueryResultMessage = $"{errorMessageSource}: {e.Message}";
        }
        return result;
    }

    public async Task<SqlResult?> ExecuteSql(string sql, object? p, string errorMessageSource, CommandType commandType = CommandType.StoredProcedure)
    {
        SqlResult? result = new();
        try
        {
            using IDbConnection cn = new SqlConnection(ConnectionString);
            result.QueryResult = await cn.ExecuteAsync(sql, p, commandType: commandType);
        }
        catch (Exception e)
        {
            result.QueryResultMessage = $"{errorMessageSource}: {e.Message}";
        }
        return result;
    }
}