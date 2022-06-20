
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

    public async Task<IEnumerable<Vendor>?> GetAllAsync(int businessUnitId, bool isWaitingForApproval = false)
    {
        IEnumerable<Vendor>? result = new List<Vendor>();

        try
        {
            var currentUser = await _appUserService.GetCurrentUserAsync();
            var sql = isWaitingForApproval ? "dbo.SP_VendorWFA" : "dbo.SP_VendorAll";
            //var sql = "dbo.SP_ZZZTest";

            var p = new DynamicParameters();
            p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
            p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);

            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            result = await cn.QueryAsync<Vendor, AppUser, Vendor>(sql,
                (vendor, user) =>
                {
                    if (vendor.CompanyUsers == null && user != null) { vendor.CompanyUsers = new(); };
                    if (user != null) { vendor.CompanyUsers.Add(user); };
                    return vendor;
                },
                param: p,
                splitOn: "Id",
                commandType: CommandType.StoredProcedure);
        }
        catch (Exception e)
        {
            var message = e.Message;
        }

        return result;
    }

    public async Task<Vendor?> GetByIdAsync(int vendorid)
    {
        var p = new DynamicParameters();
        p.Add("@VendorId", vendorid, DbType.Int32, ParameterDirection.Input);
        
        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        var vendor = await cn.QueryAsync<Vendor, AppUser, Bank, Vendor>("dbo.SP_Vendor_Load",
            (vendor, user, bank) =>
            {
                if (vendor.CompanyUsers == null && user != null) { vendor.CompanyUsers = new(); };
                if (user != null) { vendor.CompanyUsers.Add(user); };

                if (vendor.BankList == null && bank != null) { vendor.BankList = new(); };
                if (bank != null) { vendor.BankList.Add(bank); };

                return vendor;
            },
            param: p,
            splitOn: "Id,BankId",
            commandType: CommandType.StoredProcedure);

        return vendor.FirstOrDefault();
    }

    public async Task<Vendor?> GetByTaxIdAsync(string taxId)
    {
        var p = new DynamicParameters();
        p.Add("@TaxId", taxId, DbType.String, ParameterDirection.Input);
        var vendor = await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorByTaxId", p, "Vendor-GetByTaxId1");

        if (vendor != null)
        {
            var rp = new DynamicParameters();
            rp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);

            var productList = await _sqlDataAccess.QueryAll<Product>("dbo.SP_VendorProductServices_Load", rp, "Vendor-GetByTaxId2");
            if (productList != null && productList.Any())
            {
                vendor.ProvidedProducts = productList.Select(e => e.ProductServiceId).ToList();
            }

            var bankList = await _sqlDataAccess.QueryAll<Bank>("dbo.SP_VendorBank_Load", rp, "Vendor-GetByTaxId3");
            if (bankList != null && bankList.Any())
            {
                vendor.BankList = bankList.ToList();
            }

            var evaluation = await _sqlDataAccess.QuerySingle<EvaluationForm>("dbo.SP_VendorEvaluation_Load", rp, "Vendor-GetByTaxId4");
            if (evaluation != null)
            {
                vendor.EvaluationForm = evaluation;
            }
        }

        return vendor;
    }

    public async Task<Vendor?> GetByUserIdAsync(int userId)
    {
        var p = new DynamicParameters();
        p.Add("@UserId", userId, DbType.Int32, ParameterDirection.Input);
        var vendor = await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorByUserId", p, "Vendor-GetByUserId1");

        if (vendor != null)
        {
            var ap11 = new DynamicParameters();
            ap11.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
            ap11.Add("@Reference", null, DbType.String, ParameterDirection.Input);
            ap11.Add("@SourceType", "VEN_LOGO", DbType.String, ParameterDirection.Input);
            var attLogo = await _sqlDataAccess.QuerySingle<Attachment>("dbo.SP_AttachmentList_Load", ap11, "Vendor-GetByUserId1-1");
            if (attLogo != null) { vendor.CompanyLogo = attLogo; }

            var ap12 = new DynamicParameters();
            ap12.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
            ap12.Add("@Reference", null, DbType.String, ParameterDirection.Input);
            ap12.Add("@SourceType", "VEN_OLET", DbType.String, ParameterDirection.Input);
            var attOfletter = await _sqlDataAccess.QuerySingle<Attachment>("dbo.SP_AttachmentList_Load", ap12, "Vendor-GetByUserId1-2");
            if (attOfletter != null) { vendor.OfficialLetter = attOfletter; }

            var rp = new DynamicParameters();
            rp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);

            var productList = await _sqlDataAccess.QueryAll<Product>("dbo.SP_VendorProductServices_Load", rp, "Vendor-GetByUserId2");
            if (productList != null && productList.Any())
            {
                vendor.ProvidedProducts = productList.Select(e => e.ProductServiceId).ToList();
            }

            var bankList = await _sqlDataAccess.QueryAll<Bank>("dbo.SP_VendorBank_Load", rp, "Vendor-GetByUserId3");
            if (bankList != null && bankList.Any())
            {
                foreach (var item in bankList)
                {
                    var ap31 = new DynamicParameters();
                    ap31.Add("@SourceId", item.BankId, DbType.Int32, ParameterDirection.Input);
                    ap31.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                    ap31.Add("@SourceType", "VEN_BNK", DbType.String, ParameterDirection.Input);
                    var attBank = await _sqlDataAccess.QuerySingle<Attachment>("dbo.SP_AttachmentList_Load", ap31, "Vendor-GetByUserId3-1");
                    if (attBank != null)
                    {
                        item.BankLetter = attBank;
                        vendor.BankList ??= new();
                        vendor.BankList.Add(item);
                    }
                }
            }

            var evaluation = await _sqlDataAccess.QuerySingle<EvaluationForm>("dbo.SP_VendorEvaluation_Load", rp, "Vendor-GetByUserId4");
            if (evaluation != null)
            {
                vendor.EvaluationForm = evaluation;

                var ap41 = new DynamicParameters();
                ap41.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                ap41.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                ap41.Add("@SourceType", "VEN_ISO", DbType.String, ParameterDirection.Input);
                var attIso = await _sqlDataAccess.QuerySingle<Attachment>("dbo.SP_AttachmentList_Load", ap41, "Vendor-GetByUserId4-1");
                if (attIso != null) { vendor.EvaluationForm.CertificateAttachment = attIso; }

                var ap42 = new DynamicParameters();
                ap42.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                ap42.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                ap42.Add("@SourceType", "VEN_OTH", DbType.String, ParameterDirection.Input);
                var attOthers = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", ap42, "Vendor-GetByUserId4-2");
                if (attOthers != null) { vendor.EvaluationForm.OtherAttachments = attOthers.ToList(); }
            }
        }

        return vendor;
    }

    public async Task<bool> IsVendorUniqueAsync(string taxId)
    {
        var p = new DynamicParameters();
        p.Add("@TaxId", taxId, DbType.String, ParameterDirection.Input);
        var result = await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorsTaxCheck", p, "IsVendorUnique");
        if (result != null && result.TaxId != null) { return false; }
        return true;
    }

    public async Task<SqlResult?> Approve(IEnumerable<ApproveData> approveDataList)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        SqlResult? result = new();
        foreach (var item in approveDataList)
        {
            var p = new DynamicParameters();
            p.Add("@VendorId", item.ModelId, DbType.Int32, ParameterDirection.Input);
            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
            p.Add("@ApproveStatusId", item.ApproveStatusId, DbType.Int32, ParameterDirection.Input);
            p.Add("@Comment", item.Comment, DbType.String, ParameterDirection.Input);
            p.Add("@Sequence", item.Sequence, DbType.Int32, ParameterDirection.Input);
            result = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorsApprove", p, "Vendor-Approve");
        }
        return result;
    }

    public async Task<SqlResult?> RegisterSupplier(Vendor vendor)
    {
        var saveResult = await SaveSupplier(vendor);
        var user = await _appUserService.GetCurrentUserAsync();

        if (saveResult != null && saveResult.QueryResultMessage == null)
        {
            var p = new DynamicParameters();
            p.Add("@VendorId", saveResult.QueryResult, DbType.Int32, ParameterDirection.Input);
            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
            p.Add("@Status", 1, DbType.Int32, ParameterDirection.Input);
            var registerResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorsChangeStatus", p, "Vendor-ChangeStatus");

            if (registerResult != null && registerResult.QueryResultMessage != null) { saveResult.QueryResultMessage = registerResult.QueryResultMessage; }
        }

        return saveResult;
    }

    public async Task<SqlResult?> SaveSupplier(Vendor vendor)
    {
        //var timer = new Stopwatch();
        //timer.Start();
        //timer.Stop();
        //Console.WriteLine($"Vendor saved: {timer.ElapsedMilliseconds}");

        var supplierResult = new SqlResult();
        var user = await _appUserService.GetCurrentUserAsync();

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
        p.Add("@OtherProducts", vendor.OtherProducts, DbType.String, ParameterDirection.Input);
        p.Add("@NewVendorId", DbType.Int32, direction: ParameterDirection.Output);
        supplierResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Vendors_IUD", p, "Vendor-Save");
        
        if (supplierResult != null && supplierResult.QueryResultMessage == null)
        {
            vendor.VendorId = p.Get<int>("@NewVendorId");
            supplierResult.QueryResult = vendor.VendorId;

            if (vendor.ProvidedProducts != null && vendor.ProvidedProducts.Any())
            {
                var pdp = new DynamicParameters();
                pdp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                var productsDeleteResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorProductServices_ID", pdp, "Vendor-SaveDeleteProducts");

                foreach (var itemId in vendor.ProvidedProducts)
                {
                    var pp = new DynamicParameters();
                    pp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    pp.Add("@ProductServiceId", itemId, DbType.Int32, ParameterDirection.Input);
                    var productsResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorProductServices_ID", pp, "Vendor-SaveProducts");
                    if (productsResult != null && productsResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = productsResult.QueryResultMessage; return supplierResult; }
                }
            }
            
            if (vendor.BankList != null && vendor.BankList.Any())
            {
                foreach (var item in vendor.BankList)
                {
                    var bp = new DynamicParameters();
                    bp.Add("@VendorBankDetailId", item.BankId, DbType.Int32, ParameterDirection.Input);
                    bp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    bp.Add("@BankName", item.BeneficiaryBankName, DbType.String, ParameterDirection.Input);
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
                    var bankResult = await _sqlDataAccess.QueryReturnInteger("dbo.SP_VendorBankDetails_IUD", bp, "Vendor-SaveBank");
                    if (bankResult != null && bankResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = bankResult.QueryResultMessage; return supplierResult; }

                    if (item.BankLetter != null && bankResult != null && bankResult.QueryResult > 0)
                    {
                        var blp = new DynamicParameters();
                        blp.Add("@AttachmentId", item.BankLetter.AttachmentId, DbType.Int32, ParameterDirection.Input);
                        blp.Add("@FileName", item.BankLetter.FileName, DbType.String, ParameterDirection.Input);
                        blp.Add("@FileData", item.BankLetter.FileData, DbType.Binary, ParameterDirection.Input);
                        blp.Add("@SourceId", bankResult.QueryResult, DbType.Int32, ParameterDirection.Input);
                        blp.Add("@SourceType", "VEN_BNK", DbType.String, ParameterDirection.Input);
                        blp.Add("@Reference", item.BankLetter.Reference, DbType.String, ParameterDirection.Input);
                        blp.Add("@ExtensionType", item.BankLetter.ExtensionType, DbType.String, ParameterDirection.Input);
                        blp.Add("@AttachmentTypeId", item.BankLetter.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                        blp.Add("@AttachmentSubTypeId", item.BankLetter.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                        var bankLetterResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", blp, "Vendor-SaveBankLetter");
                        if (bankLetterResult != null && bankLetterResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = bankLetterResult.QueryResultMessage; return supplierResult; }
                    }
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
                var evaluationResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorEvaluationForm_IUD", ep, "Vendor-SaveEvaluation");
                if (evaluationResult != null && evaluationResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = evaluationResult.QueryResultMessage; return supplierResult; }
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
                var logoResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", lp, "Vendor-SaveVendorLogo");
                if (logoResult != null && logoResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = logoResult.QueryResultMessage; return supplierResult; }
            }
            
            if (vendor.OfficialLetter != null)
            {
                var op = new DynamicParameters();
                op.Add("@AttachmentId", vendor.OfficialLetter.AttachmentId, DbType.Int32, ParameterDirection.Input);
                op.Add("@FileName", vendor.OfficialLetter.FileName, DbType.String, ParameterDirection.Input);
                op.Add("@FileData", vendor.OfficialLetter.FileData, DbType.Binary, ParameterDirection.Input);
                op.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                op.Add("@SourceType", "VEN_OLET", DbType.String, ParameterDirection.Input);
                op.Add("@Reference", vendor.OfficialLetter.Reference, DbType.String, ParameterDirection.Input);
                op.Add("@ExtensionType", vendor.OfficialLetter.ExtensionType, DbType.String, ParameterDirection.Input);
                op.Add("@AttachmentTypeId", vendor.OfficialLetter.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                op.Add("@AttachmentSubTypeId", vendor.OfficialLetter.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                var officialLetterResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", op, "Vendor-SaveOfficialLetter");
                if (officialLetterResult != null && officialLetterResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = officialLetterResult.QueryResultMessage; return supplierResult; }
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
                var certResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", cp, "Vendor-SaveVendorCert");
                if (certResult != null && certResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = certResult.QueryResultMessage; return supplierResult; }
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
                    var otherResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", op, "Vendor-SaveEvalOther");
                    if (otherResult != null && otherResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = otherResult.QueryResultMessage; return supplierResult; }
                }
            }
            
        }
        return supplierResult;
    }

    public IEnumerable<Status> GetStatusList()
    {
        return new List<Status>() {
            new Status() { StatusId = 0, StatusName = "Draft" },
            new Status() { StatusId = 1, StatusName = "Open" },
            new Status() { StatusId = 2, StatusName = "Closed" },
            new Status() { StatusId = 3, StatusName = "Blacklisted" },
        };
    }
}