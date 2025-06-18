using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.Models.Materials;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Mobile.Views.Gps;

namespace Soditech.IntelPrev.Mobile.ViewModels.Gps;

public partial class GpsMainViewModel : MauiViewModel
{
	private EquipmentItem? _selectedEquipment;

	public EquipmentItem? SelectedEquipment
	{
		get => _selectedEquipment;
		set => SetProperty(ref _selectedEquipment, value);
	}

	public ObservableCollection<EquipmentItem> EquipmentItems { get; } = new();

	public ICommand PageAppearingCommand => new AsyncRelayCommand((Func<Task>)InitializeAsync);
	public ICommand EquipmentSelectionCommand => new AsyncRelayCommand<EquipmentItem?>(HandleEquipmentSelection);
	public ICommand GoToAddEquipmentCommandCommand => new AsyncRelayCommand(GoToAddEquipmentCommand);

	public override async Task InitializeAsync()
	{
		IsBusy = true;

		// Initialize equipment items if not already populated
		if (EquipmentItems.Count == 0)
		{
			await Task.Run(PopulateEquipmentItems);
		}

		IsBusy = false;
	}

	private void PopulateEquipmentItems()
	{
		EquipmentItems.Clear();

		// Add equipment items with their respective icons and descriptions
		EquipmentItems.Add(new EquipmentItem
		{
			Type = MaterialType.Extinguisher,
			Title = "Extincteurs",
			Description = "Localiser les extincteurs du bâtiment",
			Icon = "extinguisher_icon.png"
		});

		EquipmentItems.Add(new EquipmentItem
		{
			Type = MaterialType.Dae,
			Title = "Défibrillateurs",
			Description = "Localiser les défibrillateurs du bâtiment",
			Icon = "defibrillator_icon.png"
		});

		EquipmentItems.Add(new EquipmentItem
		{
			Type = MaterialType.AssemblyPoint,
			Title = "Points de rassemblement",
			Description = "Localiser les points de rassemblement",
			Icon = "assembly_point_icon.png"
		});

	}

	private async Task HandleEquipmentSelection(EquipmentItem? item)
	{
		if (item != null)
		{
			await ShowMaterialLocations(item.Type);

			// Reset selection after navigation
			await Task.Delay(300);
			SelectedEquipment = null;
		}
	}

	private async Task GoToAddEquipmentCommand()
	{
		await Shell.Current.GoToAsync(nameof(EquipmentTypeSelectionView));
	}

	private async Task ShowMaterialLocations(MaterialType materialType)
	{
		await Shell.Current.GoToAsync($"{nameof(MaterialLocationView)}",
			new Dictionary<string, object>
			{
				{ "MaterialType", materialType }
			});
	}
}

// Model class for equipment items
public class EquipmentItem
{
	public MaterialType Type { get; set; }
	public required string Title { get; set; }
	public required string Description { get; set; }
	public required string Icon { get; set; }
}
