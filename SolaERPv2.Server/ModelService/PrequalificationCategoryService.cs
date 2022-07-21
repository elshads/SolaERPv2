namespace SolaERPv2.Server.ModelService;

public class PrequalificationCategoryService : BaseModelService<PrequalificationCategory>
{
    AppUserService? _appUserService;
    SqlDataAccess? _sqlDataAccess;
    public PrequalificationCategoryService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<PrequalificationCategory>?> GetAll()
    {
        var sql = "SELECT * FROM dbo.VW_PrequalificationCategoryList";
        return await _sqlDataAccess.QueryAll<PrequalificationCategory>(sql, null, "PrequalificationCategory-GetAll", CommandType.Text);
    }
}
