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

    public async Task<IEnumerable<BusinessUnit>> GetAllAsync()
    {
        IEnumerable<BusinessUnit> result = new List<BusinessUnit>();
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess?.ConnectionString);
            result = await cn.QueryAsync<BusinessUnit>("SELECT * FROM dbo.VW_BusinessUnits_List");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return result;
    }

    public async Task<IEnumerable<BusinessUnit>?> GetByUserIdAsync(int userId)
    {
        var sql = "dbo.SP_UserBusinessUnitsList";
        var p = new DynamicParameters();
        p.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<BusinessUnit>(sql, p, "BU-GetByUserId");
    }
}
