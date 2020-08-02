using Thalassic.Mods;
using Semver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("ThalassicTest")]

namespace Thalassic
{
    public class Rtw2Installation
    {
        public readonly Rtw2Version InstalledVersion;
        public readonly string InstallationPath;
        public string ConnectionString => $"Data Source={Path.Combine(InstallationPath, "AuroraDB.db")}";

        // e.g. <install dir>/Aurora/1.8.0
        public string VersionedDirectory => Path.Combine(InstallationPath, "Aurora", InstalledVersion.Version.ToString());

        public Rtw2Installation(Rtw2Version version, string installationPath)
        {
            if (version == null || installationPath == null)
            {
                throw new ArgumentNullException();
            }

            InstalledVersion = version;
            InstallationPath = installationPath;
        }

        public List<Process> Launch(IList<ModVersion> modVersions)
        {
            Log.Debug($"Launching from {InstallationPath}");
            var processes = new List<Process>();

            foreach (var mod in modVersions)
            {
                try
                {
                    mod.Install(this);
                }
                catch (Exception e)
                {
                    Log.Error($"Failed to launch {mod.Mod.Name}", e);
                }
            }

            foreach (var modVersion in modVersions)
            {
                try
                {
                    modVersion.Install(this);
                }
                catch (Exception e)
                {
                    var message = $"Failed to launch {modVersion.Mod.Name} {modVersion.Version}";
                    Log.Error(message, e);
                }
            }

            var processStartInfo = new ProcessStartInfo()
            {
                WorkingDirectory = InstallationPath,
                FileName = "Aurora.exe",
                UseShellExecute = true,
                CreateNoWindow = true
            };
            processes.Insert(0, Process.Start(processStartInfo));

            return processes;
        }

        // TODO remove installation functionality and just download and run the patch
        public void UpdateRtw2(Dictionary<string, string> aurora_files)
        {
            if (aurora_files == null)
            {
                throw new ArgumentNullException();
            }
            if (!aurora_files.Any())
            {
                throw new ArgumentException("aurora_files");
            }

            var update = SemVersion.Parse(aurora_files["Version"]);

            if (InstalledVersion.Version.Major == update.Major)
            {
                aurora_files.Remove("Major");

                if (InstalledVersion.Version.Minor == update.Minor)
                {
                    aurora_files.Remove("Minor");

                    if (InstalledVersion.Version.Patch == update.Patch)
                    {
                        aurora_files.Remove("Patch");
                        aurora_files.Remove("Rev"); // deprecated
                    }
                }
            }

            foreach (var piece in aurora_files.Keys.ToList())
            {
                if (!piece.Equals("Major") && !piece.Equals("Minor") && !piece.Equals("Patch") && !piece.Equals("Rev"))
                {
                    aurora_files.Remove(piece);
                }
            }

            if (aurora_files.Count > 0)
            {
                Installer.DownloadRtw2Pieces(InstallationPath, aurora_files);
            }
        }
    }
}