using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Reports.Shared.RegisterFieldGroups;
using Soditech.IntelPrev.Reports.Shared.RegisterFields;
using Soditech.IntelPrev.Reports.Shared.RegisterTypes;
using Soditech.IntelPrev.Web.Models.Utils;
using Soditech.IntelPrev.Reports.Shared;

namespace Soditech.IntelPrev.Web.Components.Administrations.Registers;

public partial class CreateRegisterComponent : ComponentBase
{

    public string? errorMessage { get; set; }
    public string? successMessage { get; set; }
    [Inject] private ILogger<CreateRegisterComponent> Logger { get; set; } = default!;
    [Parameter]
    public RegisterTypeResult RegisterTypeResult { get; set; } = default!;

    private bool isCreating = false;

    [Parameter]
    public EventCallback<bool> OnIsCreatingChanged { get; set; }

    private void ToggleIsCreating()
    {
        isCreating = !isCreating;
        OnIsCreatingChanged.InvokeAsync(isCreating);
    }

    private void AddNewGroup()
    {
        RegisterTypeResult.RegisterFieldGroups.Add(new RegisterFieldGroupResult
        {
            Id = Guid.NewGuid(),
            RegisterFields = []
        });
    }
    private void AddNewFieldToGroup(RegisterFieldGroupResult group)
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

    private void HandleValidSubmit()
    {
        // Handle the form submission logic
        Console.WriteLine("Form submitted successfully!");


    }

    private async Task CreateRegister()
    {
        errorMessage = null;
        successMessage = null;
        try
        {
            var path = ReportRoutes.RegisterTypes.Create;
            var result = await ProxyService.PostAsync<RegisterTypeResult>(path, RegisterTypeResult);

            if (result.IsSuccess)
            {
                successMessage = "Le registre a été ajouté avec succès !";
                var registerId = result.Value.Id;
                 


                Navigation.NavigateTo("/registers");
            }
            else
            {
                errorMessage = result.Error?.Message ?? "Une erreur est survenue lors de la création du registre.";
                Logger.LogError("{code} : {message}", result.Error?.Code, result.Error?.Message);
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Une erreur interne est survenue lors de la création du registre.";
            Logger.LogError(ex, errorMessage);
        }
    }
  
    public void GoToPrevious()
    {
        Navigation.NavigateTo("/previous");
    }
}