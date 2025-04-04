using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class MainForm : Form
    {
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBrowse;
        private System.Windows.Forms.TabPage tabMyList;
        private System.Windows.Forms.TabPage tabProfile;

        // Browse tab controls
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnClearFilters;
        private System.Windows.Forms.FlowLayoutPanel flpContent;

        // My List tab controls
        private System.Windows.Forms.FlowLayoutPanel flpMyList;
        private System.Windows.Forms.Label lblNoContent;

        // Profile tab controls
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblRegistrationDate;
        private System.Windows.Forms.Button btnEditProfile;
        private System.Windows.Forms.Button btnChangePassword;
        private System.Windows.Forms.Label lblRecentlyWatched;
        private System.Windows.Forms.FlowLayoutPanel flpRecentlyWatched;
        private System.Windows.Forms.Label lblMyRatings;
        private System.Windows.Forms.DataGridView dgvMyRatings;

        private User _currentUser;
        private readonly ContentRepository _contentRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private readonly UserRepository _userRepository;

        public MainForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _contentRepository = new ContentRepository(new Data.EntityFramework.NetWatchDbContext());
            _ratingRepository = new RatingRepository(new Data.EntityFramework.NetWatchDbContext());
            _viewingHistoryRepository = new ViewingHistoryRepository(new Data.EntityFramework.NetWatchDbContext());
            _userRepository = new UserRepository(new Data.EntityFramework.NetWatchDbContext());

            // Set up event handlers
            this.Load += MainForm_Load;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Browse tab event handlers
            btnSearch.Click += BtnSearch_Click;
            cmbGenre.SelectedIndexChanged += FilterContent;
            cmbType.SelectedIndexChanged += FilterContent;
            btnClearFilters.Click += BtnClearFilters_Click;

            // Profile tab event handlers
            btnEditProfile.Click += BtnEditProfile_Click;
            btnChangePassword.Click += BtnChangePassword_Click;
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabBrowse = new System.Windows.Forms.TabPage();
            this.tabMyList = new System.Windows.Forms.TabPage();
            this.tabProfile = new System.Windows.Forms.TabPage();

            // Browse tab controls
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.flpContent = new System.Windows.Forms.FlowLayoutPanel();

            // My List tab controls
            this.flpMyList = new System.Windows.Forms.FlowLayoutPanel();
            this.lblNoContent = new System.Windows.Forms.Label();

            // Profile tab controls
            this.lblUserName = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblRegistrationDate = new System.Windows.Forms.Label();
            this.btnEditProfile = new System.Windows.Forms.Button();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.lblRecentlyWatched = new System.Windows.Forms.Label();
            this.flpRecentlyWatched = new System.Windows.Forms.FlowLayoutPanel();
            this.lblMyRatings = new System.Windows.Forms.Label();
            this.dgvMyRatings = new System.Windows.Forms.DataGridView();

            // TabControl
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(900, 600);
            this.tabControl.TabIndex = 0;

            // TabPage - Browse
            this.tabBrowse.Location = new System.Drawing.Point(4, 29);
            this.tabBrowse.Name = "tabBrowse";
            this.tabBrowse.Padding = new System.Windows.Forms.Padding(3);
            this.tabBrowse.Size = new System.Drawing.Size(892, 567);
            this.tabBrowse.TabIndex = 0;
            this.tabBrowse.Text = "Browse";
            this.tabBrowse.UseVisualStyleBackColor = true;

            // TabPage - My List
            this.tabMyList.Location = new System.Drawing.Point(4, 29);
            this.tabMyList.Name = "tabMyList";
            this.tabMyList.Padding = new System.Windows.Forms.Padding(3);
            this.tabMyList.Size = new System.Drawing.Size(892, 567);
            this.tabMyList.TabIndex = 1;
            this.tabMyList.Text = "My List";
            this.tabMyList.UseVisualStyleBackColor = true;

            // TabPage - Profile
            this.tabProfile.Location = new System.Drawing.Point(4, 29);
            this.tabProfile.Name = "tabProfile";
            this.tabProfile.Padding = new System.Windows.Forms.Padding(3);
            this.tabProfile.Size = new System.Drawing.Size(892, 567);
            this.tabProfile.TabIndex = 2;
            this.tabProfile.Text = "Profile";
            this.tabProfile.UseVisualStyleBackColor = true;

            // Browse Tab Controls

            // txtSearch
            this.txtSearch.Location = new System.Drawing.Point(20, 20);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(200, 27);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.PlaceholderText = "Search by title or description";

            // btnSearch
            this.btnSearch.Location = new System.Drawing.Point(230, 20);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 27);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;

            // cmbGenre
            this.cmbGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Location = new System.Drawing.Point(350, 20);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(150, 28);
            this.cmbGenre.TabIndex = 2;

            // cmbType
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(520, 20);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(150, 28);
            this.cmbType.TabIndex = 3;

            // btnClearFilters
            this.btnClearFilters.Location = new System.Drawing.Point(680, 20);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(90, 28);
            this.btnClearFilters.TabIndex = 4;
            this.btnClearFilters.Text = "Clear";
            this.btnClearFilters.UseVisualStyleBackColor = true;

            // flpContent
            this.flpContent.AutoScroll = true;
            this.flpContent.Location = new System.Drawing.Point(20, 60);
            this.flpContent.Name = "flpContent";
            this.flpContent.Size = new System.Drawing.Size(850, 490);
            this.flpContent.TabIndex = 5;

            // My List Tab Controls

            // flpMyList
            this.flpMyList.AutoScroll = true;
            this.flpMyList.Location = new System.Drawing.Point(20, 20);
            this.flpMyList.Name = "flpMyList";
            this.flpMyList.Size = new System.Drawing.Size(850, 530);
            this.flpMyList.TabIndex = 0;

            // lblNoContent
            this.lblNoContent.AutoSize = true;
            this.lblNoContent.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblNoContent.Location = new System.Drawing.Point(350, 250);
            this.lblNoContent.Name = "lblNoContent";
            this.lblNoContent.Size = new System.Drawing.Size(200, 28);
            this.lblNoContent.Text = "No content in your list yet";
            this.lblNoContent.Visible = false;

            // Profile Tab Controls

            // lblUserName
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblUserName.Location = new System.Drawing.Point(20, 20);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(200, 37);
            this.lblUserName.Text = "User Name";

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(20, 70);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(200, 20);
            this.lblEmail.Text = "Email: user@example.com";

            // lblRegistrationDate
            this.lblRegistrationDate.AutoSize = true;
            this.lblRegistrationDate.Location = new System.Drawing.Point(20, 100);
            this.lblRegistrationDate.Name = "lblRegistrationDate";
            this.lblRegistrationDate.Size = new System.Drawing.Size(200, 20);
            this.lblRegistrationDate.Text = "Member since: 01/01/2023";

            // btnEditProfile
            this.btnEditProfile.Location = new System.Drawing.Point(20, 140);
            this.btnEditProfile.Name = "btnEditProfile";
            this.btnEditProfile.Size = new System.Drawing.Size(150, 35);
            this.btnEditProfile.TabIndex = 0;
            this.btnEditProfile.Text = "Edit Profile";
            this.btnEditProfile.UseVisualStyleBackColor = true;

            // btnChangePassword
            this.btnChangePassword.Location = new System.Drawing.Point(180, 140);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(150, 35);
            this.btnChangePassword.TabIndex = 1;
            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.UseVisualStyleBackColor = true;

            // lblRecentlyWatched
            this.lblRecentlyWatched.AutoSize = true;
            this.lblRecentlyWatched.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRecentlyWatched.Location = new System.Drawing.Point(20, 200);
            this.lblRecentlyWatched.Name = "lblRecentlyWatched";
            this.lblRecentlyWatched.Size = new System.Drawing.Size(200, 28);
            this.lblRecentlyWatched.Text = "Recently Watched";

            // flpRecentlyWatched
            this.flpRecentlyWatched.AutoScroll = true;
            this.flpRecentlyWatched.Location = new System.Drawing.Point(20, 230);
            this.flpRecentlyWatched.Name = "flpRecentlyWatched";
            this.flpRecentlyWatched.Size = new System.Drawing.Size(850, 150);
            this.flpRecentlyWatched.TabIndex = 2;

            // lblMyRatings
            this.lblMyRatings.AutoSize = true;
            this.lblMyRatings.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblMyRatings.Location = new System.Drawing.Point(20, 390);
            this.lblMyRatings.Name = "lblMyRatings";
            this.lblMyRatings.Size = new System.Drawing.Size(200, 28);
            this.lblMyRatings.Text = "My Ratings";

            // dgvMyRatings
            this.dgvMyRatings.AllowUserToAddRows = false;
            this.dgvMyRatings.AllowUserToDeleteRows = false;
            this.dgvMyRatings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMyRatings.Location = new System.Drawing.Point(20, 420);
            this.dgvMyRatings.Name = "dgvMyRatings";
            this.dgvMyRatings.ReadOnly = true;
            this.dgvMyRatings.RowHeadersWidth = 51;
            this.dgvMyRatings.Size = new System.Drawing.Size(850, 130);
            this.dgvMyRatings.TabIndex = 3;

            // Add controls to tabs
            this.tabBrowse.Controls.Add(this.txtSearch);
            this.tabBrowse.Controls.Add(this.btnSearch);
            this.tabBrowse.Controls.Add(this.cmbGenre);
            this.tabBrowse.Controls.Add(this.cmbType);
            this.tabBrowse.Controls.Add(this.btnClearFilters);
            this.tabBrowse.Controls.Add(this.flpContent);

            this.tabMyList.Controls.Add(this.flpMyList);
            this.tabMyList.Controls.Add(this.lblNoContent);

            this.tabProfile.Controls.Add(this.lblUserName);
            this.tabProfile.Controls.Add(this.lblEmail);
            this.tabProfile.Controls.Add(this.lblRegistrationDate);
            this.tabProfile.Controls.Add(this.btnEditProfile);
            this.tabProfile.Controls.Add(this.btnChangePassword);
            this.tabProfile.Controls.Add(this.lblRecentlyWatched);
            this.tabProfile.Controls.Add(this.flpRecentlyWatched);
            this.tabProfile.Controls.Add(this.lblMyRatings);
            this.tabProfile.Controls.Add(this.dgvMyRatings);

            // Add tabs to tab control
            this.tabControl.Controls.Add(this.tabBrowse);
            this.tabControl.Controls.Add(this.tabMyList);
            this.tabControl.Controls.Add(this.tabProfile);

            // MainForm
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetWatch App";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Set up user profile information
            lblUserName.Text = $"{_currentUser.FirstName} {_currentUser.LastName}";
            lblEmail.Text = $"Email: {_currentUser.Email}";
            lblRegistrationDate.Text = $"Member since: {_currentUser.RegistrationDate.ToShortDateString()}";

            // Load filter options
            LoadFilterOptions();

            // Load content
            LoadContent();

            // Load my list
            LoadMyList();

            // Load recently watched
            LoadRecentlyWatched();

            // Load my ratings
            SetupRatingsGrid();
            LoadMyRatings();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Refresh data when switching tabs
            switch (tabControl.SelectedIndex)
            {
                case 0: // Browse tab
                    LoadContent();
                    break;
                case 1: // My List tab
                    LoadMyList();
                    break;
                case 2: // Profile tab
                    LoadRecentlyWatched();
                    LoadMyRatings();
                    break;
            }
        }

        #region Browse Tab Methods

        private void LoadFilterOptions()
        {
            try
            {
                // Load genre filter options
                cmbGenre.Items.Clear();
                cmbGenre.Items.Add("All Genres");
                cmbGenre.SelectedIndex = 0;

                var genres = _contentRepository.GetAllGenres();
                foreach (var genre in genres)
                {
                    cmbGenre.Items.Add(genre);
                }

                // Load type filter options
                cmbType.Items.Clear();
                cmbType.Items.Add("All Types");
                cmbType.Items.Add("Movie");
                cmbType.Items.Add("Series");
                cmbType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading filter options: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadContent()
        {
            try
            {
                flpContent.Controls.Clear();

                // Get filtered content
                string searchTerm = txtSearch.Text.Trim();
                string genre = cmbGenre.SelectedIndex > 0 ? cmbGenre.SelectedItem.ToString() : null;
                string type = cmbType.SelectedIndex > 0 ? cmbType.SelectedItem.ToString() : null;

                var contents = _contentRepository.Search(searchTerm, genre, type);

                // Create content cards
                foreach (var content in contents)
                {
                    var contentCard = CreateContentCard(content);
                    flpContent.Controls.Add(contentCard);
                }

                if (contents.Count == 0)
                {
                    var noContentLabel = new Label
                    {
                        Text = "No content found matching your criteria",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 12),
                        Margin = new Padding(10)
                    };
                    flpContent.Controls.Add(noContentLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateContentCard(Content content)
        {
            // Create a panel for the content card
            var panel = new Panel
            {
                Width = 160,
                Height = 240,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Create a picture box for the content image
            var pictureBox = new PictureBox
            {
                Width = 150,
                Height = 150,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(5, 5)
            };

            // Load image if available
            if (!string.IsNullOrEmpty(content.ImagePath))
            {
                try
                {
                    using (var httpClient = new System.Net.Http.HttpClient())
                    {
                        var imageData = httpClient.GetByteArrayAsync(content.ImagePath).Result;
                        using (var stream = new System.IO.MemoryStream(imageData))
                        {
                            pictureBox.Image = Image.FromStream(stream);
                        }
                    }
                }
                catch
                {
                    // Use placeholder if image loading fails
                    pictureBox.BackColor = Color.LightGray;
                }
            }
            else
            {
                // Use placeholder if no image
                pictureBox.BackColor = Color.LightGray;
            }

            // Create a label for the content title
            var titleLabel = new Label
            {
                Text = content.Title,
                AutoSize = false,
                Width = 150,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Location = new Point(5, 160)
            };

            // Create a label for additional info
            var infoLabel = new Label
            {
                Text = $"{content.Type} • {content.ReleaseYear}",
                AutoSize = false,
                Width = 150,
                Height = 20,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(5, 200)
            };

            // Add controls to the panel
            panel.Controls.Add(pictureBox);
            panel.Controls.Add(titleLabel);
            panel.Controls.Add(infoLabel);

            // Add click event to open content details
            panel.Click += (sender, e) => OpenContentDetails(content);
            pictureBox.Click += (sender, e) => OpenContentDetails(content);
            titleLabel.Click += (sender, e) => OpenContentDetails(content);
            infoLabel.Click += (sender, e) => OpenContentDetails(content);

            return panel;
        }

        private void OpenContentDetails(Content content)
        {
            using (var contentDetailsForm = new ContentDetailsForm(content, _currentUser))
            {
                contentDetailsForm.ShowDialog();

                // Refresh data after viewing details
                LoadContent();
                LoadMyList();
                LoadRecentlyWatched();
                LoadMyRatings();
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void FilterContent(object sender, EventArgs e)
        {
            LoadContent();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            cmbGenre.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            LoadContent();
        }

        #endregion

        #region My List Tab Methods

        private void LoadMyList()
        {
            try
            {
                flpMyList.Controls.Clear();

                // Get user's viewing history
                var viewingHistories = _viewingHistoryRepository.GetByUser(_currentUser.Id);

                if (viewingHistories.Count > 0)
                {
                    lblNoContent.Visible = false;

                    // Create content cards for each item in viewing history
                    foreach (var history in viewingHistories)
                    {
                        var contentCard = CreateContentCard(history.Content);
                        flpMyList.Controls.Add(contentCard);
                    }
                }
                else
                {
                    lblNoContent.Visible = true;
                    flpMyList.Controls.Add(lblNoContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading my list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Profile Tab Methods

        private void LoadRecentlyWatched()
        {
            try
            {
                flpRecentlyWatched.Controls.Clear();

                // Get recently watched content
                var recentlyWatched = _viewingHistoryRepository.GetRecentlyWatchedContent(_currentUser.Id, 5);

                if (recentlyWatched.Count > 0)
                {
                    // Create content cards for recently watched
                    foreach (var content in recentlyWatched)
                    {
                        var contentCard = CreateContentCard(content);
                        flpRecentlyWatched.Controls.Add(contentCard);
                    }
                }
                else
                {
                    var noContentLabel = new Label
                    {
                        Text = "No recently watched content",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10),
                        Margin = new Padding(10)
                    };
                    flpRecentlyWatched.Controls.Add(noContentLabel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading recently watched: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupRatingsGrid()
        {
            dgvMyRatings.AutoGenerateColumns = false;

            // Add columns
            dgvMyRatings.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ContentTitle",
                HeaderText = "Title",
                Width = 250,
                ReadOnly = true
            });

            dgvMyRatings.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Score",
                HeaderText = "Rating",
                Width = 80,
                ReadOnly = true
            });

            dgvMyRatings.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Comment",
                HeaderText = "Comment",
                Width = 350,
                ReadOnly = true
            });

            dgvMyRatings.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RatingDate",
                HeaderText = "Date",
                Width = 120,
                ReadOnly = true
            });
        }

        private void LoadMyRatings()
        {
            try
            {
                // Get user's ratings
                var ratings = _ratingRepository.GetByUser(_currentUser.Id);

                // Create a list of rating view models
                var ratingViewModels = ratings.Select(r => new
                {
                    ContentTitle = r.Content.Title,
                    r.Score,
                    r.Comment,
                    RatingDate = r.RatingDate.ToShortDateString()
                }).ToList();

                dgvMyRatings.DataSource = ratingViewModels;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ratings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditProfile_Click(object sender, EventArgs e)
        {
            using (var editUserForm = new EditUserForm(_currentUser))
            {
                if (editUserForm.ShowDialog() == DialogResult.OK)
                {
                    // Refresh user data
                    _currentUser = _userRepository.GetById(_currentUser.Id);

                    // Update profile information
                    lblUserName.Text = $"{_currentUser.FirstName} {_currentUser.LastName}";
                    lblEmail.Text = $"Email: {_currentUser.Email}";
                }
            }
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            using (var changePasswordForm = new ChangePasswordForm(_currentUser))
            {
                changePasswordForm.ShowDialog();
            }
        }

        #endregion
    }
}

