﻿
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
        var p = new DynamicParameters();
        p.Add("@VendorId", vendorId, DbType.Int32, ParameterDirection.Input);

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
            param: p,
            splitOn: "Id,BankId,VendorEvaluationFormId,ApprovalId",
            commandType: CommandType.StoredProcedure);

        return result.Values.FirstOrDefault();
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
                vendor.ProvidedProductIdList = productList.Select(e => e.ProductServiceId).ToList();
            }

            var bankList = await _sqlDataAccess.QueryAll<Bank>("dbo.SP_VendorBank_Load", rp, "Vendor-GetByTaxId3");
            if (bankList != null && bankList.Any())
            {
                vendor.BankList = bankList.ToList();
            }
            
        }

        return vendor;
    }

    public async Task<Vendor?> GetByUserIdAsync()
    {
        var currentUser = await _appUserService.GetCurrentUserAsync();
        Dictionary<int, Vendor> result = new();
        var p = new DynamicParameters();
        p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);

        using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
        _ = await cn.QueryAsync<Vendor, DueDiligence, Bank, string, string, int, Vendor>("dbo.SP_VendorByUserId",
            (vendor, dueDiligence, bank, repComp, repProd, product) =>
            {
                if (!result.ContainsKey(vendor.VendorId))
                {
                    vendor.DueDiligenceList = new();
                    vendor.BankList = new();
                    vendor.RepresentedCompanyList = new();
                    vendor.RepresentedProductList = new();
                    vendor.ProvidedProductIdList = new();
                    result.Add(vendor.VendorId, vendor);
                }
                var currentVendor = result[vendor.VendorId];
                if (dueDiligence != null && !currentVendor.DueDiligenceList.Select(e => e.DueDiligenceDesignId).Contains(dueDiligence.DueDiligenceDesignId)) { currentVendor.DueDiligenceList.Add(dueDiligence); }
                if (bank != null && !currentVendor.BankList.Select(e => e.BankId).Contains(bank.BankId)) { currentVendor.BankList.Add(bank); }
                if (repComp != null && !currentVendor.RepresentedCompanyList.Contains(repComp)) { currentVendor.RepresentedCompanyList.Add(repComp); }
                if (repProd != null && !currentVendor.RepresentedProductList.Contains(repProd)) { currentVendor.RepresentedProductList.Add(repProd); }
                if (product != null && !currentVendor.ProvidedProductIdList.Contains(product)) { currentVendor.ProvidedProductIdList.Add(product); }
                return vendor;
            },
            param: p,
            splitOn: "VendorId,DueDiligenceDesignId,BankId,RepresentedCompanyName,RepresentedProductName,ProductServiceId",
            commandType: CommandType.StoredProcedure);

        // add attachments

        return result.Values.FirstOrDefault();
    }


    public async Task<Vendor?> GetByUserIdAsyncOld()
    {
        var currentUser = await _appUserService.GetCurrentUserAsync();

        var p = new DynamicParameters();
        p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        var vendor = await _sqlDataAccess.QuerySingle<Vendor>("dbo.SP_VendorByUserId", p, "Vendor-GetByUserId1");

        if (vendor != null)
        {
            var ap11 = new DynamicParameters();
            ap11.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
            ap11.Add("@Reference", null, DbType.String, ParameterDirection.Input);
            ap11.Add("@SourceType", "VEN_LOGO", DbType.String, ParameterDirection.Input);
            var attLogo = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", ap11, "Vendor-GetByUserId1-1");
            if (attLogo != null) { vendor.CompanyLogo = attLogo.ToList(); }

            var ap12 = new DynamicParameters();
            ap12.Add("@SourceId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
            ap12.Add("@Reference", null, DbType.String, ParameterDirection.Input);
            ap12.Add("@SourceType", "VEN_OLET", DbType.String, ParameterDirection.Input);
            var attOfletter = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", ap12, "Vendor-GetByUserId1-2");
            if (attOfletter != null) { vendor.OfficialLetter = attOfletter.ToList(); }

            var rp = new DynamicParameters();
            rp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);

            var productList = await _sqlDataAccess.QueryAll<Product>("dbo.SP_VendorProductServices_Load", rp, "Vendor-GetByUserId2");
            if (productList != null && productList.Any())
            {
                vendor.ProvidedProductIdList = productList.Select(e => e.ProductServiceId).ToList();
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
                    var attBank = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", ap31, "Vendor-GetByUserId3-1");
                    if (attBank != null)
                    {
                        item.BankLetter = attBank.ToList();
                    }
                    vendor.BankList ??= new();
                    vendor.BankList.Add(item);
                }
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

                var addressList = new List<string>() {"elshad82@gmail.com"};
                var subject = $"BID Comparison Chart Approved - Comparison №";
                await _mailService.SendHtmlMailAsync(addressList, subject, htmlBody, subject);
            }
        }

        return saveResult;
    }

    public async Task<SqlResult?> SaveSupplier(Vendor vendor, AppUser user, List<int> deletedAttachmentIdList)
    {
        _ = await _appUserService.UpdateAsync(user);

        var supplierResult = new SqlResult();
        var currentUser = await _appUserService.GetCurrentUserAsync();

        foreach (var attachmentId in deletedAttachmentIdList)
        {
            var ap = new DynamicParameters();
            ap.Add("@AttachmentId", attachmentId, DbType.Int32, ParameterDirection.Input);
            var bankLetterResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", ap, "Vendor-DeleteAttachments");
        }

        var p = new DynamicParameters();
        p.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
        p.Add("@BusinessUnitId", 1, DbType.Int32, ParameterDirection.Input);
        p.Add("@VendorName", vendor.VendorName, DbType.String, ParameterDirection.Input);
        p.Add("@TaxId", vendor.TaxId, DbType.String, ParameterDirection.Input);
        p.Add("@Location", vendor.CompanyLocation, DbType.String, ParameterDirection.Input);
        p.Add("@Website", vendor.CompanyWebsite, DbType.String, ParameterDirection.Input);
        //p.Add("@RepresentedProducts", vendor.RepresentedProducts, DbType.String, ParameterDirection.Input);
        //p.Add("@RepresentedCompanies", vendor.RepresentedCompanies, DbType.String, ParameterDirection.Input);
        p.Add("@PaymentTerms", vendor.PaymentTermsCode, DbType.String, ParameterDirection.Input);
        p.Add("@CreditDays", vendor.CreditDays, DbType.Int32, ParameterDirection.Input);
        p.Add("@_0DaysPayment", vendor.AgreeWithDefaultDays, DbType.Boolean, ParameterDirection.Input);
        p.Add("@Country", vendor.CountryCode, DbType.String, ParameterDirection.Input);
        p.Add("@UserId", currentUser.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@OtherProducts", vendor.OtherProducts, DbType.String, ParameterDirection.Input);
        p.Add("@NewVendorId", DbType.Int32, direction: ParameterDirection.Output);
        supplierResult = await _sqlDataAccess.ExecuteSql("dbo.SP_Vendors_IUD", p, "Vendor-Save");

        if (supplierResult != null && supplierResult.QueryResultMessage == null)
        {
            vendor.VendorId = p.Get<int>("@NewVendorId");
            supplierResult.QueryResult = vendor.VendorId;

            if (vendor.ProvidedProductIdList != null && vendor.ProvidedProductIdList.Any())
            {
                var pdp = new DynamicParameters();
                pdp.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
                var productsDeleteResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorProductServices_ID", pdp, "Vendor-SaveDeleteProducts");

                foreach (var itemId in vendor.ProvidedProductIdList)
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
            
        }
        return supplierResult;
    }

    public async Task<SqlResult?> SendToApproval(List<Vendor> vendorList, int approveStageMainId)
    {
        var sqlResult = new SqlResult();
        foreach (var vendor in vendorList)
        {
            var p = new DynamicParameters();
            p.Add("@VendorId", vendor.VendorId, DbType.Int32, ParameterDirection.Input);
            p.Add("@ApproveStageMainId", approveStageMainId, DbType.Int32, ParameterDirection.Input);
            sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_VendorSendToApprove", p, "Vendor-SendToApproval");
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