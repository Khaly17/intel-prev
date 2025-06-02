using FastEndpoints;
using Soditech.IntelPrev.Preventions.Shared.Buildings;
using FluentValidation;

namespace Soditech.IntelPrev.Preventions.WebApi.Endpoints.Buildings.Validators;

public class CreateBuildingValidator : Validator<CreateBuildingCommand>
{
    public CreateBuildingValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Le nom du bâtiment est obligatoire.");

        RuleFor(x => x.Description)
            .MaximumLength(5)
            .WithMessage("La description ....");
    }
}
