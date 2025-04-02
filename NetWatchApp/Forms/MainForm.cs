using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Classes.Services;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnAdminPanel;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.ComboBox cmbPlatform;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.DataGridView dgvContent;
        private System.Windows.Forms.Panel pnlContentDetails;
        private System.Windows.Forms.Button btnViewDetails;
        private System.Windows.Forms.Panel pnlRecommendations;
        private System.Windows.Forms.Label lblRecommendations;
        private System.Windows.Forms.FlowLayoutPanel flpRecommendations;
        private readonly User _currentUser;
        private readonly ContentRepository _contentRepository;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private readonly RecommendationService _recommendationService;
        private List<Content> _currentContents;

        public MainForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _contentRepository = new ContentRepository(new NetWatchDbContext());
            _viewingHistoryRepository = new ViewingHistoryRepository(new NetWatchDbContext());
            _recommendationService = new RecommendationService(
                _contentRepository,
                _viewingHistoryRepository,
                new RatingRepository(new NetWatchDbContext())
            );

            // Set up user info
            lblUserName.Text = $"Welcome, {_currentUser.FirstName} {_currentUser.LastName}";

            // Set up event handlers
            btnSearch.Click += BtnSearch_Click;
            txtSearch.KeyDown += TxtSearch_KeyDown;
            cmbGenre.SelectedIndexChanged += FilterChanged;
            cmbType.SelectedIndexChanged += FilterChanged;
            cmbPlatform.SelectedIndexChanged += FilterChanged;
            btnClearFilters.Click += BtnClearFilters_Click;
            dgvContent.CellDoubleClick += DgvContent_CellDoubleClick;
            btnViewDetails.Click += BtnViewDetails_Click;
            btnAdminPanel.Click += BtnAdminPanel_Click;
            btnSettings.Click += BtnSettings_Click;
            btnStatistics.Click += BtnStatistics_Click;
            btnLogout.Click += BtnLogout_Click;

            // Load initial data
            LoadGenres();
            LoadPlatforms();
            LoadContent();
            LoadRecommendations();

            // Show/hide admin panel button based on user role
            btnAdminPanel.Visible = _currentUser.IsAdmin;
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblUserName = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnAdminPanel = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblGenre = new System.Windows.Forms.Label();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.cmbPlatform = new System.Windows.Forms.ComboBox();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.dgvContent = new System.Windows.Forms.DataGridView();
            this.pnlContentDetails = new System.Windows.Forms.Panel();
            this.btnViewDetails = new System.Windows.Forms.Button();
            this.pnlRecommendations = new System.Windows.Forms.Panel();
            this.lblRecommendations = new System.Windows.Forms.Label();
            this.flpRecommendations = new System.Windows.Forms.FlowLayoutPanel();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlHeader.Controls.Add(this.lblUserName);
            this.pnlHeader.Controls.Add(this.btnSettings);
            this.pnlHeader.Controls.Add(this.btnStatistics);
            this.pnlHeader.Controls.Add(this.btnAdminPanel);
            this.pnlHeader.Controls.Add(this.btnLogout);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 60);

            // lblUserName
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUserName.Location = new System.Drawing.Point(20, 20);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(150, 23);
            this.lblUserName.Text = "Welcome, User";

            // btnSettings
            this.btnSettings.Location = new System.Drawing.Point(600, 15);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(100, 30);
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;

            // btnStatistics
            this.btnStatistics.Location = new System.Drawing.Point(710, 15);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(100, 30);
            this.btnStatistics.Text = "Statistics";
            this.btnStatistics.UseVisualStyleBackColor = true;

            // btnAdminPanel
            this.btnAdminPanel.Location = new System.Drawing.Point(820, 15);
            this.btnAdminPanel.Name = "btnAdminPanel";
            this.btnAdminPanel.Size = new System.Drawing.Size(100, 30);
            this.btnAdminPanel.Text = "Admin";
            this.btnAdminPanel.UseVisualStyleBackColor = true;

            // btnLogout
            this.btnLogout.Location = new System.Drawing.Point(930, 15);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(60, 30);
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;

            // pnlSearch
            this.pnlSearch.Controls.Add(this.lblSearch);
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 60);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1000, 50);

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(20, 15);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(56, 20);
            this.lblSearch.Text = "Search:";

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(80, 12);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 27);

            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(390, 10);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 30);
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;

            // pnlFilters
            this.pnlFilters.Controls.Add(this.lblGenre);
            this.pnlFilters.Controls.Add(this.cmbGenre);
            this.pnlFilters.Controls.Add(this.lblType);
            this.pnlFilters.Controls.Add(this.cmbType);
            this.pnlFilters.Controls.Add(this.lblPlatform);
            this.pnlFilters.Controls.Add(this.cmbPlatform);
            this.pnlFilters.Controls.Add(this.btnClearFilters);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 110);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1000, 50);

            // lblGenre
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(20, 15);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(48, 20);
            this.lblGenre.Text = "Genre:";

            // cmbGenre
            this.cmbGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Location = new System.Drawing.Point(70, 12);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(150, 28);

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(230, 15);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(43, 20);
            this.lblType.Text = "Type:";

            // cmbType
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] { "", "Movie", "Series" });
            this.cmbType.Location = new System.Drawing.Point(280, 12);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(150, 28);

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point(440, 15);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(68, 20);
            this.lblPlatform.Text = "Platform:";

            // cmbPlatform
            this.cmbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlatform.FormattingEnabled = true;
            this.cmbPlatform.Location = new System.Drawing.Point(510, 12);
            this.cmbPlatform.Name = "cmbPlatform";
            this.cmbPlatform.Size = new System.Drawing.Size(150, 28);

            // btnClearFilters
            this.btnClearFilters.Location = new System.Drawing.Point(670, 10);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(100, 30);
            this.btnClearFilters.Text = "Clear Filters";
            this.btnClearFilters.UseVisualStyleBackColor = true;

            // dgvContent
            this.dgvContent.AllowUserToAddRows = false;
            this.dgvContent.AllowUserToDeleteRows = false;
            this.dgvContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContent.Location = new System.Drawing.Point(20, 170);
            this.dgvContent.MultiSelect = false;
            this.dgvContent.Name = "dgvContent";
            this.dgvContent.ReadOnly = true;
            this.dgvContent.RowHeadersWidth = 51;
            this.dgvContent.RowTemplate.Height = 29;
            this.dgvContent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContent.Size = new System.Drawing.Size(750, 400);

            // pnlContentDetails
            this.pnlContentDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlContentDetails.Controls.Add(this.btnViewDetails);
            this.pnlContentDetails.Location = new System.Drawing.Point(20, 580);
            this.pnlContentDetails.Name = "pnlContentDetails";
            this.pnlContentDetails.Size = new System.Drawing.Size(750, 50);

            // btnViewDetails
            this.btnViewDetails.Location = new System.Drawing.Point(10, 10);
            this.btnViewDetails.Name = "btnViewDetails";
            this.btnViewDetails.Size = new System.Drawing.Size(120, 30);
            this.btnViewDetails.Text = "View Details";
            this.btnViewDetails.UseVisualStyleBackColor = true;

            // pnlRecommendations
            this.pnlRecommendations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlRecommendations.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlRecommendations.Controls.Add(this.lblRecommendations);
            this.pnlRecommendations.Controls.Add(this.flpRecommendations);
            this.pnlRecommendations.Location = new System.Drawing.Point(780, 170);
            this.pnlRecommendations.Name = "pnlRecommendations";
            this.pnlRecommendations.Size = new System.Drawing.Size(200, 460);

            // lblRecommendations
            this.lblRecommendations.AutoSize = true;
            this.lblRecommendations.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblRecommendations.Location = new System.Drawing.Point(10, 10);
            this.lblRecommendations.Name = "lblRecommendations";
            this.lblRecommendations.Size = new System.Drawing.Size(134, 20);
            this.lblRecommendations.Text = "Recommendations";

            // flpRecommendations
            this.flpRecommendations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpRecommendations.AutoScroll = true;
            this.flpRecommendations.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpRecommendations.Location = new System.Drawing.Point(10, 40);
            this.flpRecommendations.Name = "flpRecommendations";
            this.flpRecommendations.Size = new System.Drawing.Size(178, 408);
            this.flpRecommendations.TabIndex = 0;
            this.flpRecommendations.WrapContents = false;

            // MainForm
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.dgvContent);
            this.Controls.Add(this.pnlContentDetails);
            this.Controls.Add(this.pnlRecommendations);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetWatch - Main";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
        }

        private void LoadGenres()
        {
            try
            {
                var genres = _contentRepository.GetAllGenres();
                cmbGenre.Items.Clear();
                cmbGenre.Items.Add("");
                foreach (var genre in genres)
                {
                    cmbGenre.Items.Add(genre);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading genres: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPlatforms()
        {
            try
            {
                var platforms = _contentRepository.GetAllPlatforms();
                cmbPlatform.Items.Clear();
                cmbPlatform.Items.Add("");
                foreach (var platform in platforms)
                {
                    cmbPlatform.Items.Add(platform);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading platforms: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadContent()
        {
            try
            {
                string searchTerm = txtSearch.Text.Trim();
                string genre = cmbGenre.SelectedItem?.ToString() ?? "";
                string type = cmbType.SelectedItem?.ToString() ?? "";
                string platform = cmbPlatform.SelectedItem?.ToString() ?? "";

                // Modificar la búsqueda para incluir el filtro de plataforma
                _currentContents = _contentRepository.Search(searchTerm, genre, type);

                // Filter by platform if selected
                if (!string.IsNullOrEmpty(platform))
                {
                    _currentContents = _currentContents.Where(c => c.Platform == platform).ToList();
                }

                DisplayContent(_currentContents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayContent(List<Content> contents)
        {
            dgvContent.DataSource = null;
            dgvContent.Columns.Clear();

            dgvContent.DataSource = contents.Select(c => new
            {
                c.Id,
                c.Title,
                c.Type,
                c.Genre,
                c.ReleaseYear,
                c.Platform,
                Duration = c.Type == "Movie" ? $"{c.Duration} min" : $"{c.Episodes.Count} episodes",
                Rating = $"{c.AverageRating}/5"
            }).ToList();

            // Hide ID column
            if (dgvContent.Columns["Id"] != null)
            {
                dgvContent.Columns["Id"].Visible = false;
            }

            // Auto-size columns
            dgvContent.AutoResizeColumns();
        }

        private void LoadRecommendations()
        {
            try
            {
                flpRecommendations.Controls.Clear();

                var recommendations = _recommendationService.GetRecommendedContent(_currentUser.Id, 5);

                foreach (var content in recommendations)
                {
                    var panel = new Panel
                    {
                        Width = 160,
                        Height = 80,
                        BorderStyle = BorderStyle.FixedSingle,
                        Margin = new Padding(3, 3, 3, 10)
                    };

                    var titleLabel = new Label
                    {
                        Text = content.Title,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        Location = new Point(5, 5),
                        Width = 150,
                        Height = 40
                    };

                    var typeLabel = new Label
                    {
                        Text = $"{content.Type} - {content.Genre}",
                        Location = new Point(5, 45),
                        Width = 150,
                        Height = 20,
                        ForeColor = Color.Gray
                    };

                    panel.Controls.Add(titleLabel);
                    panel.Controls.Add(typeLabel);

                    panel.Click += (sender, e) => OpenContentDetails(content);
                    titleLabel.Click += (sender, e) => OpenContentDetails(content);
                    typeLabel.Click += (sender, e) => OpenContentDetails(content);

                    flpRecommendations.Controls.Add(panel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recommendations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OpenContentDetails(Content content)
        {
            using (var detailsForm = new ContentDetailsForm(content, _currentUser))
            {
                if (detailsForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh content and recommendations after viewing details
                    LoadContent();
                    LoadRecommendations();
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadContent();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();
            cmbGenre.SelectedIndex = -1;
            cmbType.SelectedIndex = -1;
            cmbPlatform.SelectedIndex = -1;
            LoadContent();
        }

        private void DgvContent_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int contentId = Convert.ToInt32(dgvContent.Rows[e.RowIndex].Cells["Id"].Value);
                var content = _currentContents.FirstOrDefault(c => c.Id == contentId);

                if (content != null)
                {
                    OpenContentDetails(content);
                }
            }
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a content to view details.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells["Id"].Value);
            var content = _currentContents.FirstOrDefault(c => c.Id == contentId);

            if (content != null)
            {
                OpenContentDetails(content);
            }
        }

        private void BtnAdminPanel_Click(object sender, EventArgs e)
        {
            if (_currentUser.IsAdmin)
            {
                using (var adminPanelForm = new AdminPanelForm(_currentUser))
                {
                    adminPanelForm.ShowDialog();

                    // Refresh content after admin panel is closed
                    LoadContent();
                    LoadRecommendations();
                }
            }
            else
            {
                MessageBox.Show("You do not have permission to access the admin panel.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(_currentUser))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh user info
                    lblUserName.Text = $"Welcome, {_currentUser.FirstName} {_currentUser.LastName}";
                }
            }
        }

        private void BtnStatistics_Click(object sender, EventArgs e)
        {
            using (var statisticsForm = new StatisticsForm(_currentUser))
            {
                statisticsForm.ShowDialog();
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}

