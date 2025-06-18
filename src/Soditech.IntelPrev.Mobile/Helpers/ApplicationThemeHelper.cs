using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace Soditech.IntelPrev.Mobile.Helpers;

/// <summary>
/// Fournit des méthodes pour gérer le thème global de l'application
/// </summary>
public static class ApplicationThemeHelper
{
	/// <summary>
	/// Applique le thème IntelPrev à l'application
	/// </summary>
	public static void ApplyIntelPrevTheme()
	{
		// S'assurer que les ressources sont chargées
		if (Application.Current?.Resources == null)
			return;

		// Mettre à jour les couleurs de la barre de navigation
		UpdateNavigationBarColors();

		// Mettre à jour les couleurs principales de Shell
		UpdateShellColors();

		// S'abonner à l'événement de changement de thème si nécessaire
		Application.Current.RequestedThemeChanged += (s, e) =>
		{
			// Rappliquer le thème lorsque le thème système change
			UpdateNavigationBarColors();
			UpdateShellColors();
		};
	}

	/// <summary>
	/// Met à jour les couleurs de la barre de navigation
	/// </summary>
	private static void UpdateNavigationBarColors()
	{
		if (Application.Current?.Resources == null)
			return;

		// Mettre à jour les ressources pour les couleurs de navigation
		Application.Current.Resources["NavigationBarBackgroundColor"] = ThemeHelper.DeepSkyBlue;
		Application.Current.Resources["NavigationBarTextColor"] = Colors.White;

	}

	/// <summary>
	/// Met à jour les couleurs principales de Shell
	/// </summary>
	private static void UpdateShellColors()
	{
		if (Application.Current?.Resources == null)
			return;

		// Mettre à jour les couleurs de Shell via ResourceDictionary
		// Ces couleurs sont définies dans Styles.xaml et appliquées via le style Shell
		Application.Current.Resources["ShellBackgroundColor"] = ThemeHelper.PureWhite;
		Application.Current.Resources["ShellForegroundColor"] = ThemeHelper.CharcoalGray;
		Application.Current.Resources["ShellTitleColor"] = ThemeHelper.DeepSkyBlue;
		Application.Current.Resources["ShellDisabledColor"] = ThemeHelper.LightGray;
		Application.Current.Resources["ShellUnselectedColor"] = ThemeHelper.CharcoalGray;

		// Couleurs de la barre d'onglets
		Application.Current.Resources["ShellTabBarBackgroundColor"] = ThemeHelper.PureWhite;
		Application.Current.Resources["ShellTabBarForegroundColor"] = ThemeHelper.DeepSkyBlue;
		Application.Current.Resources["ShellTabBarTitleColor"] = ThemeHelper.DeepSkyBlue;
		Application.Current.Resources["ShellTabBarUnselectedColor"] = ThemeHelper.CharcoalGray;

		// Forcer la mise à jour du style Shell
		if (Shell.Current != null)
		{
			// Applique les changements de couleur en mettant à jour le BindingContext
			// Cette technique permet de forcer un refresh de l'UI
			var currentBindingContext = Shell.Current.BindingContext;
			Shell.Current.BindingContext = currentBindingContext;
		}
	}

	/// <summary>
	/// Obtient la couleur appropriée en fonction du AppTheme courant
	/// </summary>
	public static Microsoft.Maui.Graphics.Color GetAppThemeColor(
		Microsoft.Maui.Graphics.Color light,
		Microsoft.Maui.Graphics.Color dark)
	{
		return Application.Current?.RequestedTheme == AppTheme.Dark ? dark : light;
	}
}