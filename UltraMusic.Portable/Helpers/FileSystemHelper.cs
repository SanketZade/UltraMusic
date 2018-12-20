using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UltraMusic.Portable.Models;

namespace UltraMusic.Portable.Helpers
{
    public abstract class FileSystemHelper
    {
        private List<MusicProvider> musicProviders;

        public async Task<List<MusicProvider>> GetProvidersAsync()
        {
            if (musicProviders == null)
            {
                string specDirectory = GetProvidersSpecDirectory();
                string specJson = await GetText(Path.Combine(specDirectory, "Spec.json"));
                var providers = JsonConvert.DeserializeObject<List<MusicProvider>>(specJson);
                foreach (var provider in providers)
                {
                    string id = provider.Id;
                    provider.FunctionsJs = await GetText(specDirectory, id, "Functions.js");
                }
                musicProviders = providers;
            }
            
            return musicProviders;
        }

        public abstract string GetProvidersSpecDirectory();

        private async Task<string> GetText(params string[] fragments)
        {
            string filePath = Path.Combine(fragments);
            using (StreamReader reader = File.OpenText(filePath))
                return await reader.ReadToEndAsync();
        }
    }
}
