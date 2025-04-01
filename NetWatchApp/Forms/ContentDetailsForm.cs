using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class ContentDetailsForm : Form
    {
        private readonly Content _content;
        private readonly User _currentUser;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private readonly RatingRepository _ratingRepository;

        public ContentDetailsForm(Content content, User currentUser)
        {
            InitializeComponent();
            _content = content;
            _currentUser = currentUser;
            _viewingHistoryRepository = new ViewingHistoryRepository(new NetWatchDbContext());
            _ratingRepository = new RatingRepository(new NetWatchDbContext());

            LoadContentDetails();
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblReleaseYear = new System.Windows.Forms.Label();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.lblGenre = new System.Windows.Forms.Label();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.btnWatch = new System.Windows.Forms.Button();
            this.btnRate = new System.Windows.Forms.Button();
            this.pnlEpisodes = new System.Windows.Forms.Panel();
            this.lblEpisodes = new System.Windows.Forms.Label();
            this.flpEpisodes = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeader.SuspendLayout();
            this.pnlDetails.SuspendLayout();
            this.pnlEpisodes.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.  ((int)(((byte)(45)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Controls.Add(this.lblReleaseYear);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(800, 60);
            this.pnlHeader.TabIndex = 0;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";

            // lblReleaseYear
            this.lblReleaseYear.AutoSize = true;
            this.lblReleaseYear.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblReleaseYear.ForeColor = System.Drawing.Color.White;
            this.lblReleaseYear.Location = new System.Drawing.Point(700, 20);
            this.lblReleaseYear.Name = "lblReleaseYear";
            this.lblReleaseYear.Size = new System.Drawing.Size(50, 21);
            this.lblReleaseYear.TabIndex = 1;
            this.lblReleaseYear.Text = "2023";

            // pnlImage
            this.pnlImage.BackColor = System.Drawing.Color.Gray;
            this.pnlImage.Location = new System.Drawing.Point(20, 80);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Size = new System.Drawing.Size(200, 300);
            this.pnlImage.TabIndex = 1;

            // pnlDetails
            this.pnlDetails.Location = new System.Drawing.Point(240, 80);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(540, 300);
            this.pnlDetails.TabIndex = 2;
            this.pnlDetails.Controls.Add(this.lblGenre);
            this.pnlDetails.Controls.Add(this.lblPlatform);
            this.pnlDetails.Controls.Add(this.lblDuration);
            this.pnlDetails.Controls.Add(this.lblDescription);
            this.pnlDetails.Controls.Add(this.btnWatch);
            this.pnlDetails.Controls.Add(this.btnRate);

            // lblGenre
            this.lblGenre.AutoSize = true;
            this.lblGenre.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGenre.Location = new System.Drawing.Point(0, 10);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(50, 19);
            this.lblGenre.TabIndex = 0;
            this.lblGenre.Text = "Genre: Action";

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblPlatform.Location = new System.Drawing.Point(0, 40);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(100, 19);
            this.lblPlatform.TabIndex = 1;
            this.lblPlatform.Text = "Platform: Netflix";

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDuration.Location = new System.Drawing.Point(0, 70);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(100, 19);
            this.lblDuration.TabIndex = 2;
            this.lblDuration.Text = "Duration: 120 min";

            // lblDescription
            this.lblDescription.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblDescription.Location = new System.Drawing.Point(0, 100);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(540, 100);
            this.lblDescription.TabIndex = 3;
            this.lblDescription.Text = "Description";

            // btnWatch
            this.btnWatch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnWatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatch.ForeColor = System.Drawing.Color.White;
            this.btnWatch.Location = new System.Drawing.Point(0, 220);
            this.btnWatch.Name = "btnWatch";
            this.btnWatch.Size = new System.Drawing.Size(150, 40);
            this.btnWatch.TabIndex = 4;
            this.btnWatch.Text = "Watch Now";
            this.btnWatch.UseVisualStyleBackColor = false;
            this.btnWatch.Click += new System.EventHandler(this.btnWatch_Click);

            // btnRate
            this.btnRate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRate.ForeColor = System.Drawing.Color.White;
            this.btnRate.Location = new System.Drawing.Point(160, 220);
            this.btnRate.Name = "btnRate";
            this.btnRate.Size = new System.Drawing.Size(150, 40);
            this.btnRate.TabIndex = 5;
            this.btnRate.Text = "Rate";
            this.btnRate.UseVisualStyleBackColor = false;
            this.btnRate.Click += new System.EventHandler(this.btnRate_Click);

            // pnlEpisodes
            this.pnlEpisodes.Location = new System.Drawing.Point(20, 400);
            this.pnlEpisodes.Name = "pnlEpisodes";
            this.pnlEpisodes.Size = new System.Drawing.Size(760, 250);
            this.pnlEpisodes.TabIndex = 3;
            this.pnlEpisodes.Controls.Add(this.lblEpisodes);
            this.pnlEpisodes.Controls.Add(this.flpEpisodes);
            this.pnlEpisodes.Visible = false;

            // lblEpisodes
            this.lblEpisodes.AutoSize = true;
            this.lblEpisodes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEpisodes.Location = new System.Drawing.Point(0, 0);
            this.lblEpisodes.Name = "lblEpisodes";
            this.lblEpisodes.Size = new System.Drawing.Size(80, 21);
            this.lblEpisodes.TabIndex = 0;
            this.lblEpisodes.Text = "Episodes";

            // flpEpisodes
            this.flpEpisodes.AutoScroll = true;
            this.flpEpisodes.Location = new System.Drawing.Point(0, 30);
            this.flpEpisodes.Name = "flpEpisodes";
            this.flpEpisodes.Size = new System.Drawing.Size(760, 220);
            this.flpEpisodes.TabIndex = 1;

            // ContentDetailsForm
            this.ClientSize = new System.Drawing.Size(800, 680);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlImage);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.pnlEpisodes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ContentDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Content Details";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.pnlEpisodes.ResumeLayout(false);
            this.pnlEpisodes.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblReleaseYear;
        private System.Windows.Forms.Panel pnlImage;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnWatch;
        private System.Windows.Forms.Button btnRate;
        private System.Windows.Forms.Panel pnlEpisodes;
        private System.Windows.Forms.Label lblEpisodes;
        private System.Windows.Forms.FlowLayoutPanel flpEpisodes;

        private void LoadContentDetails()
        {
            // Set content details
            lblTitle.Text = _content.Title;
            lblReleaseYear.Text = _content.ReleaseYear.ToString();
            lblGenre.Text = $"Genre: {_content.Genre}";
            lblPlatform.Text = $"Platform: {_content.Platform}";
            lblDuration.Text = $"Duration: {_content.DurationMinutes} min";
            lblDescription.Text = _content.Description;

            // Show episodes panel for series
            if (_content.Type == ContentType.Series && _content.Episodes != null && _content.Episodes.Any())
            {
                pnlEpisodes.Visible = true;

                // Group episodes by season
                var episodesBySeason = _content.Episodes
                    .OrderBy(e => e.SeasonNumber)
                    .ThenBy(e => e.EpisodeNumber)
                    .GroupBy(e => e.SeasonNumber);

                foreach (var season in episodesBySeason)
                {
                    // Add season header
                    Label lblSeason = new Label
                    {
                        Text = $"Season {season.Key}",
                        Font = new Font("Segoe UI", 11, FontStyle.Bold),
                        AutoSize = true,
                        Margin = new Padding(0, 10, 0, 5),
                        Width = 740
                    };

                    flpEpisodes.Controls.Add(lblSeason);

                    // Add episodes
                    foreach (var episode in season)
                    {
                        Panel pnlEpisode = new Panel
                        {
                            Width = 740,
                            Height = 40,
                            Margin = new Padding(0, 2, 0, 2),
                            BorderStyle = BorderStyle.FixedSingle
                        };

                        Label lblEpisodeTitle = new Label
                        {
                            Text = $"Episode {episode.EpisodeNumber}: {episode.Title}",
                            Location = new Point(10, 10),
                            AutoSize = true
                        };

                        Button btnWatchEpisode = new Button
                        {
                            Text = "Watch",
                            Size = new Size(80, 25),
                            Location = new Point(650, 7),
                            BackColor = Color.FromArgb(0, 122, 204),
                            ForeColor = Color.White,
                            FlatStyle = FlatStyle.Flat
                        };

                        btnWatchEpisode.Click += (sender, e) => WatchEpisode(episode);

                        pnlEpisode.Controls.Add(lblEpisodeTitle);
                        pnlEpisode.Controls.Add(btnWatchEpisode);

                        flpEpisodes.Controls.Add(pnlEpisode);
                    }
                }
            }
        }

        private async void btnWatch_Click(object sender, EventArgs e)
        {
            if (_content.Type == ContentType.Movie)
            {
                // Create viewing history for movie
                var viewingHistory = new ViewingHistory
                {
                    UserId = _currentUser.Id,
                    ContentId = _content.Id,
                    ViewDate = DateTime.Now,
                    WatchedMinutes = _content.DurationMinutes,
                    Completed = true
                };

                await _viewingHistoryRepository.AddAsync(viewingHistory);

                MessageBox.Show("Enjoy your movie!", "Watch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // For series, scroll to episodes section
                pnlEpisodes.Focus();
            }
        }

        private async void WatchEpisode(Episode episode)
        {
            // Create viewing history for episode
            var viewingHistory = new ViewingHistory
            {
                UserId = _currentUser.Id,
                ContentId = _content.Id,
                EpisodeId = episode.Id,
                ViewDate = DateTime.Now,
                WatchedMinutes = episode.DurationMinutes,
                Completed = true
            };

            await _viewingHistoryRepository.AddAsync(viewingHistory);

            MessageBox.Show($"Enjoy watching {_content.Title} - Season {episode.SeasonNumber}, Episode {episode.EpisodeNumber}!", "Watch", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnRate_Click(object sender, EventArgs e)
        {
            RatingForm ratingForm = new RatingForm(_content, _currentUser);
            ratingForm.ShowDialog();
        }
    }
}