using Thalassic.Mods;
using Thalassic.Registry;
using Microsoft.Extensions.Configuration;
using Semver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Thalassic
{
    public partial class FormMain : Form
    {
        private Thread rtw2Thread = null;
        private Rtw2Installation rtw2Installation;

        private readonly Rtw2VersionRegistry _rtw2VersionRegistry;
        private readonly ModRegistry _modRegistry;

        private FormModDownload _modManagementWindow;
        private FormSaves _saveManagementWindow;

        public FormMain(Rtw2VersionRegistry rtw2VersionRegistry, ModRegistry modRegistry)
        {
            InitializeComponent();
            _rtw2VersionRegistry = rtw2VersionRegistry;
            _modRegistry = modRegistry;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                Icon = new Icon(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Thalassic.ico"));
            }
            catch
            {
                Log.Debug("Failed to load icon");
            }

            _ = MessageBox.Show(new Form { TopMost = true }, "Thalassic will check for updates and then launch, this might take a moment.");
            Cursor = Cursors.WaitCursor;

            // Only check mirrors for new versions at app startup
            _rtw2VersionRegistry.Update(_modRegistry.Mirrors);
            rtw2Installation = new Rtw2Installation(_rtw2VersionRegistry.CurrentRtw2Version, Path.Combine(Program.Rtw2ExecutableDirectory, "Clean"));

            _modRegistry.Update(true);
            RefreshRtw2InstallData();
            UpdateListViews();

            Cursor = Cursors.Default;
        }

        private void UpdateRtw2()
        {
            var dialog = MessageBox.Show($"RTW2 v{_rtw2VersionRegistry.Rtw2Versions.Max()?.Version} is available. Download now? This is safe and won't affect your existing games.", "Download new Aurora version", MessageBoxButtons.YesNo);
            if (dialog != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // TODO
                var thread = new Thread(() =>
                {
                    var aurora_files = Installer.GetLatestAuroraFiles();
                    var clean = new Rtw2Installation(_rtw2VersionRegistry.CurrentRtw2Version, Path.Combine(Program.Rtw2ExecutableDirectory, "Clean"));
                    clean.UpdateRtw2(aurora_files);
                });
                thread.Start();

                var progress = new FormProgress(thread) { Text = "Launching Rule the Waves 2 patcher" };
                progress.ShowDialog();
                RefreshRtw2InstallData();
                MessageBox.Show($"Update complete - you can now start new games using RTW2 {_rtw2VersionRegistry.CurrentRtw2Version.Version}!");
            }
            catch (Exception ecp)
            {
                Log.Error("Failed to update RTW2", ecp);
                Program.OpenBrowser(@"https://nws-online.proboards.com/board/27/rule-waves-2");
            }
        }

        private void ButtonUpdateRtw2_click(object sender, EventArgs e) { UpdateRtw2(); }

        private void SetCanUpdateRtw2(bool update)
        {
            PictureBoxUpdateRtw2.Enabled = update;
            PictureBoxUpdateRtw2.Visible = update;
        }

        /// <summary>
        /// Sets current install's version and checksum, and whether the update button is enabled
        /// </summary>
        private void RefreshRtw2InstallData()
        {
            _rtw2VersionRegistry.Update();
            if (_rtw2VersionRegistry.CurrentRtw2Version == null)
            {
                LabelRtw2Version.Text = "RTW2 version: Unknown";
            }
            else
            {
                // Show only the checksum if we can't identify the version of Aurora
                if (_rtw2VersionRegistry.CurrentRtw2Version.Version == SemVersion.Parse("1.0.0"))
                {
                    LabelRtw2Version.Text = $"RTW2.exe ({_rtw2VersionRegistry.CurrentRtw2Version.Checksum})";
                }
                // Default to showing the most recent installed Aurora version
                else
                {
                    LabelRtw2Version.Text = $"RTW2 v{_rtw2VersionRegistry.CurrentRtw2Version.Version} ({_rtw2VersionRegistry.CurrentRtw2Version.Checksum})";
                }

                if (_rtw2VersionRegistry.CurrentRtw2Version.Version.CompareTo(_rtw2VersionRegistry.Rtw2Versions.Max()?.Version) < 0)
                {
                    SetCanUpdateRtw2(true);
                }
                else
                {
                    SetCanUpdateRtw2(false);
                }
            }
        }

        /* Utilities tab */

        /// <summary>
        /// Populates the Utilities and Database Mod tabs
        /// </summary>
        private void UpdateListViews()
        {
            ListMods.Items.Clear();
            ListMods.Items.AddRange(_modRegistry.Mods.Select(mod => mod.Name).ToArray());
        }

        private void ButtonSinglePlayer_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            lock (this)
            {
                if (rtw2Thread != null)
                {
                    MessageBox.Show("Already running Rule the Waves 2.");
                    return;
                }
            }

            if (CheckEnableMusic.Checked && !Directory.Exists(Path.Combine(rtw2Installation.InstallationPath, "Music")))
            {
                var thread = new Thread(() =>
                {
                    Log.Debug("Installing music");
                    var aurora_files = Installer.GetLatestAuroraFiles();
                    Installer.DownloadRtw2Pieces(rtw2Installation.InstallationPath, new Dictionary<string, string> { { "Music", aurora_files["Music"] } });
                });
                thread.Start();

                var progress = new FormProgress(thread) { Text = "Installing music" };
                progress.ShowDialog();
            }

            ButtonSinglePlayer.Enabled = false;
            SetCanUpdateRtw2(false);

            var modVersions = _modRegistry.Mods.Where(mod =>
            (ListMods.CheckedItems != null && ListMods.CheckedItems.Contains(mod.Name)))
                .Select(mod => mod.LatestInstalledVersion).ToList();

            var processes = rtw2Installation.Launch(modVersions);
            rtw2Thread = new Thread(() => RunGame(processes, modVersions))
            {
                IsBackground = true
            };

            rtw2Thread.Start();
        }

        private void RunGame(List<Process> processes, List<ModVersion> modVersions)
        {
            var rtw2 = processes[0];

            var songs = new List<Song>();
            var folder = Path.Combine(rtw2Installation.InstallationPath, "Music");
            if (Directory.Exists(folder))
            {
                foreach (var mp3 in Directory.EnumerateFiles(folder, "*.mp3", SearchOption.AllDirectories))
                {
                    songs.Add(new Song(mp3));
                }
            }

            var rng = new Random();

            while (!rtw2.HasExited)
            {
                if (CheckEnableMusic.Checked && songs.Count > 0)
                {
                    var current = songs.Where(s => s.Playing).FirstOrDefault();

                    if (current == null)
                    {
                        current = songs[rng.Next(songs.Count)];

                        Log.Debug("Playing song: " + Path.GetFileNameWithoutExtension(current.File));
                        current.Play();
                    }
                    else
                    {
                        Invoke((MethodInvoker)delegate
                        {
                            current.Volume = TrackMusicVolume.Value / 10d;
                        });
                    }
                }
                else
                {
                    foreach (var song in songs)
                    {
                        song.Stop();
                    }
                }
                Thread.Sleep(1000);
            }

            foreach (var song in songs)
            {
                song.Stop();
            }

            foreach (var process in processes)
            {
                if (!process.HasExited)
                {
                    process.Kill();
                }
            }

            Invoke((MethodInvoker)delegate
            {
                Log.Debug("Game ended");
                ButtonSinglePlayer.Enabled = true;
                RefreshRtw2InstallData();
            });

            lock (this)
            {
                rtw2Thread = null;
            }
        }

        private void CheckMusic_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckEnableMusic.Checked)
            {
                TrackMusicVolume.Enabled = true;
            }
            else
            {
                TrackMusicVolume.Enabled = false;
            }
        }

        private void ButtonReadme_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Program.Rtw2ExecutableDirectory, "README.md")))
            {
                Process.Start(new ProcessStartInfo()
                {
                    WorkingDirectory = Program.Rtw2ExecutableDirectory,
                    FileName = "README.md",
                    UseShellExecute = true,
                    CreateNoWindow = true
                });
            }
            else
            {
                Program.OpenBrowser("https://github.com/Rule-the-Waves-2-Mods/Thalassic/blob/master/README.md");
            }
        }

        private void ButtonChangelog_Click(object sender, EventArgs e)
        {
            if (File.Exists(Path.Combine(Program.Rtw2ExecutableDirectory, "CHANGELOG.md")))
            {
                Process.Start(new ProcessStartInfo()
                {
                    WorkingDirectory = Program.Rtw2ExecutableDirectory,
                    FileName = "CHANGELOG.md",
                    UseShellExecute = true,
                    CreateNoWindow = true
                });
            }
            else
            {
                Program.OpenBrowser("https://github.com/Rule-the-Waves-2-Mods/Thalassic/blob/master/CHANGELOG.md");
            }
        }

        private void LinkVanillaSubreddit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.OpenBrowser(@"https://www.reddit.com/r/RuleTheWaves/");
        }

        private void LinkForums_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.OpenBrowser(@"https://nws-online.proboards.com/board/27/rule-waves-2");
        }

        private void LinkVanillaBug_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.OpenBrowser(@"https://nws-online.proboards.com/board/30/official-rtw2-bug-report-thread");
        }

        private void LinkDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Program.OpenBrowser(@"https://discord.com/channels/586214065760763925/586564542218108931");
        }

        private void ButtonManageMods_Click(object sender, EventArgs e)
        {
            if (_modManagementWindow != null)
            {
                _modManagementWindow.Close();
            }
            _modManagementWindow = new FormModDownload();
            _modManagementWindow.ShowDialog();
            UpdateListViews();
        }

        private void ButtonManageSaves_Click(object sender, EventArgs e)
        {
            if (_saveManagementWindow != null)
            {
                _saveManagementWindow.Close();
            }
            _saveManagementWindow = new FormSaves(rtw2Installation);
            _saveManagementWindow.ShowDialog();

            var name = _saveManagementWindow.SelectedGameName;
            if (name != null)
            {
                var exe = Path.Combine(Program.Rtw2ExecutableDirectory, "Games", name, "RTW2.exe");
                var bytes = File.ReadAllBytes(exe);
                var checksum = Program.GetChecksum(bytes);
                var version = _rtw2VersionRegistry.Rtw2Versions.First(v => v.Checksum == checksum);
                rtw2Installation = new Rtw2Installation(version, Path.GetDirectoryName(exe));

                UpdateListViews();
                RefreshRtw2InstallData();

                SelectedSavelabel.Text = $"Game: {name} (RTW2 v{rtw2Installation.InstalledVersion.Version})";
                ButtonSinglePlayer.Enabled = true;
            }
            else
            {
                SelectedSavelabel.Text = "No game selected";
                ButtonSinglePlayer.Enabled = false;
            }
        }
    }
}
