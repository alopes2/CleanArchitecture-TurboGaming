using FluentValidation;
using TurboGaming.Api.Resources;

namespace TurboGaming.Api.Validators;

public class SavePublisherResourceValidator : AbstractValidator<SavePublisherResource>
{
    public SavePublisherResourceValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty.")
            .MaximumLength(100)
            .WithMessage("Name must not exceed 100 characters.");
    }
}
