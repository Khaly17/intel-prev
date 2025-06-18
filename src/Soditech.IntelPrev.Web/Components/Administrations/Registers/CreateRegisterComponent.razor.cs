using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Reports.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterFielGroups;

namespace Soditech.IntelPrev.Web.Components.Administrations.Registers;

public partial class CreateRegisterComponent : ComponentBase
{

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    [Inject] private ILogger<CreateRegisterComponent> Logger { get; set; } = default!;
    [Parameter]
    public RegisterTypeResult RegisterTypeResult { get; set; } = default!;

    private bool _isCreating = false;

    [Parameter]
    public EventCallback<bool> OnIsCreatingChanged { get; set; }

    private void ToggleIsCreating()
    {
        _isCreating = !_isCreating;
        OnIsCreatingChanged.InvokeAsync(_isCreating);
    }

    private void AddNewGroup()
    {
        RegisterTypeResult.RegisterFieldGroups.Add(new RegisterFieldGroupResult
        {
            Id = Guid.NewGuid(),
            RegisterFields = []
        });
    }
    private static void AddNewFieldToGroup(RegisterFieldGroupResult group)
    {
        group.RegisterFields.Add(new RegisterFieldResult
        {
            Id = Guid.NewGuid(),
        });
    }
    
    private void RemoveGroup(RegisterFieldGroupResult group)
    {
        RegisterTypeResult.RegisterFieldGroups.Remove(group);
    }
    
    private void AddNewField()
    {
        RegisterTypeResult.RegisterFields.Add(new RegisterFieldResult
        {
            Id = Guid.NewGuid(),
        });
    }
    
    private void RemoveField(RegisterFieldResult field)
    {
        RegisterTypeResult.RegisterFields.Remove(field);
    }

    private static void HandleValidSubmit()
    {
        // Handle the form submission logic
        Console.WriteLine("Form submitted successfully!");


    }

    private async Task CreateRegister()
    {
        ErrorMessage = null;
        SuccessMessage = null;
        try
        {
            var path = ReportRoutes.RegisterTypes.Create;
            var result = await ProxyService.PostAsync<RegisterTypeResult>(path, RegisterTypeResult);

            if (result.IsSuccess)
            {
                SuccessMessage = "Le registre a été ajouté avec succès !";



                Navigation.NavigateTo("/registers");
            }
            else
            {
                ErrorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du registre.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = "Une erreur interne est survenue lors de la création du registre.";
            Logger.LogError(ex, ErrorMessage);
        }
    }
  
    public void GoToPrevious()
    {
        Navigation.NavigateTo("/previous");
    }
}