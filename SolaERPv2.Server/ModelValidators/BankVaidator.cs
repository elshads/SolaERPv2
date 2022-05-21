namespace SolaERPv2.Server.ModelValidators;
using Model = Bank;

public class BankValidator : AbstractValidator<Model>
{
    public BankValidator()
    {
        RuleSet(nameof(Model.BankAccountNumber), () =>
        {
            RuleFor(p => p.BankAccountNumber)
                .NotEmpty().WithMessage("'Bank Account Number' is mandatory");
        });

        RuleSet(nameof(Model.BeneficiaryFullName), () =>
        {
            RuleFor(p => p.BeneficiaryFullName)
                .NotEmpty().WithMessage("'Beneficiary FullName' is mandatory");
        });

        RuleSet(nameof(Model.BeneficiaryAddress), () =>
        {
            RuleFor(p => p.BeneficiaryAddress)
                .NotEmpty().WithMessage("'Beneficiary Address' is mandatory");
        });

        RuleSet(nameof(Model.BeneficiaryAddress1), () =>
        {
            RuleFor(p => p.BeneficiaryAddress1)
                .NotEmpty().WithMessage("'Beneficiary Address 1' is mandatory");
        });

        RuleSet(nameof(Model.BeneficiaryBankName), () =>
        {
            RuleFor(p => p.BeneficiaryBankName)
                .NotEmpty().WithMessage("'Beneficiary Bank Name' is mandatory");
        });

        RuleSet(nameof(Model.BeneficiaryBankAddress), () =>
        {
            RuleFor(p => p.BeneficiaryBankAddress)
                .NotEmpty().WithMessage("'Beneficiary Bank Address' is mandatory");
        });

        RuleSet(nameof(Model.BeneficiaryBankAddress1), () =>
        {
            RuleFor(p => p.BeneficiaryBankAddress1)
                .NotEmpty().WithMessage("'Beneficiary Bank Address 1' is mandatory");
        });

        RuleSet(nameof(Model.BeneficiaryBankCode), () =>
        {
            RuleFor(p => p.BeneficiaryBankCode)
                .NotEmpty().WithMessage("'Beneficiary Bank Code' is mandatory");
        });

        RuleSet(nameof(Model.IntermediaryBankCodeNumber), () =>
        {
            RuleFor(p => p.IntermediaryBankCodeNumber)
                .NotEmpty().WithMessage("'Intermediary Bank Code Number' is mandatory");
        });

        RuleSet(nameof(Model.IntermediaryBankCodeType), () =>
        {
            RuleFor(p => p.IntermediaryBankCodeType)
                .NotEmpty().WithMessage("'Intermediary Bank Code Type' is mandatory");
        });
    }
}