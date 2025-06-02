using Microsoft.AspNetCore.Components;

namespace Soditech.IntelPrev.Web.Components.Widgets.Dialogs;

public partial class GenericConfirmDialog : ComponentBase
{

    [Parameter] public bool IsVisible { get; set; }
    [Parameter] public EventCallback<bool> IsVisibleChanged { get; set; }
    [Parameter] public string HeaderLabel { get; set; } = "Confirmer la suppression";
    [Parameter] public string Message { get; set; } = "Êtes-vous sûr de vouloir supprimer cet élément ?";
    [Parameter] public string ConfirmLabel { get; set; } = "Supprimer";
    [Parameter] public string CancelLabel { get; set; } = "Annuler";
    [Parameter] public EventCallback OnConfirm { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }

    private void Cancel()
    {
        OnCancel.InvokeAsync();
        IsVisibleChanged.InvokeAsync(false);
    }
}