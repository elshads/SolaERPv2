namespace SolaERPv2.Server.ModelService;

public class PaymentDocumentService : BaseModelService<PaymentDocumentMain>
{
    AppUserService _appUserService;
    SqlDataAccess _sqlDataAccess;
    public PaymentDocumentService(AppUserService appUserService, SqlDataAccess sqlDataAccess)
    {
        _appUserService = appUserService;
        _sqlDataAccess = sqlDataAccess;
    }

    public async Task<IEnumerable<PaymentDocumentMain>?> GetAll(int businessUnitId, int tabindex)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        var sql = "";
        switch (tabindex)
        {
            case 0:
                sql = "dbo.SP_PaymentDocumentWFA";
                break;
            case 1:
                sql = "dbo.SP_PaymentDocumentDrafts";
                break;
            case 2:
                sql = "dbo.SP_PaymentDocumentAll";
                break;
            case 3:
                sql = "dbo.SP_PaymentDocumentApproved";
                break;
            case 4:
                sql = "dbo.SP_PaymentDocumentBank";
                break;
            default:
                sql = "dbo.SP_PaymentDocumentAll";
                break;
        }

        var p = new DynamicParameters();
        p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
        p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<PaymentDocumentMain>(sql, p, "PD-GetAll");
    }

    public async Task<PaymentDocumentMain?> GetById(int modelId)
    {
        PaymentDocumentMain? result = new();

        var p = new DynamicParameters();
        p.Add("@PaymentDocumentMainId", modelId, DbType.Int32, ParameterDirection.Input);
        result = await _sqlDataAccess.QuerySingle<PaymentDocumentMain>("dbo.SP_PaymentDocumentMainLoad", p, "PD-GetById1");

        if (result?.PaymentDocumentMainId > 0)
        {
            var pd = new DynamicParameters();
            pd.Add("@PaymentDocumentMainId", modelId, DbType.Int32, ParameterDirection.Input);
            var details = await _sqlDataAccess.QueryAll<PaymentDocumentDetail>("dbo.SP_PaymentDocumentDetailsLoad", pd, "PD-GetById2");
            result.PaymentDocumentDetailList = details?.ToList();

            var pa = new DynamicParameters();
            pa.Add("@SourceId", modelId, DbType.Int32, ParameterDirection.Input);
            pa.Add("@SourceType", "PYMDC", DbType.String, ParameterDirection.Input);
            pa.Add("@Reference", null, DbType.String, ParameterDirection.Input);
            var attachments = await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_AttachmentList_Load", pa, "PD-GetById3");
            result.AttachmentList = attachments?.ToList();
        }

        return result;
    }

    public async Task<PaymentDocumentMain?> GetPost(int id)
    {
        PaymentDocumentMain? result = new();

        var p = new DynamicParameters();
        p.Add("@PaymentDocumentMainId", id, DbType.Int32, ParameterDirection.Input);
        result = await _sqlDataAccess.QuerySingle<PaymentDocumentMain>("dbo.SP_PaymentDocumentMainLoad", p, "PD-GetPost1");

        if (result?.PaymentDocumentMainId > 0)
        {
            var pd = new DynamicParameters();
            pd.Add("@PaymentDocumentMainId", id, DbType.Int32, ParameterDirection.Input);
            var details = await _sqlDataAccess.QueryAll<PaymentDocumentDetail>("dbo.SP_PaymentDocumentDetailsLoad", pd, "PD-GetPost2");
            result.PaymentDocumentDetailList = details?.ToList();
        }

        return result;
    }

    public async Task<SqlResult?> PostDocument(PaymentDocumentMain model)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        SqlResult? result = new();
        try
        {
            foreach (var item in model.PaymentDocumentDetailList)
            {
                using IDbConnection cn = new SqlConnection(_sqlDataAccess.ConnectionString);
                var p = new DynamicParameters();
                p.Add("@PaymentDocumentDetailId", item.PaymentDocumentDetailId, DbType.Int32, ParameterDirection.Input);
                p.Add("@Date", model.PaymentDate, DbType.DateTime, ParameterDirection.Input);
                p.Add("@BankCode", model.BankCode, DbType.String, ParameterDirection.Input);
                p.Add("@BankAmount", item.BankAmount, DbType.Decimal, ParameterDirection.Input);
                p.Add("@VendorAmount", item.VendorAmount, DbType.Decimal, ParameterDirection.Input);
                p.Add("@BankRate", item.BankRate, DbType.Decimal, ParameterDirection.Input);
                p.Add("@VendorRate", item.VendorRate, DbType.Decimal, ParameterDirection.Input);
                p.Add("@VAT", item.VAT, DbType.Decimal, ParameterDirection.Input);
                p.Add("@VATBankAmount", item.VATBankAmount, DbType.Decimal, ParameterDirection.Input);
                p.Add("@VATBank", model.VATBankCode, DbType.String, ParameterDirection.Input);
                p.Add("@BankCharge", model.BankCharge, DbType.Decimal, ParameterDirection.Input);
                p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
                p.Add("@ExpenseCode", model.ExpenseCode, DbType.String, ParameterDirection.Input);
                p.Add("@GroupProject", model.GroupProject, DbType.String, ParameterDirection.Input);
                p.Add("@Project", model.Project, DbType.String, ParameterDirection.Input);
                await cn.QueryAsync("dbo.SP_PaymentDocumentPost", p, commandType: CommandType.StoredProcedure);
            }
        }
        catch (Exception e)
        {
            result.QueryResultMessage = e.Message;
        }
        return result;
    }


    public async Task<IEnumerable<PaymentDocumentDetail>?> GetVendorBalance(int businessUnitId, string vendorCode)
    {
        var p = new DynamicParameters();
        p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);
        p.Add("@VendorCode", vendorCode, DbType.String, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<PaymentDocumentDetail>("dbo.SP_PaymentDocumentVendorBalance", p, "PD-GetVendorBalance");
    }

    public async Task<IEnumerable<PaymentDocumentDetail>?> GetVendorDetails(int businessUnitId, string vendorCode, string currencyCode, int paymentType)
    {
        var p = new DynamicParameters();
        p.Add("@BusinessUnitId", businessUnitId, DbType.Int32, ParameterDirection.Input);
        p.Add("@VendorCode", vendorCode, DbType.String, ParameterDirection.Input);
        p.Add("@CurrencyCode", currencyCode, DbType.String, ParameterDirection.Input);
        p.Add("@DocumentType", paymentType, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<PaymentDocumentDetail>("dbo.SP_PaymentDocumentVendorDetails", p, "PD-GetVendorDetails");
    }

    public async Task<SqlResult?> Save(PaymentDocumentMain paymentDocumentMain)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        SqlResult? result = new();
        using (var cn = new SqlConnection(_sqlDataAccess.ConnectionString))
        {
            var p = new DynamicParameters();
            p.Add("@PaymentDocumentMainId", paymentDocumentMain.PaymentDocumentMainId, DbType.Int32, ParameterDirection.Input);
            p.Add("@Reference", paymentDocumentMain.Reference, DbType.String, ParameterDirection.Input);
            p.Add("@VendorCode", paymentDocumentMain.VendorCode, DbType.String, ParameterDirection.Input);
            p.Add("@CurrencyCode", paymentDocumentMain.CurrencyCode, DbType.String, ParameterDirection.Input);
            p.Add("@Comment", paymentDocumentMain.Comment, DbType.String, ParameterDirection.Input);
            p.Add("@AdvanceOrder", paymentDocumentMain.PaymentDocumentTypeId, DbType.Int32, ParameterDirection.Input);
            p.Add("@Status", paymentDocumentMain.StatusId, DbType.Int32, ParameterDirection.Input);
            p.Add("@PaymentDocumentTypeId", paymentDocumentMain.PaymentDocumentTypeId, DbType.Int32, ParameterDirection.Input);
            p.Add("@BusinessUnitId", paymentDocumentMain.BusinessUnitId, DbType.Int32, ParameterDirection.Input);
            p.Add("@PaymentDocumentPriorityId", paymentDocumentMain.PaymentDocumentPriorityId, DbType.Int32, ParameterDirection.Input);
            p.Add("@ExpenseCode", paymentDocumentMain.ExpenseCode, DbType.String, ParameterDirection.Input);
            p.Add("@GroupProject", paymentDocumentMain.GroupProject, DbType.String, ParameterDirection.Input);
            p.Add("@Project", paymentDocumentMain.Project, DbType.String, ParameterDirection.Input);
            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
            p.Add("@NewPaymentDocumentMainId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            result.QueryResult = await cn.ExecuteAsync("dbo.SP_PaymentDocumentMain_IUD", p, commandType: CommandType.StoredProcedure);
            result.QueryResult = p.Get<int>("@NewPaymentDocumentMainId");
        }


        if (result.QueryResult > 0 && paymentDocumentMain.PaymentDocumentDetailList.Any())
        {
            foreach (var item in paymentDocumentMain.PaymentDocumentDetailList)
            {
                var pd = new DynamicParameters();
                pd.Add("@PaymentDocumentDetailId", item.PaymentDocumentDetailId, DbType.Int32, ParameterDirection.Input);
                pd.Add("@PaymentDocumentMainId", (paymentDocumentMain.PaymentDocumentMainId > 0 ? paymentDocumentMain.PaymentDocumentMainId : result.QueryResult), DbType.Int32, ParameterDirection.Input);
                pd.Add("@OrderNumber", item.PONum, DbType.String, ParameterDirection.Input);
                pd.Add("@Voucher", item.Voucher, DbType.Int32, ParameterDirection.Input);
                pd.Add("@Amount", item.AmountToPay, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@VAT", item.VATToPay, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@POAmount", item.POAmount, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@PO_VAT", item.PO_VAT, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@VoucherAmount", item.VoucherAmount, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@VoucherVAT", item.VoucherVAT, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@PaidAmount", item.PaidAmount, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@PaidVAT", item.PaidVAT, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@PaymentDocumentAmount", item.PaymentDocumentAmount, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@PaymentDocumentVat", item.PaymentDocumentVAT, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@AdvanceAmount", item.AdvanceAmount, DbType.Decimal, ParameterDirection.Input);
                pd.Add("@AdvanceVAT", item.AdvanceVAT, DbType.Decimal, ParameterDirection.Input);

                await _sqlDataAccess.ExecuteSql("dbo.SP_PaymentDocumentDetails_IUD", pd, "PD-SaveDetails");
            }
        }

        if (result.QueryResult > 0 && paymentDocumentMain.AttachmentList.Any())
        {
            foreach (var item in paymentDocumentMain.AttachmentList)
            {
                var pa = new DynamicParameters();
                pa.Add("@AttachmentId", item.AttachmentId, DbType.Int32, ParameterDirection.Input);
                pa.Add("@FileName", item.FileName, DbType.String, ParameterDirection.Input);
                pa.Add("@FileData", item.FileData, DbType.Binary, ParameterDirection.Input);
                pa.Add("@SourceId", (paymentDocumentMain.PaymentDocumentMainId > 0 ? paymentDocumentMain.PaymentDocumentMainId : result.QueryResult), DbType.Int32, ParameterDirection.Input);
                pa.Add("@SourceType", "PYMDC", DbType.String, ParameterDirection.Input);
                pa.Add("@Reference", item.Reference, DbType.String, ParameterDirection.Input);
                pa.Add("@ExtensionType", item.ExtensionType, DbType.String, ParameterDirection.Input);
                pa.Add("@AttachmentTypeId", item.AttachmentTypeId, DbType.Int32, ParameterDirection.Input);
                pa.Add("@AttachmentSubTypeId", item.AttachmentSubTypeId, DbType.Int32, ParameterDirection.Input);
                await _sqlDataAccess.ExecuteSql("dbo.SP_Attachments_IUD", pa, "PD-SaveAttachments");
            }
        }
        return result;
    }

    public async Task<SqlResult?> Approve(IEnumerable<ApproveData> approveDataList)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        SqlResult? result = new();
        foreach (var item in approveDataList)
        {
            var p = new DynamicParameters();
            p.Add("@PaymentDocumentMainId", item.ModelId, DbType.Int32, ParameterDirection.Input);
            p.Add("@ApproveStatusId", item.ApproveStatusId, DbType.Int32, ParameterDirection.Input);
            p.Add("@Comment", item.Comment, DbType.String, ParameterDirection.Input);
            p.Add("@Sequence", item.Sequence, DbType.Int32, ParameterDirection.Input);
            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
            result = await _sqlDataAccess.ExecuteSql("dbo.SP_PaymentDocumentsApprove", p, "PD-Approve");
        }
        return result;
    }

    public async Task<SqlResult?> ChangeApproveStatus(IEnumerable<ApproveData> statusDataList)
    {
        var user = await _appUserService.GetCurrentUserAsync();
        SqlResult? result = new();
        foreach (var item in statusDataList)
        {
            var p = new DynamicParameters();
            p.Add("@PaymentDocumentMainId", item.ModelId, DbType.Int32, ParameterDirection.Input);
            p.Add("@Status", item.ApproveStatusId, DbType.Int32, ParameterDirection.Input);
            p.Add("@UserId", user.Id, DbType.Int32, ParameterDirection.Input);
            result = await _sqlDataAccess.ExecuteSql("dbo.SP_PaymentDocumentsChangeStatus", p, "PD-ChangeApproveStatus");
        }
        return result;
    }

    public async Task<IEnumerable<Attachment>?> GetLinkedAttachments(int paymentDocumentMainId, int attachmentTypeId)
    {
        // attachmentTypeId
        // 0 = Request
        // 1 = Bid
        // 2 = Order
        // 3 = GRN
        // 4 = Invoice
        var p = new DynamicParameters();
        p.Add("@PaymentDocumentMainId", paymentDocumentMainId, DbType.Int32, ParameterDirection.Input);
        p.Add("@AttachmentType", attachmentTypeId, DbType.Int32, ParameterDirection.Input);
        return await _sqlDataAccess.QueryAll<Attachment>("dbo.SP_PaymentDocumentsOtherAttachments", p, "PD-GetLinkedAttachments");
    }

    public async Task<SqlResult?> Delete(IEnumerable<int> paymentDocumentMainIdList)
    {
        SqlResult? sqlResult = new();
        foreach (var item in paymentDocumentMainIdList)
        {
            var p = new DynamicParameters();
            p.Add("@PaymentDocumentMainId", item, DbType.Int32, ParameterDirection.Input);
            p.Add("@NewPaymentDocumentMainId", dbType: DbType.Int32, direction: ParameterDirection.Output);
            sqlResult = await _sqlDataAccess.ExecuteSql("dbo.SP_PaymentDocumentMain_IUD", p, "PD-Delete");
            if (sqlResult.QueryResultMessage != null) { return sqlResult; }
        }
        return sqlResult;
    }
}