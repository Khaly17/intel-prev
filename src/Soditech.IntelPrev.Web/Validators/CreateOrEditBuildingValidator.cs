using FluentValidation;
using Soditech.IntelPrev.Preventions.Shared.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
