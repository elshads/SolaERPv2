using SolaERPv2.Server.Models;
using SolaERPv2.Server.Pages;
using System;

namespace SolaERPv2.Server.ModelService;

public class StageService : BaseModelService<Stage>
{
    SqlDataAccess _sqlDataAccess;

    public StageService(SqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Stage>?> GetAllByCustomer(int customerId)
    {

        string sql = "dbo.us_GetAllStageByCustomerId";
        var p = new DynamicParameters();
        p.Add("@Id", customerId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<Stage>(sql, p, "GetAllByCustomer-GetAll");
        return result;

    }

    #region StageGetAll
        //public async Task<IEnumerable<Stage>?> GetAll()
    //{
    //    Dictionary<int, Stage> result = new();

    //    var sql = @"select st.[Id] as 'StageId'
    //                      ,st.[Name]
    //                      ,st.[Sequence]
    //                      ,st.[CompanyId]
    //                   ,s.[Id] 
    //                      ,s.[Name]
    //                      ,s.[Amount]
    //                      ,s.[AmountTo]
    //                      ,s.[StageId]
    //                from StageRoles s
    //                right join Stages st
    //                on st.Id=s.StageId";

    //    using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
    //    await cn.QueryAsync<Stage, StageRole, Stage>(sql, 
    //    (stage, stagerole)=>
    //    {
    //        if (!result.ContainsKey(stage.StageId))
    //        {
    //            stage.StageRoles = new();
    //            result.Add(stage.StageId, stage);
    //        }
    //        var currentStage = result[stage.StageId];
    //        if (stagerole != null) { currentStage.StageRoles.Add(stagerole); }
    //        return stage;
    //    },
    //    splitOn: "Id",
    //    commandType: CommandType.Text);


    //    return result.Values;
    //}
    #endregion
}

