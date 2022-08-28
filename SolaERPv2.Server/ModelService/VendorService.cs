
using System.Net;
using System.Net.Mail;
using Attachment = SolaERPv2.Server.Models.Attachment;

namespace SolaERPv2.Server.ModelService;

public class VendorService : BaseModelService<Vendor>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    MailService _mailService;
    IWebHostEnvironment _env;
    public VendorService(AppUserService appUserService, SqlDataAccess sqlDataAccess, MailService mailService, IWebHostEnvironment env)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
        _mailService = mailService;
        _env = env;
    }

    public async Task<IEnumerable<Vendor>?> GetAllAsync(int businessUnitId)
    {
        IEnumerable<Vendor>? result = new List<Vendor>();

        try
        {
            var currentUser = await _appUserService.GetCurrentUserAsync();
            var sql = "dbo.SP_VendorList";

            var p = new DynamicParameters();
            p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);

            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);

            result = await _sqlDataAccess.QueryAll<Vendor>(sql, p, "Vendor-GetAll");
        }
        catch (Exception e)
        {
            var message = e.Message;
        }

        return result;
    }

    public async Task<IEnumerable<Vendor>?> GetAllExtendedAsync(int businessUnitId, int activeTabIndex)
    {
        var currentUser = await _appUserService.GetCurrentUserAsync();
        Dictionary<int, Vendor> result = new();

        var sql = activeTabIndex switch {
            0 => "dbo.SP_VendorWFA",
            1 => "dbo.SP_VendorAll",
            2 => "dbo.SP_VendorDraft",
            _ => ""
        };

        var p = new DynamicParameters();
        p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        _ = await cn.QueryAsync<Vendor, AppUser, Vendor>(sql,
            (vendor, user) =>
            {
                if (!result.ContainsKey(vendor.VendorId))
                {
                    vendor.CompanyUsers = new();
                    result.Add(vendor.VendorId, vendor);
                }
                var currentVendor = result[vendor.VendorId];
                if (user != null) { currentVendor.CompanyUsers.Add(user); }
                return vendor;
            },
            param: p,
            splitOn: "Id",
            commandType: CommandType.StoredProcedure);

        return result.Values;
    }

    public async Task<Vendor?> GetByIdAsync(int vendorId)
    {
        Dictionary<int, Vendor> result = new();

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        _ = await cn.QueryAsync<Vendor, AppUser, Bank, ApproveStage, Vendor>("dbo.SP_Vendor_Load",
            (vendor, user, bank, approve) =>
            {
                if (!result.ContainsKey(vendor.VendorId))
                {
                    vendor.CompanyUsers = new();
                    vendor.BankList = new();
                    vendor.ApproveStageList = new();
                    result.Add(vendor.VendorId, vendor);
                }
                var currentVendor = result[vendor.VendorId];
                if (user != null && !currentVendor.CompanyUsers.Select(e => e.Id).Contains(user.Id)) { currentVendor.CompanyUsers.Add(user); }
                if (bank != null && !currentVendor.BankList.Select(e => e.BankId).Contains(bank.BankId)) { currentVendor.BankList.Add(bank); }
                if (approve != null && !currentVendor.ApproveStageList.Select(e => e.ApprovalId).Contains(approve.ApprovalId)) { currentVendor.ApproveStageList.Add(approve); }
                return vendor;
            },
            param: new { VendorId = vendorId },
            splitOn: "Id,BankId,VendorEvaluationFormId,ApprovalId",
            commandType: CommandType.StoredProcedure);

        return result.Values.FirstOrDefault();
    }

    public async Task<Vendor?> GetByUserIdOrTaxIdAsync(string? taxId = null)
    {
        var currentUser = await _appUserService.GetCurrentUserAsync();
        Dictionary<int, Vendor> result = new();
        var vendor = new Vendor();

        try
        {
            using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
            var p = new DynamicParameters();
            if(taxId == null)
            {
                p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
            }
            else
            {
                p.Add("@TaxId", taxId, DbType.String, ParameterDirection.Input);
            }
            _ = await cn.QueryAsync<Vendor, DueDiligence, Prequalification, Bank, string, string, int, Vendor>(taxId == null ? "dbo.SP_VendorByUserId" : "dbo.SP_VendorByTaxId",
                (vendor, dueDiligence, prequalification, bank, repComp, repProd, product) =>
                {
                    if (!result.ContainsKey(vendor.VendorId))
                    {
                        vendor.DueDiligenceList = new();
                        vendor.PrequalificationList = new();
                        vendor.BankList = new();
                        vendor.RepresentedCompanyList = new();
                        vendor.RepresentedProductList = new();
                        vendor.ProvidedProductIdList = new();
                        result.Add(vendor.VendorId, vendor);
                    }
                    var currentVendor = result[vendor.VendorId];
                    if (dueDiligence != null && !currentVendor.DueDiligenceList.Select(e => e.DueDiligenceDesignId).Contains(dueDiligence.DueDiligenceDesignId)) { currentVendor.DueDiligenceList.Add(dueDiligence); }
                    if (prequalification != null && !currentVendor.PrequalificationList.Select(e => e.PrequalificationDesignId).Contains(prequalification.PrequalificationDesignId)) { currentVendor.PrequalificationList.Add(prequalification); }
                    if (bank != null && !currentVendor.BankList.Select(e => e.BankId).Contains(bank.BankId)) { currentVendor.BankList.Add(bank); }
                    if (repComp != null && !currentVendor.RepresentedCompanyList.Contains(repComp)) { currentVendor.RepresentedCompanyList.Add(repComp); }
                    if (repProd != null && !currentVendor.RepresentedProductList.Contains(repProd)) { currentVendor.RepresentedProductList.Add(repProd); }
                    if (product != null && !currentVendor.ProvidedProductIdList.Contains(product)) { currentVendor.ProvidedProductIdList.Add(product); }
                    return vendor;
                },
                param: p,
                splitOn: "VendorId,DueDiligenceDesignId,PrequalificationDesignId,BankId,RepresentedCompanyName,RepresentedProductName,ProductServiceId",
                commandType: CommandType.StoredProcedure);

            vendor = result.Values.FirstOrDefault();

            if (vendor != null)
            {
                var alp = new DynamicParameters();
                alp.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                alp.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                alp.Add("@SourceType", "VEN_LOGO", DbType.String, ParameterDirection.Input);
                var attLogo = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", alp, "Vendor-GetByUserIdAttachmentLoad");
                if (attLogo != null) { vendor.CompanyLogo = attLogo.ToList(); }


                var aop = new DynamicParameters();
                aop.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                aop.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                aop.Add("@SourceType", "VEN_OLET", DbType.String, ParameterDirection.Input);
                var attOfletter = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", aop, "Vendor-GetByUserIdAttachmentLoad");
                if (attOfletter != null) { vendor.OfficialLetter = attOfletter.ToList(); }

                if (vendor.BankList != null && vendor.BankList.Any())
                {
                    var newBankList = new List<Bank>();
                    foreach (var bank in vendor.BankList)
                    {
                        var abp = new DynamicParameters();
                        abp.Add("@SourceId", bank.BankId, DbType.Int32, ParameterDirection.Input);
                        abp.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                        abp.Add("@SourceType", "VEN_BNK", DbType.String, ParameterDirection.Input);
                        var attBank = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", abp, "Vendor-GetByUserIdAttachmentLoad");
                        if (attBank != null)
                        {
                            bank.BankLetter = attBank.ToList();
                        }
                        newBankList.Add(bank);
                    }
                    vendor.BankList = newBankList;
                }

                if (vendor.DueDiligenceList != null && vendor.DueDiligenceList.Any())
                {
                    var newDueDiligenceList = new List<DueDiligence>();
                    foreach (var dueDiligence in vendor.DueDiligenceList)
                    {
                        var adp = new DynamicParameters();
                        adp.Add("@SourceId", dueDiligence.VendorDueDiligenceId, DbType.Int32, ParameterDirection.Input);
                        adp.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                        adp.Add("@SourceType", "VEN_DUE", DbType.String, ParameterDirection.Input);
                        var attBank = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", adp, "Vendor-GetByUserIdAttachmentLoad");
                        if (attBank != null)
                        {
                            dueDiligence.AttachmentValue = attBank.ToList();
                        }
                        newDueDiligenceList.Add(dueDiligence);
                    }
                    vendor.DueDiligenceList = newDueDiligenceList;
                }

                if (vendor.DueDiligenceList != null && vendor.DueDiligenceList.Any())
                {
                    var newDueDiligenceList = new List<DueDiligence>();
                    foreach (var dueDiligence in vendor.DueDiligenceList)
                    {
                        var grp = new DynamicParameters();
                        grp.Add("@DueDiligenceDesignId", dueDiligence.DueDiligenceDesignId, DbType.Int32, ParameterDirection.Input);
                        var grid = await _sqlDataAccess.QueryAll<GridValueModel>("dbo.SP_DueDiligenceGridData_Load", grp, "Vendor-GetByUserIdGridLoad");
                        if (grid != null)
                        {
                            dueDiligence.GridValue = grid.ToList();
                        }
                        newDueDiligenceList.Add(dueDiligence);
                    }
                    vendor.DueDiligenceList = newDueDiligenceList;
                }

                if (vendor.PrequalificationList != null && vendor.PrequalificationList.Any())
                {
                    var newPrequalificationList = new List<Prequalification>();
                    foreach (var prequalification in vendor.PrequalificationList)
                    {
                        var adp = new DynamicParameters();
                        adp.Add("@SourceId", prequalification.VendorPrequalificationId, DbType.Int32, ParameterDirection.Input);
                        adp.Add("@Reference", null, DbType.String, ParameterDirection.Input);
                        adp.Add("@SourceType", "VEN_PREQ", DbType.String, ParameterDirection.Input);
                        var att = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", adp, "Vendor-GetByUserIdAttachmentLoad");
                        if (att != null)
                        {
                            prequalification.AttachmentValue = att.ToList();
                        }
                        newPrequalificationList.Add(prequalification);
                    }
                    vendor.PrequalificationList = newPrequalificationList;
                }

                if (vendor.PrequalificationList != null && vendor.PrequalificationList.Any())
                {
                    var newPrequalificationList = new List<Prequalification>();
                    foreach (var prequalification in vendor.PrequalificationList)
                    {
                        var grp = new DynamicParameters();
                        grp.Add("@PrequalificationDesignId", prequalification.PrequalificationDesignId, DbType.Int32, ParameterDirection.Input);
                        var grid = await _sqlDataAccess.QueryAll<GridValueModel>("dbo.SP_PrequalificationGridData_Load", grp, "Vendor-GetByUserIdGridLoad");
                        if (grid != null)
                        {
                            prequalification.GridValue = grid.ToList();
                        }
                        newPrequalificationList.Add(prequalification);
                    }
                    vendor.PrequalificationList = newPrequalificationList;
                }
            }
        }
        catch (Exception e)
        {
            var message = e.Message;
        }
        return vendor;
    }


    public async Task<bool> IsVendorUniqueAsync(string taxId)
    {
        var result = await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorsTaxCheck", new { TaxId = taxId }, "IsVendorUnique");
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

    public async Task<SqlResult?> RegisterSupplier(Vendor vendor, AppUser user, List<int> deletedIdList)
    {
        var saveResult = await SaveSupplier(vendor, user, deletedIdList);
        var currentUser = await _appUserService.GetCurrentUserAsync();

        if (saveResult != null && saveResult.QueryResultMessage == null)
        {
            var p = new DynamicParameters();
            p.Add("@VendorId", saveResult.QueryResult, DbType.Int32, ParameterDirection.Input);
            p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
            p.Add("@Status", 1, DbType.Int32, ParameterDirection.Input);
            var registerResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorsChangeStatus", p, "Vendor-ChangeStatus");

            if (registerResult != null && registerResult.QueryResultMessage != null)
            {
                saveResult.QueryResultMessage = registerResult.QueryResultMessage;
            }
            else if (registerResult != null && registerResult.QueryResultMessage == null)
            {
                var htmlContent = "";
                using StreamReader reader = new StreamReader(Path.Combine(_env.ContentRootPath, "Assets", "EmailTemplates", "1RegistrationPending.html"));
                htmlContent = await reader.ReadToEndAsync();

                var htmlBody = htmlContent.Replace("{comparisonurl}", $"replaceIt")
                .Replace("{comparisonnumber}", $"replaceIt")
                .Replace("{poreference}", $"replaceIt")
                .Replace("{buyer}", $"replaceIt");

                var addressList = new List<string>() { currentUser.Email };
                var subject = $"BID Comparison Chart Approved - Comparison №";
                await _mailService.SendHtmlMailAsync(addressList, subject, htmlBody, subject);
            }
        }

        return saveResult;
    }

    public async Task<SqlResult?> SaveSupplier(Vendor vendor, AppUser user, List<int> deletedAttachmentIdList)
    {
        var supplierResult = await _appUserService.UpdateAsync(user);
        var currentUser = await _appUserService.GetCurrentUserAsync();

        foreach (var attachmentId in deletedAttachmentIdList)
        {
            var bankLetterResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", new { AttachmentId = attachmentId }, "Vendor-DeleteAttachments");
        }

        var p = new DynamicParameters();
        p.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
        p.Add("@BusinessUnitId", 1, DbType.Int32, ParameterDirection.Input);
        p.Add("@VendorName", vendor.VendorName, DbType.String, ParameterDirection.Input);
        p.Add("@TaxId", vendor.TaxId, DbType.String, ParameterDirection.Input);
        p.Add("@TaxOffice", vendor.TaxOffice, DbType.String, ParameterDirection.Input);
        p.Add("@Location", vendor.CompanyLocation, DbType.String, ParameterDirection.Input);
        p.Add("@Website", vendor.CompanyWebsite, DbType.String, ParameterDirection.Input);
        p.Add("@PaymentTerms", vendor.PaymentTermsCode, DbType.String, ParameterDirection.Input);
        p.Add("@CreditDays", vendor.CreditDays, DbType.Int32, ParameterDirection.Input);
        p.Add("@_0DaysPayment", vendor.AgreeWithDefaultDays, DbType.Boolean, ParameterDirection.Input);
        p.Add("@Country", vendor.CountryCode, DbType.String, ParameterDirection.Input);
        p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@OtherProducts", vendor.OtherProducts, DbType.String, ParameterDirection.Input);
        p.Add("@CompanyAddress", vendor.CompanyAddress, DbType.String, ParameterDirection.Input);
        p.Add("@CompanyRegistrationDate", vendor.CompanyRegistrationDate, DbType.DateTime, ParameterDirection.Input);
        p.Add("@NewVendorId", DbType.Int32, direction: ParameterDirection.Output);
        supplierResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Vendors_IUD", p, "Vendor-Save");

        if (supplierResult != null && supplierResult.QueryResultMessage == null)
        {
            vendor.VendorId = p.Get<int>("@NewVendorId");
            supplierResult.QueryResult = vendor.VendorId;

            if (vendor.ProvidedProductIdList != null && vendor.ProvidedProductIdList.Any())
            {
                var deleteResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorProductServices_ID", new { VendorId = vendor.VendorId }, "Vendor-SaveDeleteProducts");

                foreach (var itemId in vendor.ProvidedProductIdList)
                {
                    var eachResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorProductServices_ID", new { VendorId = vendor.VendorId, ProductServiceId = itemId }, "Vendor-SaveProducts");
                    if (eachResult != null && eachResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = eachResult.QueryResultMessage; return supplierResult; }
                }
            }

            if (vendor.RepresentedProductList != null && vendor.RepresentedProductList.Any())
            {
                var deleteResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorRepresentedProducts_ID", new { VendorId = vendor.VendorId }, "Vendor-SaveDeleteRepresentedProducts");

                foreach (var item in vendor.RepresentedProductList)
                {
                    var eachResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorRepresentedProducts_ID", new { VendorId = vendor.VendorId, RepresentedProductName = item }, "Vendor-SaveRepresentedProducts");
                    if (eachResult != null && eachResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = eachResult.QueryResultMessage; return supplierResult; }
                }
            }

            if (vendor.RepresentedCompanyList != null && vendor.RepresentedCompanyList.Any())
            {
                var deleteResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorRepresentedCompany_ID", new { VendorId = vendor.VendorId }, "Vendor-SaveDeleteRepresentedCompany");

                foreach (var item in vendor.RepresentedCompanyList)
                {
                    var eachResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorRepresentedCompany_ID", new { VendorId = vendor.VendorId, RepresentedCompanyName = item }, "Vendor-SaveRepresentedCompany");
                    if (eachResult != null && eachResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = eachResult.QueryResultMessage; return supplierResult; }
                }
            }

            if (vendor.CompanyLogo != null && vendor.CompanyLogo.Any())
            {
                foreach (var file in vendor.CompanyLogo)
                {
                    var lp = new DynamicParameters();
                    lp.Add("@AttachmentId", file.AttachmentId, DbType.Int32, ParameterDirection.Input);
                    lp.Add("@FileName", file.FileName, DbType.String, ParameterDirection.Input);
                    lp.Add("@FileData", file.FileData, DbType.Binary, ParameterDirection.Input);
                    lp.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    lp.Add("@SourceType", "VEN_LOGO", DbType.String, ParameterDirection.Input);
                    lp.Add("@Reference", file.Reference, DbType.String, ParameterDirection.Input);
                    lp.Add("@ExtensionType", file.ExtensionType, DbType.String, ParameterDirection.Input);
                    lp.Add("@AttachmentTypeId", file.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                    lp.Add("@AttachmentSubTypeId", file.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                    lp.Add("@UploadDateTime", file.UploadDateTime, DbType.DateTime, ParameterDirection.Input);
                    lp.Add("@Size", file.Size, DbType.Int32, ParameterDirection.Input);
                    var logoResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", lp, "Vendor-SaveVendorLogo");
                    if (logoResult != null && logoResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = logoResult.QueryResultMessage; return supplierResult; }
                }
            }

            if (vendor.OfficialLetter != null && vendor.OfficialLetter.Any())
            {
                foreach (var ofletAtt in vendor.OfficialLetter)
                {
                    var op = new DynamicParameters();
                    op.Add("@AttachmentId", ofletAtt.AttachmentId, DbType.Int32, ParameterDirection.Input);
                    op.Add("@FileName", ofletAtt.FileName, DbType.String, ParameterDirection.Input);
                    op.Add("@FileData", ofletAtt.FileData, DbType.Binary, ParameterDirection.Input);
                    op.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    op.Add("@SourceType", "VEN_OLET", DbType.String, ParameterDirection.Input);
                    op.Add("@Reference", ofletAtt.Reference, DbType.String, ParameterDirection.Input);
                    op.Add("@ExtensionType", ofletAtt.ExtensionType, DbType.String, ParameterDirection.Input);
                    op.Add("@AttachmentTypeId", ofletAtt.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                    op.Add("@AttachmentSubTypeId", ofletAtt.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                    op.Add("@UploadDateTime", ofletAtt.UploadDateTime, DbType.DateTime, ParameterDirection.Input);
                    op.Add("@Size", ofletAtt.Size, DbType.Int32, ParameterDirection.Input);
                    var officialLetterResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", op, "Vendor-SaveOfficialLetter");
                    if (officialLetterResult != null && officialLetterResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = officialLetterResult.QueryResultMessage; return supplierResult; }
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

                    if (item.BankLetter != null && item.BankLetter.Any() && bankResult != null && bankResult.QueryResult > 0)
                    {
                        foreach (var bankAttachment in item.BankLetter)
                        {
                            var blp = new DynamicParameters();
                            blp.Add("@AttachmentId", bankAttachment.AttachmentId, DbType.Int32, ParameterDirection.Input);
                            blp.Add("@FileName", bankAttachment.FileName, DbType.String, ParameterDirection.Input);
                            blp.Add("@FileData", bankAttachment.FileData, DbType.Binary, ParameterDirection.Input);
                            blp.Add("@SourceId", bankResult.QueryResult, DbType.Int32, ParameterDirection.Input);
                            blp.Add("@SourceType", "VEN_BNK", DbType.String, ParameterDirection.Input);
                            blp.Add("@Reference", bankAttachment.Reference, DbType.String, ParameterDirection.Input);
                            blp.Add("@ExtensionType", bankAttachment.ExtensionType, DbType.String, ParameterDirection.Input);
                            blp.Add("@AttachmentTypeId", bankAttachment.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                            blp.Add("@AttachmentSubTypeId", bankAttachment.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                            blp.Add("@UploadDateTime", bankAttachment.UploadDateTime, DbType.DateTime, ParameterDirection.Input);
                            blp.Add("@Size", bankAttachment.Size, DbType.Int32, ParameterDirection.Input);
                            var bankLetterResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", blp, "Vendor-SaveBankLetter");
                            if (bankLetterResult != null && bankLetterResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = bankLetterResult.QueryResultMessage; return supplierResult; }
                        }
                    }
                }
            }

            if (vendor.DueDiligenceList != null && vendor.DueDiligenceList.Any())
            {
                foreach (var item in vendor.DueDiligenceList)
                {
                    var dd = new DynamicParameters();
                    dd.Add("@VendorDueDiligenceId", item.VendorDueDiligenceId, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@DueDiligenceDesignId", item.DueDiligenceDesignId, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@TextboxValue", item.TextboxValue, DbType.String, ParameterDirection.Input);
                    dd.Add("@TextareaValue", item.TextareaValue, DbType.String, ParameterDirection.Input);
                    dd.Add("@CheckboxValue", item.CheckboxValue, DbType.Boolean, ParameterDirection.Input);
                    dd.Add("@RadioboxValue", item.RadioboxValue, DbType.Boolean, ParameterDirection.Input);
                    dd.Add("@IntValue", item.IntValue, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@DecimalValue", item.DecimalValue, DbType.Decimal, ParameterDirection.Input);
                    dd.Add("@DateTimeValue", item.DateTimeValue, DbType.DateTime, ParameterDirection.Input);
                    dd.Add("@AgreementValue", item.AgreementValue, DbType.Boolean, ParameterDirection.Input);
                    dd.Add("@Scoring", item.Scoring, DbType.Decimal, ParameterDirection.Input);
                    var dueResult = await _sqlDataAccess.QueryReturnInteger("dbo.SP_VendorDueDiligence_IUD", dd, "Vendor-SaveDueDiligence");
                    if (dueResult != null && dueResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = dueResult.QueryResultMessage; return supplierResult; }

                    if (item.AttachmentValue != null && item.AttachmentValue.Any() && dueResult != null && dueResult.QueryResult > 0)
                    {
                        foreach (var attachment in item.AttachmentValue)
                        {
                            var dda = new DynamicParameters();
                            dda.Add("@AttachmentId", attachment.AttachmentId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@FileName", attachment.FileName, DbType.String, ParameterDirection.Input);
                            dda.Add("@FileData", attachment.FileData, DbType.Binary, ParameterDirection.Input);
                            dda.Add("@SourceId", dueResult.QueryResult, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@SourceType", "VEN_DUE", DbType.String, ParameterDirection.Input);
                            dda.Add("@Reference", attachment.Reference, DbType.String, ParameterDirection.Input);
                            dda.Add("@ExtensionType", attachment.ExtensionType, DbType.String, ParameterDirection.Input);
                            dda.Add("@AttachmentTypeId", attachment.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@AttachmentSubTypeId", attachment.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@UploadDateTime", attachment.UploadDateTime, DbType.DateTime, ParameterDirection.Input);
                            dda.Add("@Size", attachment.Size, DbType.Int32, ParameterDirection.Input);
                            var dueAttachmentResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", dda, "Vendor-SaveDueDiligenceAttachments");
                            if (dueAttachmentResult != null && dueAttachmentResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = dueAttachmentResult.QueryResultMessage; return supplierResult; }
                        }
                    }

                    if (item.GridValue != null && item.GridValue.Any() && dueResult != null && dueResult.QueryResult > 0)
                    {
                        foreach (var grid in item.GridValue)
                        {
                            var dda = new DynamicParameters();
                            dda.Add("@DueDiligenceGridDataId", grid.DueDiligenceGridDataId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@DueDiligenceDesignId", item.DueDiligenceDesignId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@Column1", grid.Column1, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column2", grid.Column2, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column3", grid.Column3, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column4", grid.Column4, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column5", grid.Column5, DbType.String, ParameterDirection.Input);
                            var dueGridResult = await _sqlDataAccess.ExecuteSql("dbo.SP_DueDiligenceGridData_IUD", dda, "Vendor-SaveDueDiligenceGridValue");
                            if (dueGridResult != null && dueGridResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = dueGridResult.QueryResultMessage; return supplierResult; }
                        }
                    }
                }
            }

            if (vendor.PrequalificationList != null && vendor.PrequalificationList.Any())
            {
                foreach (var item in vendor.PrequalificationList)
                {
                    var dd = new DynamicParameters();
                    dd.Add("@VendorPrequalificationId", item.VendorPrequalificationId, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@PrequalificationDesignId", item.PrequalificationDesignId, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@TextboxValue", item.TextboxValue, DbType.String, ParameterDirection.Input);
                    dd.Add("@TextareaValue", item.TextareaValue, DbType.String, ParameterDirection.Input);
                    dd.Add("@CheckboxValue", item.CheckboxValue, DbType.Boolean, ParameterDirection.Input);
                    dd.Add("@RadioboxValue", item.RadioboxValue, DbType.Boolean, ParameterDirection.Input);
                    dd.Add("@IntValue", item.IntValue, DbType.Int32, ParameterDirection.Input);
                    dd.Add("@DecimalValue", item.DecimalValue, DbType.Decimal, ParameterDirection.Input);
                    dd.Add("@DateTimeValue", item.DateTimeValue, DbType.DateTime, ParameterDirection.Input);
                    //dd.Add("@AgreementValue", item.AgreementValue, DbType.Boolean, ParameterDirection.Input);
                    dd.Add("@Scoring", item.Scoring, DbType.Decimal, ParameterDirection.Input);
                    var preqResult = await _sqlDataAccess.QueryReturnInteger("dbo.SP_VendorPrequalification_IUD", dd, "Vendor-SavePrequalification");
                    if (preqResult != null && preqResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = preqResult.QueryResultMessage; return supplierResult; }

                    if (item.AttachmentValue != null && item.AttachmentValue.Any() && preqResult != null && preqResult.QueryResult > 0)
                    {
                        foreach (var attachment in item.AttachmentValue)
                        {
                            var dda = new DynamicParameters();
                            dda.Add("@AttachmentId", attachment.AttachmentId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@FileName", attachment.FileName, DbType.String, ParameterDirection.Input);
                            dda.Add("@FileData", attachment.FileData, DbType.Binary, ParameterDirection.Input);
                            dda.Add("@SourceId", preqResult.QueryResult, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@SourceType", "VEN_PREQ", DbType.String, ParameterDirection.Input);
                            dda.Add("@Reference", attachment.Reference, DbType.String, ParameterDirection.Input);
                            dda.Add("@ExtensionType", attachment.ExtensionType, DbType.String, ParameterDirection.Input);
                            dda.Add("@AttachmentTypeId", attachment.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@AttachmentSubTypeId", attachment.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@UploadDateTime", attachment.UploadDateTime, DbType.DateTime, ParameterDirection.Input);
                            dda.Add("@Size", attachment.Size, DbType.Int32, ParameterDirection.Input);
                            var preqAttachmentResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", dda, "Vendor-SavePrequalificationAttachments");
                            if (preqAttachmentResult != null && preqAttachmentResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = preqAttachmentResult.QueryResultMessage; return supplierResult; }
                        }
                    }

                    if (item.GridValue != null && item.GridValue.Any() && preqResult != null && preqResult.QueryResult > 0)
                    {
                        foreach (var grid in item.GridValue)
                        {
                            var dda = new DynamicParameters();
                            dda.Add("@PrequalificationGridDataId", grid.PrequalificationGridDataId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@PrequalificationDesignId", item.PrequalificationDesignId, DbType.Int32, ParameterDirection.Input);
                            dda.Add("@Column1", grid.Column1, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column2", grid.Column2, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column3", grid.Column3, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column4", grid.Column4, DbType.String, ParameterDirection.Input);
                            dda.Add("@Column5", grid.Column5, DbType.String, ParameterDirection.Input);
                            var preqGridResult = await _sqlDataAccess.ExecuteSql("dbo.SP_PrequalificationGridData_IUD", dda, "Vendor-SavePrequalificationGridValue");
                            if (preqGridResult != null && preqGridResult.QueryResultMessage != null) { supplierResult.QueryResultMessage = preqGridResult.QueryResultMessage; return supplierResult; }
                        }
                    }
                }
            }
        }
        return supplierResult;
    }

    public async Task<SqlResult?> SendToApproval(List<Vendor> vendorList, int approveStageMainId)
    {
        var sqlResult = new SqlResult();
        foreach (var vendor in vendorList)
        {
            sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorSendToApprove", new { VendorId = vendor.VendorId, ApproveStageMainId = approveStageMainId }, "Vendor-SendToApproval");
        }

        return sqlResult;
    }

    public IEnumerable<Status> GetStatusList()
    {
        return new List<Status>() {
            new Status() { StatusId = 0, StatusName = "Draft" },
            new Status() { StatusId = 1, StatusName = "Submitted" },
            new Status() { StatusId = 2, StatusName = "Open" },
            new Status() { StatusId = 3, StatusName = "Closed" },
            new Status() { StatusId = 4, StatusName = "Blacklisted" },
        };
    }
}