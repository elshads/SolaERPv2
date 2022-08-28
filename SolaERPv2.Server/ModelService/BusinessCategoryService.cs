namespace SolaERPv2.Server.ModelService;

public class BusinessCategoryService : BaseModelService<BusinessCategory>
{
    AppUserService? _appUserService;
    SqlDataAccess? _sqlDataAccess;
    public BusinessCategoryService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<BusinessCategory>?> GetAll()
    {
        var sql = "SELECT * FROM dbo.VW_BusinessCategory";
        return await _sqlDataAccess.QueryAll<BusinessCategory>(sql, null, "BusinessCategory-GetAll", CommandType.Text);
    }
}