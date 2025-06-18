using System;
using FluentValidation;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;

namespace Soditech.IntelPrev.Web.Validators;

public class EquipmentResultValidator : AbstractValidator<EquipmentResult>
{
    public EquipmentResultValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom de l'équipement est obligatoire.")
            .MaximumLength(200).WithMessage("Le nom de l'équipement ne doit pas dépasser 200 caractères.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("La description de l'équipement ne doit pas dépasser 1000 caractères.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Le type de l'équipement est obligatoire.")
            .MaximumLength(100).WithMessage("Le type de l'équipement ne doit pas dépasser 100 caractères.");

        RuleFor(x => x.LastInspectionDate)
            .NotEmpty().WithMessage("La date de la dernière inspection est obligatoire.")
            .LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("La date de la dernière inspection ne peut pas être dans le futur.");

        RuleFor(x => x.NextInspectionDate)
            .NotEmpty().WithMessage("La date de la prochaine inspection est obligatoire.")
            .GreaterThanOrEqualTo(x => x.LastInspectionDate).WithMessage("La date de la prochaine inspection doit être postérieure ou égale à la date de la dernière inspection.");

        RuleFor(x => x.BuildingName)
            .NotEmpty().WithMessage("Le nom du bâtiment est obligatoire.")
            .MaximumLength(200).WithMessage("Le nom du bâtiment ne doit pas dépasser 200 caractères.");

        RuleFor(x => x.FloorNumber)
            .GreaterThanOrEqualTo(0).When(x => x.FloorNumber.HasValue)
            .WithMessage("Le numéro d'étage doit être supérieur ou égal à 0.");

    }
}