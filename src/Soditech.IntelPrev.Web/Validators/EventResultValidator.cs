using System;
using FluentValidation;
using Soditech.IntelPrev.Prevensions.Shared.Events;

namespace Soditech.IntelPrev.Web.Validators;

public class EventResultValidator : AbstractValidator<EventResult>
    {
        public EventResultValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Le nom de l'événement est obligatoire.")
                .MaximumLength(200).WithMessage("Le nom de l'événement ne doit pas dépasser 200 caractères.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("La description de l'événement ne doit pas dépasser 1000 caractères.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("La date de début est obligatoire.")
                .GreaterThanOrEqualTo(DateTimeOffset.UtcNow.Date).WithMessage("La date de début doit être aujourd'hui ou une date ultérieure.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("La date de fin est obligatoire.")
                .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("La date de fin doit être postérieure ou égale à la date de début.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("L'heure de début est obligatoire.")
                .Must(BeAValidTime).WithMessage("L'heure de début doit être une heure valide.");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithMessage("L'heure de fin est obligatoire.")
                .Must(BeAValidTime).WithMessage("L'heure de fin doit être une heure valide.")
                .GreaterThanOrEqualTo(x => x.StartTime).WithMessage("L'heure de fin doit être postérieure ou égale à l'heure de début.");

            RuleFor(x => x.Location)
                .NotEmpty().WithMessage("Le lieu de l'événement est obligatoire.")
                .MaximumLength(200).WithMessage("Le lieu de l'événement ne doit pas dépasser 200 caractères.");

            RuleFor(x => x.OrganizerId)
                .NotEmpty().WithMessage("L'ID de l'organisateur est obligatoire.");

            RuleFor(x => x.OrganizerName)
                .NotEmpty().WithMessage("Le nom de l'organisateur est obligatoire.")
                .MaximumLength(100).WithMessage("Le nom de l'organisateur ne doit pas dépasser 100 caractères.");

        }

        private bool BeAValidTime(DateTime time)
        {
            return time != default;
        }
    }
