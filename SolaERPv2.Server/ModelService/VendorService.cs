namespace SolaERPv2.Server.ModelService;

public class VendorService : BaseModelService<Vendor>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public VendorService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<Vendor>?> GetAllAsync(int businessUnitId)
    {
        var p = new DynamicParameters();
        p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<Vendor>("dbo.SP_VendorList", p);
    }

    public async Task<Vendor?> GetByTaxIdAsync(string taxId)
    {
        var p = new DynamicParameters();
        p.Add("@TaxId", taxId, DbType.String, ParameterDirection.Input);
        return await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorsTaxCheck", p);
    }

    public async Task<bool> IsVendorUniqueAsync(string taxId)
    {
        var p = new DynamicParameters();
        p.Add("@TaxId", taxId, DbType.String, ParameterDirection.Input);
        var result = await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorsTaxCheck", p);
        if (result != null && result.TaxId != null) { return false; }
        return true;
    }

    public async Task<SqlResult?> RegisterSupplier(Vendor vendor)
    {
        var supplierResult = new SqlResult();
        var user = await _appUserService.GetCurrentUserAsync();

        if (vendor.VendorId == 0)
        {
            var p = new DynamicParameters();
            p.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
            p.Add("@BusinessUnitId", 1, DbType.Int32, ParameterDirection.Input);
            p.Add("@VendorName", vendor.VendorName, DbType.String, ParameterDirection.Input);
            p.Add("@TaxId", vendor.TaxId, DbType.String, ParameterDirection.Input);
            p.Add("@Location", vendor.CompanyLocation, DbType.String, ParameterDirection.Input);
            p.Add("@Website", vendor.CompanyWebsite, DbType.String, ParameterDirection.Input);
            p.Add("@RepresentedProducts", vendor.RepresentedProducts, DbType.String, ParameterDirection.Input);
            p.Add("@RepresentedCompanies", vendor.RepresentedCompanies, DbType.String, ParameterDirection.Input);
            p.Add("@PaymentTerms", vendor.PaymentTermsCode, DbType.String, ParameterDirection.Input);
            p.Add("@CreditDays", vendor.CreditDays, DbType.Int32, ParameterDirection.Input);
            p.Add("@_0DaysPayment", vendor.AgreeWithDefaultDays, DbType.Boolean, ParameterDirection.Input);
            p.Add("@Country", vendor.CountryCode, DbType.String, ParameterDirection.Input);
            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
            p.Add("@NewVendorId", DbType.Int32, direction: ParameterDirection.Output);
            supplierResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Vendors_IUD", p);


            if (supplierResult != null && supplierResult.QueryResultMessage == null)
            {
                vendor.VendorId = p.Get<int>("@NewVendorId");
                if (vendor.BankList != null && vendor.BankList.Any())
                {
                    foreach (var item in vendor.BankList)
                    {
                        var bp = new DynamicParameters();
                        bp.Add("@VendorBankDetailId", item.BankId, DbType.Int32, ParameterDirection.Input);
                        bp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                        bp.Add("@BankName", item.BankName, DbType.String, ParameterDirection.Input);
                        bp.Add("@CurrencyCode", item.CurrencyCode, DbType.String, ParameterDirection.Input);
                        bp.Add("@IBAN", item.BankAccountNumber, DbType.String, ParameterDirection.Input);
                        bp.Add("@BankAddress", item.BeneficiaryBankAddress, DbType.String, ParameterDirection.Input);
                        bp.Add("@BankAddress1", item.BeneficiaryBankAddress1, DbType.String, ParameterDirection.Input);
                        bp.Add("@BeneficiarName", item.BeneficiaryFullName, DbType.String, ParameterDirection.Input);
                        bp.Add("@BeneficiarAddress", item.BeneficiaryAddress, DbType.String, ParameterDirection.Input);
                        bp.Add("@BeneficiarAddress1", item.BeneficiaryAddress1, DbType.String, ParameterDirection.Input);
                        bp.Add("@BeneficiarBankCode", item.BeneficiaryBankCode, DbType.String, ParameterDirection.Input);
                        bp.Add("@IntermediaryBankCode", item.IntermediaryBankCodeNumber, DbType.String, ParameterDirection.Input);
                        bp.Add("@IntermediaryBankCodeType", item.IntermediaryBankCodeType, DbType.String, ParameterDirection.Input);
                        var bankResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorBankDetails_IUD", bp);
                    }
                }

                if (vendor.EvaluationFormList != null && vendor.EvaluationFormList.Any())
                {
                    foreach (var item in vendor.EvaluationFormList)
                    {
                        var ep = new DynamicParameters();
                        ep.Add("@VendorEvaluationFormId", item.VendorEvaluationFormId, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@ContextOfTheOrganization1", item.ContextOfTheOrganization1, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@ContextOfTheOrganization2", item.ContextOfTheOrganization2, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@ContextOfTheOrganization3", item.ContextOfTheOrganization3, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@ExpirationDate", item.ExpirationDate, DbType.DateTime, ParameterDirection.Input);
                        ep.Add("@Leadership1", item.Leadership1, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@Leadership2", item.Leadership2, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@Planning1", item.Planning1, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@Planning2", item.Planning2, DbType.Int32, ParameterDirection.Input);
                        ep.Add("@Planning3", item.Planning3, DbType.Int32, ParameterDirection.Input);
                        var evaluationResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorEvaluationForm_IUD", ep);
                    }
                }
            }

            
        }
        return supplierResult;
    }
}