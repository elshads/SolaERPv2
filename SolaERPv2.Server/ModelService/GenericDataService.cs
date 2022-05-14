namespace SolaERPv2.Server.ModelService;

public class GenericDataService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public GenericDataService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<string>?> GetSomething()
    {
        var sql = $"SELECT * FROM Table";
        return await _sqlDataAccess.QueryAll<string>(sql, null, CommandType.Text);
    }
}
