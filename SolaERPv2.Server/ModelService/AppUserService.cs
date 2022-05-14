using Microsoft.AspNetCore.Components.Authorization;

namespace SolaERPv2.Server.ModelService;

public class AppUserService : BaseModelService<AppUser>
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
        var appUser = await _userManager.GetUserAsync(user);
        if (appUser != null)
        {
            var p = new DynamicParameters();
            p.Add("@UserId", appUser.Id, DbType.Int32, ParameterDirection.Input);
            appUser.UserMenuList = await _sqlDataAccess.QueryAll<Menu>("dbo.SP_UserMenuAccess", p);
        }
        return appUser;
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

    public async Task<IEnumerable<string>?> GetAllEmailsAsync()
    {
        IEnumerable<string>? result = new List<string>();
        var sql = $"SELECT LOWER(Email) FROM Config.AppUser";
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            result = await cn.QueryAsync<string>(sql);
        }
        catch (Exception e)
        {
            var message = e.Message;
        }
        return result;
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var sql = $"SELECT LOWER(Email) FROM Config.AppUser WHERE Email = '{email}'";
        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            var result = await cn.QueryAsync<string>(sql);
            if (result != null) { return false; }
        }
        catch (Exception e)
        {
            var message = e.Message;
            return false;
        }
        return true;
    }

    public async Task<SqlResult?> UpdateAsync(AppUser appUser)
    {
        var p = new DynamicParameters();
        p.Add("@Id", appUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@FullName", appUser.FullName, DbType.String, ParameterDirection.Input);
        p.Add("@Position", appUser.Position, DbType.String, ParameterDirection.Input);
        p.Add("@PhoneNumber", appUser.PhoneNumber, DbType.String, ParameterDirection.Input);
        p.Add("@Photo", appUser.Photo, DbType.Binary, ParameterDirection.Input);
        var sql = $"UPDATE Config.AppUser SET FullName = @FullName, Position = @Position, PhoneNumber = @PhoneNumber, Photo = @Photo WHERE Id = @Id";
        return await _sqlDataAccess.ExecuteSql(sql, p, CommandType.Text);
    }
}
