using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Controls.UserDialogs.Maui;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Mobile.Views.Gps;
using Soditech.IntelPrev.Preventions.Shared.Materials;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Soditech.IntelPrev.Mobile.ViewModels.Gps;

public partial class EquipmentTypeSelectionViewModel : MauiViewModel
{
	private EquipmentTypeItem? _selectedEquipmentType;

	public EquipmentTypeItem? SelectedEquipmentType
	{
		get => _selectedEquipmentType;
		set => SetProperty(ref _selectedEquipmentType, value);
	}

	private ObservableCollection<EquipmentTypeItem> _equipmentTypes = new ObservableCollection<EquipmentTypeItem>();

	public ObservableCollection<EquipmentTypeItem> EquipmentTypes
	{
		get => _equipmentTypes;
		set => SetProperty(ref _equipmentTypes, value);
	}

	public ICommand PageAppearingCommand => new AsyncRelayCommand((Func<Task>)InitializeAsync);
	public ICommand EquipmentLocationTrackerAsyncCommand => new AsyncRelayCommand<MaterialType>(EquipmentLocationTrackerAsync);
	public ICommand SelectionChangedCommand => new AsyncRelayCommand<EquipmentTypeItem?>(HandleEquipmentTypeSelection);

	public override async Task InitializeAsync()
	{
		IsBusy = true;

		// Populate equipment types if not already done
		if (EquipmentTypes.Count == 0)
		{
			await Task.Run(() => PopulateEquipmentTypes());
		}

		IsBusy = false;
	}

	private async Task HandleEquipmentTypeSelection(EquipmentTypeItem? item)
	{
		if (item != null)
		{
			await EquipmentLocationTrackerAsync(item.Type);

			// Reset selection after navigation
			await Task.Delay(300);
			SelectedEquipmentType = null;
		}
	}

	private void PopulateEquipmentTypes()
	{
		EquipmentTypes.Clear();

		// Add equipment type items - using known values from the MaterialType enum
		EquipmentTypes.Add(new EquipmentTypeItem
		{
			Type = MaterialType.Extinguisher,
			Name = "Extincteur",
			Description = "Localiser les extincteurs du bâtiment",
			Icon = "extinguisher_icon.png"
		});

		EquipmentTypes.Add(new EquipmentTypeItem
		{
			Type = MaterialType.Dae,
			Name = "Défibrillateur",
			Description = "Localiser les défibrillateurs du bâtiment",
			Icon = "defibrillator_icon.png"
		});

		EquipmentTypes.Add(new EquipmentTypeItem
		{
			Type = MaterialType.AssemblyPoint,
			Name = "Point de rassemblement",
			Description = "Localiser les points de rassemblement du bâtiment",
			Icon = "assembly_point_icon.png"
		});

		// Add more equipment types as needed
	}

	private async Task EquipmentLocationTrackerAsync(MaterialType type)
	{
		await Shell.Current.GoToAsync($"{nameof(EquipmentLocationTrackerView)}",
			new Dictionary<string, object>
			{
				{ "MaterialType", type }
			});
	}
}

// Model class for equipment types
public class EquipmentTypeItem
{
	public MaterialType Type { get; set; }
	public required string Name { get; set; }
	public required string Description { get; set; }
	public required string Icon { get; set; }
}
