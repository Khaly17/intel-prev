using System.Collections.Generic;
using System.Windows.Input;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Soditech.IntelPrev.Mediatheques.Shared;
using Soditech.IntelPrev.Mobile.Core.Dependency;
using Soditech.IntelPrev.Mobile.ViewModels.Base;
using Soditech.IntelPrev.Proxy;

namespace Soditech.IntelPrev.Mobile.ViewModels.Tutoriels
{
	public class VideoPlayerViewModel : MauiViewModel, IQueryAttributable

	{
        private MediaSource? _videoSource;
        public MediaSource? VideoSource
        {
            get => _videoSource;
            set => SetProperty(ref _videoSource, value);
        }

		private string _videoUrl = string.Empty;
		public string VideoUrl
		{
			get => _videoUrl;
			set
			{
				if (_videoUrl == value) return;

				_videoUrl = value;
				OnPropertyChanged(nameof(VideoSource));
			}
		}

		private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);

        }

        private readonly IProxyService _proxyClientService = DependencyResolver.GetRequiredService<IProxyService>();

        public async Task PlayVideo(string path)
        {
            // set the path of the video file in the cache
            var tempFilePath = Path.Combine(FileSystem.CacheDirectory, path);

            // check if the file exists
            if (File.Exists(tempFilePath))
            {
                // if the file exists, read the video from the file
                VideoSource = MediaSource.FromFile(tempFilePath);
            }
            else
            {
                // if the file does not exist, download the video from the server
                var videoBytes = await _proxyClientService
                    .GetAsync<byte[]>(MediathequeRoutes.Documents.GetBytes.Replace("{path}", VideoUrl));

                if (videoBytes.IsSuccess)
                {
                    // write the video to a temporary file
                    await File.WriteAllBytesAsync(tempFilePath, videoBytes.Value);

                    // read the video from the file
                    VideoSource = MediaSource.FromFile(tempFilePath);
                }
                else
                {
                    //TODO: display error message
                }
            }
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
		{
            IsBusy = true;
            if (query.TryGetValue("videoUrl", out var videoUrl))
			{
				VideoUrl = videoUrl as string ?? string.Empty; // Updates the VideoUrl property
			}
            
            if (query.TryGetValue("video", out var videoObj))
			{
                if (videoObj is not Video video) return;
                Title = video.Title;
                VideoUrl = video.VideoUrl;
                await PlayVideo(video.VideoUrl);

            }

            IsBusy = false;
        }
	}
}
