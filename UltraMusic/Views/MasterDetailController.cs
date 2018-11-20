﻿// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using AppKit;
using CoreGraphics;
using Foundation;
using UltraMusic.Helpers;
using UltraMusic.Portable.Models;
using UltraMusic.Portable.ViewModels;
using UltraMusic.ViewModels;
using WebKit;

namespace UltraMusic.Views
{
    public partial class MasterDetailController : NSSplitViewController, IWKScriptMessageHandler
    {
        private SideBarController leftController;
        private WebRendererController rightController;
        private ViewModels.MainViewModel viewModel;

        public MasterDetailController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        private void Initialize()
        {
            viewModel = ViewModelLocator.MainViewModel;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Container.WKScriptMessageHandler = this;

            var children = ChildViewControllers;
            leftController = SideBarItem.ViewController as SideBarController;
            rightController = WebRendererItem.ViewController as WebRendererController;

            leftController.ProviderClicked += RenderProvider;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;
            viewModel.Loaded();

            var res = NSBundle.MainBundle.ResourcePath;

        }

        void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string propertyName = e.PropertyName;
            switch (propertyName)
            {
                case nameof(viewModel.MusicProviders):
                    ProvidersChanged();
                    break;
                case nameof(viewModel.PlayerState):
                    PlayerStateChanged();
                    break;
            }
        }

        private void PlayerStateChanged()
        {
            switch (viewModel.PlayerState)
            {
                case PlayerState.Idle:
                    break;
            }
        }

        private void ProvidersChanged() 
        {
            leftController.RenderProviders(viewModel.MusicProviders);
        }

        private void RenderProvider(MusicProvider provider)
        {
            var rightView = rightController.View;

            if (rightView.Subviews.Length > 0)
            {
                rightView.Subviews[0].RemoveFromSuperview();
            }
            WKWebView webView = (WKWebView)viewModel.GetWebViewWrapper(provider).WebView ;
            rightView.AddSubview(webView);

            webView.TopAnchor.ConstraintEqualToAnchor(rightView.TopAnchor).Active = true;
            webView.BottomAnchor.ConstraintEqualToAnchor(rightView.BottomAnchor).Active = true;
            webView.LeftAnchor.ConstraintEqualToAnchor(rightView.LeftAnchor).Active = true;
            webView.RightAnchor.ConstraintEqualToAnchor(rightView.RightAnchor).Active = true;
        }

        public async void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            string providerId = message.Body.ToString();
            await viewModel.PlaybackStateChanged(providerId);
        }
    }
}
