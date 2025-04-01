using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Classes.Services;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly User _currentUser;
        private readonly ContentRepository _contentRepository;
        private readonly RecommendationService _recommendationService;

        private List<Content> _allContent;
        private List<Content> _filteredContent;

        public MainForm(User user)
        {
            InitializeComponent();
            _currentUser = user;
            _contentRepository = new ContentRepository(new NetWatchDbContext());
            _recommendationService = new RecommendationService(new NetWatchDbContext());

            // Set user name in welcome label
            lblWelcome.Text = $"Welcome, {_currentUser.Name}!";

            // Show/hide admin button based on user role
            btnAdmin.Visible = _currentUser.IsAdmin;

            // Load content
            LoadContent();
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.btnSettings = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pnlTabs = new System.Windows.Forms.Panel();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabAll = new System.Windows.Forms.TabPage();
            this.flpAllContent = new System.Windows.Forms.FlowLayoutPanel();
            this.tabMovies = new System.Windows.Forms.TabPage();
            this.flpMovies = new System.Windows.Forms.FlowLayoutPanel();
            this.tabSeries = new System.Windows.Forms.TabPage();
            this.flpSeries = new System.Windows.Forms.FlowLayoutPanel();
            this.tabRecommended = new System.Windows.Forms.TabPage();
            this.flpRecommended = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnAdmin = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            this.pnlTabs.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabAll.SuspendLayout();
            this.tabMovies.SuspendLayout();
            this.tabSeries.SuspendLayout();
            this.tabRecommended.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();

            // pnlHeader
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.pnlHeader.Controls.Add(this.lblWelcome);
            this.pnlHeader.Controls.Add(this.btnSettings);
            this.pnlHeader.Controls.Add(this.btnLogout);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 60);
            this.pnlHeader.TabIndex = 0;

            // lblWelcome
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblWelcome.ForeColor = System.Drawing.Color.White;
            this.lblWelcome.Location = new System.Drawing.Point(20, 15);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(150, 25);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Welcome, User!";

            // btnSettings
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSettings.FlatAppearance.BorderSize = 0;
            this.btnSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSettings.ForeColor = System.Drawing.Color.White;
            this.btnSettings.Location = new System.Drawing.Point(820, 10);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(80, 40);
            this.btnSettings.TabIndex = 1;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);

            // btnLogout
            this.btnLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.ForeColor = System.Drawing.Color.White;
            this.btnLogout.Location = new System.Drawing.Point(910, 10);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(80, 40);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = false;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);

            // pnlSearch
            this.pnlSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Controls.Add(this.cmbGenre);
            this.pnlSearch.Controls.Add(this.cmbType);
            this.pnlSearch.Controls.Add(this.btnSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 60);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(1000, 60);
            this.pnlSearch.TabIndex = 1;

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(20, 18);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 23);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.PlaceholderText = "Search...";

            // cmbGenre
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Location = new System.Drawing.Point(340, 18);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(150, 23);
            this.cmbGenre.TabIndex = 1;

            // cmbType
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(510, 18);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(150, 23);
            this.cmbType.TabIndex = 2;

            // btnSearch
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(680, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 23);
            this.btnSearch.TabIndex = 3;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);

            // pnlTabs
            this.pnlTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabs.Location = new System.Drawing.Point(0, 120);
            this.pnlTabs.Name = "pnlTabs";
            this.pnlTabs.Size = new System.Drawing.Size(1000, 530);
            this.pnlTabs.TabIndex = 2;
            this.pnlTabs.Controls.Add(this.tabControl);

            // tabControl
            this.tabControl.Controls.Add(this.tabAll);
            this.tabControl.Controls.Add(this.tabMovies);
            this.tabControl.Controls.Add(this.tabSeries);
            this.tabControl.Controls.Add(this.tabRecommended);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 530);
            this.tabControl.TabIndex = 0;

            // tabAll
            this.tabAll.Controls.Add(this.flpAllContent);
            this.tabAll.Location = new System.Drawing.Point(4, 24);
            this.tabAll.Name = "tabAll";
            this.tabAll.Padding = new System.Windows.Forms.Padding(3);
            this.tabAll.Size = new System.Drawing.Size(992, 502);
            this.tabAll.TabIndex = 0;
            this.tabAll.Text = "All Content";
            this.tabAll.UseVisualStyleBackColor = true;

            // flpAllContent
            this.flpAllContent.AutoScroll = true;
            this.flpAllContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpAllContent.Location = new System.Drawing.Point(3, 3);
            this.flpAllContent.Name = "flpAllContent";
            this.flpAllContent.Size = new System.Drawing.Size(986, 496);
            this.flpAllContent.TabIndex = 0;

            // tabMovies
            this.tabMovies.Controls.Add(this.flpMovies);
            this.tabMovies.Location = new System.Drawing.Point(4, 24);
            this.tabMovies.Name = "tabMovies";
            this.tabMovies.Padding = new System.Windows.Forms.Padding(3);
            this.tabMovies.Size = new System.Drawing.Size(992, 502);
            this.tabMovies.TabIndex = 1;
            this.tabMovies.Text = "Movies";
            this.tabMovies.UseVisualStyleBackColor = true;

            // flpMovies
            this.flpMovies.AutoScroll = true;
            this.flpMovies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpMovies.Location = new System.Drawing.Point(3, 3);
            this.flpMovies.Name = "flpMovies";
            this.flpMovies.Size = new System.Drawing.Size(986, 496);
            this.flpMovies.TabIndex = 0;

            // tabSeries
            this.tabSeries.Controls.Add(this.flpSeries);
            this.tabSeries.Location = new System.Drawing.Point(4, 24);
            this.tabSeries.Name = "tabSeries";
            this.tabSeries.Padding = new System.Windows.Forms.Padding(3);
            this.tabSeries.Size = new System.Drawing.Size(992, 502);
            this.tabSeries.TabIndex = 2;
            this.tabSeries.Text = "Series";
            this.tabSeries.UseVisualStyleBackColor = true;

            // flpSeries
            this.flpSeries.AutoScroll = true;
            this.flpSeries.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpSeries.Location = new System.Drawing.Point(3, 3);
            this.flpSeries.Name = "flpSeries";
            this.flpSeries.Size = new System.Drawing.Size(986, 496);
            this.flpSeries.TabIndex = 0;

            // tabRecommended
            this.tabRecommended.Controls.Add(this.flpRecommended);
            this.tabRecommended.Location = new System.Drawing.Point(4, 24);
            this.tabRecommended.Name = "tabRecommended";
            this.tabRecommended.Padding = new System.Windows.Forms.Padding(3);
            this.tabRecommended.Size = new System.Drawing.Size(992, 502);
            this.tabRecommended.TabIndex = 3;
            this.tabRecommended.Text = "Recommended";
            this.tabRecommended.UseVisualStyleBackColor = true;

            // flpRecommended
            this.flpRecommended.AutoScroll = true;
            this.flpRecommended.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpRecommended.Location = new System.Drawing.Point(3, 3);
            this.flpRecommended.Name = "flpRecommended";
            this.flpRecommended.Size = new System.Drawing.Size(986, 496);
            this.flpRecommended.TabIndex = 0;

            // pnlFooter
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pnlFooter.Controls.Add(this.btnStatistics);
            this.pnlFooter.Controls.Add(this.btnAdmin);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 650);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1000, 50);
            this.pnlFooter.TabIndex = 3;

            // btnStatistics
            this.btnStatistics.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnStatistics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStatistics.ForeColor = System.Drawing.Color.White;
            this.btnStatistics.Location = new System.Drawing.Point(20, 10);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(100, 30);
            this.btnStatistics.TabIndex = 0;
            this.btnStatistics.Text = "Statistics";
            this.btnStatistics.UseVisualStyleBackColor = false;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);

            // btnAdmin
            this.btnAdmin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdmin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnAdmin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdmin.ForeColor = System.Drawing.Color.White;
            this.btnAdmin.Location = new System.Drawing.Point(880, 10);
            this.btnAdmin.Name = "btnAdmin";
            this.btnAdmin.Size = new System.Drawing.Size(100, 30);
            this.btnAdmin.TabIndex = 1;
            this.btnAdmin.Text = "Admin Panel";
            this.btnAdmin.UseVisualStyleBackColor = false;
            this.btnAdmin.Click += new System.EventHandler(this.btnAdmin_Click);

            // MainForm
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.pnlTabs);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetWatch - Main";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            this.pnlTabs.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabAll.ResumeLayout(false);
            this.tabMovies.ResumeLayout(false);
            this.tabSeries.ResumeLayout(false);
            this.tabRecommended.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Button btnSettings;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Panel pnlSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Panel pnlTabs;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabAll;
        private System.Windows.Forms.FlowLayoutPanel flpAllContent;
        private System.Windows.Forms.TabPage tabMovies;
        private System.Windows.Forms.FlowLayoutPanel flpMovies;
        private System.Windows.Forms.TabPage tabSeries;
        private System.Windows.Forms.FlowLayoutPanel flpSeries;
        private System.Windows.Forms.TabPage tabRecommended;
        private System.Windows.Forms.FlowLayoutPanel flpRecommended;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnAdmin;

        private async void LoadContent()
        {
            try
            {
                // Clear existing content
                flpAllContent.Controls.Clear();
                flpMovies.Controls.Clear();
                flpSeries.Controls.Clear();
                flpRecommended.Controls.Clear();

                // Get all content
                _allContent = (await _contentRepository.GetAllAsync()).ToList();
                _filteredContent = _allContent;

                // Populate genre filter
                var genres = _allContent.Select(c => c.Genre).Distinct().OrderBy(g => g).ToList();
                cmbGenre.Items.Clear();
                cmbGenre.Items.Add("All Genres");
                foreach (var genre in genres)
                {
                    cmbGenre.Items.Add(genre);
                }
                cmbGenre.SelectedIndex = 0;

                // Populate type filter
                cmbType.Items.Clear();
                cmbType.Items.Add("All Types");
                cmbType.Items.Add("Movies");
                cmbType.Items.Add("Series");
                cmbType.SelectedIndex = 0;

                // Display content in tabs
                DisplayContent(_filteredContent, flpAllContent);

                var movies = _allContent.Where(c => c.Type == ContentType.Movie).ToList();
                DisplayContent(movies, flpMovies);

                var series = _allContent.Where(c => c.Type == ContentType.Series).ToList();
                DisplayContent(series, flpSeries);

                // Get recommendations
                var recommendations = await _recommendationService.GetRecommendationsForUserAsync(_currentUser.Id);
                DisplayContent(recommendations.ToList(), flpRecommended);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayContent(List<Content> contentList, FlowLayoutPanel panel)
        {
            panel.Controls.Clear();

            foreach (var content in contentList)
            {
                // Create content card
                Panel card = new Panel
                {
                    Width = 180,
                    Height = 280,
                    Margin = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Image panel
                Panel imagePanel = new Panel
                {
                    Width = 180,
                    Height = 200,
                    BackColor = Color.Gray,
                    Dock = DockStyle.Top
                };

                // Title label
                Label lblTitle = new Label
                {
                    Text = content.Title,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = 180,
                    Height = 40,
                    Location = new Point(0, 200)
                };

                // Info label
                Label lblInfo = new Label
                {
                    Text = $"{content.Type} | {content.ReleaseYear}",
                    Font = new Font("Segoe UI", 8),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Width = 180,
                    Height = 20,
                    Location = new Point(0, 240)
                };

                // Add controls to card
                card.Controls.Add(imagePanel);
                card.Controls.Add(lblTitle);
                card.Controls.Add(lblInfo);

                // Add click event to open content details
                card.Click += (sender, e) => OpenContentDetails(content);
                imagePanel.Click += (sender, e) => OpenContentDetails(content);
                lblTitle.Click += (sender, e) => OpenContentDetails(content);
                lblInfo.Click += (sender, e) => OpenContentDetails(content);

                // Add card to panel
                panel.Controls.Add(card);
            }
        }

        private void OpenContentDetails(Content content)
        {
            ContentDetailsForm detailsForm = new ContentDetailsForm(content, _currentUser);
            detailsForm.ShowDialog();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim().ToLower();
            string selectedGenre = cmbGenre.SelectedItem.ToString();
            string selectedType = cmbType.SelectedItem.ToString();

            // Filter content
            _filteredContent = _allContent;

            // Apply search term filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                _filteredContent = _filteredContent
                    .Where(c => c.Title.ToLower().Contains(searchTerm) ||
                               c.Description.ToLower().Contains(searchTerm))
                    .ToList();
            }

            // Apply genre filter
            if (selectedGenre != "All Genres")
            {
                _filteredContent = _filteredContent
                    .Where(c => c.Genre == selectedGenre)
                    .ToList();
            }

            // Apply type filter
            if (selectedType == "Movies")
            {
                _filteredContent = _filteredContent
                    .Where(c => c.Type == ContentType.Movie)
                    .ToList();
            }
            else if (selectedType == "Series")
            {
                _filteredContent = _filteredContent
                    .Where(c => c.Type == ContentType.Series)
                    .ToList();
            }

            // Display filtered content
            DisplayContent(_filteredContent, flpAllContent);

            // Switch to All Content tab
            tabControl.SelectedTab = tabAll;
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm(_currentUser);
            settingsForm.ShowDialog();
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            StatisticsForm statisticsForm = new StatisticsForm(_currentUser);
            statisticsForm.ShowDialog();
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            if (_currentUser.IsAdmin)
            {
                AdminPanelForm adminForm = new AdminPanelForm();
                adminForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("You do not have permission to access the admin panel.", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}