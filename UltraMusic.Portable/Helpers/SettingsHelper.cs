using System;
using System.Collections.Generic;
using System.Text;

namespace UltraMusic.Portable.Helpers
{
     public abstract class SettingsHelper
    {
        private const string EnabledProvidersKey = "EnabledProviders";

        protected abstract T GetValue<T>(string key, T defaultValue = default(T));
        protected abstract void SetValue<T>(string key, T Value);

        public string[] GetEnabledProviders()
        {
            string p = GetValue<string>(EnabledProvidersKey, "");
            return p.Split(',');
        }

        public void SetEnabledProviders(List<string> providerIds)
        {
            string p = string.Join(",", providerIds);
            SetValue(EnabledProvidersKey, p);
        }
    }
}
