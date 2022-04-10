namespace SolaERPv2.Server.ModelService;

public class AttachmentService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public AttachmentService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<Attachment?> GetByIdAsync(int id)
    {
        var sql = "dbo.SP_Attachment_Load";
        var p = new DynamicParameters();
        p.Add("@AttachmentId", id, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QuerySingle<Attachment>(sql, p);
    }
}
