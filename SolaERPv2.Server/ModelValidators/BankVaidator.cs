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
    }
}