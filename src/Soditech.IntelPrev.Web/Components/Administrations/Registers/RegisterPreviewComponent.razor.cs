using System;
using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Reports.Shared.Reports;

namespace Soditech.IntelPrev.Web.Components.Administrations.Registers;

public partial class RegisterPreviewComponent : ComponentBase
{
    [Parameter] public RegisterTypeResult RegisterType { get; set; } = default!;
    private CreateReportCommand _report = default!;

    private DateTimeOffset _dateTest = DateTimeOffset.UtcNow;
    private string _textTest = string.Empty;
    private int _numberTest ;
    private bool _booleanTest;
    private bool _isCreating = false;
    [Parameter]
    public string SaveBtnLabel { get; set; } = "Enregistrer";

    [Parameter]
    public EventCallback<bool> OnIsCreatingChanged { get; set; }
    [Parameter]
    public EventCallback<bool> OnSaveChanged { get; set; }

    private void SaveRegister()
    {
        OnSaveChanged.InvokeAsync();
    }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        _report = new CreateReportCommand(RegisterType);
    }

    private void ToPrevious()
    {
        OnIsCreatingChanged.InvokeAsync(false);
    }
}

