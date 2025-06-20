using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Navigations;
using Soditech.IntelPrev.Web.Events;
using Soditech.IntelPrev.Web.Services.UserInfo;
using System.Globalization;

namespace Soditech.IntelPrev.Web.Shared.SideBars;

public partial class SideBar
{
    // auth provider
    [Inject] public AuthenticationStateProvider AuthStateProvider { get; set; } = default!;

    // userState
    private AuthenticationState UserState { get; set; } = default!;

    [Inject]
    public SiteEventService SiteEventService { get; set; } = default!;
    [Inject] private IUserInfoService UserInfoService { get; set; } = default!;

    private bool _isLoading = true;
    private List<SidebarSection> Sections { get; set; } = [];

    [Parameter] public bool SidebarToggle { get; set; } = true;
    [Parameter] public EventCallback<bool> SidebarToggleChanged { get; set; }
    private string _activeUrl;

    private bool IsActive(string url) => _activeUrl == url;
    private bool IsTenant { get; set; }
    private void ToggleSection(SidebarSection section)
    {
        section.IsExpanded = !section.IsExpanded;
        StateHasChanged();
    }

    private void ToggleSidebar()
    {
        SidebarToggle = !SidebarToggle;
        StateHasChanged();
    }

    private void SetActive(string url)
    {
        _activeUrl = url;
        Navigation.NavigateTo(url);
    }

    private static string GetFontAwesomeIcon(string iconClass)
    {
        return iconClass switch
        {
            "home" => "fa-home",
            "site" => "fa-chart-simple",
            "profile" => "fa-user",
            "info" => "fa-info-circle",
            "settings" => "fa-cog",
            "archive" => "fa-archive",
            "hard-hat" => "fa-hard-hat",
            "grid" => "fa-th",
            "store-alt" => "fa-store-alt",
            "guitar" => "fa-guitar",
            "sidebar" => "fa-sliders-h",
            "sitemap" => "fa-tasks",
            "city" => "fa-city",
            "album" => "fa-album",
            "docs" => "fa-file-alt",
            "building" => "fa-building", 
            "tools" => "fa-tools", 
            "calendar" => "fa-calendar", 
            "users" => "fa-users", 
            _ => "fa-archive" 
        };
    }
    protected override void OnInitialized()
    {
        _activeUrl = Navigation.Uri.Replace(Navigation.BaseUri, "/");
 
        _ = InitializeSectionsAsync();

        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadCountsAsync();
        StateHasChanged();
        _isLoading = false;

    }

