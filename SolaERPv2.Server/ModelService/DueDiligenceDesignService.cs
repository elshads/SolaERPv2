namespace SolaERPv2.Server.ModelService;

public class DueDiligenceDesignService : BaseModelService<DueDiligenceDesign>
{
    AppUserService? _appUserService;
    SqlDataAccess? _sqlDataAccess;
    public DueDiligenceDesignService(AppUserService? appUserService, SqlDataAccess? sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }


    public async Task<IEnumerable<DueDiligenceDesign>?> GetAllAsync()
    {
        IEnumerable<DueDiligenceDesign>? result = new List<DueDiligenceDesign>();
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            result = await cn.QueryAsync<DueDiligenceDesign>("dbo.SP_DueDiligenceDesign_Load");
        }
        catch (Exception e)
        {
            var message = e.Message;
            Console.WriteLine($"DueDiligenceDesignService: {message}");
        }
        return result;
    }
}
