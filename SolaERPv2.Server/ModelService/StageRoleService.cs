using SolaERPv2.Server.Models;

namespace SolaERPv2.Server.ModelService;

public class StageRoleService : BaseModelService<StageRole>
{

    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;

    public StageRoleService(SqlDataAccess sqlDataAccess, AppUserService appUserService)
    {
        _sqlDataAccess = sqlDataAccess;
        _appUserService = appUserService;

    }

    public async Task<IEnumerable<StageRole>?> GetAllByStageId(int? stageId)
    {
        string sql = "dbo.us_GetAllStageRoleByStageId";
        var p = new DynamicParameters();
        p.Add("@Id", stageId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<StageRole>(sql, p, "CS-GetAll");
        return result;

        
    }

    public async Task<IEnumerable<StageRole>?> GetAll()
    {
        var sql = "Select * from dbo.StageRoles";
        return await _sqlDataAccess.QueryAll<StageRole>(sql, null, "StageRole-GetAll", CommandType.Text);
    }


    //public async Task<IEnumerable<StageRole>?> GetAllAsync(int stageId)
    //{
    //    var sql = "dbo.sp_StageRole_Load";
    //    var p = new DynamicParameters();
    //    p.Add("@StageId", stageId, DbType.Int32, ParameterDirection.Input);
    //    return await _sqlDataAccess.QueryAll<StageRole>(sql, p, "GetAllStageRole");
    //}

}

