using Dapper;
using SolaERPv2.Server.Models;

namespace SolaERPv2.Server.ModelService;

public class ApproveStageService : BaseModelService<ApproveStage>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public ApproveStageService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<ApproveStage>?> GetAllAsync(int paymentDocumentMainId)
    {
        var sql = "dbo.SP_PaymentDocumentApprovals";
        var p = new DynamicParameters();
        p.Add("@PaymentDocumentMainId", paymentDocumentMainId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<ApproveStage>(sql, p, "GetAllApproveStages");
    }

    public async Task<IEnumerable<ApproveStage>?> GetApproveStagesAsync(string procedureKey)
    {
        var p = new DynamicParameters();
        p.Add("@ProcedureKey", procedureKey, DbType.String, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<ApproveStage>("dbo.SP_ApproveStages_List", p, "GetApproveStageList");
    }

    public async Task<IEnumerable<ApproveRole>?> GetAllRolesAsync()
    {
        return await _sqlDataAccess.QueryAll<ApproveRole>("SELECT * FROM dbo.VW_ApproveRoles_List", null, "ApproveStage-GetAllRoles", CommandType.Text);
    }




    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public async Task<IEnumerable<ApproveStageMain>?> ApproveStageMainLoad(int? businessUnit)
    {
        if (businessUnit == null) return new List<ApproveStageMain>();

        string sql = "dbo.usp_ApproveStageMain_Load";
        var p = new DynamicParameters();
        p.Add("@BusinessUnitId", businessUnit, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<ApproveStageMain>(sql, p, "ApproveStageMain_Load-GetAll");

        if (result == null) return new List<ApproveStageMain>();

        return result;
    }

    public async Task<IEnumerable<ApproveStageDetail>?> ApproveStageDetailLoad(int approveMainId)
    {
        string sql = "dbo.SP_ApproveStageDetails_Load ";
        var p = new DynamicParameters();
        p.Add("@ApproveStageMainId", approveMainId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<ApproveStageDetail>(sql, p, "ApproveStageDetails_Load-GetAll");

        foreach (var item in result)
        {
            var roleList = await ApproveStageRoleLoad(item.ApproveStageDetailsId);
            item.ApproveStageRoles = roleList.ToList();
        }

        return result; 
    }

    public async Task<IEnumerable<ApproveStageRole>?> ApproveStageRoleLoad(int? approveDetailId)
    {
        string sql = "dbo.SP_ApproveStageRoles_Load";
        var p = new DynamicParameters();
        p.Add("@ApproveStageDetailId", approveDetailId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<ApproveStageRole>(sql, p, "ApproveStageRoles_Load-GetAll");
        return result;
    }

    public async Task<IEnumerable<Procedure>> GetProcedures()
    {
        IEnumerable<Procedure> result = new List<Procedure>();
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess?.ConnectionString);
            result = await cn.QueryAsync<Procedure>("select * from dbo.VW_Procedures_List");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return result;
    }

    public async Task<IEnumerable<ApproveRole>> GetRoles()
    {
        IEnumerable<ApproveRole> result = new List<ApproveRole>();
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess?.ConnectionString);
            result = await cn.QueryAsync<ApproveRole>("select * from dbo.VW_ApproveRoles_List");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return result;
    }

    public async Task<Procedure> GetProcuderByMainId(int? approveStageMainId)
    {
        if (approveStageMainId == null) return new Procedure();

        string sql = "dbo.usp_ApproveStageHeaderProcedure_Load";
        var p = new DynamicParameters();
        p.Add("@ApproveStageMainId", approveStageMainId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QuerySingle<Procedure>(sql, p, "GetProcuderByMainId-Single");

        if (result == null) return new Procedure();

        return result;
    }

    public async Task<BusinessUnit> GetBusinessUnitByMainId(int? approveStageMainId)
    {
        if (approveStageMainId == null) return new BusinessUnit();

        string sql = "dbo.usp_ApproveStageHeaderBusinessUnit_Load";
        var p = new DynamicParameters();
        p.Add("@ApproveStageMainId", approveStageMainId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QuerySingle<BusinessUnit>(sql, p, "GetBusinessUnitByMainId-Single");

        if (result == null) return new BusinessUnit();

        return result;
    }

    public async Task<ApproveStageMain> GetMainByMainId(int? approveStageMainId)
    {
        if (approveStageMainId == null) return new ApproveStageMain();

        string sql = "dbo.usp_ApproveStageHeaderMain_Load";
        var p = new DynamicParameters();
        p.Add("@ApproveStageMainId", approveStageMainId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QuerySingle<ApproveStageMain>(sql, p, "GetMainByMainId-Single");

        if (result == null) return new ApproveStageMain();

        return result;
    }

    public async Task<ApproveStageDetail> GetDetailsByMainId(int? approveStageMainId)
    {
        if (approveStageMainId == null) return new ApproveStageDetail();

        string sql = "dbo.usp_ApproveStageHeaderDetails_Load";
        var p = new DynamicParameters();
        p.Add("@ApproveStageDetailsId", approveStageMainId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QuerySingle<ApproveStageDetail>(sql, p, "GetDetailsByMainId-Single");

        if (result == null) return new ApproveStageDetail();

        return result;
    }

    public async Task<SqlResult?> SaveMain(ApproveStageMain approveStageMain)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        SqlResult? result = new();
        using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
        {
            var p = new DynamicParameters();
            p.Add("@ApproveStageMainId", approveStageMain.ApproveStageMainId, DbType.Int32, ParameterDirection.Input);
            p.Add("@ApproveStageName", approveStageMain.ApproveStageName, DbType.String, ParameterDirection.Input);
            p.Add("@ProcedureId", approveStageMain.ProcedureId, DbType.Int32, ParameterDirection.Input);
            p.Add("@BusinessUnitId", approveStageMain.BusinessUnitId, DbType.Int32, ParameterDirection.Input);
            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);

            result.QueryResult = await cn.ExecuteAsync("dbo.SP_ApproveStagesMain_IUD", p, commandType: CommandType.StoredProcedure);
        }

        return result;
    }


    public async Task<SqlResult?> SaveDetails(List<ApproveStageDetail> approveStageDetailList)
    {
        SqlResult? result = new();
        foreach (var item in approveStageDetailList)
        {
            using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
            {
                var p = new DynamicParameters();
                p.Add("@ApproveStageDetailsId", item.ApproveStageDetailsId, DbType.Int32, ParameterDirection.Input);
                p.Add("@ApproveStageMainId", item.ApproveStageMainId, DbType.Int32, ParameterDirection.Input);
                p.Add("@@ApproveStageDetailsName", item.ApproveStageDetailsName, DbType.String, ParameterDirection.Input);
                p.Add("@Sequence", item.Sequence, DbType.Int32, ParameterDirection.Input);

                result.QueryResult = await cn.QueryFirstOrDefaultAsync<int>("dbo.SP_ApproveStagesDetails_IUD", p, commandType: CommandType.StoredProcedure);

                Console.WriteLine("New Id/////////////////////:" + result.QueryResultMessage);
            }
            //using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
            //{
            //    var p = new DynamicParameters();
            //    p.Add("@ApproveStageDetailId", result.QueryResult, DbType.Int32, ParameterDirection.Input);
            //    result.QueryResult = await cn.ExecuteAsync("dbo.SP_ApproveStageRoles_IUD", p, commandType: CommandType.StoredProcedure);
            //}

            if (item.ApproveStageRoles != null && item.ApproveStageRoles.Any())
            {
                foreach (var roleItem in item.ApproveStageRoles)
                {
                    using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
                    {
                        var p = new DynamicParameters();
                        p.Add("@ApproveStageRoleId", roleItem.ApproveStageRoleId, DbType.Int32, ParameterDirection.Input);
                        p.Add("@ApproveStageDetailId", result.QueryResult, DbType.Int32, ParameterDirection.Input);
                        p.Add("@ApproveRoleId", roleItem.ApproveRoleId, DbType.Int32, ParameterDirection.Input);
                        p.Add("@AmountFrom", roleItem.AmountFrom, DbType.Decimal, ParameterDirection.Input);
                        p.Add("@AmountTo", roleItem.AmountFrom, DbType.Decimal, ParameterDirection.Input);

                        result.QueryResult = await cn.ExecuteAsync("dbo.SP_ApproveStageRoles_IUD", p, commandType: CommandType.StoredProcedure);
                    }
                }
            }
        }

        return result;
    }

    public async Task<int> GetTestIdAsync(ApproveStageDetail approveStageDetail)
    {
        var result = 0;
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess?.ConnectionString);
            var p = new
            {
                ApproveStageDetailsId = approveStageDetail.ApproveStageDetailsId,
                ApproveStageMainId = approveStageDetail.ApproveStageMainId,
                ApproveStageDetailsName = approveStageDetail.ApproveStageDetailsName,
                Sequence = approveStageDetail.Sequence
            };
            result = await cn.QueryFirstOrDefaultAsync<int>("dbo.SP_ApproveStagesDetails_IUD", p, commandType: CommandType.StoredProcedure);
            Console.WriteLine("New Id: " + result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return result;
    }


    public async Task<SqlResult?> Delete(IEnumerable<int> approveStageMain)
    {
        SqlResult? sqlResult = new();
        foreach (var item in approveStageMain)
        {
            var p = new DynamicParameters();
            p.Add("@ApproveStageMainId", item, DbType.Int32, ParameterDirection.Input);
            sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_ApproveStagesMain_IUD", p, "AP-Delete");
            if (sqlResult.QueryResultMessage != null) { return sqlResult; }
        }
        return sqlResult;
    }

    #region Useless

    //public async Task<SqlResult?> SaveDetails(ApproveStageDetail approveStageDetail)
    //{
    //    SqlResult? result = new();
    //    using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
    //    {
    //        var p = new DynamicParameters();
    //        p.Add("@ApproveStageDetailsId", approveStageDetail.ApproveStageDetailsId, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@ApproveStageMainId", approveStageDetail.ApproveStageMainId, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@@ApproveStageDetailsName", approveStageDetail.ApproveStageDetailsName, DbType.String, ParameterDirection.Input);
    //        p.Add("@Sequence", approveStageDetail.Sequence, DbType.Int32, ParameterDirection.Input);

    //        result.QueryResult = await cn.ExecuteAsync("dbo.SP_ApproveStagesDetails_IUD", p, commandType: CommandType.StoredProcedure);
    //    }

    //    return result;
    //}

    //public async Task<SqlResult?> SaveRoles(ApproveStageRole approveStageRole)
    //{
    //    SqlResult? result = new();
    //    using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
    //    {
    //        var p = new DynamicParameters();
    //        p.Add("@ApproveStageRoleId", approveStageRole.ApproveStageRoleId, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@ApproveStageDetailId", approveStageRole.ApproveStageDetailId, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@ApproveRoleId", approveStageRole.ApproveRoleId, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@AmountFrom", approveStageRole.AmountFrom, DbType.Decimal, ParameterDirection.Input);
    //        p.Add("@AmountTo", approveStageRole.AmountFrom, DbType.Decimal, ParameterDirection.Input);

    //        result.QueryResult = await cn.ExecuteAsync("dbo.SP_ApproveStageRoles_IUD", p, commandType: CommandType.StoredProcedure);
    //    }

    //    return result;
    //}


    #endregion
}
