using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class ContentDetailsForm : Form
    {
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblReleaseYear;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.PictureBox picContentImage;
        private System.Windows.Forms.Panel pnlEpisodes;
        private System.Windows.Forms.Label lblEpisodes;
        private System.Windows.Forms.DataGridView dgvEpisodes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeDuration;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colWatched;
        private System.Windows.Forms.Button btnMarkAsWatched;
        private System.Windows.Forms.Button btnRate;
        private System.Windows.Forms.Button btnClose;
        private readonly Content _content;
        private readonly User _currentUser;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private readonly RatingRepository _ratingRepository;

        public ContentDetailsForm(Content content, User currentUser)
        {
            InitializeComponent();
            _content = content;
            _currentUser = currentUser;
            _viewingHistoryRepository = new ViewingHistoryRepository(new Data.EntityFramework.NetWatchDbContext());
            _ratingRepository = new RatingRepository(new Data.EntityFramework.NetWatchDbContext());

            // Load content details
            LoadContentDetails();

            // Set up event handlers
            btnMarkAsWatched.Click += BtnMarkAsWatched_Click;
            btnRate.Click += BtnRate_Click;
            btnClose.Click += BtnClose_Click;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblReleaseYear = new System.Windows.Forms.Label();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();
            this.picContentImage = new System.Windows.Forms.PictureBox();
            this.pnlEpisodes = new System.Windows.Forms.Panel();
            this.lblEpisodes = new System.Windows.Forms.Label();
            this.dgvEpisodes = new System.Windows.Forms.DataGridView();
            this.colEpisodeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWatched = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnMarkAsWatched = new System.Windows.Forms.Button();
            this.btnRate = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 37);
            this.lblTitle.Text = "Content Title";

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(20, 70);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(85, 20);
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Location = new System.Drawing.Point(20, 100);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(400, 100);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.Text = "Content description goes here...";

            // lblGenre
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(20, 210);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(100, 20);
            this.lblGenre.Text = "Genre: Action";

            // lblReleaseYear
            this.lblReleaseYear.AutoSize = true;
            this.lblReleaseYear.Location = new System.Drawing.Point(20, 240);
            this.lblReleaseYear.Name = "lblReleaseYear";
            this.lblReleaseYear.Size = new System.Drawing.Size(120, 20);
            this.lblReleaseYear.Text = "Release Year: 2023";

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point(20, 270);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(120, 20);
            this.lblPlatform.Text = "Platform: Netflix";

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(20, 300);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(120, 20);
            this.lblDuration.Text = "Duration: 120 min";

            // lblRating
            this.lblRating.AutoSize = true;
            this.lblRating.Location = new System.Drawing.Point(20, 330);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(120, 20);
            this.lblRating.Text = "Rating: 4.5/5";

            // picContentImage
            this.picContentImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picContentImage.Location = new System.Drawing.Point(450, 20);
            this.picContentImage.Name = "picContentImage";
            this.picContentImage.Size = new System.Drawing.Size(200, 300);
            this.picContentImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picContentImage.TabIndex = 0;
            this.picContentImage.TabStop = false;

            // pnlEpisodes
            this.pnlEpisodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEpisodes.Controls.Add(this.lblEpisodes);
            this.pnlEpisodes.Controls.Add(this.dgvEpisodes);
            this.pnlEpisodes.Location = new System.Drawing.Point(20, 370);
            this.pnlEpisodes.Name = "pnlEpisodes";
            this.pnlEpisodes.Size = new System.Drawing.Size(630, 200);
            this.pnlEpisodes.Visible = false;

            // lblEpisodes
            this.lblEpisodes.AutoSize = true;
            this.lblEpisodes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEpisodes.Location = new System.Drawing.Point(10, 10);
            this.lblEpisodes.Name = "lblEpisodes";
            this.lblEpisodes.Size = new System.Drawing.Size(70, 20);
            this.lblEpisodes.Text = "Episodes:";

            // dgvEpisodes
            this.dgvEpisodes.AllowUserToAddRows = false;
            this.dgvEpisodes.AllowUserToDeleteRows = false;
            this.dgvEpisodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEpisodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colEpisodeNumber,
                this.colEpisodeTitle,
                this.colEpisodeDuration,
                this.colWatched
            });
            this.dgvEpisodes.Location = new System.Drawing.Point(10, 40);
            this.dgvEpisodes.Name = "dgvEpisodes";
            this.dgvEpisodes.ReadOnly = true;
            this.dgvEpisodes.RowHeadersWidth = 51;
            this.dgvEpisodes.Size = new System.Drawing.Size(610, 150);
            this.dgvEpisodes.TabIndex = 0;

            // colEpisodeNumber
            this.colEpisodeNumber.HeaderText = "Number";
            this.colEpisodeNumber.MinimumWidth = 6;
            this.colEpisodeNumber.Name = "colEpisodeNumber";
            this.colEpisodeNumber.ReadOnly = true;
            this.colEpisodeNumber.Width = 70;

            // colEpisodeTitle
            this.colEpisodeTitle.HeaderText = "Title";
            this.colEpisodeTitle.MinimumWidth = 6;
            this.colEpisodeTitle.Name = "colEpisodeTitle";
            this.colEpisodeTitle.ReadOnly = true;
            this.colEpisodeTitle.Width = 250;

            // colEpisodeDuration
            this.colEpisodeDuration.HeaderText = "Duration";
            this.colEpisodeDuration.MinimumWidth = 6;
            this.colEpisodeDuration.Name = "colEpisodeDuration";
            this.colEpisodeDuration.ReadOnly = true;
            this.colEpisodeDuration.Width = 80;

            // colWatched
            this.colWatched.HeaderText = "Watched";
            this.colWatched.MinimumWidth = 6;
            this.colWatched.Name = "colWatched";
            this.colWatched.ReadOnly = true;
            this.colWatched.Width = 80;

            // btnMarkAsWatched
            this.btnMarkAsWatched.Location = new System.Drawing.Point(20, 590);
            this.btnMarkAsWatched.Name = "btnMarkAsWatched";
            this.btnMarkAsWatched.Size = new System.Drawing.Size(150, 35);
            this.btnMarkAsWatched.Text = "Mark as Watched";
            this.btnMarkAsWatched.UseVisualStyleBackColor = true;

            // btnRate
            this.btnRate.Location = new System.Drawing.Point(180, 590);
            this.btnRate.Name = "btnRate";
            this.btnRate.Size = new System.Drawing.Size(150, 35);
            this.btnRate.Text = "Rate Content";
            this.btnRate.UseVisualStyleBackColor = true;

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(500, 590);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 35);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;

            // ContentDetailsForm
            this.ClientSize = new System.Drawing.Size(680, 650);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.lblReleaseYear);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.picContentImage);
            this.Controls.Add(this.pnlEpisodes);
            this.Controls.Add(this.btnMarkAsWatched);
            this.Controls.Add(this.btnRate);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContentDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Content Details";
        }

        private void LoadContentDetails()
        {
            // Set content details
            lblTitle.Text = _content.Title;
            txtDescription.Text = _content.Description;
            lblGenre.Text = $"Genre: {_content.Genre}";
            lblReleaseYear.Text = $"Release Year: {_content.ReleaseYear}";
            lblPlatform.Text = $"Platform: {_content.Platform}";

            // Set rating
            if (_content.Ratings != null)
            {
                lblRating.Text = $"Rating: {_content.AverageRating}/5 ({_content.Ratings.Count} ratings)";
            }
            else
            {
                lblRating.Text = "Rating: No ratings yet";
            }

            // Set duration or episodes
            if (_content.Type == "Movie")
            {
                lblDuration.Text = $"Duration: {_content.Duration} min";
                pnlEpisodes.Visible = false;
                this.ClientSize = new Size(680, 450);
                btnMarkAsWatched.Location = new Point(20, 390);
                btnRate.Location = new Point(180, 390);
                btnClose.Location = new Point(500, 390);
            }
            else
            {
                lblDuration.Text = $"Episodes: {_content.Episodes.Count}";
                pnlEpisodes.Visible = true;
                LoadEpisodes();
            }

            // Load image if available
            if (!string.IsNullOrEmpty(_content.ImagePath))
            {
                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _content.ImagePath);
                if (File.Exists(fullPath))
                {
                    picContentImage.Image = Image.FromFile(fullPath);
                }
            }
        }

        private void LoadEpisodes()
        {
            dgvEpisodes.Rows.Clear();

            // Get viewing history for this user and content
            var viewingHistory = _viewingHistoryRepository.GetByUserAndContent(_currentUser.Id, _content.Id);

            foreach (var episode in _content.Episodes.OrderBy(e => e.EpisodeNumber))
            {
                bool watched = viewingHistory != null &&
                               viewingHistory.WatchedEpisodes != null &&
                               viewingHistory.WatchedEpisodes.Contains(episode.EpisodeNumber.ToString());

                dgvEpisodes.Rows.Add(episode.EpisodeNumber, episode.Title, $"{episode.Duration} min", watched);
            }
        }

        private void BtnMarkAsWatched_Click(object sender, EventArgs e)
        {
            try
            {
                if (_content.Type == "Movie")
                {
                    // Mark movie as watched
                    var viewingHistory = _viewingHistoryRepository.GetByUserAndContent(_currentUser.Id, _content.Id);

                    if (viewingHistory == null)
                    {
                        viewingHistory = new ViewingHistory
                        {
                            UserId = _currentUser.Id,
                            ContentId = _content.Id,
                            WatchDate = DateTime.Now,
                            WatchedEpisodes = ""
                        };
                        _viewingHistoryRepository.Add(viewingHistory);
                    }
                    else
                    {
                        viewingHistory.WatchDate = DateTime.Now;
                        _viewingHistoryRepository.Update(viewingHistory);
                    }

                    MessageBox.Show("Movie marked as watched!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Open episode selection form for series
                    using (var episodeSelectionForm = new EpisodeSelectionForm(_content, _currentUser))
                    {
                        if (episodeSelectionForm.ShowDialog() == DialogResult.OK)
                        {
                            LoadEpisodes(); // Refresh episodes list
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error marking content as watched: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRate_Click(object sender, EventArgs e)
        {
            using (var ratingForm = new RatingForm(_content, _currentUser))
            {
                if (ratingForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh rating display
                    lblRating.Text = $"Rating: {_content.AverageRating}/5 ({_content.Ratings.Count} ratings)";
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

