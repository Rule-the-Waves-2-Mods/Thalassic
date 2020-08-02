using System.Windows.Forms;

namespace Thalassic
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.ButtonSinglePlayer = new System.Windows.Forms.Button();
            this.LabelRtw2Version = new System.Windows.Forms.Label();
            this.TrackMusicVolume = new System.Windows.Forms.TrackBar();
            this.CheckEnableMusic = new System.Windows.Forms.CheckBox();
            this.ButtonReadme = new System.Windows.Forms.Button();
            this.ButtonChangelog = new System.Windows.Forms.Button();
            this.LabelThalassicVersion = new System.Windows.Forms.Label();
            this.ListMods = new System.Windows.Forms.CheckedListBox();
            this.LinkForums = new System.Windows.Forms.LinkLabel();
            this.LinkReportBug = new System.Windows.Forms.LinkLabel();
            this.LinkSubreddit = new System.Windows.Forms.LinkLabel();
            this.LinkDiscord = new System.Windows.Forms.LinkLabel();
            this.LabelMods = new System.Windows.Forms.Label();
            this.ButtonManageMods = new System.Windows.Forms.Button();
            this.SelectedSavelabel = new System.Windows.Forms.Label();
            this.ButtonManageSaves = new System.Windows.Forms.Button();
            this.PictureBoxUpdateRtw2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.TrackMusicVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxUpdateRtw2)).BeginInit();
            this.SuspendLayout();
            // 
            // ButtonSinglePlayer
            // 
            this.ButtonSinglePlayer.Enabled = false;
            this.ButtonSinglePlayer.Location = new System.Drawing.Point(14, 61);
            this.ButtonSinglePlayer.Margin = new System.Windows.Forms.Padding(5);
            this.ButtonSinglePlayer.Name = "ButtonSinglePlayer";
            this.ButtonSinglePlayer.Size = new System.Drawing.Size(96, 32);
            this.ButtonSinglePlayer.TabIndex = 2;
            this.ButtonSinglePlayer.Text = "Play";
            this.ButtonSinglePlayer.UseVisualStyleBackColor = true;
            this.ButtonSinglePlayer.Click += new System.EventHandler(this.ButtonSinglePlayer_Click);
            // 
            // LabelRtw2Version
            // 
            this.LabelRtw2Version.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelRtw2Version.Location = new System.Drawing.Point(469, 336);
            this.LabelRtw2Version.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelRtw2Version.Name = "LabelRtw2Version";
            this.LabelRtw2Version.Size = new System.Drawing.Size(152, 20);
            this.LabelRtw2Version.TabIndex = 7;
            this.LabelRtw2Version.Text = "Rule the Waves 2 v#.##.#";
            this.LabelRtw2Version.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // TrackMusicVolume
            // 
            this.TrackMusicVolume.Enabled = false;
            this.TrackMusicVolume.LargeChange = 1;
            this.TrackMusicVolume.Location = new System.Drawing.Point(250, 61);
            this.TrackMusicVolume.Margin = new System.Windows.Forms.Padding(5);
            this.TrackMusicVolume.Name = "TrackMusicVolume";
            this.TrackMusicVolume.Size = new System.Drawing.Size(185, 56);
            this.TrackMusicVolume.TabIndex = 20;
            this.TrackMusicVolume.Value = 4;
            // 
            // CheckEnableMusic
            // 
            this.CheckEnableMusic.AutoSize = true;
            this.CheckEnableMusic.Location = new System.Drawing.Point(119, 68);
            this.CheckEnableMusic.Margin = new System.Windows.Forms.Padding(5);
            this.CheckEnableMusic.Name = "CheckEnableMusic";
            this.CheckEnableMusic.Size = new System.Drawing.Size(130, 24);
            this.CheckEnableMusic.TabIndex = 2;
            this.CheckEnableMusic.Text = "In-Game Music";
            this.CheckEnableMusic.UseVisualStyleBackColor = true;
            this.CheckEnableMusic.CheckedChanged += new System.EventHandler(this.CheckMusic_CheckedChanged);
            // 
            // ButtonReadme
            // 
            this.ButtonReadme.Location = new System.Drawing.Point(474, 16);
            this.ButtonReadme.Margin = new System.Windows.Forms.Padding(5);
            this.ButtonReadme.Name = "ButtonReadme";
            this.ButtonReadme.Size = new System.Drawing.Size(96, 32);
            this.ButtonReadme.TabIndex = 13;
            this.ButtonReadme.Text = "Readme";
            this.ButtonReadme.UseVisualStyleBackColor = true;
            this.ButtonReadme.Click += new System.EventHandler(this.ButtonReadme_Click);
            // 
            // ButtonChangelog
            // 
            this.ButtonChangelog.Location = new System.Drawing.Point(580, 16);
            this.ButtonChangelog.Margin = new System.Windows.Forms.Padding(5);
            this.ButtonChangelog.Name = "ButtonChangelog";
            this.ButtonChangelog.Size = new System.Drawing.Size(96, 32);
            this.ButtonChangelog.TabIndex = 14;
            this.ButtonChangelog.Text = "Changelog";
            this.ButtonChangelog.UseVisualStyleBackColor = true;
            this.ButtonChangelog.Click += new System.EventHandler(this.ButtonChangelog_Click);
            // 
            // LabelThalassicVersion
            // 
            this.LabelThalassicVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelThalassicVersion.Location = new System.Drawing.Point(264, 336);
            this.LabelThalassicVersion.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.LabelThalassicVersion.Name = "LabelThalassicLoaderVersion";
            this.LabelThalassicVersion.Size = new System.Drawing.Size(152, 20);
            this.LabelThalassicVersion.TabIndex = 7;
            this.LabelThalassicVersion.Text = "Thalassic v#.##.#";
            this.LabelThalassicVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ListMods
            // 
            this.ListMods.FormattingEnabled = true;
            this.ListMods.Location = new System.Drawing.Point(14, 162);
            this.ListMods.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ListMods.Name = "ListMods";
            this.ListMods.Size = new System.Drawing.Size(665, 136);
            this.ListMods.TabIndex = 26;
            // 
            // LinkForums
            // 
            this.LinkForums.AutoSize = true;
            this.LinkForums.Location = new System.Drawing.Point(567, 67);
            this.LinkForums.Name = "LinkForums";
            this.LinkForums.Size = new System.Drawing.Size(109, 20);
            this.LinkForums.TabIndex = 33;
            this.LinkForums.TabStop = true;
            this.LinkForums.Text = "Official Forums";
            this.LinkForums.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkForums_LinkClicked);
            // 
            // LinkReportBug
            // 
            this.LinkReportBug.AutoSize = true;
            this.LinkReportBug.Location = new System.Drawing.Point(583, 128);
            this.LinkReportBug.Name = "LinkReportBug";
            this.LinkReportBug.Size = new System.Drawing.Size(96, 20);
            this.LinkReportBug.TabIndex = 34;
            this.LinkReportBug.TabStop = true;
            this.LinkReportBug.Text = "Report a Bug";
            this.LinkReportBug.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkVanillaBug_LinkClicked);
            // 
            // LinkSubreddit
            // 
            this.LinkSubreddit.AutoSize = true;
            this.LinkSubreddit.Location = new System.Drawing.Point(501, 88);
            this.LinkSubreddit.Name = "LinkSubreddit";
            this.LinkSubreddit.Size = new System.Drawing.Size(178, 20);
            this.LinkSubreddit.TabIndex = 35;
            this.LinkSubreddit.TabStop = true;
            this.LinkSubreddit.Text = "Rule the Waves Subreddit";
            this.LinkSubreddit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkVanillaSubreddit_LinkClicked);
            // 
            // LinkDiscord
            // 
            this.LinkDiscord.AutoSize = true;
            this.LinkDiscord.Location = new System.Drawing.Point(619, 108);
            this.LinkDiscord.Name = "LinkDiscord";
            this.LinkDiscord.Size = new System.Drawing.Size(60, 20);
            this.LinkDiscord.TabIndex = 36;
            this.LinkDiscord.TabStop = true;
            this.LinkDiscord.Text = "Discord";
            this.LinkDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkDiscord_LinkClicked);
            // 
            // LabelMods
            // 
            this.LabelMods.AutoSize = true;
            this.LabelMods.Location = new System.Drawing.Point(14, 128);
            this.LabelMods.Name = "LabelMods";
            this.LabelMods.Size = new System.Drawing.Size(92, 20);
            this.LabelMods.TabIndex = 39;
            this.LabelMods.Text = "Apply mods:";
            // 
            // ButtonManageMods
            // 
            this.ButtonManageMods.Location = new System.Drawing.Point(14, 324);
            this.ButtonManageMods.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonManageMods.Name = "ButtonManageMods";
            this.ButtonManageMods.Size = new System.Drawing.Size(118, 36);
            this.ButtonManageMods.TabIndex = 41;
            this.ButtonManageMods.Text = "Manage Mods";
            this.ButtonManageMods.UseVisualStyleBackColor = true;
            this.ButtonManageMods.Click += new System.EventHandler(this.ButtonManageMods_Click);
            // 
            // SelectedSavelabel
            // 
            this.SelectedSavelabel.AutoSize = true;
            this.SelectedSavelabel.Location = new System.Drawing.Point(117, 24);
            this.SelectedSavelabel.Name = "SelectedSavelabel";
            this.SelectedSavelabel.Size = new System.Drawing.Size(130, 20);
            this.SelectedSavelabel.TabIndex = 42;
            this.SelectedSavelabel.Text = "No game selected";
            // 
            // ButtonManageSaves
            // 
            this.ButtonManageSaves.Location = new System.Drawing.Point(14, 16);
            this.ButtonManageSaves.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ButtonManageSaves.Name = "ButtonManageSaves";
            this.ButtonManageSaves.Size = new System.Drawing.Size(96, 36);
            this.ButtonManageSaves.TabIndex = 41;
            this.ButtonManageSaves.Text = "Select Game";
            this.ButtonManageSaves.UseVisualStyleBackColor = true;
            this.ButtonManageSaves.Click += new System.EventHandler(this.ButtonManageSaves_Click);
            // 
            // PictureBoxUpdateRtw2
            // 
            this.PictureBoxUpdateRtw2.Image = ((System.Drawing.Image)(resources.GetObject("PictureBoxUpdateRtw2.Image")));
            this.PictureBoxUpdateRtw2.Location = new System.Drawing.Point(639, 317);
            this.PictureBoxUpdateRtw2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.PictureBoxUpdateRtw2.Name = "PictureBoxUpdateRtw2";
            this.PictureBoxUpdateRtw2.Size = new System.Drawing.Size(37, 43);
            this.PictureBoxUpdateRtw2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBoxUpdateRtw2.TabIndex = 43;
            this.PictureBoxUpdateRtw2.TabStop = false;
            this.PictureBoxUpdateRtw2.Click += new System.EventHandler(this.ButtonUpdateRtw2_click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 383);
            this.Controls.Add(this.PictureBoxUpdateRtw2);
            this.Controls.Add(this.ButtonManageSaves);
            this.Controls.Add(this.SelectedSavelabel);
            this.Controls.Add(this.ButtonManageMods);
            this.Controls.Add(this.LabelMods);
            this.Controls.Add(this.LinkDiscord);
            this.Controls.Add(this.LinkSubreddit);
            this.Controls.Add(this.LinkReportBug);
            this.Controls.Add(this.LinkForums);
            this.Controls.Add(this.ListMods);
            this.Controls.Add(this.LabelThalassicVersion);
            this.Controls.Add(this.ButtonReadme);
            this.Controls.Add(this.ButtonChangelog);
            this.Controls.Add(this.CheckEnableMusic);
            this.Controls.Add(this.TrackMusicVolume);
            this.Controls.Add(this.LabelRtw2Version);
            this.Controls.Add(this.ButtonSinglePlayer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormMain";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thalassic - A Rule the Waves 2 Mod Manager";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TrackMusicVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxUpdateRtw2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ButtonSinglePlayer;
        private System.Windows.Forms.Label LabelRtw2Version;
        private System.Windows.Forms.TrackBar TrackMusicVolume;
        private System.Windows.Forms.CheckBox CheckEnableMusic;
        private System.Windows.Forms.Button ButtonReadme;
        private System.Windows.Forms.Button ButtonChangelog;
        private System.Windows.Forms.Label LabelThalassicVersion;
        private System.Windows.Forms.CheckedListBox ListMods;
        private System.Windows.Forms.LinkLabel LinkForums;
        private System.Windows.Forms.LinkLabel LinkReportBug;
        private System.Windows.Forms.LinkLabel LinkSubreddit;
        private System.Windows.Forms.LinkLabel LinkDiscord;
        private System.Windows.Forms.Label LabelMods;
        private Button ButtonManageMods;
        private Label SelectedSavelabel;
        private Button ButtonManageSaves;
        private PictureBox PictureBoxUpdateRtw2;
    }
}

