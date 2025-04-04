using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class ContentDetailsForm : Form
    {
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTitleValue;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblReleaseYear;
        private System.Windows.Forms.Label lblReleaseYearValue;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblGenreValue;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblTypeValue;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.Label lblPlatformValue;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblDurationValue;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.Label lblRatingValue;
        private System.Windows.Forms.PictureBox picContentImage;
        private System.Windows.Forms.Panel pnlEpisodes;
        private System.Windows.Forms.Label lblEpisodes;
        private System.Windows.Forms.DataGridView dgvEpisodes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEpisodeDuration;
        private System.Windows.Forms.Button btnRate;
        private System.Windows.Forms.Button btnMarkWatched;
        private System.Windows.Forms.Button btnClose;
        private System.ComponentModel.IContainer components = null;

        private readonly Content _content;
        private readonly User _currentUser;
        private readonly RatingRepository _ratingRepository;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;

        public ContentDetailsForm(Content content, User currentUser)
        {
            InitializeComponent();
            _content = content;
            _currentUser = currentUser;
            _ratingRepository = new RatingRepository(new Data.EntityFramework.NetWatchDbContext());
            _viewingHistoryRepository = new ViewingHistoryRepository(new Data.EntityFramework.NetWatchDbContext());

            // Load content details
            LoadContentDetails();

            // Set up event handlers
            btnRate.Click += BtnRate_Click;
            btnMarkWatched.Click += BtnMarkWatched_Click;
            btnClose.Click += BtnClose_Click;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitleValue = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblReleaseYear = new System.Windows.Forms.Label();
            this.lblReleaseYearValue = new System.Windows.Forms.Label();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblGenreValue = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblTypeValue = new System.Windows.Forms.Label();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.lblPlatformValue = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblDurationValue = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();
            this.lblRatingValue = new System.Windows.Forms.Label();
            this.picContentImage = new System.Windows.Forms.PictureBox();
            this.pnlEpisodes = new System.Windows.Forms.Panel();
            this.lblEpisodes = new System.Windows.Forms.Label();
            this.dgvEpisodes = new System.Windows.Forms.DataGridView();
            this.colEpisodeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRate = new System.Windows.Forms.Button();
            this.btnMarkWatched = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.picContentImage)).BeginInit();
            this.pnlEpisodes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEpisodes)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(38, 20);
            this.lblTitle.Text = "Title:";

            // lblTitleValue
            this.lblTitleValue.AutoSize = true;
            this.lblTitleValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitleValue.Location = new System.Drawing.Point(150, 20);
            this.lblTitleValue.Name = "lblTitleValue";
            this.lblTitleValue.Size = new System.Drawing.Size(120, 28);
            this.lblTitleValue.Text = "Content Title";

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDescription.Location = new System.Drawing.Point(20, 60);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(85, 20);
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(150, 60);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(400, 100);
            this.txtDescription.TabIndex = 0;

            // lblReleaseYear
            this.lblReleaseYear.AutoSize = true;
            this.lblReleaseYear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblReleaseYear.Location = new System.Drawing.Point(20, 170);
            this.lblReleaseYear.Name = "lblReleaseYear";
            this.lblReleaseYear.Size = new System.Drawing.Size(93, 20);
            this.lblReleaseYear.Text = "Release Year:";

            // lblReleaseYearValue
            this.lblReleaseYearValue.AutoSize = true;
            this.lblReleaseYearValue.Location = new System.Drawing.Point(150, 170);
            this.lblReleaseYearValue.Name = "lblReleaseYearValue";
            this.lblReleaseYearValue.Size = new System.Drawing.Size(40, 20);
            this.lblReleaseYearValue.Text = "2023";

            // lblGenre
            this.lblGenre.AutoSize = true;
            this.lblGenre.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblGenre.Location = new System.Drawing.Point(20, 200);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(48, 20);
            this.lblGenre.Text = "Genre:";

            // lblGenreValue
            this.lblGenreValue.AutoSize = true;
            this.lblGenreValue.Location = new System.Drawing.Point(150, 200);
            this.lblGenreValue.Name = "lblGenreValue";
            this.lblGenreValue.Size = new System.Drawing.Size(50, 20);
            this.lblGenreValue.Text = "Action";

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblType.Location = new System.Drawing.Point(20, 230);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(43, 20);
            this.lblType.Text = "Type:";

            // lblTypeValue
            this.lblTypeValue.AutoSize = true;
            this.lblTypeValue.Location = new System.Drawing.Point(150, 230);
            this.lblTypeValue.Name = "lblTypeValue";
            this.lblTypeValue.Size = new System.Drawing.Size(50, 20);
            this.lblTypeValue.Text = "Movie";

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPlatform.Location = new System.Drawing.Point(20, 260);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(68, 20);
            this.lblPlatform.Text = "Platform:";

            // lblPlatformValue
            this.lblPlatformValue.AutoSize = true;
            this.lblPlatformValue.Location = new System.Drawing.Point(150, 260);
            this.lblPlatformValue.Name = "lblPlatformValue";
            this.lblPlatformValue.Size = new System.Drawing.Size(60, 20);
            this.lblPlatformValue.Text = "Netflix";

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDuration.Location = new System.Drawing.Point(20, 290);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(70, 20);
            this.lblDuration.Text = "Duration:";

            // lblDurationValue
            this.lblDurationValue.AutoSize = true;
            this.lblDurationValue.Location = new System.Drawing.Point(150, 290);
            this.lblDurationValue.Name = "lblDurationValue";
            this.lblDurationValue.Size = new System.Drawing.Size(80, 20);
            this.lblDurationValue.Text = "120 minutes";

            // lblRating
            this.lblRating.AutoSize = true;
            this.lblRating.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRating.Location = new System.Drawing.Point(20, 320);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(52, 20);
            this.lblRating.Text = "Rating:";

            // lblRatingValue
            this.lblRatingValue.AutoSize = true;
            this.lblRatingValue.Location = new System.Drawing.Point(150, 320);
            this.lblRatingValue.Name = "lblRatingValue";
            this.lblRatingValue.Size = new System.Drawing.Size(60, 20);
            this.lblRatingValue.Text = "4.5 / 5.0";

            // picContentImage
            this.picContentImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picContentImage.Location = new System.Drawing.Point(580, 20);
            this.picContentImage.Name = "picContentImage";
            this.picContentImage.Size = new System.Drawing.Size(200, 300);
            this.picContentImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picContentImage.TabIndex = 0;
            this.picContentImage.TabStop = false;

            // pnlEpisodes
            this.pnlEpisodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEpisodes.Controls.Add(this.lblEpisodes);
            this.pnlEpisodes.Controls.Add(this.dgvEpisodes);
            this.pnlEpisodes.Location = new System.Drawing.Point(20, 360);
            this.pnlEpisodes.Name = "pnlEpisodes";
            this.pnlEpisodes.Size = new System.Drawing.Size(760, 200);
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
                this.colEpisodeDuration
            });
            this.dgvEpisodes.Location = new System.Drawing.Point(10, 40);
            this.dgvEpisodes.Name = "dgvEpisodes";
            this.dgvEpisodes.ReadOnly = true;
            this.dgvEpisodes.RowHeadersWidth = 51;
            this.dgvEpisodes.Size = new System.Drawing.Size(740, 150);
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
            this.colEpisodeTitle.Width = 400;

            // colEpisodeDuration
            this.colEpisodeDuration.HeaderText = "Duration";
            this.colEpisodeDuration.MinimumWidth = 6;
            this.colEpisodeDuration.Name = "colEpisodeDuration";
            this.colEpisodeDuration.ReadOnly = true;
            this.colEpisodeDuration.Width = 80;

            // btnRate
            this.btnRate.Location = new System.Drawing.Point(150, 580);
            this.btnRate.Name = "btnRate";
            this.btnRate.Size = new System.Drawing.Size(150, 35);
            this.btnRate.Text = "Rate Content";
            this.btnRate.UseVisualStyleBackColor = true;

            // btnMarkWatched
            this.btnMarkWatched.Location = new System.Drawing.Point(320, 580);
            this.btnMarkWatched.Name = "btnMarkWatched";
            this.btnMarkWatched.Size = new System.Drawing.Size(150, 35);
            this.btnMarkWatched.Text = "Mark as Watched";
            this.btnMarkWatched.UseVisualStyleBackColor = true;

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(490, 580);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 35);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;

            // ContentDetailsForm
            this.ClientSize = new System.Drawing.Size(800, 630);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblTitleValue);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblReleaseYear);
            this.Controls.Add(this.lblReleaseYearValue);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.lblGenreValue);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.lblTypeValue);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.lblPlatformValue);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.lblDurationValue);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.lblRatingValue);
            this.Controls.Add(this.picContentImage);
            this.Controls.Add(this.pnlEpisodes);
            this.Controls.Add(this.btnRate);
            this.Controls.Add(this.btnMarkWatched);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ContentDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Content Details";

            ((System.ComponentModel.ISupportInitialize)(this.picContentImage)).EndInit();
            this.pnlEpisodes.ResumeLayout(false);
            this.pnlEpisodes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEpisodes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadContentDetails()
        {
            // Set content details
            lblTitleValue.Text = _content.Title;
            txtDescription.Text = _content.Description;
            lblReleaseYearValue.Text = _content.ReleaseYear.ToString();
            lblGenreValue.Text = _content.Genre;
            lblTypeValue.Text = _content.Type;
            lblPlatformValue.Text = _content.Platform;

            // Set duration or show episodes based on content type
            if (_content.Type == "Movie")
            {
                lblDuration.Visible = true;
                lblDurationValue.Visible = true;
                lblDurationValue.Text = $"{_content.Duration} minutes";
                pnlEpisodes.Visible = false;

                // Adjust form height
                this.ClientSize = new Size(800, 630);
                btnRate.Location = new Point(150, 580);
                btnMarkWatched.Location = new Point(320, 580);
                btnClose.Location = new Point(490, 580);

                // Set button text
                btnMarkWatched.Text = "Mark as Watched";
            }
            else
            {
                lblDuration.Visible = false;
                lblDurationValue.Visible = false;
                pnlEpisodes.Visible = true;

                // Load episodes
                LoadEpisodes();

                // Adjust form height
                this.ClientSize = new Size(800, 630);
                btnRate.Location = new Point(150, 580);
                btnMarkWatched.Location = new Point(320, 580);
                btnClose.Location = new Point(490, 580);

                // Set button text
                btnMarkWatched.Text = "Select Episodes";
            }

            // Load rating
            double averageRating = _ratingRepository.GetAverageRatingForContent(_content.Id);
            lblRatingValue.Text = $"{averageRating:F1} / 5.0";

            // Load image if available
            if (!string.IsNullOrEmpty(_content.ImagePath))
            {
                try
                {
                    using (var httpClient = new System.Net.Http.HttpClient())
                    {
                        var imageData = httpClient.GetByteArrayAsync(_content.ImagePath).Result;
                        using (var ms = new System.IO.MemoryStream(imageData))
                        {
                            picContentImage.Image = Image.FromStream(ms);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading image: {ex.Message}", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoadEpisodes()
        {
            dgvEpisodes.Rows.Clear();
            foreach (var episode in _content.Episodes.OrderBy(e => e.EpisodeNumber))
            {
                dgvEpisodes.Rows.Add(episode.EpisodeNumber, episode.Title, $"{episode.Duration} min");
            }
        }

        private void BtnRate_Click(object sender, EventArgs e)
        {
            using (var ratingForm = new RatingForm(_content, _currentUser))
            {
                if (ratingForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh rating display
                    double averageRating = _ratingRepository.GetAverageRatingForContent(_content.Id);
                    lblRatingValue.Text = $"{averageRating:F1} / 5.0";
                }
            }
        }

        private void BtnMarkWatched_Click(object sender, EventArgs e)
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
                        WatchedEpisodes = string.Empty
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
                using (var episodeForm = new EpisodeSelectionForm(_content, _currentUser))
                {
                    episodeForm.ShowDialog();
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                if (picContentImage != null && picContentImage.Image != null)
                {
                    picContentImage.Image.Dispose();
                    picContentImage.Image = null;
                }
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}

