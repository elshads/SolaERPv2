namespace SolaERPv2.Server.ModelService;

public class MainCompanyService:BaseModelService<MainCompany>
{
    SqlDataAccess _sqlDataAccess;

    public MainCompanyService(SqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<MainCompany>> GetAllAsync()
    {
        IEnumerable<MainCompany> result = new List<MainCompany>();
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess?.ConnectionString);
            result = await cn.QueryAsync<MainCompany>("SELECT * FROM dbo.MainCompanies");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return result;
    }

}
