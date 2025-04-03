using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class EpisodeSelectionForm : Form
    {
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckedListBox chkEpisodes;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        private readonly Content _content;
        private readonly User _currentUser;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private string _watchedEpisodes;

        public EpisodeSelectionForm(Content content, User currentUser)
        {
            InitializeComponent();
            _content = content;
            _currentUser = currentUser;
            _viewingHistoryRepository = new ViewingHistoryRepository(new Data.EntityFramework.NetWatchDbContext());

            // Load episodes and check watched ones
            LoadEpisodes();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkEpisodes = new System.Windows.Forms.CheckedListBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 28);
            this.lblTitle.Text = "Select Watched Episodes";

            // chkEpisodes
            this.chkEpisodes.FormattingEnabled = true;
            this.chkEpisodes.Location = new System.Drawing.Point(20, 60);
            this.chkEpisodes.Name = "chkEpisodes";
            this.chkEpisodes.Size = new System.Drawing.Size(360, 220);
            this.chkEpisodes.TabIndex = 0;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(100, 300);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(220, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // EpisodeSelectionForm
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.chkEpisodes);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EpisodeSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Episode Selection";
        }

        private void LoadEpisodes()
        {
            // Get viewing history for this user and content
            var viewingHistory = _viewingHistoryRepository.GetByUserAndContent(_currentUser.Id, _content.Id);
            _watchedEpisodes = viewingHistory?.WatchedEpisodes ?? string.Empty;

            // Parse watched episodes
            var watchedEpisodeNumbers = new List<int>();
            if (!string.IsNullOrEmpty(_watchedEpisodes))
            {
                watchedEpisodeNumbers = _watchedEpisodes.Split(',')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(s => int.Parse(s))
                    .ToList();
            }

            // Add episodes to checklist
            foreach (var episode in _content.Episodes.OrderBy(e => e.EpisodeNumber))
            {
                string itemText = $"Episode {episode.EpisodeNumber}: {episode.Title} ({episode.Duration} min)";
                int index = chkEpisodes.Items.Add(itemText);

                // Check if this episode was watched
                if (watchedEpisodeNumbers.Contains(episode.EpisodeNumber))
                {
                    chkEpisodes.SetItemChecked(index, true);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Get selected episodes
                var watchedEpisodeNumbers = new List<int>();
                for (int i = 0; i < chkEpisodes.Items.Count; i++)
                {
                    if (chkEpisodes.GetItemChecked(i))
                    {
                        // Extract episode number from the item text
                        string itemText = chkEpisodes.Items[i].ToString();
                        int episodeNumber = int.Parse(itemText.Substring(8, itemText.IndexOf(':') - 8));
                        watchedEpisodeNumbers.Add(episodeNumber);
                    }
                }

                // Create or update viewing history
                var viewingHistory = _viewingHistoryRepository.GetByUserAndContent(_currentUser.Id, _content.Id);

                if (viewingHistory == null)
                {
                    viewingHistory = new ViewingHistory
                    {
                        UserId = _currentUser.Id,
                        ContentId = _content.Id,
                        WatchDate = DateTime.Now,
                        WatchedEpisodes = string.Join(",", watchedEpisodeNumbers)
                    };
                    _viewingHistoryRepository.Add(viewingHistory);
                }
                else
                {
                    viewingHistory.WatchDate = DateTime.Now;
                    viewingHistory.WatchedEpisodes = string.Join(",", watchedEpisodeNumbers);
                    _viewingHistoryRepository.Update(viewingHistory);
                }

                MessageBox.Show("Viewing history updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating viewing history: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

