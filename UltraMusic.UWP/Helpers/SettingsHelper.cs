using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UltraMusic.UWP.Helpers
{
    public class SettingsHelper : Portable.Helpers.SettingsHelper
    {
        protected override T GetValue<T>(string key, T defaultValue = default(T))
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey(key))
                return (T)roamingSettings.Values[key];
            return defaultValue;
        }

        protected override void SetValue<T>(string key, T Value)
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[key] = Value;
        }
    }
}
