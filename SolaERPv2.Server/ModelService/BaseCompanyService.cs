namespace SolaERPv2.Server.ModelService;

public class BaseCompanyService:BaseModelService<BaseCompany>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;

    public BaseCompanyService(SqlDataAccess sqlDataAccess, AppUserService appUserService)
    {
        _sqlDataAccess = sqlDataAccess;
        _appUserService = appUserService;
    }

    public async Task<IEnumerable<BaseCompany>?> GetAll()
    {
        var sql = "Select * from dbo.BaseCompanies";
        return await _sqlDataAccess.QueryAll<BaseCompany>(sql, null, "BaseCompany-GetAll", CommandType.Text);
    }


}

//public class BaseCompanyServic
//{
//    //public IConfiguration Configuration;
//    //private const string BUGTRACKER_DATABASE = "BugTrackerDatabase";
//    //private const string SELECT_BUG = "select * from bugs";
//    //public BaseCompanyServic(IConfiguration configuration)
//    //{
//    //    Configuration = configuration; //Inject configuration to access Connection string from appsettings.json.
//    //}

//    //public async Task<List<BaseCompany>> GetBugsAsync()
//    //{
//    //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(BUGTRACKER_DATABASE)))
//    //    {
//    //        db.Open();
//    //        IEnumerable<BaseCompany> result = await db.QueryAsync<BaseCompany>(SELECT_BUG);
//    //        return result.ToList();
//    //    }
//    //}

//    //public async Task<int> GetBugCountAsync()
//    //{
//    //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(BUGTRACKER_DATABASE)))
//    //    {
//    //        db.Open();
//    //        int result = await db.ExecuteScalarAsync<int>("select count(*) from bugs");
//    //        return result;
//    //    }
//    //}

//    //public async Task AddBugAsync(Bug bug)
//    //{
//    //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(BUGTRACKER_DATABASE)))
//    //    {
//    //        db.Open();
//    //        await db.ExecuteAsync("insert into bugs (Summary, BugPriority, Assignee, BugStatus) values (@Summary, @BugPriority, @Assignee, @BugStatus)", bug);
//    //    }
//    //}

//    //public async Task UpdateBugAsync(Bug bug)
//    //{
//    //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(BUGTRACKER_DATABASE)))
//    //    {
//    //        db.Open();
//    //        await db.ExecuteAsync("update bugs set Summary=@Summary, BugPriority=@BugPriority, Assignee=@Assignee, BugStatus=@BugStatus where id=@Id", bug);
//    //    }
//    //}

//    //public async Task RemoveBugAsync(int bugid)
//    //{
//    //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(BUGTRACKER_DATABASE)))
//    //    {
//    //        db.Open();
//    //        await db.ExecuteAsync("delete from bugs Where id=@BugId", new { BugId = bugid });
//    //    }
//    //}
//}
