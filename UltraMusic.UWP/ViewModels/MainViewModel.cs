﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraMusic.Portable.Helpers;
using UltraMusic.Portable.Models;
using UltraMusic.Portable.ViewModels;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace UltraMusic.UWP.ViewModels
{
    class MainViewModel : Portable.ViewModels.MainViewModel
    {
        public MainViewModel(SettingsHelper settingsHelper, FileSystemHelper fileSystemHelper) 
            : base(BuildWebViewWrapper, settingsHelper, fileSystemHelper)
        {
        }

        public static WebViewWrapper BuildWebViewWrapper(MusicProvider musicProvider)
        {
            WebView webView = new WebView();
            webView.Navigate(new Uri(musicProvider.Url));
            return new WebViewWrapper(webView, musicProvider);
        }
    }
}
