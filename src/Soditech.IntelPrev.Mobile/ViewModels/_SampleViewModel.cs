using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Users.Shared;

namespace Soditech.IntelPrev.Mobile.ViewModels;

public class _SampleViewModel : MauiViewModel
{
	private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

	private string _text = "Hello World!";
	public string Text
	{
		get => _text;
		set
		{
			if (_text == value) return;
			_text = value;
			OnPropertyChanged();
		}
	}

	public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());
	/// <inheritdoc />
	public override async Task InitializeAsync()
	{
		var countResult = await _proxyClientService.GetAsync<int>(UserRoutes.Users.Count);
		if (countResult.IsSuccess)
		{
			Text = $"Hello World! there is {countResult.Value} user(s) in the system";
		}
		else
		{
			Text = $"Hello World! [{countResult.Error.Code}] Failed to get the count of users : {countResult.Error.Message}";
		}
	}
}