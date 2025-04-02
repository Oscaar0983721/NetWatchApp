using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class StatisticsForm : Form
    {
        private readonly User _currentUser;
        private readonly ContentRepository _contentRepository;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private readonly RatingRepository _ratingRepository;

        public StatisticsForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _contentRepository = new ContentRepository(new NetWatchDbContext());
            _viewingHistoryRepository = new ViewingHistoryRepository(new NetWatchDbContext());
            _ratingRepository = new RatingRepository(new NetWatchDbContext());

            // Load statistics
            LoadWatchingStatistics();
            LoadGenreStatistics();
            LoadRatingStatistics();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabWatching = new System.Windows.Forms.TabPage();
            this.tabGenres = new System.Windows.Forms.TabPage();
            this.tabRatings = new System.Windows.Forms.TabPage();
            this.lblTotalWatched = new System.Windows.Forms.Label();
            this.lblTotalWatchTime = new System.Windows.Forms.Label();
            this.lblRecentlyWatched = new System.Windows.Forms.Label();
            this.dgvRecentlyWatched = new System.Windows.Forms.DataGridView();
            this.lblFavoriteGenres = new System.Windows.Forms.Label();
            this.pnlGenreChart = new System.Windows.Forms.Panel();
            this.lblRatingDistribution = new System.Windows.Forms.Label();
            this.pnlRatingChart = new System.Windows.Forms.Panel();
            this.lblTopRated = new System.Windows.Forms.Label();
            this.dgvTopRated = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 37);
            this.lblTitle.Text = "Your Statistics";

            // tabControl
            this.tabControl.Controls.Add(this.tabWatching);
            this.tabControl.Controls.Add(this.tabGenres);
            this.tabControl.Controls.Add(this.tabRatings);
            this.tabControl.Location = new System.Drawing.Point(20, 70);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(760, 450);

            // tabWatching
            this.tabWatching.Controls.Add(this.lblTotalWatched);
            this.tabWatching.Controls.Add(this.lblTotalWatchTime);
            this.tabWatching.Controls.Add(this.lblRecentlyWatched);
            this.tabWatching.Controls.Add(this.dgvRecentlyWatched);
            this.tabWatching.Location = new System.Drawing.Point(4, 29);
            this.tabWatching.Name = "tabWatching";
            this.tabWatching.Padding = new System.Windows.Forms.Padding(3);
            this.tabWatching.Size = new System.Drawing.Size(752, 417);
            this.tabWatching.Text = "Watching";
            this.tabWatching.UseVisualStyleBackColor = true;

            // lblTotalWatched
            this.lblTotalWatched.AutoSize = true;
            this.lblTotalWatched.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalWatched.Location = new System.Drawing.Point(20, 20);
            this.lblTotalWatched.Name = "lblTotalWatched";
            this.lblTotalWatched.Size = new System.Drawing.Size(200, 23);
            this.lblTotalWatched.Text = "Total Watched: 0 titles";

            // lblTotalWatchTime
            this.lblTotalWatchTime.AutoSize = true;
            this.lblTotalWatchTime.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalWatchTime.Location = new System.Drawing.Point(20, 50);
            this.lblTotalWatchTime.Name = "lblTotalWatchTime";
            this.lblTotalWatchTime.Size = new System.Drawing.Size(200, 23);
            this.lblTotalWatchTime.Text = "Total Watch Time: 0 hours";

            // lblRecentlyWatched
            this.lblRecentlyWatched.AutoSize = true;
            this.lblRecentlyWatched.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRecentlyWatched.Location = new System.Drawing.Point(20, 90);
            this.lblRecentlyWatched.Name = "lblRecentlyWatched";
            this.lblRecentlyWatched.Size = new System.Drawing.Size(150, 23);
            this.lblRecentlyWatched.Text = "Recently Watched";

            // dgvRecentlyWatched
            this.dgvRecentlyWatched.AllowUserToAddRows = false;
            this.dgvRecentlyWatched.AllowUserToDeleteRows = false;
            this.dgvRecentlyWatched.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecentlyWatched.Location = new System.Drawing.Point(20, 120);
            this.dgvRecentlyWatched.Name = "dgvRecentlyWatched";
            this.dgvRecentlyWatched.ReadOnly = true;
            this.dgvRecentlyWatched.RowHeadersWidth = 51;
            this.dgvRecentlyWatched.Size = new System.Drawing.Size(710, 280);

            // tabGenres
            this.tabGenres.Controls.Add(this.lblFavoriteGenres);
            this.tabGenres.Controls.Add(this.pnlGenreChart);
            this.tabGenres.Location = new System.Drawing.Point(4, 29);
            this.tabGenres.Name = "tabGenres";
            this.tabGenres.Padding = new System.Windows.Forms.Padding(3);
            this.tabGenres.Size = new System.Drawing.Size(752, 417);
            this.tabGenres.Text = "Genres";
            this.tabGenres.UseVisualStyleBackColor = true;

            // lblFavoriteGenres
            this.lblFavoriteGenres.AutoSize = true;
            this.lblFavoriteGenres.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFavoriteGenres.Location = new System.Drawing.Point(20, 20);
            this.lblFavoriteGenres.Name = "lblFavoriteGenres";
            this.lblFavoriteGenres.Size = new System.Drawing.Size(150, 23);
            this.lblFavoriteGenres.Text = "Favorite Genres";

            // pnlGenreChart
            this.pnlGenreChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGenreChart.Location = new System.Drawing.Point(20, 50);
            this.pnlGenreChart.Name = "pnlGenreChart";
            this.pnlGenreChart.Size = new System.Drawing.Size(710, 350);

            // tabRatings
            this.tabRatings.Controls.Add(this.lblRatingDistribution);
            this.tabRatings.Controls.Add(this.pnlRatingChart);
            this.tabRatings.Controls.Add(this.lblTopRated);
            this.tabRatings.Controls.Add(this.dgvTopRated);
            this.tabRatings.Location = new System.Drawing.Point(4, 29);
            this.tabRatings.Name = "tabRatings";
            this.tabRatings.Padding = new System.Windows.Forms.Padding(3);
            this.tabRatings.Size = new System.Drawing.Size(752, 417);
            this.tabRatings.Text = "Ratings";
            this.tabRatings.UseVisualStyleBackColor = true;

            // lblRatingDistribution
            this.lblRatingDistribution.AutoSize = true;
            this.lblRatingDistribution.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblRatingDistribution.Location = new System.Drawing.Point(20, 20);
            this.lblRatingDistribution.Name = "lblRatingDistribution";
            this.lblRatingDistribution.Size = new System.Drawing.Size(150, 23);
            this.lblRatingDistribution.Text = "Your Ratings";

            // pnlRatingChart
            this.pnlRatingChart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRatingChart.Location = new System.Drawing.Point(20, 50);
            this.pnlRatingChart.Name = "pnlRatingChart";
            this.pnlRatingChart.Size = new System.Drawing.Size(710, 150);

            // lblTopRated
            this.lblTopRated.AutoSize = true;
            this.lblTopRated.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTopRated.Location = new System.Drawing.Point(20, 210);
            this.lblTopRated.Name = "lblTopRated";
            this.lblTopRated.Size = new System.Drawing.Size(150, 23);
            this.lblTopRated.Text = "Your Top Rated";

            // dgvTopRated
            this.dgvTopRated.AllowUserToAddRows = false;
            this.dgvTopRated.AllowUserToDeleteRows = false;
            this.dgvTopRated.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTopRated.Location = new System.Drawing.Point(20, 240);
            this.dgvTopRated.Name = "dgvTopRated";
            this.dgvTopRated.ReadOnly = true;
            this.dgvTopRated.RowHeadersWidth = 51;
            this.dgvTopRated.Size = new System.Drawing.Size(710, 160);

            // btnClose
            this.btnClose.Location = new System.Drawing.Point(680, 530);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 35);
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += (sender, e) => this.Close();

            // StatisticsForm
            this.ClientSize = new System.Drawing.Size(800, 580);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Your Statistics";
        }

        private void LoadWatchingStatistics()
        {
            try
            {
                // Get user's viewing history
                var viewingHistory = _viewingHistoryRepository.GetByUser(_currentUser.Id);

                // Calculate total watched
                int totalWatched = viewingHistory.Count;
                lblTotalWatched.Text = $"Total Watched: {totalWatched} titles";

                // Calculate total watch time
                int totalMinutes = 0;
                foreach (var history in viewingHistory)
                {
                    if (history.Content.Type == "Movie")
                    {
                        totalMinutes += history.Content.Duration;
                    }
                    else
                    {
                        // For series, calculate based on watched episodes
                        if (!string.IsNullOrEmpty(history.WatchedEpisodes))
                        {
                            var watchedEpisodeIds = history.WatchedEpisodes
                                .Split(',')
                                .Where(s => !string.IsNullOrEmpty(s))
                                .Select(int.Parse)
                                .ToList();

                            foreach (var episode in history.Content.Episodes)
                            {
                                if (watchedEpisodeIds.Contains(episode.EpisodeNumber))
                                {
                                    totalMinutes += episode.Duration;
                                }
                            }
                        }
                    }
                }

                double totalHours = Math.Round(totalMinutes / 60.0, 1);
                lblTotalWatchTime.Text = $"Total Watch Time: {totalHours} hours";

                // Load recently watched
                var recentlyWatched = viewingHistory
                    .OrderByDescending(vh => vh.WatchDate)
                    .Take(10)
                    .ToList();

                dgvRecentlyWatched.DataSource = recentlyWatched.Select(vh => new
                {
                    Title = vh.Content.Title,
                    Type = vh.Content.Type,
                    Genre = vh.Content.Genre,
                    Platform = vh.Content.Platform,
                    WatchDate = vh.WatchDate.ToShortDateString()
                }).ToList();

                // Auto-size columns
                dgvRecentlyWatched.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading watching statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGenreStatistics()
        {
            try
            {
                // Get user's viewing history
                var viewingHistory = _viewingHistoryRepository.GetByUser(_currentUser.Id);

                // Count genres
                var genreCounts = new Dictionary<string, int>();
                foreach (var history in viewingHistory)
                {
                    string genre = history.Content.Genre;
                    if (genreCounts.ContainsKey(genre))
                    {
                        genreCounts[genre]++;
                    }
                    else
                    {
                        genreCounts[genre] = 1;
                    }
                }

                // Sort by count
                var sortedGenres = genreCounts
                    .OrderByDescending(kv => kv.Value)
                    .ToList();

                // Draw simple bar chart
                pnlGenreChart.Paint += (sender, e) =>
                {
                    Graphics g = e.Graphics;
                    int maxCount = sortedGenres.Count > 0 ? sortedGenres.Max(kv => kv.Value) : 0;
                    int barHeight = 30;
                    int spacing = 10;
                    int maxBarWidth = pnlGenreChart.Width - 150;

                    for (int i = 0; i < Math.Min(sortedGenres.Count, 10); i++)
                    {
                        var genre = sortedGenres[i];
                        int barWidth = maxCount > 0 ? (int)((double)genre.Value / maxCount * maxBarWidth) : 0;

                        // Draw genre name
                        g.DrawString(genre.Key, this.Font, Brushes.Black, 10, i * (barHeight + spacing) + 10);

                        // Draw bar
                        g.FillRectangle(Brushes.SteelBlue, 120, i * (barHeight + spacing) + 10, barWidth, barHeight);

                        // Draw count
                        g.DrawString(genre.Value.ToString(), this.Font, Brushes.Black,
                            120 + barWidth + 5, i * (barHeight + spacing) + 10);
                    }
                };

                pnlGenreChart.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genre statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRatingStatistics()
        {
            try
            {
                // Get user's ratings
                var ratings = _ratingRepository.GetByUser(_currentUser.Id);

                // Count ratings by score
                var ratingCounts = new int[6]; // Index 0 not used, ratings are 1-5
                foreach (var rating in ratings)
                {
                    ratingCounts[rating.Score]++;
                }

                // Draw simple bar chart for rating distribution
                pnlRatingChart.Paint += (sender, e) =>
                {
                    Graphics g = e.Graphics;
                    int maxCount = ratingCounts.Max();
                    int barWidth = 50;
                    int spacing = 20;
                    int maxBarHeight = pnlRatingChart.Height - 50;

                    for (int i = 1; i <= 5; i++)
                    {
                        int barHeight = maxCount > 0 ? (int)((double)ratingCounts[i] / maxCount * maxBarHeight) : 0;

                        // Draw bar
                        g.FillRectangle(Brushes.SteelBlue,
                            50 + (i - 1) * (barWidth + spacing),
                            pnlRatingChart.Height - 30 - barHeight,
                            barWidth, barHeight);

                        // Draw rating number
                        g.DrawString(i.ToString(), this.Font, Brushes.Black,
                            50 + (i - 1) * (barWidth + spacing) + barWidth / 2 - 5,
                            pnlRatingChart.Height - 25);

                        // Draw count
                        g.DrawString(ratingCounts[i].ToString(), this.Font, Brushes.Black,
                            50 + (i - 1) * (barWidth + spacing) + barWidth / 2 - 5,
                            pnlRatingChart.Height - 40 - barHeight);
                    }
                };

                pnlRatingChart.Invalidate();

                // Load top rated content
                var topRated = ratings
                    .Where(r => r.Score >= 4) // 4 or 5 stars
                    .OrderByDescending(r => r.Score)
                    .ThenByDescending(r => r.RatingDate)
                    .Take(10)
                    .ToList();

                dgvTopRated.DataSource = topRated.Select(r => new
                {
                    Title = r.Content.Title,
                    Type = r.Content.Type,
                    Genre = r.Content.Genre,
                    Rating = r.Score,
                    RatingDate = r.RatingDate.ToShortDateString()
                }).ToList();

                // Auto-size columns
                dgvTopRated.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rating statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

