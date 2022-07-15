namespace SolaERPv2.Server.ModelService;

public class RequestService : BaseModelService<RequestMain>
{
    AppUserService? _appUserService;
    SqlDataAccess? _sqlDataAccess;
    public RequestService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<RequestMain>?> GetAllFromSytelineAsync(int businessUnitId, int requestTypeId)
    {
        var currentUser = await _appUserService.GetCurrentUserAsync();
        Dictionary<string, RequestMain> result = new();
        
        var p = new DynamicParameters();
        p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);
        p.Add("RequestType", requestTypeId, DbType.Int32, ParameterDirection.Input);

        using IDbConnection cn = new SqlConnection(_sqlDataAccess?.ConnectionString);
        _ = await cn.QueryAsync<RequestMain, RequestDetails, RequestMain>("dbo.SP_RequestListForRFQ",
            (main, details) =>
            {
                if (!result.ContainsKey(main.RequestNumber))
                {
                    main.RequestDetailsList = new();
                    result.Add(main.RequestNumber, main);
                }
                var currentMain = result[main.RequestNumber];
                if (details != null) { currentMain.RequestDetailsList.Add(details); }
                return main;
            },
            param: p,
            splitOn: "RequestLine",
            commandType: CommandType.StoredProcedure);

        return result.Values;
    }

    public IEnumerable<RequestType> GetAllRequestTypes()
    {
        return new List<RequestType>() {
            new RequestType() { RequestTypeId = 1, RequestTypeName = "Purchase" },
            new RequestType() { RequestTypeId = 2, RequestTypeName = "Service" },
            new RequestType() { RequestTypeId = 3, RequestTypeName = "Logistics" },
            new RequestType() { RequestTypeId = 4, RequestTypeName = "Rent" },
        };
    }
}
