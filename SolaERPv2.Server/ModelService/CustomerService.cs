using Microsoft.AspNetCore.Mvc;
using SolaERPv2.Server.Models;
using SolaERPv2.Server.Pages;

namespace SolaERPv2.Server.ModelService;

public class CustomerService:BaseModelService<Customer>
{
    SqlDataAccess _sqlDataAccess;

    public CustomerService(SqlDataAccess sqlDataAccess)
    {
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Customer>?> GetAll(int? mainCustomerId)
    {
        if (mainCustomerId == null) return new List<Customer>();

        string sql = "dbo.us_GetAllCustomerByMainCompanyId";
        var p = new DynamicParameters();
        p.Add("@Id", mainCustomerId, DbType.Int32, ParameterDirection.Input);

        var result = await _sqlDataAccess.QueryAll<Customer>(sql, p, "CusMain-GetAll");

        if (result == null) return new List<Customer>();

        return result;
    }

    //public async Task<SqlResult?> Save(Customer paymentDocumentMain)
    //{
    //    SqlResult? result = new();
    //    using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
    //    {
    //        var p = new DynamicParameters();
    //        p.Add("@CustomerId", paymentDocumentMain.CustomerId, DbType.Int32, ParameterDirection.Input);
    //        p.Add("@Description", paymentDocumentMain.Description, DbType.String, ParameterDirection.Input);
    //        p.Add("@Name", paymentDocumentMain.Name, DbType.String, ParameterDirection.Input);
    //        p.Add("@UnitCode", paymentDocumentMain.UnitCode, DbType.String, ParameterDirection.Input);
    //        p.Add("@MainCompanyId", paymentDocumentMain.MainCompanyId, DbType.Int32, ParameterDirection.Input);

    //        p.Add("@NewCustomerId", dbType: DbType.Int32, direction: ParameterDirection.Output);

    //        result.QueryResult = await cn.ExecuteAsync("dbo.us_Customer_IUD", p, commandType: CommandType.StoredProcedure);
    //        result.QueryResult = p.Get<int>("@NewCustomerId");
    //    }



    //    return result;
    //}



    #region GetById
    //public async Task<Customer?> GetByIdAsync(int? customerId)
    //{
    //    if (customerId == null) return new Customer();
    //    string sql = "dbo.us_GetAllCustomerByMainCustomerId";
    //    var p = new DynamicParameters();
    //    p.Add("@Id", customerId, DbType.Int32, ParameterDirection.Input);

    //    var result = await _sqlDataAccess.QuerySingle<Customer>(sql, p, "CS-GetAll");
    //    //foreach (var item in result)
    //    //{
    //    //    Console.WriteLine(item.CustomerId+" " +item.Name + $" MainVopany {item.ma}");
    //    //}

    //    if (result == null)
    //        result = new Customer();

    //    return result;
    //}
    #endregion

    #region GetByIdList


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



}
