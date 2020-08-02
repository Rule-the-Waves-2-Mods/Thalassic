using Thalassic.Registry;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace Thalassic
{
    static class Program
    {
        public static readonly string Rtw2ExecutableDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        public static readonly string ModDirectory = Path.Combine(Rtw2ExecutableDirectory, "Mods");
        public static readonly string CacheDirectory = Path.Combine(Path.GetTempPath(), "thalassic_cache");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Log.Clear();
            Log.Debug("Start logging");

            if (!File.Exists(Path.Combine(Rtw2ExecutableDirectory, "Clean", "rtw2.exe")))
            {
                Log.Debug("Aurora not installed");
                var dialog = MessageBox.Show("Rule the Waves 2 not installed. Please purchase, download, and install before using Thalassic.", "Install RTW2", MessageBoxButtons.OK);
                if (dialog == DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }
            }

            if (!Directory.Exists(ModDirectory))
            {
                Directory.CreateDirectory(ModDirectory);
            }

            var rtw2VersionRegistry = new Rtw2VersionRegistry();
            var modRegistry = new ModRegistry();
            Log.Debug("Launching main form");
            Application.Run(new FormMain(rtw2VersionRegistry, modRegistry));
        }

        public static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }

        public static string GetChecksum(byte[] bytes)
        {
            using var sha = SHA256.Create();
            var hash = sha.ComputeHash(bytes);
            var str = Convert.ToBase64String(hash);

            return str.Replace("/", "").Replace("+", "").Replace("=", "").Substring(0, 6);
        }
    }
}
