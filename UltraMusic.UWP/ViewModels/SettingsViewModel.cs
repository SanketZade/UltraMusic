using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraMusic.Portable.Helpers;

namespace UltraMusic.UWP.ViewModels
{
    public class SettingsViewModel : Portable.ViewModels.SettingsViewModel
    {
        public SettingsViewModel(SettingsHelper settingsHelper, FileSystemHelper fileSystemHelper)
           : base(settingsHelper, fileSystemHelper)
        {
        }

    }
}
