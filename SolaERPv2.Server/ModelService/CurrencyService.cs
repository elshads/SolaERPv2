namespace SolaERPv2.Server.ModelService;

public class CurrencyService : BaseModelService<Currency>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public CurrencyService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Currency>?> GetAll()
    {
        var sql = "SELECT * FROM dbo.VW_Currency_List";
        return await _sqlDataAccess.QueryAll<Currency>(sql, null, CommandType.Text);
    }
}