using System;
using System.Globalization;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile.ViewModels.Base;

public static class ViewManager
{
    public static readonly BindableProperty AutoWireViewModelProperty =
        BindableProperty.CreateAttached(
            "AutoWireViewModel",
            typeof(bool),
            typeof(object),
            // typeof(ApplicationBootstrapper),
            default(bool),
            propertyChanged: OnAutoWireViewModelChanged
        );

    public static bool GetAutoWireViewModel(BindableObject bindable)
    {
        return (bool)bindable.GetValue(AutoWireViewModelProperty);
    }

    public static void SetAutoWireViewModel(BindableObject bindable, bool value)
    {
        bindable.SetValue(AutoWireViewModelProperty, value);
    }

    private static void OnAutoWireViewModelChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not Element view)
        {
            return;
        }

        var viewType = view.GetType();
        var viewName = viewType.FullName?.Replace(".Views.", ".ViewModels.");
        var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
        var viewModelName = string.Format(CultureInfo.InvariantCulture, "{0}Model, {1}", viewName, viewAssemblyName);

        var viewModelType = Type.GetType(viewModelName);
        if (viewModelType == null)
        {
            return;
        }

        var serviceProvider = IPlatformApplication.Current?.Services;

        if (serviceProvider is null)
        {
            // Throws exceptions
            return;
        }

        view.BindingContext = serviceProvider.GetRequiredService(viewModelType);
            
    }
}