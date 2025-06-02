using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Carousel;

namespace Soditech.IntelPrev.Mobile.Views.Components.Carousel
{
    public partial class CarouselComponent : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty = 
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<SfCarouselItem>), typeof(CarouselComponent), null, 
                propertyChanged: OnItemsSourceChanged);

        public static readonly BindableProperty PositionProperty =
            BindableProperty.Create(nameof(Position), typeof(int), typeof(CarouselComponent), 0,
                propertyChanged: OnPositionChanged);

        public static readonly BindableProperty CurrentItemChangedCommandProperty =
            BindableProperty.Create(nameof(CurrentItemChangedCommand), typeof(ICommand), typeof(CarouselComponent), null);

        public IEnumerable<SfCarouselItem> ItemsSource
        {
            get => (IEnumerable<SfCarouselItem>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public int Position
        {
            get => (int)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public ICommand CurrentItemChangedCommand
        {
            get => (ICommand)GetValue(CurrentItemChangedCommandProperty);
            set => SetValue(CurrentItemChangedCommandProperty, value);
        }

        public CarouselComponent()
        {
            InitializeComponent();
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CarouselComponent carousel && newValue is IEnumerable<SfCarouselItem> items)
            {
                carousel.carouselView.ItemsSource = items;
            }
        }

        private static void OnPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is CarouselComponent carousel && newValue is int position)
            {
                carousel.carouselView.Position = position;
            }
        }

        private void CarouselView_OnCurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (CurrentItemChangedCommand?.CanExecute(e) == true)
            {
                CurrentItemChangedCommand.Execute(e);
            }
        }
    }
}
