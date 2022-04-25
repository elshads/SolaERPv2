namespace SolaERPv2.Server.ModelService;

public class GroupService
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
        return await _sqlDataAccess.QueryAll<Group>("dbo.SP_GroupMain_Load", null);
    }

    public async Task<Group?> GetByIdAsync(int id)
    {
        Group group = new();
        var p = new DynamicParameters();
        p.Add("@GroupId", id, DbType.Int32, ParameterDirection.Input);
        var _group = await _sqlDataAccess.QuerySingle<Group>("dbo.SP_GroupHeader_Load", p);
        if (_group != null) 
        { 
            group = _group;
            var groupUsers = await _sqlDataAccess.QueryAll<AppUser>("dbo.SP_GroupUsers_Load", p);
            group.Users = groupUsers.ToList();
            var groupBusinessUnits = await _sqlDataAccess.QueryAll<BusinessUnit>("dbo.SP_GroupBusinessUnit_Load", p);
            group.BusinessUnits = groupBusinessUnits.ToList();
            var groupMenus = await _sqlDataAccess.QueryAll<Menu>("dbo.SP_GroupMenus_Load", p);
            group.Menus = groupMenus.ToList();
        }
        
        return group;
    }
}