    protected override Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            SiteEventService.OnSiteChanged += async () => await InvokeAsync(StateHasChanged);
        }

        return Task.CompletedTask;
    }

    private async Task InitializeSectionsAsync()
    {
        var userResult = await UserInfoService.GetUserInfoAsync();

        if (userResult.IsSuccess && !string.IsNullOrEmpty(userResult.Value.TenantId))
        {
            IsTenant = true;
        }
        Sections =
        [
            new SidebarSection
            {
                Name = "Tableau de bord",
                IconClass = "home",
                Items = [
                    new SidebarItem { Url = "/", Name = "Table de bord" },
                ]
            },
        ];

        var userState = await AuthStateProvider.GetAuthenticationStateAsync();

        if (userState.User.IsInRole("Admin") || userState.User.IsInRole("Super Admin"))
        {
            List<SidebarItem> items =
            [
                new SidebarItem
                {
                    Key = "usersCount", Url = "/users", Name = "Utilisateurs ", IconClass = "profile",
                    IsHost = true, IsTenant = IsTenant
                },
                new SidebarItem
                {
                    Key = "rolesCount", Url = "/roles", Name = "Rôles ", IconClass = "info", IsHost = true,
                    IsTenant = IsTenant
                },
                // new SidebarItem
                //     { Key = "sitesCount", Url = "/maintenance", Name = "Sites ", IconClass = "site", IsTenant = IsTenant },
                new SidebarItem
                {
                    Key = "tenantesCount", Url = "/tenants", Name = "Structures ", IconClass = "tenant",
                    IsHost = true
                },
                new SidebarItem
                {
                    Key = "documentsCount", Url = "/documents", Name = "Documents ", IconClass = "docs",
                    IsTenant = IsTenant
                },
                new SidebarItem
                {
                    Key = "buildingsCount", Url = "/buildings", Name = "Batiments ", IconClass = "building",
                    IsTenant = IsTenant
                },
                new SidebarItem
                {
                    Key = "compaignsCount", Url = "/campaigns", Name = "Campagnes ", IconClass = "docs",
                    IsTenant = IsTenant
                },
                new SidebarItem
                {
                    Key = "medicalsCount", Url = "/medicalcontacts", Name = "Contact Médical ", IconClass = "contact",
                    IsTenant = IsTenant
                },
                new SidebarItem
                {
                    Key = "eventsCount", Url = "/agenda", Name = "Agenda ", IconClass = "calendar",
                    IsTenant = IsTenant
                },
                new SidebarItem
                {
                    Key = "committeeMembersCount", Url = "/committeemembers", Name = "Membres du Comité ", IconClass = "users",
                    IsTenant = IsTenant
                },
                new SidebarItem
                {
                    Key = "registerTypesCount", Url = "/registers", Name = "Registres ", IconClass = "registers",
                    IsTenant = true
                },
            ];
                
            var userInfoResult = await UserInfoService.GetUserInfoAsync();
            if (userInfoResult.IsSuccess)
            {
                var tenantId = userInfoResult.Value.TenantId;
    
                items = !string.IsNullOrEmpty(tenantId) ? items.Where(i => i.IsTenant).ToList() : items.Where(i => i.IsHost).ToList();
            }
                
            Sections.Add(new SidebarSection(name: "Administration",
                iconClass: "settings",
                items: items));
        }
            
        List<SidebarItem> proPrevItems = [
            new SidebarItem { Url = "/proprev/protocol", Name = "Analyse des risques", IconClass = "file-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/tools", Name = "Outils d’analyse", IconClass = "tools", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/organization", Name = "Organisation", IconClass = "tasks", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/visits", Name = "Visites des sites", IconClass = "map-marker-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/agenda", Name = "Agenda du CSE", IconClass = "calendar-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/safety-minute", Name = "Quart d’heure sécurité", IconClass = "clock", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/epi-control", Name = "Contrôle des EPI", IconClass = "shield-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/safety-data-sheets", Name = "FDS", IconClass = "file-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/first-aid", Name = "Premiers secours", IconClass = "medkit", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/training", Name = "Sécurité au travail", IconClass = "chalkboard-teacher", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/proprev/library", Name = "Ma bibliothèque", IconClass = "book", IsHost = true, IsTenant = IsTenant },
        ];

        Sections.Add(new SidebarSection(name: "Pro-Prev",
            iconClass: "hard-hat",
            items: proPrevItems));
            
        List<SidebarItem> preventionItems = [
            new SidebarItem { Url = "/prev/prevention", Name = "Definition", IconClass = "file-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/prev/sensibilisation", Name = "Sensibilisation", IconClass = "book", IsHost = true, IsTenant = IsTenant },
        ];

        Sections.Add(new SidebarSection(name: "Prévention",
            iconClass: "hard-hat",
            items: preventionItems));
            
        List<SidebarItem> fireSecurityItems = [
            new SidebarItem { Url = "/firesecurity/definition", Name = "Definition", IconClass = "file-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/firesecurity/evacuation", Name = "Evacuation", IconClass = "file-alt", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/firesecurity/consign", Name = "Consigne", IconClass = "book", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/firesecurity/material", Name = "Material", IconClass = "book", IsHost = true, IsTenant = IsTenant },
            new SidebarItem { Url = "/firesecurity/knownmyentreprise", Name = "Mon entreprise", IconClass = "book", IsHost = true, IsTenant = IsTenant },
        ];

        Sections.Add(new SidebarSection(name: "Sécurité incendie",
            iconClass: "hard-hat",
            items: fireSecurityItems));
        StateHasChanged();

    }

    private async Task LoadCountsAsync()
    {
        var loadTasks = new List<Task>
        {
            //LoadSitesAsync(),
        };

        var userState = await AuthStateProvider.GetAuthenticationStateAsync();

        if (userState.User.IsInRole("Admin") || userState.User.IsInRole("Super Admin"))
        {
               
        }

        await Task.WhenAll(loadTasks);

    }

    public static string ConvertToTitleCase(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        var cultureInfo = new CultureInfo("fr-FR", false);
        return cultureInfo.TextInfo.ToTitleCase(input.ToLower(cultureInfo));
    }

}