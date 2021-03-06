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
}
