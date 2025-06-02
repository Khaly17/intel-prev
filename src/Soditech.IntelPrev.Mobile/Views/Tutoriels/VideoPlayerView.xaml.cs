using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace Soditech.IntelPrev.Mobile.Views.Tutoriels
{
	public partial class VideoPlayerView : ContentPage
	{
		public VideoPlayerView()
		{
			InitializeComponent();
		}


		private void SetVideo()
		{
			mediaElement.Source = MediaSource.FromResource(VideoUrl);
		}

		private string _videoUrl = "extinguisher_video.mp4";
		public string VideoUrl
		{
			get => _videoUrl;
			set
			{
				if (_videoUrl == value) return;
				_videoUrl = value;
				OnPropertyChanged(nameof(VideoUrl));
			}
		}
	}
}