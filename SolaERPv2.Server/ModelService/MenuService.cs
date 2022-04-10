﻿namespace SolaERPv2.Server.ModelService;

public class MenuService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public MenuService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Menu>?> GetAllAsync()
    {
        var sql = "SELECT * FROM dbo.VW_Menus_List";
        return await _sqlDataAccess.QueryAll<Menu>(sql, null, CommandType.Text);
    }

    public async Task<Menu?> GetByIdAsync(int id)
    {
        var sql = $"SELECT * FROM dbo.VW_Menus_List WHERE MenuId = {id}";
        return await _sqlDataAccess.QuerySingle<Menu>(sql, null, CommandType.Text);
    }

    public async Task<IEnumerable<Menu>?> GetUserItemsAsync()
    {
        var user = await _appUserService.GetCurrentUserAsync();
        // select according userId - not implemented yet
        var sql = "SELECT * FROM dbo.VW_Menus_List";
        var result = await _sqlDataAccess.QueryAll<Menu>(sql, null, CommandType.Text);
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
            Icon = (e.Icon != null ? Icons.Filled.GetType().GetProperty(e.Icon)?.GetValue(Icons.Filled, null)?.ToString() : ""),
            Children = GetCildren(flatList, e.MenuId)
        });
        return tempList;
    }

    IEnumerable<Menu> GetCildren(IEnumerable<Menu> flatList, int parentId)
    {
        var tempList = flatList.Where(e => e.ParentId == parentId).Select(e => new Menu {
            MenuId = e.MenuId,
            ParentId = e.ParentId,
            MenuCode = e.MenuCode,
            MenuName = e.MenuName,
            URL = e.URL,
            Icon = (e.Icon != null ? Icons.Filled.GetType().GetProperty(e.Icon)?.GetValue(Icons.Filled, null)?.ToString() : ""),
            Children = GetCildren(flatList, e.MenuId)
        });
        return tempList;
    }
}