using Thalassic.Registry;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Thalassic
{
    public partial class FormModDownload : Form
    {
        private readonly Rtw2VersionRegistry _auroraVersionRegistry;
        private readonly ModRegistry _modRegistry;

        public FormModDownload()
        {
            InitializeComponent();
            _auroraVersionRegistry = new Rtw2VersionRegistry();
            _modRegistry = new ModRegistry();
        }

        private void FormModDownload_Load(object sender, EventArgs e)
        {
            _auroraVersionRegistry.Update(_modRegistry.Mirrors);
            _modRegistry.Update(true);
            UpdateManageModsListView();
        }

        private void ListManageMods_SelectedIndexChanged(object sender, EventArgs e)
        {
            ButtonGetMod.Enabled = false;
            ButtonModReadme.Enabled = false;
            ButtonModChangelog.Enabled = false;

            if (ListViewRegistryMods.SelectedItems.Count > 0)
            {
                var selected = _modRegistry.Mods.Single(mod => mod.Name == ListViewRegistryMods.SelectedItems[0].Text);
                RichTextBoxDescription.Text = selected.Description;
                if (selected.Installed)
                {
                    ButtonGetMod.Text = "Update";
                    if (selected.CanBeUpdated)
                    {
                        ButtonGetMod.Enabled = true;
                    }
                    if (selected.Installed && (selected.ReadmeFile != null || selected.ReadmeUrl != null))
                    {
                        ButtonModReadme.Enabled = true;
                    }
                    if (selected.Installed && (selected.ChangelogFile != null || selected.ChangelogUrl != null))
                    {
                        ButtonModChangelog.Enabled = true;
                    }
                }
                else
                {
                    ButtonGetMod.Text = "Install";
                    ButtonGetMod.Enabled = true;
                }
            }
            else
            {
                ButtonGetMod.Text = "Update";
                ButtonGetMod.Enabled = false;
            }
        }

        private void ButtonGetMod_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            var mod = _modRegistry.Mods.Single(mod => mod.Name == ListViewRegistryMods.SelectedItems[0].Text);
            mod.LatestVersion.Download();
            UpdateManageModsListView();
            Cursor = Cursors.Default;
        }

        public void UpdateManageModsListView()
        {
            ListViewRegistryMods.BeginUpdate();
            ListViewRegistryMods.Clear();
            ListViewRegistryMods.AllowColumnReorder = true;
            ListViewRegistryMods.FullRowSelect = true;
            ListViewRegistryMods.View = View.Details;
            ListViewRegistryMods.Columns.Add("Name");
            ListViewRegistryMods.Columns.Add("Current");
            ListViewRegistryMods.Columns.Add("Latest");
            ListViewRegistryMods.Columns.Add("Description");

            foreach (var mod in _modRegistry.Mods)
            {
                if (mod.Name != "AuroraLoader")
                {
                    var li = new ListViewItem(new string[] {
                        mod.Name,
                        mod.LatestInstalledVersion?.Version?.ToString() ?? "Not Installed",
                        mod.LatestVersion?.Version == mod.LatestVersion?.Version
                            ? "Up to date"
                            : mod.LatestVersion?.Version?.ToString()
                            ?? "-",
                        mod.Description
                    });
                    ListViewRegistryMods.Items.Add(li);
                }
            }

            ListViewRegistryMods.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListViewRegistryMods.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            ListViewRegistryMods.EndUpdate();

            ButtonGetMod.Enabled = false;
            ButtonModReadme.Enabled = false;
            ButtonModChangelog.Enabled = false;
        }

        // TODO need to handle URL
        private void ButtonModChangelog_click(object sender, EventArgs e)
        {
            try
            {
                var mod = _modRegistry.Mods.Single(mod => mod.Name == ListViewRegistryMods.SelectedItems[0].Text);
                if (mod.ModFolder == null || (string.IsNullOrWhiteSpace(mod.ChangelogFile) && string.IsNullOrWhiteSpace(mod.ChangelogUrl)))
                {
                    throw new Exception("Invalid mod selected for changelog");
                }

                if (!string.IsNullOrWhiteSpace(mod.ChangelogFile))
                {
                    var pieces = mod.ChangelogFile.Split(' ');
                    var exe = pieces[0];
                    var args = "";
                    if (pieces.Length > 1)
                    {
                        for (int i = 1; i < pieces.Length; i++)
                        {
                            args += " " + pieces[i];
                        }

                        args = args.Substring(1);
                    }

                    var modVersion = mod.LatestInstalledVersion;
                    Log.Debug($"{mod.Name} changelog file: run {exe} in {modVersion.DownloadPath} with args {args}");
                    if (!File.Exists(Path.Combine(modVersion.DownloadPath, exe)))
                    {
                        MessageBox.Show($"Couldn't launch {Path.Combine(modVersion.DownloadPath, exe)} - make sure {Path.Combine(mod.ModFolder, "mod.json")} is correctly configured.");
                        return;
                    }
                    var info = new ProcessStartInfo()
                    {
                        WorkingDirectory = modVersion.DownloadPath,
                        FileName = exe,
                        Arguments = args,
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };

                    Process.Start(info);
                }
                else if (!string.IsNullOrWhiteSpace(mod.ChangelogUrl))
                {
                    // TODO
                }
            }
            catch (Exception exc)
            {
                Log.Error($"Failed while trying to open {ListViewRegistryMods.SelectedItems[0]} changelog", exc);
            }
        }

        // TODO need to handle URL
        private void ButtonModReadme_click(object sender, EventArgs e)
        {
            try
            {
                var mod = _modRegistry.Mods.Single(mod => mod.Name == ListViewRegistryMods.SelectedItems[0].Text);
                if (mod.ModFolder == null || (string.IsNullOrWhiteSpace(mod.ReadmeFile) && string.IsNullOrWhiteSpace(mod.ReadmeUrl)))
                {
                    throw new Exception("Invalid mod selected for readme");
                }

                if (!string.IsNullOrWhiteSpace(mod.ReadmeFile))
                {
                    var pieces = mod.ReadmeFile.Split(' ');
                    var exe = pieces[0];
                    var args = "";
                    if (pieces.Length > 1)
                    {
                        for (int i = 1; i < pieces.Length; i++)
                        {
                            args += " " + pieces[i];
                        }

                        args = args.Substring(1);
                    }

                    var modVersion = mod.LatestInstalledVersion;
                    Log.Debug($"{mod.Name} readme file: run {exe} in {modVersion.DownloadPath} with args {args}");
                    if (!File.Exists(Path.Combine(modVersion.DownloadPath, exe)))
                    {
                        MessageBox.Show($"Couldn't launch {Path.Combine(modVersion.DownloadPath, exe)} - make sure {Path.Combine(mod.ModFolder, "mod.json")} is correctly configured.");
                        return;
                    }
                    var info = new ProcessStartInfo()
                    {
                        WorkingDirectory = modVersion.DownloadPath,
                        FileName = exe,
                        Arguments = args,
                        UseShellExecute = true,
                        CreateNoWindow = true
                    };

                    Process.Start(info);
                }
                else if (!string.IsNullOrWhiteSpace(mod.ReadmeUrl))
                {
                    // TODO
                }
            }
            catch (Exception exc)
            {
                Log.Error($"Failed while trying to open {ListViewRegistryMods.SelectedItems[0]} readme", exc);
            }
        }
    }
}
