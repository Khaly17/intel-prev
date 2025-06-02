using Microsoft.AspNetCore.Components;
using Soditech.IntelPrev.Web.Services.Authentications;
using Soditech.IntelPrev.Web.Services.UserInfo;

using Microsoft.JSInterop;
using Syncfusion.Blazor.Buttons;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;

namespace Soditech.IntelPrev.Web.Shared.Headers;

public partial class Header : IDisposable
{
    public string FullName { get; set; } = "Abdou DIOP";
    public string TenantName { get; set; } = "Tenant Name";
    [Inject] private IAuthenticationServices AuthenticationServices { get; set; } = default!;
    [Inject] private IUserInfoService UserInfoService { get; set; } = default!;
    [Parameter] public bool OnToggledSidebar { get; set; }
    [Parameter] public EventCallback<bool> OnToggledSidebarChanged { get; set; }
    private string GetNotificationCountStyle() => Notifications.Count > 0 ? "display: block;" : "display: none;";
    private ILogger<Header> Logger { get; set; } = default!;
    [Inject] private ICookieStorageAccessor CookieStorageAccessor { get; set; } = default!;


    private bool IsDropdownVisible { get; set; } = false;
    [Inject] private IConfiguration Configuration { get; set; } = default!;

    private HubConnection _hubConnection;

    public List<string> Notifications { get; set; } = new List<string>{};
    private bool IsNotificationVisible { get; set; } = false;

    private DotNetObjectReference<Header>? _dotNetRef;

    private void ToggleNotifications() => IsNotificationVisible = !IsNotificationVisible;
    private string GetNotificationStyle() => IsNotificationVisible ? "display: block;" : "display: none;";

    private async Task OnValueChange(ChangeEventArgs<bool> args)
    {
        await OnToggledSidebarChanged.InvokeAsync(args.Checked);
        await Js.InvokeVoidAsync("eval", "window.dispatchEvent(new Event('resize'));");
        // set isSidebarOpened value in cookie
        await CookieStorageAccessor.SetValueAsync("isSidebarOpened", args.Checked);
    }

    private void ToggleDropdown() => IsDropdownVisible = !IsDropdownVisible;

    private string GetDropdownStyle() => IsDropdownVisible ? "display: block;" : "display: none;";

    private async Task Logout()
    {
        await AuthenticationServices.Logout();
    }

    private async Task ToggleFullscreen()
    {
        await Js.InvokeVoidAsync("toggleFullScreenMode");
    }

    protected override async Task OnInitializedAsync()
    {
        var baseUrl = Configuration["HostApi:BaseUri"];
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(baseUrl + "/notifications")
            .WithAutomaticReconnect()
            .Build();

        await SetSidebarBehavior();

        _hubConnection.On<string>("ReceiveNotification", async (message) =>
        {
            
            Notifications.Add(message);
            await Js.InvokeVoidAsync("playNotificationSound");
            StateHasChanged();
        });

        try
        {
            await _hubConnection.StartAsync();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erreur lors de la connexion au hub SignalR.");
        }
        await GetUserInfo();
    }

    private async Task SetSidebarBehavior()
    {
        //get isSidebarOpened value from cookie
        var isSidebarOpened = await CookieStorageAccessor.GetValueAsync<string>("isSidebarOpened");
        if (string.IsNullOrEmpty(isSidebarOpened))
        {
            OnToggledSidebar = true;
            await CookieStorageAccessor.SetValueAsync("isSidebarOpened", true, 525600);
        }
        else
        {
            OnToggledSidebar = bool.Parse(isSidebarOpened);
        }

        await OnToggledSidebarChanged.InvokeAsync(OnToggledSidebar);
    }

    private async Task GetUserInfo()
    {

        var userResult = await UserInfoService.GetUserInfoAsync();

        if (userResult.IsSuccess)
        {
            FullName = userResult.Value.FullName;
            TenantName = userResult.Value.TenantName;
        }
    }

    private void RemoveNotification(string notification)
    {
        Notifications.Remove(notification);
        StateHasChanged();
    }

    private string GetInitials()
    {
        if (string.IsNullOrWhiteSpace(FullName)) return "";

        var names = FullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (names.Length == 1)
            return names[0].Substring(0, 1).ToUpper();
        else
            return (names[0].Substring(0, 1) + names[^1].Substring(0, 1)).ToUpper();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _dotNetRef = DotNetObjectReference.Create(this);
            await Js.InvokeVoidAsync("registerClickOutsideMultiple", _dotNetRef, new[] { ".user-info", ".notifications" });
        }
    }


    [JSInvokable]
    public void CloseAllDropdowns()
    {
        bool stateChanged = false;

        if (IsDropdownVisible)
        {
            IsDropdownVisible = false;
            stateChanged = true;
        }

        if (IsNotificationVisible)
        {
            IsNotificationVisible = false;
            stateChanged = true;
        }

        if (stateChanged)
        {
            StateHasChanged();
        }
    }


    public void Dispose()
    {
        _dotNetRef?.Dispose();
    }

}
