using Thalassic.Mods;
using Microsoft.Extensions.Configuration;
using Semver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Thalassic.Registry
{
    /// <summary>
    /// Must be initialized by calling Update()
    /// </summary>
    public class Rtw2VersionRegistry
    {
        public List<Rtw2Version> Rtw2Versions { get; private set; } = new List<Rtw2Version>();

        public Rtw2Version CurrentRtw2Version { get; private set; }

        private readonly string _versionCachePath = Path.Combine(Program.Rtw2ExecutableDirectory, "rtw2_versions.ini");

        public Rtw2VersionRegistry()
        {
        }

        public void Update(IList<string> mirrors = null)
        {
            if (!File.Exists(_versionCachePath) && mirrors == null)
            {
                throw new Exception($"RTW2 version cache not found at {_versionCachePath} and no mirrors provided");
            }

            if (File.Exists(_versionCachePath))
            {
                UpdateKnownVersionsFromCache();
            }

            if (mirrors != null)
            {
                UpdateKnownRTW2VersionsFromMirrors(mirrors);
            }

            var checksum = GetChecksum(File.ReadAllBytes(Path.Combine(Program.Rtw2ExecutableDirectory, "Clean", "RTW2.exe")));
            Log.Debug($"Identified checksum {checksum}");
            try
            {
                CurrentRtw2Version = Rtw2Versions.First(v => v.Checksum.Equals(checksum));
            }
            catch (Exception e)
            {
                Log.Error($"Couldn't find RTW2 version associated with checksum {checksum}", e);
                CurrentRtw2Version = new Rtw2Version(SemVersion.Parse("1.0.0"), checksum);
            }
            Log.Debug($"Running Rule the Waves 2 {CurrentRtw2Version.Version}");
        }

        internal void UpdateKnownVersionsFromCache()
        {
            Log.Debug($"Loading RTW2 versions from {Path.Combine(Program.Rtw2ExecutableDirectory, "rtw2_versions.ini")}");
            try
            {
                var rawFileContents = File.ReadAllText(Path.Combine(Program.Rtw2ExecutableDirectory, "rtw2_versions.ini"));
                Rtw2Versions.AddRange(ModConfigurationReader.Rtw2VersionsFromString(rawFileContents).ToList());
            }
            catch (Exception e)
            {
                Log.Error($"Failed to parse version data", e);
            }
        }

        internal void UpdateKnownRTW2VersionsFromMirrors(IList<string> mirrors)
        {
            var allKnownVersions = new List<Rtw2Version>(Rtw2Versions);
            foreach (var mirror in mirrors)
            {
                Log.Debug($"Retrieving version information from {mirror} if available");
                var mirrorKnownVersions = new List<Rtw2Version>();
                var versionsUrl = Path.Combine(mirror, "rtw2_versions.ini");
                using (var client = new WebClient())
                {
                    try
                    {
                        var response = client.DownloadString(versionsUrl);
                        mirrorKnownVersions.AddRange(ModConfigurationReader.Rtw2VersionsFromString(response));
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Didn't find a RTW2 version listing at {versionsUrl}", e);
                    }
                }
                foreach (var version in mirrorKnownVersions)
                {
                    if (!allKnownVersions.Any(existing => version.Checksum == existing.Checksum))
                    {
                        allKnownVersions.Add(version);
                    }
                }
            }
            Rtw2Versions.AddRange(allKnownVersions);

            UpdateCache();
        }

        internal void UpdateCache()
        {
            // Update cache
            var versions = Rtw2Versions.Select(v => $"{v.Version}={v.Checksum}");

            Log.Debug($"Updating cache with {String.Join("\n\r", versions)}");
            File.WriteAllLines(
                Path.Combine(Program.Rtw2ExecutableDirectory, "rtw2_versions.ini"),
                Rtw2Versions.Select(v => $"{v.Version}={v.Checksum}").Distinct());
        }

        internal string GetChecksum(byte[] bytes)
        {
            return Program.GetChecksum(bytes);
        }
    }
}
