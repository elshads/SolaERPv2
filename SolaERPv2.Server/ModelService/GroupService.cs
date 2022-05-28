namespace SolaERPv2.Server.ModelService;

public class GroupService : BaseModelService<Group>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public GroupService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Group>?> GetAllAsync()
    {
        return await _sqlDataAccess.QueryAll<Group>("dbo.SP_GroupMain_Load", null, "Group-GetAll");
    }

    public async Task<Group?> GetByIdAsync(int id)
    {
        Group group = new();
        var p = new DynamicParameters();
        p.Add("@GroupId", id, DbType.Int32, ParameterDirection.Input);
        var _group = await _sqlDataAccess.QuerySingle<Group>("dbo.SP_GroupHeader_Load", p, "Group-GetById");
        if (_group != null) 
        { 
            group = _group;
            var groupUsers = await _sqlDataAccess.QueryAll<AppUser>("dbo.SP_GroupUsers_Load", p, "Group-UsersLoad");
            group.Users = groupUsers.ToList();
            var groupBusinessUnits = await _sqlDataAccess.QueryAll<BusinessUnit>("dbo.SP_GroupBusinessUnit_Load", p, "Group-BULoad");
            group.BusinessUnits = groupBusinessUnits.ToList();
            var groupApproveRoles = await _sqlDataAccess.QueryAll<ApproveRole>("dbo.SP_GroupApproveRoles_Load", p, "Group-RolesLoad");
            group.ApproveRoles = groupApproveRoles.ToList();
            var groupMenus = await _sqlDataAccess.QueryAll<Menu>("dbo.SP_GroupMenus_Load", p, "Group-MenusLoad");
            group.Menus = groupMenus.ToList();
        }
        
        return group;
    }

    public async Task<SqlResult?> Save(Group group) 
    {
        SqlResult? sqlResult = new ();
        var groupId = group.GroupId;
        var appUser = await _appUserService.GetCurrentUserAsync();

        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            var p = new DynamicParameters();
            p.Add("@GroupId", group.GroupId, DbType.Int32, ParameterDirection.Input);
            p.Add("@GroupName", group.GroupName, DbType.String, ParameterDirection.Input);
            p.Add("@Description", group.Description, DbType.String, ParameterDirection.Input);
            p.Add("@UserId", appUser.Id, DbType.Int32, ParameterDirection.Input);
            var result = await cn.QueryAsync<int>("dbo.SP_Groups_IUD", p, commandType: CommandType.StoredProcedure);
            if (result.Any()) { groupId = result.FirstOrDefault(); }
        }
        catch (Exception e)
        {
            sqlResult.QueryResultMessage = e.Message;
        }

        if (sqlResult?.QueryResultMessage != null) { return sqlResult; } // return if failed

        var up = new DynamicParameters();
        up.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteSql("dbo.SP_GroupUsers_ID", up, "Group-SaveUsers1");
        if (group.Users.Any())
        {
            foreach (var item in group.Users)
            {
                var dp = new DynamicParameters();
                dp.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
                dp.Add("@UserId", item.Id, DbType.Int32, ParameterDirection.Input);
                sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_GroupUsers_ID", dp, "Group-SaveUsers2");
                if (sqlResult?.QueryResultMessage != null) { return sqlResult; } // return if failed
            }
        }

        var bp = new DynamicParameters();
        bp.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteSql("dbo.SP_GroupBusinessUnits_ID", bp, "Group-SaveBU1");
        if (group.BusinessUnits.Any())
        {
            foreach (var item in group.BusinessUnits)
            {
                var dp = new DynamicParameters();
                dp.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
                dp.Add("@BusinessUnitId", item.BusinessUnitId, DbType.Int32, ParameterDirection.Input);
                sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_GroupBusinessUnits_ID", dp, "Group-SaveBU2");
                if (sqlResult?.QueryResultMessage != null) { return sqlResult; } // return if failed
            }
        }

        var rp = new DynamicParameters();
        rp.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
        var xxx = await _sqlDataAccess.ExecuteSql("dbo.SP_GroupApproveRoles_ID", rp, "Group-SaveRoles1");
        if (group.ApproveRoles.Any())
        {
            foreach (var item in group.ApproveRoles)
            {
                var dp = new DynamicParameters();
                dp.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
                dp.Add("@ApproveRoleId", item.ApproveRoleId, DbType.Int32, ParameterDirection.Input);
                sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_GroupApproveRoles_ID", dp, "Group-SaveRoles2");
                if (sqlResult?.QueryResultMessage != null) { return sqlResult; } // return if failed
            }
        }

        var mp = new DynamicParameters();
        mp.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
        await _sqlDataAccess.ExecuteSql("dbo.SP_GroupMenus_ID", mp, "Group-SaveMenus1");
        if (group.Menus.Any())
        {
            foreach (var item in group.Menus)
            {
                var dp = new DynamicParameters();
                dp.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
                dp.Add("@MenuId", item.MenuId, DbType.Int32, ParameterDirection.Input);
                dp.Add("@Create", item.CreateAccess, DbType.Boolean, ParameterDirection.Input);
                dp.Add("@Edit", item.EditAccess, DbType.Boolean, ParameterDirection.Input);
                dp.Add("@Delete", item.DeleteAccess, DbType.Boolean, ParameterDirection.Input);
                dp.Add("@Export", item.ExportAccess, DbType.Boolean, ParameterDirection.Input);
                sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_GroupMenus_ID", dp, "Group-SaveMenus2");
                if (sqlResult?.QueryResultMessage != null) { return sqlResult; } // return if failed
            }
        }

        sqlResult.ReturnId = groupId;
        return sqlResult;
    }
}