using System.Collections;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile.Views.Components;

public partial class SelectableListView : ContentView
{
    public SelectableListView()
    {
        InitializeComponent();
    }

    #region Bindable Properties

    public static readonly BindableProperty MainMessageProperty =
        BindableProperty.Create(nameof(MainMessage), typeof(string), typeof(SelectableListView),
            defaultValue: "Sélectionnez un élément");

    public static readonly BindableProperty SubMessageProperty =
        BindableProperty.Create(nameof(SubMessage), typeof(string), typeof(SelectableListView),
            defaultValue: "Veuillez faire votre sélection");

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IEnumerable),
        typeof(SelectableListView),
        defaultValue: null);

    public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
        nameof(SelectedItem),
        typeof(object),
        typeof(SelectableListView),
        defaultValue: null,
        defaultBindingMode: BindingMode.TwoWay,
        propertyChanged: OnSelectedItemChanged);

    private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is SelectableListView view && view.SelectionChangedCommand != null && newValue != null)
        {
            if (view.SelectionChangedCommand.CanExecute(newValue))
            {
                view.SelectionChangedCommand.Execute(newValue);
            }
        }
    }

    public static readonly BindableProperty IsBusyProperty = BindableProperty.Create(
        nameof(IsBusy),
        typeof(bool),
        typeof(SelectableListView),
        defaultValue: false);

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate),
        typeof(DataTemplate),
        typeof(SelectableListView),
        defaultValue: null);

    public static readonly BindableProperty SelectionChangedCommandProperty = BindableProperty.Create(
        nameof(SelectionChangedCommand),
        typeof(ICommand),
        typeof(SelectableListView),
        defaultValue: null);

    public static readonly BindableProperty IsSelectableProperty = BindableProperty.Create(
        nameof(IsSelectable), 
        typeof(bool), 
        typeof(SelectableListView), 
        true);

    public bool IsSelectable
    {
        get => (bool)GetValue(IsSelectableProperty);
        set => SetValue(IsSelectableProperty, value);
    }

    public string MainMessage
    {
        get => (string)GetValue(MainMessageProperty);
        set => SetValue(MainMessageProperty, value);
    }

    public string SubMessage
    {
        get => (string)GetValue(SubMessageProperty);
        set => SetValue(SubMessageProperty, value);
    }

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set => SetValue(IsBusyProperty, value);
    }

    public DataTemplate ItemTemplate
    {
        get => (DataTemplate)GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }

    public ICommand SelectionChangedCommand
    {
        get => (ICommand)GetValue(SelectionChangedCommandProperty);
        set => SetValue(SelectionChangedCommandProperty, value);
    }

    public bool IsNotBusy => !IsBusy;

    #endregion

    // Refresh IsNotBusy when IsBusy changes
    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == nameof(IsBusy))
            OnPropertyChanged(nameof(IsNotBusy));

        if (propertyName == nameof(IsSelectable))
        {
            if (!IsSelectable)
            {
                ItemsCollectionView.SelectionChangedCommand = null;
                ItemsCollectionView.SelectedItem = null;
            }
        }
    }
}