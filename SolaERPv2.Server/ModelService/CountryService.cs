namespace SolaERPv2.Server.ModelService;

public class CountryService : BaseModelService<Country>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public CountryService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Country>?> GetAll()
    {
        var sql = "SELECT * FROM dbo.VW_Country_List";
        return await _sqlDataAccess.QueryAll<Country>(sql, null, CommandType.Text);
    }
}
