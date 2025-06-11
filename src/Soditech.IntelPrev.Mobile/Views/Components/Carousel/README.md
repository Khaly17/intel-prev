# Reusable Carousel Component

This component provides a reusable carousel that can be used across multiple pages in the application.

## Features

- Displays a collection of images in a carousel format
- Customizable position
- Event handling for item changes
- Indicator view for current position
- Consistent styling across the application

## Usage

### XAML

1. Add the namespace to your XAML:

```xml
xmlns:components="clr-namespace:Soditech.IntelPrev.Mobile.Views.Components.Carousel"
```

2. Add the component to your page:

```xml
<components:CarouselComponent
    ItemsSource="{Binding CarouselItems}"
    Position="0"
    CurrentItemChangedCommand="{Binding CarouselItemChangedCommand}"/>
```

### ViewModel

1. Define a collection of `SfCarouselItem` objects:

```csharp
private List<SfCarouselItem> _carouselItems = [
    new SfCarouselItem() { ImageName = "image1.jpg" },
    new SfCarouselItem() { ImageName = "image2.jpg" },
    new SfCarouselItem() { ImageName = "image3.jpg" }
];

public List<SfCarouselItem> CarouselItems
{
    get => _carouselItems;
    set => SetProperty(ref _carouselItems, value);
}
```

2. Create a command to handle item changes:

```csharp
public ICommand CarouselItemChangedCommand => new RelayCommand<CurrentItemChangedEventArgs>(OnCarouselItemChanged);

private void OnCarouselItemChanged(CurrentItemChangedEventArgs args)
{
    SfCarouselItem? previousItem = args.PreviousItem as SfCarouselItem;
    SfCarouselItem? currentItem _ = args.CurrentItem as SfCarouselItem;
    // Add your logic here
}
```

## Properties

| Property | Type | Description |
| --- | --- | --- |
| ItemsSource | IEnumerable<SfCarouselItem> | The collection of items to display in the carousel |
| Position | int | The starting position/index of the carousel |
| CurrentItemChangedCommand | ICommand | Command that is executed when the current item changes |

## Example

See `SampleCarouselPage.xaml` and `SampleCarouselPageViewModel.cs` for a complete example of how to use this component.
