namespace SolaERPv2.Server.ModelService;

public class UserMenuAccessService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public UserMenuAccessService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<UserMenuAccess>?> GetByIdAsync()
    {
        var user = _appUserService.GetCurrentUserAsync();
        var p = new DynamicParameters();
        p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<UserMenuAccess>("dbo.SP_UserMenuAccess", p);
    }
}