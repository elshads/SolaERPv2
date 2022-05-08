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
                .NotEmpty().WithMessage("This field is mandatory");
        });

        RuleSet(nameof(Model.VendorName), () =>
        {
            RuleFor(p => p.VendorName)
                .NotEmpty().WithMessage("This field is mandatory")
                .MaximumLength(50).WithMessage("Max length 50 characters");
        });

        //RuleSet(nameof(Model.TaxId), () =>
        //{
        //    RuleFor(p => p.TaxId)
        //        .NotEmpty().WithMessage("This field is mandatory")
        //        .MustAsync(async (taxId, cancellationToken) => await IsVendorUniqueAsync(taxId)).WithMessage("Email address must be unique").When(p => !string.IsNullOrEmpty(p.TaxId));
        //});
    }

    async Task<bool> IsVendorUniqueAsync(string taxId)
    {
        return await _vendorService.IsVendorUniqueAsync(taxId);
    }
}
