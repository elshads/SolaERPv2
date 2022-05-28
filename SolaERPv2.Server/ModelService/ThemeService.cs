namespace SolaERPv2.Server.ModelService;

public class ThemeService
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public ThemeService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<SqlResult?> UpdateUserTheme(string theme)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        var sql = $"UPDATE Config.AppUser SET Theme = '{theme}' WHERE Id = {user.Id}";
        return await _sqlDataAccess.ExecuteSql(sql, null, "Theme-Update", CommandType.Text);
    }
}
