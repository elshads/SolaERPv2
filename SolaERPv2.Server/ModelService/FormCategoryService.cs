namespace SolaERPv2.Server.ModelService;

public class FormCategoryService : BaseModelService<FormCategory>
{
    AppUserService? _appUserService;
    SqlDataAccess? _sqlDataAccess;
    public FormCategoryService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<FormCategory>?> GetAllAsync(int appFunctionId)
    {
        var currentUser = await _appUserService.GetCurrentUserAsync();
        Dictionary<int, FormCategory> result = new();

        var p = new DynamicParameters();
        p.Add("@AppFunctionId", appFunctionId, DbType.Int32, ParameterDirection.Input);

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        _ = await cn.QueryAsync<FormCategory, FormSubcategory, FormCategory>("dbo.SP_SelectFormFields",
            (cat, subcat) =>
            {
                if (!result.ContainsKey(cat.FormCategoryId))
                {
                    cat.FormSubcategoryList = new();
                    result.Add(cat.FormCategoryId, cat);
                }
                var currentCat = result[cat.FormCategoryId];
                if (subcat != null) { currentCat.FormSubcategoryList?.Add(subcat); }
                return cat;
            },
            param: p,
            splitOn: "FormCategoryId, FormSubcategoryId",
            commandType: CommandType.StoredProcedure);

        return result.Values;
    }
}