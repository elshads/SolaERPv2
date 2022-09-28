using SolaERPv2.Server.Models;
using SolaERPv2.Server.Pages;
using System;

namespace SolaERPv2.Server.ModelService;

public class StageService : BaseModelService<Stage>
{

    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;

    public StageService(SqlDataAccess sqlDataAccess, AppUserService appUserService)
    {
        _sqlDataAccess = sqlDataAccess;
        _appUserService = appUserService;

    }

    public async Task<IEnumerable<Stage>?> GetAllByCustomer(int customerId)
    {

        string sql = "dbo.us_GetAllStageByCustomerId";
        var p = new DynamicParameters();
        p.Add("@Id", customerId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<Stage>(sql, p, "CS-GetAll");
        return result;

    }


    public async Task<IEnumerable<Stage>?> GetAll()
    {
        Dictionary<int, Stage> result = new();

        var sql = @"select st.[Id] as 'StageId'
                          ,st.[Name]
                          ,st.[Sequence]
                          ,st.[CompanyId]
	                      ,s.[Id] 
                          ,s.[Name]
                          ,s.[Amount]
                          ,s.[AmountTo]
                          ,s.[StageId]
                    from StageRoles s
                    right join Stages st
                    on st.Id=s.StageId";

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        await cn.QueryAsync<Stage, StageRole, Stage>(sql, 
        (stage, stagerole)=>
        {
            if (!result.ContainsKey(stage.StageId))
            {
                stage.StageRoles = new();
                result.Add(stage.StageId, stage);
            }
            var currentStage = result[stage.StageId];
            if (stagerole != null) { currentStage.StageRoles.Add(stagerole); }
            return stage;
        },
        splitOn: "Id",
        commandType: CommandType.Text);


        return result.Values;
    }

    //public async Task<IEnumerable<Stage>?> GetAllExtendedAsync()
    //{
    


    //    var sql = @"select * from StageRoles
    //                inner join Stages
    //                on Stages.Id=StageRoles.StageId"
    //    ;


    //    var remainingHorsemen = new Dictionary<int, Stage>();
    //    using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);

    //    _ = await cn.QueryAsync<Stage, StageRole, Stage>(sql, (person, stageRole) => {
    //        //person
    //        Stage personEntity;
           
    //        //trip
    //        if (!remainingHorsemen.TryGetValue(person.Id, out personEntity))
    //        {
    //            remainingHorsemen.Add(person.Id, personEntity = person);
    //        }


    //        //books
    //        if (personEntity.StageRoles == null)
    //        {
    //            personEntity.StageRoles = new List<StageRole>();
    //        }

    //        if (stageRole != null)
    //        {
    //            if (!personEntity.StageRoles.Any(x => x.StageId == stageRole.StageId))
    //            {
    //                personEntity.StageRoles.Add(stageRole);
    //            }
    //        }

    //        return personEntity;
    //    },
    //    splitOn: "Id");
    //}


}

