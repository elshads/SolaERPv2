namespace SolaERPv2.Server.ModelService;

public class BankService : BaseModelervice<Bank>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public BankService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Bank>?> GetAll()
    {
        var sql = "SELECT * FROM dbo.BankCodesList";
        return await _sqlDataAccess.QueryAll<Bank>(sql, null, CommandType.Text);
    }
}