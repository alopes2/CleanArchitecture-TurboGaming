using FluentValidation;
using TurboGaming.Api.Resources;

namespace TurboGaming.Api.Validators;

public class SaveGameResourceValidator : AbstractValidator<SaveGameResource>
{
    public SaveGameResourceValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");

        RuleFor(r => r.PublisherId)
            .NotEmpty()
            .WithMessage("PublisherId must not be empty.");
    }
}
