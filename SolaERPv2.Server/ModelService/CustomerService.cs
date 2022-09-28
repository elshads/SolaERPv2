using Microsoft.AspNetCore.Mvc;
using SolaERPv2.Server.Models;
using SolaERPv2.Server.Pages;

namespace SolaERPv2.Server.ModelService;

public class CustomerService:BaseModelService<Customer>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;

    public CustomerService(SqlDataAccess sqlDataAccess, AppUserService appUserService)
    {
        _sqlDataAccess = sqlDataAccess;
        _appUserService = appUserService;

    }

    public async Task<IEnumerable<Customer>?> GetAll(int? mainCustomerId)
    {
        if (mainCustomerId == null) return new List<Customer>();

        string sql = "dbo.us_GetAllCustomerByMainCompanyId";
        var p = new DynamicParameters();
        p.Add("@Id", mainCustomerId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<Customer>(sql, p, "CS-GetAll");
        //foreach (var item in result)
        //{
        //    Console.WriteLine(item.CustomerId+" " +item.Name + $" MainVopany {item.ma}");
        //}

        if (result == null) return new List<Customer>();

        return result;
    }

    public async Task<IEnumerable<Customer>?> GetAll()
    {
        Dictionary<int, Customer> result = new();

  //      var sql = @"select c.[Id] as 'CustomerId'
		//,c.[Name]
		//,c.[Description]
		//,c.[BaseCustomerId]
		//,st.[Id]
		//,st.[Name]
		//,st.[Sequence]
		//,st.[CustomerId]
		//from Stages st
		//right join Companies c
		//on c.Id=st.CustomerId";

  //      using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
  //      await cn.QueryAsync<Customer, Stage, Customer>(sql,
  //      (customer, stage) =>
  //      {
  //          if (!result.ContainsKey(customer.CustomerId))
  //          {
  //              customer.Stages = new();
  //              result.Add(customer.CustomerId, customer);
  //          }
  //          var currentCompany = result[customer.CustomerId];
  //          if (stage != null) { currentCompany.Stages.Add(stage); }
  //          return customer;
  //      },
  //      splitOn: "Id",
  //      commandType: CommandType.Text);


        return result.Values;
    }

    public async Task<IEnumerable<Customer>?> GetAllAsync(int baseCustomerId/*, int tabindex*/)
    {

        var sql = @"select c.[Id] as 'CustomerId'
        ,c.[Name]
        ,c.[Description]
        ,c.[BaseCustomerId]
        ,st.[Id]
        ,st.[Name]
        ,st.[Sequence]
        ,st.[CustomerId]
        from Stages st
        right join Companies c
        on c.Id=st.CustomerId";

        //var sql = "";
        //switch (tabindex)
        //{
        //    case 0:
        //        sql = @"select c.[Id] as 'CustomerId'
        //  ,c.[Name]
        //  ,c.[Description]
        //  ,c.[BaseCustomerId]
        //  ,st.[Id]
        //  ,st.[Name]
        //  ,st.[Sequence]
        //  ,st.[CustomerId]
        //  from Stages st
        //  right join Companies c
        //  on c.Id=st.CustomerId";
        //        break;

        //}

        var p = new DynamicParameters();
        p.Add("@Id", baseCustomerId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<Customer>(sql, p, "PD-GetAll");
    }

    public async Task<Customer?> GetByIdAsync(int? customerId)
    {
        if (customerId == null) return new Customer();
        string sql = "dbo.us_GetAllCustomerByMainCustomerId";
        var p = new DynamicParameters();
        p.Add("@Id", customerId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QuerySingle<Customer>(sql, p, "CS-GetAll");
        //foreach (var item in result)
        //{
        //    Console.WriteLine(item.CustomerId+" " +item.Name + $" MainVopany {item.ma}");
        //}

        if (result == null)
            result = new Customer();

        return result;
    }

    #region GetByIdAsync


    //public async Task<Customer?> GetByIdAsync(int customerId)
    //{
    //    Dictionary<int, Customer> result = new();

    //    using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
    //    await cn.QueryAsync<Customer, Stage, StageRole, Customer>("dbo.sp_Company_Load",
    //        (customer, stage, stageRole) =>
    //        {
    //            if (!result.ContainsKey(customer.CustomerId))
    //            {
    //                customer.Stages = new();
    //                result.Add(customer.CustomerId, customer);
    //            }
    //            var currentCompany = result[customer.CustomerId];


    //            if (stage != null)
    //            {
    //                stage.StageRoles ??= new();
    //                stage.StageRoles?.Add(stageRole);

    //                currentCompany.Stages?.Add(stage);

    //            }

    //            return customer;
    //        },
    //        param: new { CustomerId = customerId },
    //        splitOn: "Id,StageId",
    //        commandType: CommandType.StoredProcedure);

    //    return result.Values.FirstOrDefault();
    //}
    #endregion

    public async Task<SqlResult?> Save(Customer customer)
    {
        SqlResult? sqlResult = new();
        //var customerId = customer.CustomerId;

        //try
        //{
        //    using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        //    var p = new DynamicParameters();
        //    p.Add("@Id", customer.CustomerId, DbType.Int32, ParameterDirection.Input);
        //    p.Add("@Name", customer.Name, DbType.String, ParameterDirection.Input);
        //    p.Add("@Description", customer.Description, DbType.String, ParameterDirection.Input);
        //    var result = await cn.QueryAsync<int>("select * from dbo.Companies", p, commandType: CommandType.Text);

        //    if (result.Any()) { customerId = result.FirstOrDefault(); }
        //}
        //catch (Exception e)
        //{
        //    sqlResult.QueryResultMessage = e.Message;
        //}

        //if (sqlResult?.QueryResultMessage != null) { return sqlResult; } // return if failed


        //sqlResult.QueryResult = customerId;
        return sqlResult;
    }


  //  public async Task<IEnumerable<Customer>?> GetAll()
  //  {
  //      Dictionary<int, Customer> result = new();

  //      var sql = @"select c.[Id] as 'CustomerId'
		//,c.[Name]
		//,c.[Description]
		//,c.[BaseCustomerId]
		//,st.[Id]
		//,st.[Name]
		//,st.[Sequence]
		//,st.[CustomerId]
		//from Stages st
		//right join Companies c
		//on c.Id=st.CustomerId";

  //      using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
  //      await cn.QueryAsync<Customer, Stage, Customer>(sql,
  //      (customer, stage) =>
  //      {
  //          if (!result.ContainsKey(customer.CustomerId))
  //          {
  //              customer.Stages = new();
  //              result.Add(customer.CustomerId, customer);
  //          }
  //          var currentCompany = result[customer.CustomerId];
  //          if (stage != null) { currentCompany.Stages.Add(stage); }
  //          return customer;
  //      },
  //      splitOn: "Id",
  //      commandType: CommandType.Text);


  //      return result.Values;
  //  }
}
