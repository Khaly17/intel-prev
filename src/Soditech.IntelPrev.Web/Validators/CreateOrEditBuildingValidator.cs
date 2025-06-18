using FluentValidation;
using Soditech.IntelPrev.Prevensions.Shared.Buildings;

namespace Soditech.IntelPrev.Web.Validators;

public class CreateOrEditBuildingValidator : AbstractValidator<BuildingResult>  
{
    public CreateOrEditBuildingValidator() 
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom du bâtiment est obligatoire.");
        RuleFor(x => x.Description)
            .MaximumLength(5)
            .WithMessage("La description ....");
    }
}
