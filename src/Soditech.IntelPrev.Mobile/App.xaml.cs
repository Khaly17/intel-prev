using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Soditech.IntelPrev.Mobile.Services.Settings;
using Soditech.IntelPrev.Mobile.Helpers;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile;

public partial class App : Application
{
	private readonly ISettingsManager? _settingsManager;

	public App(IServiceProvider serviceProvider)
	{
		InitializeComponent();

		// Appliquer le thème IntelPrev
		ApplicationThemeHelper.ApplyIntelPrevTheme();

		// Get the settings manager from the service provider
		_settingsManager = serviceProvider.GetService<ISettingsManager>();

		// Apply settings asynchronously
	}

	private async Task ApplySettingsAsync()
	{
		// Apply all app settings at startup
		if (_settingsManager != null)
		{
			await _settingsManager.ApplySettings();
		}
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		ApplySettingsAsync();

		var window = new Window(new AppShell());
		
		// Appliquer les paramètres de thème après la création de la fenêtre
		// pour s'assurer que Shell.Current est disponible
		window.Created += (sender, args) => MainThread.BeginInvokeOnMainThread(() => ApplicationThemeHelper.ApplyIntelPrevTheme());
		
		return window;
	}
}