namespace SolaERPv2.Server.ModelService;

public class VendorService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public VendorService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Vendor>?> GetAllAsync(int businessUnitId)
    {
        var p = new DynamicParameters();
        p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<Vendor>("dbo.SP_VendorList", p);
    }

    public async Task<Vendor?> GetByTaxIdAsync(string taxId)
    {
        var p = new DynamicParameters();
        p.Add("@TaxId", taxId, DbType.String, ParameterDirection.Input);
        return await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorsTaxCheck", p);
    }
}