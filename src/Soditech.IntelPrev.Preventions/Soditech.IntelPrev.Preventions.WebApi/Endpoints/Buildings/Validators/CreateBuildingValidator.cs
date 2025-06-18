using FastEndpoints;
using FluentValidation;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;

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
