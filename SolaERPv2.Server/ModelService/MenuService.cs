﻿namespace SolaERPv2.Server.ModelService;

public class MenuService : BaseModelService<Menu>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public MenuService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Menu>?> GetAllAsync(bool hierarchyData)
    {
        var sql = "SELECT * FROM dbo.VW_Menus_List";
        var result = await _sqlDataAccess.QueryAll<Menu>(sql, null, "Menu-GetAll", CommandType.Text);
        if (hierarchyData) { return GetHierarchy(result); }
        return result;
    }

    public async Task<IEnumerable<Menu>?> GetUserItemsAsync()
    {
        var user = await _appUserService.GetCurrentUserAsync();
        var p = new DynamicParameters();
        p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
        var result = await _sqlDataAccess.QueryAll<Menu>("dbo.SP_UserMenu_Load", p, "Menu-GetUserItems");
        return GetHierarchy(result);
    }

    public async Task<IEnumerable<Menu>?> GetGroupItemsAsync(int groupId)
    {
        var p = new DynamicParameters();
        p.Add("@GroupId", groupId, DbType.Int32, ParameterDirection.Input);
        var result = await _sqlDataAccess.QueryAll<Menu>("dbo.SP_GroupMenus_Load", p, "Menu-GetGroupItems");
        return GetHierarchy(result);
    }

    public IEnumerable<Menu>? GetHierarchy(IEnumerable<Menu>? flatList)
    {
        var tempList = flatList?.Where(e => e.ParentId == null || e.ParentId == 0).Select(e => new Menu {
            MenuId = e.MenuId,
            ParentId = e.ParentId,
            MenuCode = e.MenuCode,
            MenuName = e.MenuName,
            URL = e.URL,
            CreateAccess = e.CreateAccess,
            EditAccess = e.EditAccess,
            DeleteAccess = e.DeleteAccess,
            ExportAccess = e.ExportAccess,
            UserId = e.UserId,
            HasChildren = e.HasChildren,
            Icon = e.Icon != null ? typeof(Icons.Filled).GetField(e.Icon)?.GetValue(new Icons.Filled())?.ToString() : "",
            Children = GetChildren(flatList, e.MenuId)
        });
        return tempList;
    }

    IEnumerable<Menu> GetChildren(IEnumerable<Menu> flatList, int parentId)
    {
        var tempList = flatList.Where(e => e.ParentId == parentId).Select(e => new Menu {
            MenuId = e.MenuId,
            ParentId = e.ParentId,
            MenuCode = e.MenuCode,
            MenuName = e.MenuName,
            URL = e.URL,
            CreateAccess = e.CreateAccess,
            EditAccess = e.EditAccess,
            DeleteAccess = e.DeleteAccess,
            ExportAccess = e.ExportAccess,
            UserId = e.UserId,
            HasChildren = e.HasChildren,
            Icon = e.Icon != null ? typeof(Icons.Filled).GetField(e.Icon)?.GetValue(new Icons.Filled())?.ToString() : "",
            Children = GetChildren(flatList, e.MenuId)
        });
        return tempList;
    }
}