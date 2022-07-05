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
        var sql = "SELECT u.*, (CASE WHEN u.StatusId = 0 THEN 'Draft' WHEN u.StatusId = 1 THEN 'Open' WHEN u.StatusId = 2 THEN 'Closed' ELSE 'none' END) StatusName, (CASE WHEN u.VendorId > 0 THEN v.VendorName ELSE 'local' END) CompanyName FROM Config.AppUser u LEFT JOIN Procurement.Vendors v ON u.VendorId = v.VendorId";
        return await _sqlDataAccess.QueryAll<AppUser>(sql, null, "AppUser-GetAll", CommandType.Text);
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
        if (currentUser == null || currentUser.Id == 0) { return new SqlResult() {QueryResultMessage = "currentUser is null"}; }

        var p = new DynamicParameters();
        p.Add("@CurrentUserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@LastActivity", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
        var sql = $"UPDATE Config.AppUser SET Sessions = Sessions + 1, LastActivity = @LastActivity WHERE Id = @CurrentUserId SELECT @@ROWCOUNT";
        return await _sqlDataAccess.QueryReturnInteger(sql, p, "AppUser-LoggedIn", CommandType.Text);
    }

    public async Task<SqlResult?> UserLoggedOut()
    {
        var currentUser = await GetCurrentUserAsync();
        if (currentUser == null || currentUser.Id == 0) { return new SqlResult() { QueryResultMessage = "currentUser is null" }; }
        var p = new DynamicParameters();
        p.Add("@CurrentUserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@LastActivity", DateTime.Now, DbType.DateTime, ParameterDirection.Input);
        var sql = $"UPDATE Config.AppUser SET Sessions = Sessions - 1, LastActivity = @LastActivity WHERE Id = @CurrentUserId SELECT @@ROWCOUNT";
        return await _sqlDataAccess.QueryReturnInteger(sql, p, "AppUser-LoggedOut", CommandType.Text);
    }

    public async Task<SqlResult?> ResetSessionsCounter(List<int> userIdList)
    {
        var sqlString = "";
        for (var x = 0; x < userIdList.Count(); x++)
        {
            sqlString = sqlString + $" Id = {userIdList[x]} ";
            if (x < userIdList.Count() - 1) { sqlString = sqlString + $" OR "; }
        }
        var sql = $"UPDATE Config.AppUser SET Sessions = 0 WHERE {sqlString} SELECT @@ROWCOUNT";
        return await _sqlDataAccess.QueryReturnInteger(sql, null, "AppUser-ResetSessions", CommandType.Text);
    }

    public async Task<SqlResult?> DeleteById(List<int> userIdList)
    {
        var sqlString = "";
        for (var x = 0; x < userIdList.Count(); x++)
        {
            sqlString = sqlString + $" Id = {userIdList[x]} ";
            if (x < userIdList.Count() - 1) { sqlString = sqlString + $" OR "; }
        }
        var sql = $"DELETE FROM Config.AppUser WHERE {sqlString} SELECT @@ROWCOUNT";
        return await _sqlDataAccess.QueryReturnInteger(sql, null, "AppUser-Delete", CommandType.Text);
    }

    public IEnumerable<Status> GetStatusList()
    {
        return new List<Status>() {
            new Status() { StatusId = 0, StatusName = "Draft" },
            new Status() { StatusId = 1, StatusName = "Open" },
            new Status() { StatusId = 2, StatusName = "Closed" },
            new Status() { StatusId = 3, StatusName = "Approve" },
        };
    }

    public async Task<SqlResult?> UpdateUserToken(string userName)
    {
        var token = Guid.NewGuid();
        var p = new DynamicParameters();
        p.Add("@UserName", userName, DbType.String, ParameterDirection.Input);
        p.Add("@UserToken", token, DbType.Guid, ParameterDirection.Input);
        var sql = $"UPDATE Config.AppUser SET UserToken = @UserToken WHERE UserName = @UserName SELECT @@ROWCOUNT";
        var result = await _sqlDataAccess.QueryReturnInteger(sql, p, "AppUser-UpdateUserToken", CommandType.Text);
        return result;
    }

    //public async Task<SqlResult?> UpdateAsync(AppUser user)
    //{
    //    var p = new DynamicParameters();
    //    p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
    //    p.Add("@Position", user.Position, DbType.String, ParameterDirection.Input);
    //    p.Add("@PhoneNumber", user.PhoneNumber, DbType.String, ParameterDirection.Input);
    //    var result = await _sqlDataAccess.QueryReturnInteger("dbo.SP_UserData_U", p, "AppUser-Update", CommandType.Text);
    //    return result;
    //}
}
