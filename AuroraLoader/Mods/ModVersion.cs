using Microsoft.Data.Sqlite;
using Semver;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Windows.Forms;

namespace Thalassic.Mods
{
    public class ModVersion
    {
        [JsonPropertyName("version")]
        [JsonConverter(typeof(SemVersionJsonConverter))]
        public SemVersion Version { get; set; }

        [JsonPropertyName("download_url")]
        public string DownloadUrl { get; set; }

        [JsonIgnore]
        public string DownloadPath => Path.Combine(Mod.ModFolder, Version.ToString());

        [JsonIgnore]
        public bool Downloaded => Directory.Exists(DownloadPath);

        [JsonIgnore]
        public Mod Mod { get; set; }


        public void Download()
        {
            if (Downloaded)
            {
                throw new Exception($"{Mod.Name} {Version} is already installed");
            }

            Log.Debug($"Preparing caches in {Program.CacheDirectory}");
            var zip = Path.Combine(Program.ModDirectory, "update.current");
            if (File.Exists(zip))
            {
                File.Delete(zip);
            }

            var extract_folder = Path.Combine(Program.CacheDirectory, "Extract");
            if (Directory.Exists(extract_folder))
            {
                Directory.Delete(extract_folder, true);
            }
            Directory.CreateDirectory(extract_folder);

            Log.Debug($"Downloading from {DownloadUrl}");
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadFile(DownloadUrl, zip);
                }

                ZipFile.ExtractToDirectory(zip, extract_folder);

                if (Directory.Exists(DownloadPath))
                {
                    Directory.Delete(DownloadPath, true);
                }
                Directory.CreateDirectory(DownloadPath);

                ZipFile.ExtractToDirectory(zip, DownloadPath);

                Mod.UpdateCache();
            }
            catch (Exception e)
            {
                var message = $"Failed while installing or updating {Mod.Name} {Version} to {DownloadPath} from {DownloadUrl}";
                Log.Error(message, e);
                MessageBox.Show(message);
            }
            finally
            {
                // Cleanup
                File.Delete(zip);
                Directory.Delete(extract_folder, true);
            }
        }

        // TODO Currently only "copy crap into the RTW2 folder"-style mods are supported
        public void Install(Rtw2Installation installation)
        {
            Log.Debug($"{Mod.Name} {Version}");
            foreach (var file in Directory.EnumerateFiles(DownloadPath, "*.*", SearchOption.AllDirectories).Where(f => !Path.GetFileName(f).Equals("mod.json")))
            {
                var out_file = Path.Combine(installation.InstallationPath, Path.GetRelativePath(DownloadPath, file));
                File.Copy(file, out_file, true);
            }
        }
    }
}
