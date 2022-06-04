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
        var currentUser = Task.Run(GetCurrentUserAsync);
        return currentUser.Result;
    }

    public async Task<AppUser> GetCurrentUserAsync()
    {
        var authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var user = authenticationState.User;
        var appUser = await _userManager.GetUserAsync(user);
        if (appUser != null)
        {
            appUser.StatusName = GetStatusList().FirstOrDefault(e => e.StatusId == appUser.StatusId).StatusName;
            var p = new DynamicParameters();
            p.Add("@UserId", appUser.Id, DbType.Int32, ParameterDirection.Input);
            appUser.UserMenuList = await _sqlDataAccess.QueryAll<Menu>("dbo.SP_UserMenuAccess", p, "AppUser-GetCurrentUserMenu");
        }
        if (appUser != null && appUser.VendorId > 0)
        {
            var p = new DynamicParameters();
            p.Add("@VendorId", appUser.Id, DbType.Int32, ParameterDirection.Input);
            appUser.CompanyName = await _sqlDataAccess.QuerySingle<string>("SELECT v.VendorName CompanyName FROM Procurement.Vendors v INNER JOIN Config.AppUser u ON v.VendorId = u.VendorId WHERE u.Id = @VendorId", p, "AppUser-GetCurrentUserCompany", CommandType.Text);
        }
        return appUser;
    }

    public async Task<IEnumerable<AppUser>?> GetAllAsync()
    {
        var sql = $"SELECT u.*, (CASE WHEN u.VendorId > 0 THEN v.VendorName ELSE 'local' END) CompanyName FROM Config.AppUser u LEFT JOIN Procurement.Vendors v ON u.VendorId = v.VendorId";
        var userList = await _sqlDataAccess.QueryAll<AppUser>(sql, null, "AppUser-GetAll", CommandType.Text);
        List<AppUser> newList = new();
        if (userList == null)
        {
            return newList;
        }

        foreach (var item in userList)
        {
            item.StatusName = GetStatusList().FirstOrDefault(e => e.StatusId == item.StatusId).StatusName;
            newList.Add(item);
        }
        return newList;
    }

    public async Task<AppUser?> GetByIdAsync(int id)
    {
        var sql = $"SELECT * FROM Config.AppUser WHERE Id = {id}";
        return await _sqlDataAccess.QuerySingle<AppUser>(sql, null, "AppUser-GetById", CommandType.Text);
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
        return await _sqlDataAccess.ExecuteSql(sql, p, "AppUser-Update", CommandType.Text);
    }

    public async Task<SqlResult?> UserLoggedIn()
    {
        var currentUser = await GetCurrentUserAsync();
        var p = new DynamicParameters();
        p.Add("@CurrentUserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@LastActivity", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
        var sql = $"UPDATE Config.AppUser SET Sessions = Sessions + 1, LastActivity = @LastActivity WHERE Id = @CurrentUserId SELECT @@ROWCOUNT";
        return await _sqlDataAccess.QueryReturnInteger(sql, p, "AppUser-LoggedIn", CommandType.Text);
    }

    public async Task<SqlResult?> UserLoggedOut()
    {
        var currentUser = await GetCurrentUserAsync();
        var p = new DynamicParameters();
        p.Add("@CurrentUserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@LastActivity", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
        var sql = $"UPDATE Config.AppUser SET Sessions = Sessions - 1, LastActivity = @LastActivity WHERE Id = @CurrentUserId SELECT @@ROWCOUNT";
        return await _sqlDataAccess.QueryReturnInteger(sql, p, "AppUser-LoggedOut", CommandType.Text);
    }

    public IEnumerable<UserStatus> GetStatusList()
    {
        return new List<UserStatus>() {
            new UserStatus() { StatusId = 0, StatusName = "Open" },
            new UserStatus() { StatusId = 1, StatusName = "Draft" },
            new UserStatus() { StatusId = 2, StatusName = "Approval" },
            new UserStatus() { StatusId = 9, StatusName = "Closed" },
        };
    }
}
