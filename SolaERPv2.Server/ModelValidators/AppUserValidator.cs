namespace SolaERPv2.Server.ModelValidators;
using Model = AppUser;

public class AppUserValidator : AbstractValidator<Model>
{
    AppUserService _appUserService;
    public AppUserValidator(AppUserService appUserService)
    {
        _appUserService = appUserService;

        RuleSet(nameof(Model.FullName), () =>
        {
            RuleFor(p => p.FullName)
                .NotEmpty().WithMessage("You must enter your name")
                .MaximumLength(50).WithMessage("Max length 50 characters");
        });

        //RuleSet(nameof(Model.Email), () =>
        //{
        //    RuleFor(p => p.Email)
        //        .NotEmpty().WithMessage("You must enter an email address")
        //        .EmailAddress().WithMessage("You must provide a valid email address")
        //        .MustAsync(async (email, cancellationToken) => await IsEmailUniqueAsync(email)).WithMessage("Email address must be unique").When(p => !string.IsNullOrEmpty(p.Email));
        //});
    }

    async Task<bool> IsEmailUniqueAsync(string email)
    {
        return await _appUserService.IsEmailUniqueAsync(email);
    }
}
