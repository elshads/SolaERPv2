namespace SolaERPv2.Server.ModelService;

public class ProductService : BaseModelService<Product>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public ProductService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Product>?> GetAll()
    {
        var sql = "SELECT * FROM dbo.VW_ProductService_List";
        return await _sqlDataAccess.QueryAll<Product>(sql, null, "Product-GetAll", CommandType.Text);
    }
}
