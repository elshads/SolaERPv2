namespace SolaERPv2.Server.ModelService;

public class AnalysisService : BaseModelService<Analysis>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public AnalysisService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Analysis>?> GetAll(string sqlViewOrTable)
    {
        var sql = $"SELECT * FROM {sqlViewOrTable}";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<Analysis>?> GetExpenseTypes()
    {
        var sql = $"SELECT * FROM dbo.ExpenseTypesList";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<Analysis>?> GetGroupProjects()
    {
        var sql = $"SELECT * FROM dbo.GroupProjectsList";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<Analysis>?> GetProjectCodes()
    {
        var sql = $"SELECT * FROM dbo.ProjectCodesList";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<Analysis>?> GetOrNumbers()
    {
        var sql = $"SELECT * FROM dbo.OrNumberList";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<Analysis>?> GetJobNumbers()
    {
        var sql = $"SELECT * FROM dbo.JobNumberList";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<Analysis>?> GetPaymentTerms()
    {
        var sql = $"SELECT * FROM dbo.VW_PaymentTerms_List";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }
}
