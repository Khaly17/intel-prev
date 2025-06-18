using System;
using FluentValidation;
using Soditech.IntelPrev.Prevensions.Shared.Campaigns;

namespace Soditech.IntelPrev.Web.Validators;

public class CampaignResultValidator : AbstractValidator<CampaignResult>
{
    public CampaignResultValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom de la campagne est obligatoire.")
            .MaximumLength(100).WithMessage("Le nom de la campagne ne doit pas dépasser 100 caractères.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La description de la campagne ne doit pas dépasser 500 caractères.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("La date de début est obligatoire.")
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow.Date).WithMessage("La date de début doit être aujourd'hui ou une date ultérieure.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("La date de fin est obligatoire.")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("La date de fin doit être postérieure ou égale à la date de début.");

    }
}