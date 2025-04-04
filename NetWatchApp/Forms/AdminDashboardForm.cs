using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class AdminDashboardForm : Form
    {
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabContent;
        private System.Windows.Forms.TabPage tabStatistics;

        // Users tab controls
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.TextBox txtSearchUser;
        private System.Windows.Forms.Button btnSearchUser;

        // Content tab controls
        private System.Windows.Forms.DataGridView dgvContent;
        private System.Windows.Forms.Button btnAddContent;
        private System.Windows.Forms.Button btnEditContent;
        private System.Windows.Forms.Button btnDeleteContent;
        private System.Windows.Forms.Button btnViewContent;
        private System.Windows.Forms.TextBox txtSearchContent;
        private System.Windows.Forms.Button btnSearchContent;
        private System.Windows.Forms.ComboBox cmbGenre;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Button btnClearFilters;

        // Statistics tab controls
        private System.Windows.Forms.Label lblTotalUsers;
        private System.Windows.Forms.Label lblTotalContent;
        private System.Windows.Forms.Label lblTotalMovies;
        private System.Windows.Forms.Label lblTotalSeries;
        private System.Windows.Forms.Label lblTotalRatings;
        private System.Windows.Forms.Label lblAverageRating;
        private System.Windows.Forms.Label lblMostWatched;
        private System.Windows.Forms.Label lblMostRated;

        private readonly User _currentUser;
        private readonly UserRepository _userRepository;
        private readonly ContentRepository _contentRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;

        public AdminDashboardForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRepository = new UserRepository(new Data.EntityFramework.NetWatchDbContext());
            _contentRepository = new ContentRepository(new Data.EntityFramework.NetWatchDbContext());
            _ratingRepository = new RatingRepository(new Data.EntityFramework.NetWatchDbContext());
            _viewingHistoryRepository = new ViewingHistoryRepository(new Data.EntityFramework.NetWatchDbContext());

            // Set up event handlers
            this.Load += AdminDashboardForm_Load;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Users tab event handlers
            btnAddUser.Click += BtnAddUser_Click;
            btnEditUser.Click += BtnEditUser_Click;
            btnDeleteUser.Click += BtnDeleteUser_Click;
            btnSearchUser.Click += BtnSearchUser_Click;

            // Content tab event handlers
            btnAddContent.Click += BtnAddContent_Click;
            btnEditContent.Click += BtnEditContent_Click;
            btnDeleteContent.Click += BtnDeleteContent_Click;
            btnViewContent.Click += BtnViewContent_Click;
            btnSearchContent.Click += BtnSearchContent_Click;
            cmbGenre.SelectedIndexChanged += FilterContent;
            cmbType.SelectedIndexChanged += FilterContent;
            btnClearFilters.Click += BtnClearFilters_Click;
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.tabContent = new System.Windows.Forms.TabPage();
            this.tabStatistics = new System.Windows.Forms.TabPage();

            // Users tab controls
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.txtSearchUser = new System.Windows.Forms.TextBox();
            this.btnSearchUser = new System.Windows.Forms.Button();

            // Content tab controls
            this.dgvContent = new System.Windows.Forms.DataGridView();
            this.btnAddContent = new System.Windows.Forms.Button();
            this.btnEditContent = new System.Windows.Forms.Button();
            this.btnDeleteContent = new System.Windows.Forms.Button();
            this.btnViewContent = new System.Windows.Forms.Button();
            this.txtSearchContent = new System.Windows.Forms.TextBox();
            this.btnSearchContent = new System.Windows.Forms.Button();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.btnClearFilters = new System.Windows.Forms.Button();

            // Statistics tab controls
            this.lblTotalUsers = new System.Windows.Forms.Label();
            this.lblTotalContent = new System.Windows.Forms.Label();
            this.lblTotalMovies = new System.Windows.Forms.Label();
            this.lblTotalSeries = new System.Windows.Forms.Label();
            this.lblTotalRatings = new System.Windows.Forms.Label();
            this.lblAverageRating = new System.Windows.Forms.Label();
            this.lblMostWatched = new System.Windows.Forms.Label();
            this.lblMostRated = new System.Windows.Forms.Label();

            // TabControl
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 600);
            this.tabControl.TabIndex = 0;

            // TabPage - Users
            this.tabUsers.Location = new System.Drawing.Point(4, 29);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(792, 567);
            this.tabUsers.TabIndex = 0;
            this.tabUsers.Text = "Users";
            this.tabUsers.UseVisualStyleBackColor = true;

            // TabPage - Content
            this.tabContent.Location = new System.Drawing.Point(4, 29);
            this.tabContent.Name = "tabContent";
            this.tabContent.Padding = new System.Windows.Forms.Padding(3);
            this.tabContent.Size = new System.Drawing.Size(792, 567);
            this.tabContent.TabIndex = 1;
            this.tabContent.Text = "Content";
            this.tabContent.UseVisualStyleBackColor = true;

            // TabPage - Statistics
            this.tabStatistics.Location = new System.Drawing.Point(4, 29);
            this.tabStatistics.Name = "tabStatistics";
            this.tabStatistics.Padding = new System.Windows.Forms.Padding(3);
            this.tabStatistics.Size = new System.Drawing.Size(792, 567);
            this.tabStatistics.TabIndex = 2;
            this.tabStatistics.Text = "Statistics";
            this.tabStatistics.UseVisualStyleBackColor = true;

            // Users Tab Controls

            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(20, 60);
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(750, 400);
            this.dgvUsers.TabIndex = 0;

            // txtSearchUser
            this.txtSearchUser.Location = new System.Drawing.Point(20, 20);
            this.txtSearchUser.Name = "txtSearchUser";
            this.txtSearchUser.Size = new System.Drawing.Size(200, 27);
            this.txtSearchUser.TabIndex = 1;
            this.txtSearchUser.PlaceholderText = "Search by name or email";

            // btnSearchUser
            this.btnSearchUser.Location = new System.Drawing.Point(230, 20);
            this.btnSearchUser.Name = "btnSearchUser";
            this.btnSearchUser.Size = new System.Drawing.Size(100, 27);
            this.btnSearchUser.TabIndex = 2;
            this.btnSearchUser.Text = "Search";
            this.btnSearchUser.UseVisualStyleBackColor = true;

            // btnAddUser
            this.btnAddUser.Location = new System.Drawing.Point(20, 480);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(120, 35);
            this.btnAddUser.TabIndex = 3;
            this.btnAddUser.Text = "Add User";
            this.btnAddUser.UseVisualStyleBackColor = true;

            // btnEditUser
            this.btnEditUser.Location = new System.Drawing.Point(160, 480);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(120, 35);
            this.btnEditUser.TabIndex = 4;
            this.btnEditUser.Text = "Edit User";
            this.btnEditUser.UseVisualStyleBackColor = true;

            // btnDeleteUser
            this.btnDeleteUser.Location = new System.Drawing.Point(300, 480);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(120, 35);
            this.btnDeleteUser.TabIndex = 5;
            this.btnDeleteUser.Text = "Delete User";
            this.btnDeleteUser.UseVisualStyleBackColor = true;

            // Content Tab Controls

            // dgvContent
            this.dgvContent.AllowUserToAddRows = false;
            this.dgvContent.AllowUserToDeleteRows = false;
            this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContent.Location = new System.Drawing.Point(20, 60);
            this.dgvContent.Name = "dgvContent";
            this.dgvContent.ReadOnly = true;
            this.dgvContent.RowHeadersWidth = 51;
            this.dgvContent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContent.Size = new System.Drawing.Size(750, 400);
            this.dgvContent.TabIndex = 0;

            // txtSearchContent
            this.txtSearchContent.Location = new System.Drawing.Point(20, 20);
            this.txtSearchContent.Name = "txtSearchContent";
            this.txtSearchContent.Size = new System.Drawing.Size(200, 27);
            this.txtSearchContent.TabIndex = 1;
            this.txtSearchContent.PlaceholderText = "Search by title or description";

            // btnSearchContent
            this.btnSearchContent.Location = new System.Drawing.Point(230, 20);
            this.btnSearchContent.Name = "btnSearchContent";
            this.btnSearchContent.Size = new System.Drawing.Size(100, 27);
            this.btnSearchContent.TabIndex = 2;
            this.btnSearchContent.Text = "Search";
            this.btnSearchContent.UseVisualStyleBackColor = true;

            // cmbGenre
            this.cmbGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Location = new System.Drawing.Point(350, 20);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(150, 28);
            this.cmbGenre.TabIndex = 3;

            // cmbType
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(520, 20);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(150, 28);
            this.cmbType.TabIndex = 4;

            // btnClearFilters
            this.btnClearFilters.Location = new System.Drawing.Point(680, 20);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(90, 28);
            this.btnClearFilters.TabIndex = 5;
            this.btnClearFilters.Text = "Clear";
            this.btnClearFilters.UseVisualStyleBackColor = true;

            // btnAddContent
            this.btnAddContent.Location = new System.Drawing.Point(20, 480);
            this.btnAddContent.Name = "btnAddContent";
            this.btnAddContent.Size = new System.Drawing.Size(120, 35);
            this.btnAddContent.TabIndex = 6;
            this.btnAddContent.Text = "Add Content";
            this.btnAddContent.UseVisualStyleBackColor = true;

            // btnEditContent
            this.btnEditContent.Location = new System.Drawing.Point(160, 480);
            this.btnEditContent.Name = "btnEditContent";
            this.btnEditContent.Size = new System.Drawing.Size(120, 35);
            this.btnEditContent.TabIndex = 7;
            this.btnEditContent.Text = "Edit Content";
            this.btnEditContent.UseVisualStyleBackColor = true;

            // btnDeleteContent
            this.btnDeleteContent.Location = new System.Drawing.Point(300, 480);
            this.btnDeleteContent.Name = "btnDeleteContent";
            this.btnDeleteContent.Size = new System.Drawing.Size(120, 35);
            this.btnDeleteContent.TabIndex = 8;
            this.btnDeleteContent.Text = "Delete Content";
            this.btnDeleteContent.UseVisualStyleBackColor = true;

            // btnViewContent
            this.btnViewContent.Location = new System.Drawing.Point(440, 480);
            this.btnViewContent.Name = "btnViewContent";
            this.btnViewContent.Size = new System.Drawing.Size(120, 35);
            this.btnViewContent.TabIndex = 9;
            this.btnViewContent.Text = "View Details";
            this.btnViewContent.UseVisualStyleBackColor = true;

            // Statistics Tab Controls

            // lblTotalUsers
            this.lblTotalUsers.AutoSize = true;
            this.lblTotalUsers.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalUsers.Location = new System.Drawing.Point(20, 20);
            this.lblTotalUsers.Name = "lblTotalUsers";
            this.lblTotalUsers.Size = new System.Drawing.Size(150, 28);
            this.lblTotalUsers.Text = "Total Users: 0";

            // lblTotalContent
            this.lblTotalContent.AutoSize = true;
            this.lblTotalContent.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalContent.Location = new System.Drawing.Point(20, 60);
            this.lblTotalContent.Name = "lblTotalContent";
            this.lblTotalContent.Size = new System.Drawing.Size(150, 28);
            this.lblTotalContent.Text = "Total Content: 0";

            // lblTotalMovies
            this.lblTotalMovies.AutoSize = true;
            this.lblTotalMovies.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalMovies.Location = new System.Drawing.Point(20, 100);
            this.lblTotalMovies.Name = "lblTotalMovies";
            this.lblTotalMovies.Size = new System.Drawing.Size(150, 28);
            this.lblTotalMovies.Text = "Total Movies: 0";

            // lblTotalSeries
            this.lblTotalSeries.AutoSize = true;
            this.lblTotalSeries.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalSeries.Location = new System.Drawing.Point(20, 140);
            this.lblTotalSeries.Name = "lblTotalSeries";
            this.lblTotalSeries.Size = new System.Drawing.Size(150, 28);
            this.lblTotalSeries.Text = "Total Series: 0";

            // lblTotalRatings
            this.lblTotalRatings.AutoSize = true;
            this.lblTotalRatings.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblTotalRatings.Location = new System.Drawing.Point(20, 180);
            this.lblTotalRatings.Name = "lblTotalRatings";
            this.lblTotalRatings.Size = new System.Drawing.Size(150, 28);
            this.lblTotalRatings.Text = "Total Ratings: 0";

            // lblAverageRating
            this.lblAverageRating.AutoSize = true;
            this.lblAverageRating.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblAverageRating.Location = new System.Drawing.Point(20, 220);
            this.lblAverageRating.Name = "lblAverageRating";
            this.lblAverageRating.Size = new System.Drawing.Size(200, 28);
            this.lblAverageRating.Text = "Average Rating: 0.0";

            // lblMostWatched
            this.lblMostWatched.AutoSize = true;
            this.lblMostWatched.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblMostWatched.Location = new System.Drawing.Point(20, 260);
            this.lblMostWatched.Name = "lblMostWatched";
            this.lblMostWatched.Size = new System.Drawing.Size(300, 28);
            this.lblMostWatched.Text = "Most Watched: None";

            // lblMostRated
            this.lblMostRated.AutoSize = true;
            this.lblMostRated.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblMostRated.Location = new System.Drawing.Point(20, 300);
            this.lblMostRated.Name = "lblMostRated";
            this.lblMostRated.Size = new System.Drawing.Size(300, 28);
            this.lblMostRated.Text = "Most Rated: None";

            // Add controls to tabs
            this.tabUsers.Controls.Add(this.dgvUsers);
            this.tabUsers.Controls.Add(this.txtSearchUser);
            this.tabUsers.Controls.Add(this.btnSearchUser);
            this.tabUsers.Controls.Add(this.btnAddUser);
            this.tabUsers.Controls.Add(this.btnEditUser);
            this.tabUsers.Controls.Add(this.btnDeleteUser);

            this.tabContent.Controls.Add(this.dgvContent);
            this.tabContent.Controls.Add(this.txtSearchContent);
            this.tabContent.Controls.Add(this.btnSearchContent);
            this.tabContent.Controls.Add(this.cmbGenre);
            this.tabContent.Controls.Add(this.cmbType);
            this.tabContent.Controls.Add(this.btnClearFilters);
            this.tabContent.Controls.Add(this.btnAddContent);
            this.tabContent.Controls.Add(this.btnEditContent);
            this.tabContent.Controls.Add(this.btnDeleteContent);
            this.tabContent.Controls.Add(this.btnViewContent);

            this.tabStatistics.Controls.Add(this.lblTotalUsers);
            this.tabStatistics.Controls.Add(this.lblTotalContent);
            this.tabStatistics.Controls.Add(this.lblTotalMovies);
            this.tabStatistics.Controls.Add(this.lblTotalSeries);
            this.tabStatistics.Controls.Add(this.lblTotalRatings);
            this.tabStatistics.Controls.Add(this.lblAverageRating);
            this.tabStatistics.Controls.Add(this.lblMostWatched);
            this.tabStatistics.Controls.Add(this.lblMostRated);

            // Add tabs to tab control
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Controls.Add(this.tabContent);
            this.tabControl.Controls.Add(this.tabStatistics);

            // AdminDashboardForm
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "AdminDashboardForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetWatch App - Admin Dashboard";
        }

        private void AdminDashboardForm_Load(object sender, EventArgs e)
        {
            // Set up users data grid
            SetupUsersDataGrid();
            LoadUsers();

            // Set up content data grid
            SetupContentDataGrid();
            LoadContent();

            // Load filter options
            LoadFilterOptions();

            // Load statistics
            LoadStatistics();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Refresh data when switching tabs
            switch (tabControl.SelectedIndex)
            {
                case 0: // Users tab
                    LoadUsers();
                    break;
                case 1: // Content tab
                    LoadContent();
                    break;
                case 2: // Statistics tab
                    LoadStatistics();
                    break;
            }
        }

        #region Users Tab Methods

        private void SetupUsersDataGrid()
        {
            dgvUsers.AutoGenerateColumns = false;

            // Add columns
            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50,
                ReadOnly = true
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "IdentificationNumber",
                HeaderText = "ID Number",
                Width = 100,
                ReadOnly = true
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "FirstName",
                HeaderText = "First Name",
                Width = 120,
                ReadOnly = true
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LastName",
                HeaderText = "Last Name",
                Width = 120,
                ReadOnly = true
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Email",
                HeaderText = "Email",
                Width = 200,
                ReadOnly = true
            });

            dgvUsers.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "IsAdmin",
                HeaderText = "Admin",
                Width = 60,
                ReadOnly = true
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "RegistrationDate",
                HeaderText = "Registration Date",
                Width = 150,
                ReadOnly = true
            });
        }

        private void LoadUsers(string searchTerm = "")
        {
            try
            {
                var users = _userRepository.GetAll();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    users = users.Where(u =>
                        u.FirstName.ToLower().Contains(searchTerm) ||
                        u.LastName.ToLower().Contains(searchTerm) ||
                        u.Email.ToLower().Contains(searchTerm) ||
                        u.IdentificationNumber.ToLower().Contains(searchTerm)
                    ).ToList();
                }

                dgvUsers.DataSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearchUser_Click(object sender, EventArgs e)
        {
            LoadUsers(txtSearchUser.Text.Trim());
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            using (var registerForm = new RegisterForm(true))
            {
                if (registerForm.ShowDialog() == DialogResult.OK)
                {
                    LoadUsers();
                }
            }
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[0].Value);
            var user = _userRepository.GetById(userId);

            if (user != null)
            {
                using (var editUserForm = new EditUserForm(user))
                {
                    if (editUserForm.ShowDialog() == DialogResult.OK)
                    {
                        LoadUsers();
                    }
                }
            }
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells[0].Value);

            // Prevent deleting yourself
            if (userId == _currentUser.Id)
            {
                MessageBox.Show("You cannot delete your own account.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this user? This action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _userRepository.Delete(userId);
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion

        #region Content Tab Methods

        private void SetupContentDataGrid()
        {
            dgvContent.AutoGenerateColumns = false;

            // Add columns
            dgvContent.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50,
                ReadOnly = true
            });

            dgvContent.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Title",
                HeaderText = "Title",
                Width = 200,
                ReadOnly = true
            });

            dgvContent.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Type",
                HeaderText = "Type",
                Width = 80,
                ReadOnly = true
            });

            dgvContent.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Genre",
                HeaderText = "Genre",
                Width = 100,
                ReadOnly = true
            });

            dgvContent.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ReleaseYear",
                HeaderText = "Year",
                Width = 60,
                ReadOnly = true
            });

            dgvContent.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Platform",
                HeaderText = "Platform",
                Width = 100,
                ReadOnly = true
            });

            dgvContent.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "AverageRating",
                HeaderText = "Rating",
                Width = 60,
                ReadOnly = true
            });
        }

        private void LoadContent(string searchTerm = "")
        {
            try
            {
                var contents = _contentRepository.GetAll();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    contents = contents.Where(c =>
                        c.Title.ToLower().Contains(searchTerm) ||
                        c.Description.ToLower().Contains(searchTerm)
                    ).ToList();
                }

                // Apply genre filter
                if (cmbGenre.SelectedIndex > 0) // Skip "All Genres"
                {
                    string genre = cmbGenre.SelectedItem.ToString();
                    contents = contents.Where(c => c.Genre == genre).ToList();
                }

                // Apply type filter
                if (cmbType.SelectedIndex > 0) // Skip "All Types"
                {
                    string type = cmbType.SelectedItem.ToString();
                    contents = contents.Where(c => c.Type == type).ToList();
                }

                dgvContent.DataSource = contents;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

        private void FilterContent(object sender, EventArgs e)
        {
            LoadContent(txtSearchContent.Text.Trim());
        }

        private void BtnSearchContent_Click(object sender, EventArgs e)
        {
            LoadContent(txtSearchContent.Text.Trim());
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearchContent.Text = string.Empty;
            cmbGenre.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            LoadContent();
        }

        private void BtnAddContent_Click(object sender, EventArgs e)
        {
            using (var addContentForm = new AddContentForm())
            {
                if (addContentForm.ShowDialog() == DialogResult.OK)
                {
                    LoadContent();
                }
            }
        }

        private void BtnEditContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select content to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells[0].Value);

            using (var editContentForm = new EditContentForm(contentId))
            {
                if (editContentForm.ShowDialog() == DialogResult.OK)
                {
                    LoadContent();
                }
            }
        }

        private void BtnDeleteContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select content to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells[0].Value);

            if (MessageBox.Show("Are you sure you want to delete this content? This action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _contentRepository.Delete(contentId);
                    MessageBox.Show("Content deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadContent();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnViewContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select content to view.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells[0].Value);
            var content = _contentRepository.GetById(contentId);

            if (content != null)
            {
                using (var contentDetailsForm = new ContentDetailsForm(content, _currentUser))
                {
                    contentDetailsForm.ShowDialog();
                }
            }
        }

        #endregion

        #region Statistics Tab Methods

        private void LoadStatistics()
        {
            try
            {
                // Get counts
                int totalUsers = _userRepository.GetAll().Count;
                var contents = _contentRepository.GetAll();
                int totalContent = contents.Count;
                int totalMovies = contents.Count(c => c.Type == "Movie");
                int totalSeries = contents.Count(c => c.Type == "Series");
                int totalRatings = _ratingRepository.GetAll().Count;

                // Calculate average rating
                double averageRating = 0;
                if (totalRatings > 0)
                {
                    averageRating = _ratingRepository.GetAll().Average(r => r.Score);
                }

                // Find most watched content
                string mostWatchedTitle = "None";
                int mostWatchedCount = 0;

                var viewingHistories = _viewingHistoryRepository.GetAll();
                if (viewingHistories.Any())
                {
                    var contentWatchCounts = viewingHistories
                        .GroupBy(vh => vh.ContentId)
                        .Select(g => new { ContentId = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .FirstOrDefault();

                    if (contentWatchCounts != null)
                    {
                        var mostWatchedContent = contents.FirstOrDefault(c => c.Id == contentWatchCounts.ContentId);
                        if (mostWatchedContent != null)
                        {
                            mostWatchedTitle = mostWatchedContent.Title;
                            mostWatchedCount = contentWatchCounts.Count;
                        }
                    }
                }

                // Find most rated content
                string mostRatedTitle = "None";
                int mostRatedCount = 0;

                var ratings = _ratingRepository.GetAll();
                if (ratings.Any())
                {
                    var contentRatingCounts = ratings
                        .GroupBy(r => r.ContentId)
                        .Select(g => new { ContentId = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count)
                        .FirstOrDefault();

                    if (contentRatingCounts != null)
                    {
                        var mostRatedContent = contents.FirstOrDefault(c => c.Id == contentRatingCounts.ContentId);
                        if (mostRatedContent != null)
                        {
                            mostRatedTitle = mostRatedContent.Title;
                            mostRatedCount = contentRatingCounts.Count;
                        }
                    }
                }

                // Update labels
                lblTotalUsers.Text = $"Total Users: {totalUsers}";
                lblTotalContent.Text = $"Total Content: {totalContent}";
                lblTotalMovies.Text = $"Total Movies: {totalMovies}";
                lblTotalSeries.Text = $"Total Series: {totalSeries}";
                lblTotalRatings.Text = $"Total Ratings: {totalRatings}";
                lblAverageRating.Text = $"Average Rating: {averageRating:F1}";
                lblMostWatched.Text = $"Most Watched: {mostWatchedTitle} ({mostWatchedCount} views)";
                lblMostRated.Text = $"Most Rated: {mostRatedTitle} ({mostRatedCount} ratings)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}

