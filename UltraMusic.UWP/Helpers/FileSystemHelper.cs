using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltraMusic.UWP.Helpers
{
    public class FileSystemHelper : Portable.Helpers.FileSystemHelper
    {
        public override string GetProvidersSpecDirectory()
        {
            return Path.Combine(Windows.ApplicationModel.Package.Current.InstalledLocation.Path, "Assets", "ProvidersSpec");
        }
    }
}
