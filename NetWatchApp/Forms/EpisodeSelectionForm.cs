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
        private readonly Content _content;
        private readonly User _currentUser;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private List<int> _selectedEpisodes = new List<int>();

        public EpisodeSelectionForm(Content content, User currentUser)
        {
            InitializeComponent();
            _content = content;
            _currentUser = currentUser;
            _viewingHistoryRepository = new ViewingHistoryRepository(new Data.EntityFramework.NetWatchDbContext());

            // Load episodes
            LoadEpisodes();

            // Set up event handlers
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSelectAll.Click += BtnSelectAll_Click;
            btnUnselectAll.Click += BtnUnselectAll_Click;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.clbEpisodes = new System.Windows.Forms.CheckedListBox();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnUnselectAll = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 28);
            this.lblTitle.Text = "Select Watched Episodes";

            // clbEpisodes
            this.clbEpisodes.FormattingEnabled = true;
            this.clbEpisodes.Location = new System.Drawing.Point(20, 60);
            this.clbEpisodes.Name = "clbEpisodes";
            this.clbEpisodes.Size = new System.Drawing.Size(400, 300);
            this.clbEpisodes.TabIndex = 0;

            // btnSelectAll
            this.btnSelectAll.Location = new System.Drawing.Point(20, 370);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 30);
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;

            // btnUnselectAll
            this.btnUnselectAll.Location = new System.Drawing.Point(130, 370);
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(100, 30);
            this.btnUnselectAll.Text = "Unselect All";
            this.btnUnselectAll.UseVisualStyleBackColor = true;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(220, 420);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(330, 420);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // EpisodeSelectionForm
            this.ClientSize = new System.Drawing.Size(450, 480);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.clbEpisodes);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.btnUnselectAll);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EpisodeSelectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Episodes";
        }

        private void LoadEpisodes()
        {
            // Get viewing history for this user and content
            var viewingHistory = _viewingHistoryRepository.GetByUserAndContent(_currentUser.Id, _content.Id);
            var watchedEpisodes = new List<int>();

            if (viewingHistory != null && !string.IsNullOrEmpty(viewingHistory.WatchedEpisodes))
            {
                watchedEpisodes = viewingHistory.WatchedEpisodes
                    .Split(',')
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Select(int.Parse)
                    .ToList();

                _selectedEpisodes = new List<int>(watchedEpisodes);
            }

            // Populate checklist with episodes
            clbEpisodes.Items.Clear();
            foreach (var episode in _content.Episodes.OrderBy(e => e.EpisodeNumber))
            {
                string itemText = $"Episode {episode.EpisodeNumber}: {episode.Title} ({episode.Duration} min)";
                clbEpisodes.Items.Add(itemText, watchedEpisodes.Contains(episode.EpisodeNumber));
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbEpisodes.Items.Count; i++)
            {
                clbEpisodes.SetItemChecked(i, true);
            }
        }

        private void BtnUnselectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < clbEpisodes.Items.Count; i++)
            {
                clbEpisodes.SetItemChecked(i, false);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Get selected episodes
                _selectedEpisodes.Clear();
                for (int i = 0; i < clbEpisodes.Items.Count; i++)
                {
                    if (clbEpisodes.GetItemChecked(i))
                    {
                        _selectedEpisodes.Add(_content.Episodes.OrderBy(ep => ep.EpisodeNumber).ElementAt(i).EpisodeNumber);
                    }
                }

                // Update viewing history
                var viewingHistory = _viewingHistoryRepository.GetByUserAndContent(_currentUser.Id, _content.Id);

                if (viewingHistory == null)
                {
                    viewingHistory = new ViewingHistory
                    {
                        UserId = _currentUser.Id,
                        ContentId = _content.Id,
                        WatchDate = DateTime.Now,
                        WatchedEpisodes = string.Join(",", _selectedEpisodes)
                    };
                    _viewingHistoryRepository.Add(viewingHistory);
                }
                else
                {
                    viewingHistory.WatchDate = DateTime.Now;
                    viewingHistory.WatchedEpisodes = string.Join(",", _selectedEpisodes);
                    _viewingHistoryRepository.Update(viewingHistory);
                }

                MessageBox.Show("Episodes marked as watched!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving watched episodes: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

