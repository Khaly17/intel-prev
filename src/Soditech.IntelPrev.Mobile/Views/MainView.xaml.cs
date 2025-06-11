using Syncfusion.Maui.Carousel;

namespace Soditech.IntelPrev.Mobile.Views;

public partial class MainView : ContentPage, IXamarinView
{
	public MainView()
	{
		InitializeComponent();
	}

	private void CarouselView_OnCurrentItemChanged(object? sender, CurrentItemChangedEventArgs e)
	{
		// SfCarouselItem? previousItem = e.PreviousItem as SfCarouselItem;
		// SfCarouselItem? currentItem = e.CurrentItem as SfCarouselItem;
	}
}
