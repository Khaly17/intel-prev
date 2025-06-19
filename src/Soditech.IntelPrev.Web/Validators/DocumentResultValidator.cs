using FluentValidation;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;
using System.Collections.Generic;

namespace Soditech.IntelPrev.Web.Validators;

public class DocumentResultValidator : AbstractValidator<DocumentResult>
{
    public DocumentResultValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom du document est obligatoire.")
            .MaximumLength(200).WithMessage("Le nom du document ne doit pas dépasser 200 caractères.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("La description du document ne doit pas dépasser 1000 caractères.");

        RuleFor(x => x.BlobFile)
            .NotEmpty().WithMessage("Le fichier Blob est obligatoire.")
            .Must(BeAValidBlobFile).WithMessage("Le fichier Blob doit être valide et non vide.");

        RuleFor(x => x.Path)
            .NotEmpty().WithMessage("Le chemin du fichier est obligatoire.")
            .MaximumLength(500).WithMessage("Le chemin du fichier ne doit pas dépasser 500 caractères.");

        RuleFor(x => x.Extension)
            .NotEmpty().WithMessage("L'extension du fichier est obligatoire.")
            .MaximumLength(10).WithMessage("L'extension du fichier ne doit pas dépasser 10 caractères.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Le type de document est obligatoire.")
            .MaximumLength(50).WithMessage("Le type de document ne doit pas dépasser 50 caractères.");

        RuleFor(x => x.FileType)
            .NotEmpty().WithMessage("Le type de fichier est obligatoire.")
            .MaximumLength(50).WithMessage("Le type de fichier ne doit pas dépasser 50 caractères.");

    }

    private bool BeAValidBlobFile(ICollection<byte> blobFile)
    {
        return blobFile != null && blobFile.Count > 0;
    }
}