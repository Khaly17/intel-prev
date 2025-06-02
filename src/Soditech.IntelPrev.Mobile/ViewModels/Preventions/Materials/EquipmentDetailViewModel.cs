using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Preventions.Shared.Equipments;

namespace Soditech.IntelPrev.Mobile.ViewModels.Preventions.Materials;

class EquipmentDetailViewModel : MauiViewModel, IQueryAttributable
{
    private EquipmentResult _equipment;
    public EquipmentResult Equipment
    {
        get => _equipment;
        set => SetProperty(ref _equipment, value);
    }

    /// <inheritdoc />
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("Equipment", out var equipment))
        {
            Equipment = (EquipmentResult)equipment;
        }
    }
}