using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Thalassic.Mods
{
    public class Mod
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("readme_url")]
        public string ReadmeUrl { get; set; }

        [JsonPropertyName("changelog_url")]
        public string ChangelogUrl { get; set; }

        [JsonPropertyName("readme_file")]
        public string ReadmeFile { get; set; }

        [JsonPropertyName("changelog_file")]
        public string ChangelogFile { get; set; }

        [JsonPropertyName("downloads")]
        public List<ModVersion> Downloads { get; set; } = new List<ModVersion>();

        internal Mod() { }


        // Helper props
        [JsonIgnore]
        public ModVersion LatestVersion => Downloads.OrderByDescending(v => v.Version).FirstOrDefault();

        [JsonIgnore]
        public ModVersion LatestInstalledVersion => Downloads.OrderByDescending(v => v.Version)
            .Where(v => v.Downloaded)
            .FirstOrDefault();

        public bool Installed => LatestInstalledVersion != null;
        public bool CanBeUpdated => LatestVersion != null
                && LatestInstalledVersion != null;

        public string ModFolder => Path.Combine(Program.ModDirectory, Name);

        public void UpdateCache()
        {
            Directory.CreateDirectory(ModFolder);
            File.WriteAllText(Path.Combine(ModFolder, "mod.json"), JsonSerializer.Serialize(this, new JsonSerializerOptions()
            {
                IgnoreNullValues = true,
                WriteIndented = true
            }));
        }

        public static Mod DeserializeMod(string rawJson)
        {
            var mod = JsonSerializer.Deserialize<Mod>(rawJson, new JsonSerializerOptions()
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                PropertyNameCaseInsensitive = true
            });
            foreach (var modVersion in mod.Downloads)
            {
                modVersion.Mod = mod;
            }
            return mod;
        }
    }
}
