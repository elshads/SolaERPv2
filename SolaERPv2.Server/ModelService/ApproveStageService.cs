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

        string sql = "dbo.SP_ApproveStageMain_Load";
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

    public async Task<Procedure?> GetByIdAsync(int procedurId)
    {
        Dictionary<int, Procedure> result = new();

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        await cn.QueryAsync<Procedure, ApproveStageMain, Procedure>("dbo.sp_Company_Load",
            (procedure, stage) =>
            {
                if (!result.ContainsKey(procedure.ProcedureId))
                {
                    procedure.ApproveStageMains = new();
                    result.Add(procedure.ProcedureId, procedure);
                }
                var currentCompany = result[procedure.ProcedureId];
                return procedure;
            },
            param: new { ProcedureId = procedurId },
            splitOn: "ApproveStageMainId",
            commandType: CommandType.StoredProcedure);

        return result.Values.FirstOrDefault();
    }

}
