namespace SolaERPv2.Server.ModelService;

public class ApproveStageService
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
        return await _sqlDataAccess.QueryAll<ApproveStage>(sql, p);
    }
}
