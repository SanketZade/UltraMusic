using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UltraMusic.UWP.Helpers;
using UltraMusic.UWP.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UltraMusic.UWP.View
{
    public sealed partial class SettingsUserControl : UserControl
    {
        public SettingsUserControl()
        {
            this.InitializeComponent();
            DataContext = ViewModelLocator.SettingsViewModel;
            VM.Loaded();
        }

        private SettingsViewModel VM { get { return ViewModelLocator.SettingsViewModel as SettingsViewModel; } }

        private void SaveProvidersButton_Click(object sender, RoutedEventArgs e)
        {
            VM.Save();
        }
    }
}
