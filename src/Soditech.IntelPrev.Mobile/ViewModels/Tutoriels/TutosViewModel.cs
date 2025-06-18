using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Proxy;
using Soditech.IntelPrev.Mediatheques.Shared.Documents;

namespace Soditech.IntelPrev.Mobile.ViewModels.Tutoriels;

public class Video
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Thumbnail { get; set; } // URL or file path for the thumbnail
    public string VideoUrl { get; set; }
}
public partial class TutosViewModel : MauiViewModel
{
    private IEnumerable<Video> _videos = [];
    public IEnumerable<Video> Videos
    {
        get => _videos;
        set => SetProperty(ref _videos, value );

    }

    private Video _selectedVideo = default!;
    public Video SelectedVideo
    {
        get => _selectedVideo;
        set => SetProperty( ref _selectedVideo, value );
    }

    private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

    public ICommand PageAppearingCommand => new AsyncRelayCommand(async () => await InitializeAsync());

    // Adding the PlayVideoCommand for use with the SelectableListView
    [RelayCommand]
    private async Task PlayVideo()
    {
        if (SelectedVideo != null)
        {
            // Navigate to the VideoPlayerPage and pass the video URL
            await Shell.Current.GoToAsync(new ShellNavigationState(AppRoutes.VideoPlayerPage),
                new ShellNavigationQueryParameters
                {
                    { "video", SelectedVideo }
                });
        }
    }

    /// <inheritdoc />
    public override async Task InitializeAsync()
    {
        IsBusy = true;
        var tutosResult = await _proxyClientService.GetAsync<IEnumerable<DocumentResult>>(MediathequeRoutes.Documents.GetAllByType);

        if (tutosResult.IsSuccess)
        {
            Videos = tutosResult.Value.Select(t => new Video()
            {
                Id = t.Id,
                Title = t.Name,
                VideoUrl = t.Path,
                Thumbnail = "tutoriels_videos.png" // Default thumbnail
            });

        }

        IsBusy = false;
    }
}