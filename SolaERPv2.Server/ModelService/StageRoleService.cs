using SolaERPv2.Server.Models;

namespace SolaERPv2.Server.ModelService;

public class StageRoleService : BaseModelService<StageRole>
{
    SqlDataAccess _sqlDataAccess;

    public StageRoleService(SqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<StageRole>?> GetAllByStageId(int? stageId)
    {
        string sql = "dbo.us_GetAllStageRoleByStageId";
        var p = new DynamicParameters();
        p.Add("@Id", stageId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<StageRole>(sql, p, "GetAllByStageId-GetAll");
        return result;
    }

}

