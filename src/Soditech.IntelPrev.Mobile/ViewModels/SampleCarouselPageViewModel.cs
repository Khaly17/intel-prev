using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Syncfusion.Maui.Carousel;

namespace Soditech.IntelPrev.Mobile.ViewModels
{
	public class SampleCarouselPageViewModel : MauiViewModel
	{
		private List<SfCarouselItem> _carouselItems;
		private string _currentImageName;

		public SampleCarouselPageViewModel()
		{
			// Initialize carousel items
			_carouselItems = new List<SfCarouselItem>
			{
				new SfCarouselItem() { ImageName = "sensibilisation1.png" },
				new SfCarouselItem() { ImageName = "sensibilisation2.png" },
				new SfCarouselItem() { ImageName = "sensibilisation3.png" }
			};

			// Set default image name
			CurrentImageName = "sensibilisation1.png";
		}

		public List<SfCarouselItem> CarouselItems
		{
			get => _carouselItems;
			set => SetProperty(ref _carouselItems, value);
		}

		public string CurrentImageName
		{
			get => _currentImageName;
			set => SetProperty(ref _currentImageName, value);
		}

		public ICommand CarouselItemChangedCommand => new RelayCommand<CurrentItemChangedEventArgs>(OnCarouselItemChanged);
		public ICommand GoBackCommand => new AsyncRelayCommand(GoBackAsync);

		private void OnCarouselItemChanged(CurrentItemChangedEventArgs args)
		{
			if (args.CurrentItem is SfCarouselItem currentItem)
			{
				CurrentImageName = currentItem.ImageName;
			}
		}

		private async Task GoBackAsync()
		{
			await Shell.Current.GoToAsync("..");
		}
	}
}
