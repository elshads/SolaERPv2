﻿namespace SolaERPv2.Server.ModelService;

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

    public async Task<SqlResult?> Save(ApproveStageMain approveStageMain)
    {
        //var user = await _appUserService.GetCurrentUserAsync();
        SqlResult? result = new();
        using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
        {
            var p = new DynamicParameters();
            p.Add("@ApproveStageMainId", approveStageMain.ApproveStageMainId, DbType.Int32, ParameterDirection.Input);
            p.Add("@ApproveStageName", approveStageMain.ApproveStageName, DbType.String, ParameterDirection.Input);
            p.Add("@ProcedureId", approveStageMain.ProcedureId, DbType.Int32, ParameterDirection.Input);
            p.Add("@BusinessUnitId", approveStageMain.BusinessUnitId, DbType.Int32, ParameterDirection.Input);
       

            //p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
            p.Add("@NewApproveStageMainId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            result.QueryResult = await cn.ExecuteAsync("dbo.SP_ApproveStagesMain_IUD", p, commandType: CommandType.StoredProcedure);
            result.QueryResult = p.Get<int>("@NewApproveStageMainId");
        }

        return result;
    }

    #region Useless

    //public async Task<Procedure?> GetByIdAsync(int procedurId)
    //{
    //    Dictionary<int, Procedure> result = new();

    //    using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
    //    await cn.QueryAsync<Procedure, ApproveStageMain, Procedure>("dbo.sp_Company_Load",
    //        (procedure, stage) =>
    //        {
    //            if (!result.ContainsKey(procedure.ProcedureId))
    //            {
    //                procedure.ApproveStageMains = new();
    //                result.Add(procedure.ProcedureId, procedure);
    //            }
    //            var currentCompany = result[procedure.ProcedureId];
    //            return procedure;
    //        },
    //        param: new { ProcedureId = procedurId },
    //        splitOn: "ApproveStageMainId",
    //        commandType: CommandType.StoredProcedure);

    //    return result.Values.FirstOrDefault();
    //}


    //public async Task<SqlResult?> Save(List<ApproveStageMain>  approveStageMainList)
    //{
    //    var user = await _appUserService.GetCurrentUserAsync();
    //    SqlResult? result = new();
    //    using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
    //    {
    //        foreach (var approveStageMain in approveStageMainList)
    //        {
    //            var p = new DynamicParameters();


    //            p.Add("@ProcedureId", approveStageMain.ProcedureId, DbType.Int32, ParameterDirection.Input);
    //            p.Add("@BusinessUnitId", approveStageMain.BusinessUnitId, DbType.Int32, ParameterDirection.Input);
    //            p.Add("@ApproveStageName", approveStageMain.ApproveStageName, DbType.String, ParameterDirection.Input);
    //            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);

    //            result.QueryResult = await cn.ExecuteAsync("dbo.SP_ApproveStagesMain_IUD", p, commandType: CommandType.StoredProcedure);
    //        }
    //    }


    //    return result;
    //}


    //public async Task<ApproveStageRole> GetRoleByStageRoleId(int? stageRoleId)
    //{
    //    if (stageRoleId == null) return new ApproveStageRole();

    //    string sql = "dbo.usp_ApproveStageRole_Load";
    //    var p = new DynamicParameters();
    //    p.Add("@ApproveStageMainId", stageRoleId, DbType.Int32, ParameterDirection.Input);

    //    var result = await _sqlDataAccess.QuerySingle<ApproveStageRole>(sql, p, "GetRoleByStageRoleId-Single");

    //    if (result == null) return new ApproveStageRole();

    //    return result;
    //}

    //public async Task<IEnumerable<ApproveStageMain>?> GetAllProcdureWithMain(int? businessUnitId)
    //{

    //    string sql = "dbo.usp_GetAllProcduresWithMain ";
    //    var p = new DynamicParameters();
    //    p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);
    //    var sqlResult = await _sqlDataAccess.QueryAll<FullApproveStageMain>(sql, p, "ApproveStageDetails_Load-GetAll");
    //    var result = sqlResult.Select(a => new ApproveStageMain {
    //        BusinessUnitId = a.BusinessUnitId,
    //        ApproveStageMainId = a.ApproveStageMainId,
    //        ApproveStageName = a.ApproveStageName,
    //        ProcedureId = a.ProcedureId,
    //        Procedure = new Procedure {
    //            ProcedureId = a.ProcedureId,
    //            ProcedureName = a.ProcedureName,
    //            ProcedureKey = a.ProcedureKey
    //        }
    //    }).ToList();
    //    return result;
    //}

    #endregion
}
