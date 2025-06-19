using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Prevensions.Shared.Equipments;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Materials;

class EquipmentLocationUpdateViewModel : MauiViewModel, IQueryAttributable
{
    private bool _isCheckingLocation;
    private Location? _currentLocation;

    private EquipmentResult _equipment ;
    public EquipmentResult Equipment
    {
        get => _equipment;
        set => SetProperty(ref _equipment, value);
    }
    
    

    /// <inheritdoc />
    public void ApplyQueryAttributesAsync(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Equipment", out var equipment))
        {
            Equipment = (EquipmentResult)equipment;
        }
    }
}