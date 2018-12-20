using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraMusic.UWP.ViewModels;

namespace UltraMusic.UWP.Helpers
{
    internal class ViewModelLocator
    {
        private static MainViewModel mainViewModel;
        public static MainViewModel MainViewModel
        {
            get
            {
                mainViewModel = mainViewModel ?? new MainViewModel(SettingsHelper, FileSystemHelper);
                return mainViewModel;
            }
        }

        private static SettingsViewModel settingsViewModel;
        public static SettingsViewModel SettingsViewModel
        {
            get
            {
                settingsViewModel = settingsViewModel ?? new SettingsViewModel(SettingsHelper, FileSystemHelper);
                return settingsViewModel;
            }
        }

        private static FileSystemHelper fileSystemHelper;
        public static FileSystemHelper FileSystemHelper
        {
            get
            {
                fileSystemHelper = fileSystemHelper ?? new FileSystemHelper();
                return fileSystemHelper;
            }
        }

        private static SettingsHelper settingsHelper;
        public static SettingsHelper SettingsHelper
        {
            get
            {
                settingsHelper = settingsHelper ?? new SettingsHelper();
                return settingsHelper;
            }
        }
    }
}
