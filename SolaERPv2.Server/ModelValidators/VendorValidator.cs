namespace SolaERPv2.Server.ModelValidators;
using Model = Vendor;

public class VendorValidator : AbstractValidator<Model>
{
    VendorService _vendorService;
    public VendorValidator(VendorService vendorService)
    {
        _vendorService = vendorService;

        RuleSet(nameof(Model.TaxId), () =>
        {
            RuleFor(p => p.TaxId)
                .NotEmpty().WithMessage("'Tax ID' is mandatory");
        });

        RuleSet(nameof(Model.VendorName), () =>
        {
            RuleFor(p => p.VendorName)
                .NotEmpty().WithMessage("'Name' is mandatory")
                .MaximumLength(50).WithMessage("Max length 50 characters");
        });

        RuleSet(nameof(Model.CountryCode), () =>
        {
            RuleFor(p => p.CountryCode)
                .NotEmpty().WithMessage("'Country' is mandatory");
        });

        RuleSet(nameof(Model.CompanyAddress), () =>
        {
            RuleFor(p => p.CompanyAddress)
                .NotEmpty().WithMessage("'Company Address' is mandatory");
        });

        RuleSet(nameof(Model.CompanyRegistrationDate), () =>
        {
            RuleFor(p => p.CompanyRegistrationDate)
                .NotEmpty().WithMessage("'Registration Date' is mandatory");
        });

        RuleSet(nameof(Model.CompanyLocation), () =>
        {
            RuleFor(p => p.CompanyLocation)
                .NotEmpty().WithMessage("'Location' is mandatory");
        });

        RuleSet(nameof(Model.ProvidedProductIdList), () =>
        {
            RuleFor(p => p.ProvidedProductIdList)
            .NotEmpty().WithMessage("Choose at least one item");
        });

        RuleSet(nameof(Model.PaymentTermsCode), () =>
        {
            RuleFor(p => p.PaymentTermsCode)
            .NotEmpty().WithMessage("'Payment Terms' is mandatory");
        });

        RuleSet(nameof(Model.CreditDays), () =>
        {
            RuleFor(p => p.CreditDays)
            .GreaterThan(0)
            .WithMessage("Numeric value must be more than 0");
        });

        //RuleSet(nameof(Model.TaxId), () =>
        //{
        //    RuleFor(p => p.TaxId)
        //        .NotEmpty().WithMessage("This field is mandatory")
        //        .MustAsync(async (taxId, cancellationToken) => await IsVendorUniqueAsync(taxId)).WithMessage("Email address must be unique").When(p => !string.IsNullOrEmpty(p.TaxId));
        //});
    }

    //async Task<bool> IsVendorUniqueAsync(string taxId)
    //{
    //    return await _vendorService.IsVendorUniqueAsync(taxId);
    //}
}
