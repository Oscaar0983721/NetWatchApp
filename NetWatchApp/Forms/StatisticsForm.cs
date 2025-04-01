using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class StatisticsForm : Form
    {
        private readonly User _currentUser;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private readonly ContentRepository _contentRepository;

        public StatisticsForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _viewingHistoryRepository = new ViewingHistoryRepository(new NetWatchDbContext());
            _contentRepository = new ContentRepository(new NetWatchDbContext());

            LoadStatistics();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlWatchTime = new System.Windows.Forms.Panel();
            this.lblWatchTimeTitle = new System.Windows.Forms.Label();
            this.lblTotalWatchTime = new System.Windows.Forms.Label();
            this.lblMoviesWatched = new System.Windows.Forms.Label();
            this.lblEpisodesWatched = new System.Windows.Forms.Label();
            this.pnlGenres = new System.Windows.Forms.Panel();
            this.lblGenresTitle = new System.Windows.Forms.Label();
            this.flpGenres = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlPlatforms = new System.Windows.Forms.Panel();
            this.lblPlatformsTitle = new System.Windows.Forms.Label();
            this.flpPlatforms = new System.Windows.Forms.FlowLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlWatchTime.SuspendLayout();
            this.pnlGenres.SuspendLayout();
            this.pnlPlatforms.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Your Statistics";

            // pnlWatchTime
            this.pnlWatchTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlWatchTime.Location = new System.Drawing.Point(20, 70);
            this.pnlWatchTime.Name = "pnlWatchTime";
            this.pnlWatchTime.Size = new System.Drawing.Size(360, 150);
            this.pnlWatchTime.TabIndex = 1;
            this.pnlWatchTime.Controls.Add(this.lblWatchTimeTitle);
            this.pnlWatchTime.Controls.Add(this.lblTotalWatchTime);
            this.pnlWatchTime.Controls.Add(this.lblMoviesWatched);
            this.pnlWatchTime.Controls.Add(this.lblEpisodesWatched);

            // lblWatchTimeTitle
            this.lblWatchTimeTitle.AutoSize = true;
            this.lblWatchTimeTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblWatchTimeTitle.Location = new System.Drawing.Point(10, 10);
            this.lblWatchTimeTitle.Name = "lblWatchTimeTitle";
            this.lblWatchTimeTitle.Size = new System.Drawing.Size(150, 21);
            this.lblWatchTimeTitle.TabIndex = 0;
            this.lblWatchTimeTitle.Text = "Watch Time";

            // lblTotalWatchTime
            this.lblTotalWatchTime.AutoSize = true;
            this.lblTotalWatchTime.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTotalWatchTime.Location = new System.Drawing.Point(10, 40);
            this.lblTotalWatchTime.Name = "lblTotalWatchTime";
            this.lblTotalWatchTime.Size = new System.Drawing.Size(150, 19);
            this.lblTotalWatchTime.TabIndex = 1;
            this.lblTotalWatchTime.Text = "Total Watch Time: 0 hours";

            // lblMoviesWatched
            this.lblMoviesWatched.AutoSize = true;
            this.lblMoviesWatched.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMoviesWatched.Location = new System.Drawing.Point(10, 70);
            this.lblMoviesWatched.Name = "lblMoviesWatched";
            this.lblMoviesWatched.Size = new System.Drawing.Size(150, 19);
            this.lblMoviesWatched.TabIndex = 2;
            this.lblMoviesWatched.Text = "Movies Watched: 0";

            // lblEpisodesWatched
            this.lblEpisodesWatched.AutoSize = true;
            this.lblEpisodesWatched.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEpisodesWatched.Location = new System.Drawing.Point(10, 100);
            this.lblEpisodesWatched.Name = "lblEpisodesWatched";
            this.lblEpisodesWatched.Size = new System.Drawing.Size(150, 19);
            this.lblEpisodesWatched.TabIndex = 3;
            this.lblEpisodesWatched.Text = "Episodes Watched: 0";

            // pnlGenres
            this.pnlGenres.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGenres.Location = new System.Drawing.Point(20, 240);
            this.pnlGenres.Name = "pnlGenres";
            this.pnlGenres.Size = new System.Drawing.Size(360, 200);
            this.pnlGenres.TabIndex = 2;
            this.pnlGenres.Controls.Add(this.lblGenresTitle);
            this.pnlGenres.Controls.Add(this.flpGenres);

            // lblGenresTitle
            this.lblGenresTitle.AutoSize = true;
            this.lblGenresTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblGenresTitle.Location = new System.Drawing.Point(10, 10);
            this.lblGenresTitle.Name = "lblGenresTitle";
            this.lblGenresTitle.Size = new System.Drawing.Size(150, 21);
            this.lblGenresTitle.TabIndex = 0;
            this.lblGenresTitle.Text = "Most Watched Genres";

            // flpGenres
            this.flpGenres.AutoScroll = true;
            this.flpGenres.Location = new System.Drawing.Point(10, 40);
            this.flpGenres.Name = "flpGenres";
            this.flpGenres.Size = new System.Drawing.Size(340, 150);
            this.flpGenres.TabIndex = 1;

            // pnlPlatforms
            this.pnlPlatforms.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlatforms.Location = new System.Drawing.Point(400, 70);
            this.pnlPlatforms.Name = "pnlPlatforms";
            this.pnlPlatforms.Size = new System.Drawing.Size(360, 200);
            this.pnlPlatforms.TabIndex = 3;
            this.pnlPlatforms.Controls.Add(this.lblPlatformsTitle);
            this.pnlPlatforms.Controls.Add(this.flpPlatforms);

            // lblPlatformsTitle
            this.lblPlatformsTitle.AutoSize = true;
            this.lblPlatformsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPlatformsTitle.Location = new System.Drawing.Point(10, 10);
            this.lblPlatformsTitle.Name = "lblPlatformsTitle";
            this.lblPlatformsTitle.Size = new System.Drawing.Size(150, 21);
            this.lblPlatformsTitle.TabIndex = 0;
            this.lblPlatformsTitle.Text = "Most Used Platforms";

            // flpPlatforms
            this.flpPlatforms.AutoScroll = true;
            this.flpPlatforms.Location = new System.Drawing.Point(10, 40);
            this.flpPlatforms.Name = "flpPlatforms";
            this.flpPlatforms.Size = new System.Drawing.Size(340, 150);
            this.flpPlatforms.TabIndex = 1;

            // btnClose
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(610, 400);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(150, 40);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);

            // StatisticsForm
            this.ClientSize = new System.Drawing.Size(780, 460);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlWatchTime);
            this.Controls.Add(this.pnlGenres);
            this.Controls.Add(this.pnlPlatforms);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "StatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Statistics";
            this.pnlWatchTime.ResumeLayout(false);
            this.pnlWatchTime.PerformLayout();
            this.pnlGenres.ResumeLayout(false);
            this.pnlGenres.PerformLayout();
            this.pnlPlatforms.ResumeLayout(false);
            this.pnlPlatforms.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlWatchTime;
        private System.Windows.Forms.Label lblWatchTimeTitle;
        private System.Windows.Forms.Label lblTotalWatchTime;
        private System.Windows.Forms.Label lblMoviesWatched;
        private System.Windows.Forms.Label lblEpisodesWatched;
        private System.Windows.Forms.Panel pnlGenres;
        private System.Windows.Forms.Label lblGenresTitle;
        private System.Windows.Forms.FlowLayoutPanel flpGenres;
        private System.Windows.Forms.Panel pnlPlatforms;
        private System.Windows.Forms.Label lblPlatformsTitle;
        private System.Windows.Forms.FlowLayoutPanel flpPlatforms;
        private System.Windows.Forms.Button btnClose;

        private async void LoadStatistics()
        {
            // Get user's viewing history
            var viewingHistory = await _viewingHistoryRepository.GetByUserIdAsync(_currentUser.Id);

            // Calculate total watch time
            int totalMinutes = viewingHistory.Sum(vh => vh.WatchedMinutes);
            int hours = totalMinutes / 60;
            int minutes = totalMinutes % 60;
            lblTotalWatchTime.Text = $"Total Watch Time: {hours} hours, {minutes} minutes";

            // Count movies and episodes watched
            int moviesWatched = viewingHistory.Count(vh => vh.EpisodeId == null);
            int episodesWatched = viewingHistory.Count(vh => vh.EpisodeId != null);
            lblMoviesWatched.Text = $"Movies Watched: {moviesWatched}";
            lblEpisodesWatched.Text = $"Episodes Watched: {episodesWatched}";

            // Get content details for all watched content
            var contentIds = viewingHistory.Select(vh => vh.ContentId).Distinct().ToList();
            var watchedContent = await Task.WhenAll(contentIds.Select(id => _contentRepository.GetByIdAsync(id)));

            // Calculate most watched genres
            var genreCounts = watchedContent
                .GroupBy(c => c.Genre)
                .Select(g => new { Genre = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToList();

            foreach (var genre in genreCounts)
            {
                Panel pnlGenre = new Panel
                {
                    Width = 320,
                    Height = 30,
                    Margin = new Padding(0, 2, 0, 2)
                };

                Label lblGenre = new Label
                {
                    Text = genre.Genre,
                    Location = new Point(0, 5),
                    AutoSize = true
                };

                Label lblCount = new Label
                {
                    Text = genre.Count.ToString(),
                    Location = new Point(280, 5),
                    AutoSize = true
                };

                pnlGenre.Controls.Add(lblGenre);
                pnlGenre.Controls.Add(lblCount);

                flpGenres.Controls.Add(pnlGenre);
            }

            // Calculate most used platforms
            var platformCounts = watchedContent
                .GroupBy(c => c.Platform)
                .Select(g => new { Platform = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToList();

            foreach (var platform in platformCounts)
            {
                Panel pnlPlatform = new Panel
                {
                    Width = 320,
                    Height = 30,
                    Margin = new Padding(0, 2, 0, 2)
                };

                Label lblPlatform = new Label
                {
                    Text = platform.Platform,
                    Location = new Point(0, 5),
                    AutoSize = true
                };

                Label lblCount = new Label
                {
                    Text = platform.Count.ToString(),
                    Location = new Point(280, 5),
                    AutoSize = true
                };

                pnlPlatform.Controls.Add(lblPlatform);
                pnlPlatform.Controls.Add(lblCount);

                flpPlatforms.Controls.Add(pnlPlatform);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}