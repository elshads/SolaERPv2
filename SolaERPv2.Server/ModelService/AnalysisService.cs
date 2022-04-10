namespace SolaERPv2.Server.ModelService;

public class AnalysisService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public AnalysisService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Analysis>?> GetAll(int analysisTypeId)
    {
        var view = (analysisTypeId == 1 ? "ExpenseTypesList" : analysisTypeId == 2 ? "GroupProjectsList" : analysisTypeId == 3 ? "ProjectCodesList" : "AnalysisServiceError");
        var sql = $"SELECT * FROM dbo.{view}";
        return await _sqlDataAccess.QueryAll<Analysis>(sql, null, CommandType.Text);
    }
}
