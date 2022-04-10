using Microsoft.AspNetCore.Components.Authorization;

namespace SolaERPv2.Server.ModelService;

public class AppUserService
{
    //IHttpContextAccessor _httpContextAccessor;
    AuthenticationStateProvider _authenticationStateProvider;
    UserManager<AppUser> _userManager;
    SqlDataAccess _sqlDataAccess;

    public AppUserService(AuthenticationStateProvider authenticationStateProvider, UserManager<AppUser> userManager, SqlDataAccess sqlDataAccess)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _userManager = userManager;
        _sqlDataAccess = sqlDataAccess;
    }

    //public int GetCurrentUserId()
    //{
    //    var result = 0;
    //    try
    //    {
    //        var httpUser = _httpContextAccessor.HttpContext.User;
    //        var userId = _userManager.GetUserId(httpUser);
    //        if (userId != null) { result = int.Parse(userId); }
    //    }
    //    catch (Exception e)
    //    {
    //        var message = e.Message;
    //    }
    //    return result;
    //}

    public AppUser GetCurrentUser()
    {
        var currentUser = Task.Run(() => GetCurrentUserAsync());
        return currentUser.Result;
    }

    public async Task<AppUser> GetCurrentUserAsync()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;
        return await _userManager.GetUserAsync(user);
    }

    public async Task<IEnumerable<AppUser>?> GetAllAsync()
    {
        var sql = $"SELECT * FROM Config.AppUser";
        return await _sqlDataAccess.QueryAll<AppUser>(sql, null, CommandType.Text);
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        var sql = $"SELECT * FROM Config.AppUser WHERE Id = {id}";
        return await _sqlDataAccess.QuerySingle<AppUser>(sql, null, CommandType.Text);
    }
}
