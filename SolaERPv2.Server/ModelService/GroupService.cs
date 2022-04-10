namespace SolaERPv2.Server.ModelService;

public class GroupService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public GroupService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Group>?> GetAllAsync()
    {
        var sql = "dbo.SP_GroupMain_Load";
        return await _sqlDataAccess.QueryAll<Group>(sql, null);
    }
}