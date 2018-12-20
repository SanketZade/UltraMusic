using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using UltraMusic.Portable.Models;
using Newtonsoft.Json;
using System.IO;
using UltraMusic.Portable.Helpers;
using System.Linq;

namespace UltraMusic.Portable.ViewModels
{
    public abstract class MainViewModel : ViewModelBase
    {
        private readonly Func<MusicProvider, WebViewWrapperBase> wrapperFactory;
        private readonly FileSystemHelper fileSystemHelper;
        private readonly SettingsHelper settingsHelper;

        public MainViewModel(Func<MusicProvider, WebViewWrapperBase> wrapperFactory, SettingsHelper settingsHelper, FileSystemHelper fileSystemHelper) 
        {
            this.wrapperFactory = wrapperFactory;
            this.settingsHelper = settingsHelper;
            this.fileSystemHelper = fileSystemHelper;
        }

        private WebViewWrapperBase nowPlayingViewWrapper;

        #region Playback Controls

        public async Task TogglePlayPause()
        {
            if (PlayerState == PlayerState.Paused)
                await Play();
            else if (PlayerState == PlayerState.Playing)
                await Pause();
        }

        public async Task Pause()
        {
            if (nowPlayingViewWrapper != null) await nowPlayingViewWrapper.Pause();
        }

        public async Task Play()
        {
            if (nowPlayingViewWrapper != null) await nowPlayingViewWrapper.Play();
        }

        public async Task Next()
        {
            if (nowPlayingViewWrapper != null) await nowPlayingViewWrapper.Next();
        }

        public async Task Previous()
        {
            if (nowPlayingViewWrapper != null) await nowPlayingViewWrapper.Previous();
        }

        #endregion

        public void Search(string query)
        {
            foreach (var provider in MusicProviders)
            {
                WebViewWrapperBase wrapper = GetWebViewWrapper(provider);
                wrapper.Search(query);
            }
        }

        private Dictionary<string, WebViewWrapperBase> webViewWrappers;

        public WebViewWrapperBase GetWebViewWrapper(MusicProvider provider)
        {
            webViewWrappers = webViewWrappers ?? new Dictionary<string, WebViewWrapperBase>();
            if (!webViewWrappers.ContainsKey(provider.Id))
            {
                WebViewWrapperBase wrapper = wrapperFactory(provider);
                wrapper.PlayerStateChanged += Wrapper_PlayerStateChanged;
                wrapper.NowPlayingChanged += Wrapper_NowPlayingChanged;
                webViewWrappers[provider.Id] = wrapper;
            }
            return webViewWrappers[provider.Id];
        }

        
        #region Now Playing

        private NowPlaying nowPlaying;
        public NowPlaying NowPlaying
        {
            get { return nowPlaying; }
            set => Set(ref nowPlaying, value);
        }

        private async void Wrapper_NowPlayingChanged(object sender, EventArgs e)
        {
            await Task.Delay(100);
            NowPlaying = await nowPlayingViewWrapper.GetNowPlaying();
        }

        #endregion


        #region Playback State


        public async Task PlaybackStateChanged(string providerId)
        {
            var wrapper = GetWebViewWrapper(MusicProviders.Find(m => m.Id == providerId));
            await HandlePlaybackStateCanged(wrapper);
        }

        private async void Wrapper_PlayerStateChanged(object sender, EventArgs e)
        {
            WebViewWrapperBase wrapper = (WebViewWrapperBase)sender;
            await HandlePlaybackStateCanged(wrapper);
        }

        private async Task HandlePlaybackStateCanged(WebViewWrapperBase wrapper)
        {
            PlayerState state = await wrapper.GetPlayerState();
            switch (state)
            {
                case PlayerState.Playing:
                    if (nowPlayingViewWrapper != wrapper)
                    {
                        //background player started playing
                        //pause current player
                        await Pause();
                        //bring new one to foreground
                        nowPlayingViewWrapper = wrapper;
                    }
                    PlayerState = state;
                    break;
                case PlayerState.Paused:
                case PlayerState.Idle:
                    if (nowPlayingViewWrapper == wrapper)
                    {
                        PlayerState = state;
                    }
                    break;
            }
            if (nowPlayingViewWrapper != null)
            {
                NowPlaying = await nowPlayingViewWrapper.GetNowPlaying();
            }
        }


        private PlayerState playerState = PlayerState.Idle;
        public PlayerState PlayerState
        {
            get { return playerState; }
            set => Set(ref playerState, value);
        }

#endregion

        #region Music Providers


        private List<MusicProvider> musicProviders;
        public List<MusicProvider> MusicProviders
        {
            get
            {
                if (musicProviders == null)
                    LoadMusicProvidersAsync();
                return musicProviders;
            }
            set => Set(ref musicProviders, value);
        }

        public async void LoadMusicProvidersAsync()
        {
            List<MusicProvider> musicProviders = await fileSystemHelper.GetProvidersAsync();
            string[] enabledMusicProviders = settingsHelper.GetEnabledProviders();
            var filtered = musicProviders.Where(p => enabledMusicProviders.Contains(p.Id)).ToList();
            MusicProviders = filtered;
        }

        #endregion

        public override void Loaded()
        {
            LoadMusicProvidersAsync();
        }
    }
}
