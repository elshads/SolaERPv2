namespace SolaERPv2.Server.ModelService;

public class BusinessUnitService : BaseModelService<BusinessUnit>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public BusinessUnitService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<BusinessUnit>?> GetAllAsync()
    {
        var sql = "SELECT * FROM dbo.VW_BusinessUnits_List";
        return await _sqlDataAccess.QueryAll<BusinessUnit>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<BusinessUnit>?> GetByUserIdAsync(int userId)
    {
        var sql = "dbo.SP_UserBusinessUnitsList";
        var p = new DynamicParameters();
        p.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<BusinessUnit>(sql, p);
    }
}
