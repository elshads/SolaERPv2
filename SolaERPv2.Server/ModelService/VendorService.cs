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
        var result = await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorsTaxCheck", p);
        return result;
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
                        if (bankResult != null) { supplierResult.QueryResultMessage = bankResult.QueryResultMessage; }
                    }
                }

                if (vendor.EvaluationForm != null)
                {
                    var ep = new DynamicParameters();
                    ep.Add("@VendorEvaluationFormId", vendor.EvaluationForm.VendorEvaluationFormId, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@ContextOfTheOrganization1", vendor.EvaluationForm.ContextOfTheOrganization1, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@ContextOfTheOrganization2", vendor.EvaluationForm.ContextOfTheOrganization2, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@ContextOfTheOrganization3", vendor.EvaluationForm.ContextOfTheOrganization3, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@ExpirationDate", vendor.EvaluationForm.ExpirationDate, DbType.DateTime, ParameterDirection.Input);
                    ep.Add("@Leadership1", vendor.EvaluationForm.Leadership1, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@Leadership2", vendor.EvaluationForm.Leadership2, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@Planning1", vendor.EvaluationForm.Planning1, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@Planning2", vendor.EvaluationForm.Planning2, DbType.Int32, ParameterDirection.Input);
                    ep.Add("@Planning3", vendor.EvaluationForm.Planning3, DbType.Int32, ParameterDirection.Input);
                    var evaluationResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorEvaluationForm_IUD", ep);
                    if (evaluationResult != null) { supplierResult.QueryResultMessage = evaluationResult.QueryResultMessage; }
                }

                if (vendor.CompanyLogo != null)
                {
                    var lp = new DynamicParameters();
                    lp.Add("@AttachmentId", vendor.CompanyLogo.AttachmentId, DbType.Int32, ParameterDirection.Input);
                    lp.Add("@FileName", vendor.CompanyLogo.FileName, DbType.String, ParameterDirection.Input);
                    lp.Add("@FileData", vendor.CompanyLogo.FileData, DbType.Binary, ParameterDirection.Input);
                    lp.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    lp.Add("@SourceType", "VEN_LOGO", DbType.String, ParameterDirection.Input);
                    lp.Add("@Reference", vendor.CompanyLogo.Reference, DbType.String, ParameterDirection.Input);
                    lp.Add("@ExtensionType", vendor.CompanyLogo.ExtensionType, DbType.String, ParameterDirection.Input);
                    lp.Add("@AttachmentTypeId", vendor.CompanyLogo.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                    lp.Add("@AttachmentSubTypeId", vendor.CompanyLogo.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                    var logoResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", lp);
                    if (logoResult != null) { supplierResult.QueryResultMessage = logoResult.QueryResultMessage; }
                }

                if (vendor.EvaluationForm != null && vendor.EvaluationForm.CertificateAttachment != null)
                {
                    var cp = new DynamicParameters();
                    cp.Add("@AttachmentId", vendor.EvaluationForm.CertificateAttachment.AttachmentId, DbType.Int32, ParameterDirection.Input);
                    cp.Add("@FileName", vendor.EvaluationForm.CertificateAttachment.FileName, DbType.String, ParameterDirection.Input);
                    cp.Add("@FileData", vendor.EvaluationForm.CertificateAttachment.FileData, DbType.Binary, ParameterDirection.Input);
                    cp.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    cp.Add("@SourceType", "VEN_ISO", DbType.String, ParameterDirection.Input);
                    cp.Add("@Reference", vendor.EvaluationForm.CertificateAttachment.Reference, DbType.String, ParameterDirection.Input);
                    cp.Add("@ExtensionType", vendor.EvaluationForm.CertificateAttachment.ExtensionType, DbType.String, ParameterDirection.Input);
                    cp.Add("@AttachmentTypeId", vendor.EvaluationForm.CertificateAttachment.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                    cp.Add("@AttachmentSubTypeId", vendor.EvaluationForm.CertificateAttachment.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                    var certResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", cp);
                    if (certResult != null) { supplierResult.QueryResultMessage = certResult.QueryResultMessage; }
                }

                if (vendor.EvaluationForm != null && vendor.EvaluationForm.OtherAttachments != null && vendor.EvaluationForm.OtherAttachments.Any())
                {
                    foreach (var item in vendor.EvaluationForm.OtherAttachments)
                    {
                        var op = new DynamicParameters();
                        op.Add("@AttachmentId", item.AttachmentId, DbType.Int32, ParameterDirection.Input);
                        op.Add("@FileName", item.FileName, DbType.String, ParameterDirection.Input);
                        op.Add("@FileData", item.FileData, DbType.Binary, ParameterDirection.Input);
                        op.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                        op.Add("@SourceType", "VEN_OTH", DbType.String, ParameterDirection.Input);
                        op.Add("@Reference", item.Reference, DbType.String, ParameterDirection.Input);
                        op.Add("@ExtensionType", item.ExtensionType, DbType.String, ParameterDirection.Input);
                        op.Add("@AttachmentTypeId", item.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                        op.Add("@AttachmentSubTypeId", item.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                        var otherResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", op);
                        if (otherResult != null) { supplierResult.QueryResultMessage = otherResult.QueryResultMessage; }
                    }
                }
            }
        }
        return supplierResult;
    }
}