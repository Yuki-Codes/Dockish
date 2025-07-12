// Dockish
namespace Dockish.Modules;

using System;
using System.Threading.Tasks;
using DependencyPropertyGenerator;
using Windows.Media.Control;
using WindowsMediaController;

[DependencyProperty<string>("Player")]
[DependencyProperty<string>("PlayerIcon")]
[DependencyProperty<string>("Title")]
[DependencyProperty<string>("Artist")]
public partial class Media : ModuleBase
{
	private readonly MediaManager mediaManager = new();

	public Media()
	{
		mediaManager.OnAnyMediaPropertyChanged += OnMediaPropertyChanged;
		mediaManager.OnAnyPlaybackStateChanged += OnPlaybackStateChanged;
		mediaManager.Start();

		Task.Run(this.UpdateProperties);
	}

	private void OnPlaybackStateChanged(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionPlaybackInfo playbackInfo)
	{
		Task.Run(this.UpdateProperties);
	}

	private void OnMediaPropertyChanged(MediaManager.MediaSession mediaSession, GlobalSystemMediaTransportControlsSessionMediaProperties mediaProperties)
	{
		Task.Run(this.UpdateProperties);
	}

	private async Task UpdateProperties()
	{
		var session = mediaManager.GetFocusedSession().ControlSession;
		var playbackInfo = session.GetPlaybackInfo();
		var mediaProperties = await session.TryGetMediaPropertiesAsync();

		await this.Dispatcher.BeginInvoke(() =>
		{
			this.Player = session.SourceAppUserModelId;

			if (mediaProperties != null)
			{
				this.Title = mediaProperties.Title;
				this.Artist = mediaProperties.Artist;
			}

			if (playbackInfo.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
			{
				this.PlayerIcon = "\xea16";
			}
			else
			{
				this.PlayerIcon = session.SourceAppUserModelId switch
				{
					"Spotify.exe" => "\xe901",
					"308046B0AF4A39CB" => "\xe900",
					_ => string.Empty,
				};
			}
		});
	}
}