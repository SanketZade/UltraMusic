using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraMusic.Portable.Helpers;
using UltraMusic.Portable.Models;

namespace UltraMusic.Portable.ViewModels
{
    public abstract class SettingsViewModel : ViewModelBase
    {
        private readonly SettingsHelper settingsHelper;
        private readonly FileSystemHelper fileSystemHelper;

        public SettingsViewModel(SettingsHelper settingsHelper, FileSystemHelper fileSystemHelper)
        {
            this.settingsHelper = settingsHelper;
            this.fileSystemHelper = fileSystemHelper;
        }

        private List<ProviderSetting> providers;
        public List<ProviderSetting> Providers
        {
            get { return providers; }
            set => Set(ref providers, value);
        }

        private async Task LoadProviders()
        {
            List<MusicProvider> providers = await fileSystemHelper.GetProvidersAsync();
            string[] enabledProviders = settingsHelper.GetEnabledProviders();

            List<ProviderSetting> providerSettings = new List<ProviderSetting>();
            foreach (var provider in providers)
            {
                var providerSetting = new ProviderSetting
                {
                    Id = provider.Id,
                    Name = provider.Name,
                    IsEnabled = enabledProviders.Contains(provider.Id)
                };
                providerSettings.Add(providerSetting);
            }
            Providers = providerSettings;
        }

        public async override void Loaded()
        {
            await LoadProviders();
        }

        public void Save()
        {
            var ps = Providers
                .Where(p => p.IsEnabled)
                .Select(p => p.Id)
                .ToList();
            settingsHelper.SetEnabledProviders(ps);
        }

        public class ProviderSetting
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool IsEnabled { get; set; }
        }
    }
}
