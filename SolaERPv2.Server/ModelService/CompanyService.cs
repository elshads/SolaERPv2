using Microsoft.AspNetCore.Mvc;
using SolaERPv2.Server.Models;

namespace SolaERPv2.Server.ModelService;

public class CompanyService:BaseModelService<Company>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;

    public CompanyService(SqlDataAccess sqlDataAccess, AppUserService appUserService)
    {
        _sqlDataAccess = sqlDataAccess;
        _appUserService = appUserService;

    }

    //public async Task<IEnumerable<Company>?> GetAll()
    //{
    //    var sql = "Select * from dbo.Companies";
    //    return await _sqlDataAccess.QueryAll<Company>(sql, null, "Company-GetAll", CommandType.Text);
    //}

    public async Task<IEnumerable<Company>?> GetAll()
    {
        Dictionary<int, Company> result = new();

        var sql = @"select c.[Id] as 'CompanyId'
		,c.[Name]
		,c.[Description]
		,c.[BaseCompanyId]
		,st.[Id]
		,st.[Name]
		,st.[Sequence]
		,st.[CompanyId]
		from Stages st
		right join Companies c
		on c.Id=st.CompanyId";

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        await cn.QueryAsync<Company, Stage, Company>(sql,
        (company, stage) =>
        {
            if (!result.ContainsKey(company.CompanyId))
            {
                company.Stages = new();
                result.Add(company.CompanyId, company);
            }
            var currentCompany = result[company.CompanyId];
            if (stage != null) { currentCompany.Stages.Add(stage); }
            return company;
        },
        splitOn: "Id",
        commandType: CommandType.Text);


        return result.Values;
    }


    public async Task<Company?> GetByIdAsync(int companyId)
    {
        Dictionary<int, Company> result = new();

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        await cn.QueryAsync<Company, Stage,StageRole, Company>("dbo.sp_Company_Load",
            (company, stage,stageRole) =>
            {
                if (!result.ContainsKey(company.CompanyId))
                {
                    company.Stages = new();
                    result.Add(company.CompanyId, company);
                }
                var currentCompany = result[company.CompanyId];


                if (stage != null)
                {
                    stage.StageRoles ??= new();
                    stage.StageRoles?.Add(stageRole);

                    currentCompany.Stages?.Add(stage);

                }

                return company;
            },
            param: new { CompanyId = companyId },
            splitOn: "Id,StageId",
            commandType: CommandType.StoredProcedure);

        return result.Values.FirstOrDefault();
    }


    public async Task<SqlResult?> Save(Company company)
    {
        SqlResult? sqlResult = new();
        var companyId = company.CompanyId;

        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            var p = new DynamicParameters();
            p.Add("@Id", company.CompanyId, DbType.Int32, ParameterDirection.Input);
            p.Add("@Name", company.Name, DbType.String, ParameterDirection.Input);
            p.Add("@Description", company.Description, DbType.String, ParameterDirection.Input);
            var result = await cn.QueryAsync<int>("select * from dbo.Companies", p, commandType: CommandType.Text);

            if (result.Any()) { companyId = result.FirstOrDefault(); }
        }
        catch (Exception e)
        {
            sqlResult.QueryResultMessage = e.Message;
        }

        if (sqlResult?.QueryResultMessage != null) { return sqlResult; } // return if failed


        sqlResult.QueryResult = companyId;
        return sqlResult;
    }



    //public async Task<Company?> GetByIdAsync(int id)
    //{
    //    var sql = "dbo.spCompany_Get";
    //    var p = new DynamicParameters();
    //    p.Add("@CompanyId", id, DbType.Int32, ParameterDirection.Input);
    //    return await _sqlDataAccess.QuerySingle<Company>(sql, p, "Company-GetById");
    //}

    //public async Task<SqlResult?> Save(Company company)
    //{
    //    var user = await _appUserService.GetCurrentUserAsync();
    //    SqlResult? result = new();
    //    using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
    //    {
    //        var p = new DynamicParameters();
    //        p.Add("@Id", company.Id, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@Name", company.Name, DbType.String, ParameterDirection.Input);
    //        p.Add("@Description", company.Description, DbType.String, ParameterDirection.Input);

    //        p.Add("@NewCompanyId", dbType: DbType.Int32, direction: ParameterDirection.Output);

    //        result.QueryResult = await cn.ExecuteAsync("dbo.Companies", p, commandType: CommandType.StoredProcedure);
    //        result.QueryResult = p.Get<int>("@NewId");
    //    }


    //    return result;
    //}




    //public async Task<SqlResult?> Delete(IEnumerable<int> companyIdList)
    //{
    //    SqlResult? sqlResult = new();
    //    foreach (var item in companyIdList)
    //    {
    //        var p = new DynamicParameters();
    //        p.Add("@CompanyId", item, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@NewCompanyId", dbType: DbType.Int32, direction: ParameterDirection.Output);
    //        sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Company_IUD", p, "PD-Delete");
    //        if (sqlResult.QueryResultMessage != null) { return sqlResult; }
    //    }
    //    return sqlResult;
    //}

    //public async Task UpdateBugAsync(Bug bug)
    //    //{
    //    //    using (IDbConnection db = new SqlConnection(Configuration.GetConnectionString(BUGTRACKER_DATABASE)))
    //    //    {
    //    //        db.Open();
    //    //        await db.ExecuteAsync("update bugs set Summary=@Summary, BugPriority=@BugPriority, Assignee=@Assignee, BugStatus=@BugStatus where id=@Id", bug);
    //    //    }
    //    //}



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

    //public async Task<IEnumerable<Company>?> GetAll(int baseCompanyId)
    //{
    //    var p = new DynamicParameters();
    //    p.Add("@BaseCompanyId", baseCompanyId, DbType.Int32, ParameterDirection.Input);
    //    var sql = "";
    //    return await _sqlDataAccess.QueryAll<Company>(sql, p, "Company-GetAll");
    //}

}
