namespace SolaERPv2.Server.ModelService;

public class PrequalificationDesignService : BaseModelService<PrequalificationDesign>
{
    AppUserService? _appUserService;
    SqlDataAccess? _sqlDataAccess;
    public PrequalificationDesignService(AppUserService? appUserService, SqlDataAccess? sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }


    public async Task<IEnumerable<PrequalificationDesign>?> GetAllAsync(int categoryId)
    {
        IEnumerable<PrequalificationDesign>? result = new List<PrequalificationDesign>();
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            result = await cn.QueryAsync<PrequalificationDesign>("dbo.SP_PrequalificationDesign_Load", new { CategoryId = categoryId }, commandType: CommandType.StoredProcedure);
        }
        catch (Exception e)
        {
            var message = e.Message;
            Console.WriteLine($"PrequalificationDesignService: {message}");
        }
        return result;
    }
}