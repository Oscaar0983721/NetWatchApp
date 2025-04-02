using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class AdminPanelForm : Form
    {
        private readonly ContentRepository _contentRepository;
        private readonly UserRepository _userRepository;
        private User _currentUser;

        public AdminPanelForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _contentRepository = new ContentRepository(new NetWatchDbContext());
            _userRepository = new UserRepository(new NetWatchDbContext());

            // Set up tab control
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;

            // Set up content management
            btnAddContent.Click += BtnAddContent_Click;
            btnEditContent.Click += BtnEditContent_Click;
            btnDeleteContent.Click += BtnDeleteContent_Click;
            btnRefreshContent.Click += BtnRefreshContent_Click;
            txtContentSearch.TextChanged += TxtContentSearch_TextChanged;

            // Set up user management
            btnEditUser.Click += BtnEditUser_Click;
            btnDeleteUser.Click += BtnDeleteUser_Click;
            btnRefreshUsers.Click += BtnRefreshUsers_Click;
            txtUserSearch.TextChanged += TxtUserSearch_TextChanged;

            // Load initial data
            LoadContentData();
            LoadUserData();
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabContentManagement = new System.Windows.Forms.TabPage();
            this.tabUserManagement = new System.Windows.Forms.TabPage();
            this.pnlContentControls = new System.Windows.Forms.Panel();
            this.lblContentSearch = new System.Windows.Forms.Label();
            this.txtContentSearch = new System.Windows.Forms.TextBox();
            this.btnAddContent = new System.Windows.Forms.Button();
            this.btnEditContent = new System.Windows.Forms.Button();
            this.btnDeleteContent = new System.Windows.Forms.Button();
            this.btnRefreshContent = new System.Windows.Forms.Button();
            this.dgvContent = new System.Windows.Forms.DataGridView();
            this.pnlUserControls = new System.Windows.Forms.Panel();
            this.lblUserSearch = new System.Windows.Forms.Label();
            this.txtUserSearch = new System.Windows.Forms.TextBox();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();
            this.btnRefreshUsers = new System.Windows.Forms.Button();
            this.dgvUsers = new System.Windows.Forms.DataGridView();

            // tabControl
            this.tabControl.Controls.Add(this.tabContentManagement);
            this.tabControl.Controls.Add(this.tabUserManagement);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 600);
            this.tabControl.TabIndex = 0;

            // tabContentManagement
            this.tabContentManagement.Controls.Add(this.pnlContentControls);
            this.tabContentManagement.Controls.Add(this.dgvContent);
            this.tabContentManagement.Location = new System.Drawing.Point(4, 29);
            this.tabContentManagement.Name = "tabContentManagement";
            this.tabContentManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabContentManagement.Size = new System.Drawing.Size(792, 567);
            this.tabContentManagement.TabIndex = 0;
            this.tabContentManagement.Text = "Content Management";
            this.tabContentManagement.UseVisualStyleBackColor = true;

            // tabUserManagement
            this.tabUserManagement.Controls.Add(this.pnlUserControls);
            this.tabUserManagement.Controls.Add(this.dgvUsers);
            this.tabUserManagement.Location = new System.Drawing.Point(4, 29);
            this.tabUserManagement.Name = "tabUserManagement";
            this.tabUserManagement.Padding = new System.Windows.Forms.Padding(3);
            this.tabUserManagement.Size = new System.Drawing.Size(792, 567);
            this.tabUserManagement.TabIndex = 1;
            this.tabUserManagement.Text = "User Management";
            this.tabUserManagement.UseVisualStyleBackColor = true;

            // pnlContentControls
            this.pnlContentControls.Controls.Add(this.lblContentSearch);
            this.pnlContentControls.Controls.Add(this.txtContentSearch);
            this.pnlContentControls.Controls.Add(this.btnAddContent);
            this.pnlContentControls.Controls.Add(this.btnEditContent);
            this.pnlContentControls.Controls.Add(this.btnDeleteContent);
            this.pnlContentControls.Controls.Add(this.btnRefreshContent);
            this.pnlContentControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlContentControls.Location = new System.Drawing.Point(3, 3);
            this.pnlContentControls.Name = "pnlContentControls";
            this.pnlContentControls.Size = new System.Drawing.Size(786, 60);
            this.pnlContentControls.TabIndex = 0;

            // lblContentSearch
            this.lblContentSearch.AutoSize = true;
            this.lblContentSearch.Location = new System.Drawing.Point(10, 20);
            this.lblContentSearch.Name = "lblContentSearch";
            this.lblContentSearch.Size = new System.Drawing.Size(56, 20);
            this.lblContentSearch.TabIndex = 0;
            this.lblContentSearch.Text = "Search:";

            // txtContentSearch
            this.txtContentSearch.Location = new System.Drawing.Point(70, 17);
            this.txtContentSearch.Name = "txtContentSearch";
            this.txtContentSearch.Size = new System.Drawing.Size(200, 27);
            this.txtContentSearch.TabIndex = 1;

            // btnAddContent
            this.btnAddContent.Location = new System.Drawing.Point(290, 15);
            this.btnAddContent.Name = "btnAddContent";
            this.btnAddContent.Size = new System.Drawing.Size(100, 30);
            this.btnAddContent.TabIndex = 2;
            this.btnAddContent.Text = "Add";
            this.btnAddContent.UseVisualStyleBackColor = true;

            // btnEditContent
            this.btnEditContent.Location = new System.Drawing.Point(400, 15);
            this.btnEditContent.Name = "btnEditContent";
            this.btnEditContent.Size = new System.Drawing.Size(100, 30);
            this.btnEditContent.TabIndex = 3;
            this.btnEditContent.Text = "Edit";
            this.btnEditContent.UseVisualStyleBackColor = true;

            // btnDeleteContent
            this.btnDeleteContent.Location = new System.Drawing.Point(510, 15);
            this.btnDeleteContent.Name = "btnDeleteContent";
            this.btnDeleteContent.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteContent.TabIndex = 4;
            this.btnDeleteContent.Text = "Delete";
            this.btnDeleteContent.UseVisualStyleBackColor = true;

            // btnRefreshContent
            this.btnRefreshContent.Location = new System.Drawing.Point(620, 15);
            this.btnRefreshContent.Name = "btnRefreshContent";
            this.btnRefreshContent.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshContent.TabIndex = 5;
            this.btnRefreshContent.Text = "Refresh";
            this.btnRefreshContent.UseVisualStyleBackColor = true;

            // dgvContent
            this.dgvContent.AllowUserToAddRows = false;
            this.dgvContent.AllowUserToDeleteRows = false;
            this.dgvContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContent.Location = new System.Drawing.Point(3, 69);
            this.dgvContent.MultiSelect = false;
            this.dgvContent.Name = "dgvContent";
            this.dgvContent.ReadOnly = true;
            this.dgvContent.RowHeadersWidth = 51;
            this.dgvContent.RowTemplate.Height = 29;
            this.dgvContent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContent.Size = new System.Drawing.Size(786, 495);
            this.dgvContent.TabIndex = 1;

            // pnlUserControls
            this.pnlUserControls.Controls.Add(this.lblUserSearch);
            this.pnlUserControls.Controls.Add(this.txtUserSearch);
            this.pnlUserControls.Controls.Add(this.btnEditUser);
            this.pnlUserControls.Controls.Add(this.btnDeleteUser);
            this.pnlUserControls.Controls.Add(this.btnRefreshUsers);
            this.pnlUserControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlUserControls.Location = new System.Drawing.Point(3, 3);
            this.pnlUserControls.Name = "pnlUserControls";
            this.pnlUserControls.Size = new System.Drawing.Size(786, 60);
            this.pnlUserControls.TabIndex = 0;

            // lblUserSearch
            this.lblUserSearch.AutoSize = true;
            this.lblUserSearch.Location = new System.Drawing.Point(10, 20);
            this.lblUserSearch.Name = "lblUserSearch";
            this.lblUserSearch.Size = new System.Drawing.Size(56, 20);
            this.lblUserSearch.TabIndex = 0;
            this.lblUserSearch.Text = "Search:";

            // txtUserSearch
            this.txtUserSearch.Location = new System.Drawing.Point(70, 17);
            this.txtUserSearch.Name = "txtUserSearch";
            this.txtUserSearch.Size = new System.Drawing.Size(200, 27);
            this.txtUserSearch.TabIndex = 1;

            // btnEditUser
            this.btnEditUser.Location = new System.Drawing.Point(290, 15);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(100, 30);
            this.btnEditUser.TabIndex = 2;
            this.btnEditUser.Text = "Edit";
            this.btnEditUser.UseVisualStyleBackColor = true;

            // btnDeleteUser
            this.btnDeleteUser.Location = new System.Drawing.Point(400, 15);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteUser.TabIndex = 3;
            this.btnDeleteUser.Text = "Delete";
            this.btnDeleteUser.UseVisualStyleBackColor = true;

            // btnRefreshUsers
            this.btnRefreshUsers.Location = new System.Drawing.Point(510, 15);
            this.btnRefreshUsers.Name = "btnRefreshUsers";
            this.btnRefreshUsers.Size = new System.Drawing.Size(100, 30);
            this.btnRefreshUsers.TabIndex = 4;
            this.btnRefreshUsers.Text = "Refresh";
            this.btnRefreshUsers.UseVisualStyleBackColor = true;

            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(3, 69);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowHeadersWidth = 51;
            this.dgvUsers.RowTemplate.Height = 29;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(786, 495);
            this.dgvUsers.TabIndex = 1;

            // AdminPanelForm
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControl);
            this.Name = "AdminPanelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Panel";
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                LoadContentData();
            }
            else if (tabControl.SelectedIndex == 1)
            {
                LoadUserData();
            }
        }

        #region Content Management

        private void LoadContentData()
        {
            try
            {
                var contents = _contentRepository.GetAll();
                DisplayContentData(contents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading content data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayContentData(System.Collections.Generic.List<Content> contents)
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
                EpisodeCount = c.Type == "Series" ? c.Episodes.Count : 0
            }).ToList();

            // Hide ID column
            if (dgvContent.Columns["Id"] != null)
            {
                dgvContent.Columns["Id"].Visible = false;
            }

            // Auto-size columns
            dgvContent.AutoResizeColumns();
        }

        private void BtnAddContent_Click(object sender, EventArgs e)
        {
            using (var addContentForm = new AddContentForm())
            {
                if (addContentForm.ShowDialog() == DialogResult.OK)
                {
                    LoadContentData();
                }
            }
        }

        private void BtnEditContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a content to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells["Id"].Value);

            using (var editContentForm = new EditContentForm(contentId))
            {
                if (editContentForm.ShowDialog() == DialogResult.OK)
                {
                    LoadContentData();
                }
            }
        }

        private void BtnDeleteContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a content to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells["Id"].Value);
            string contentTitle = dgvContent.SelectedRows[0].Cells["Title"].Value.ToString();

            if (MessageBox.Show($"Are you sure you want to delete '{contentTitle}'? This action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _contentRepository.Delete(contentId);
                    MessageBox.Show("Content deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadContentData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefreshContent_Click(object sender, EventArgs e)
        {
            txtContentSearch.Clear();
            LoadContentData();
        }

        private void TxtContentSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtContentSearch.Text.Trim();
                var contents = _contentRepository.Search(searchTerm);
                DisplayContentData(contents);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region User Management

        private void LoadUserData()
        {
            try
            {
                var users = _userRepository.GetAll();
                DisplayUserData(users);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayUserData(System.Collections.Generic.List<User> users)
        {
            dgvUsers.DataSource = null;
            dgvUsers.Columns.Clear();

            dgvUsers.DataSource = users.Select(u => new
            {
                u.Id,
                u.IdentificationNumber,
                u.FirstName,
                u.LastName,
                u.Email,
                u.IsAdmin,
                u.RegistrationDate
            }).ToList();

            // Hide ID column
            if (dgvUsers.Columns["Id"] != null)
            {
                dgvUsers.Columns["Id"].Visible = false;
            }

            // Auto-size columns
            dgvUsers.AutoResizeColumns();
        }

        private void BtnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);

            // Prevent editing yourself
            if (userId == _currentUser.Id)
            {
                MessageBox.Show("You cannot edit your own account from here. Please use the Settings form instead.",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Open user edit form (not implemented in this example)
            MessageBox.Show("User edit functionality would be implemented here.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);

            // Prevent deleting yourself
            if (userId == _currentUser.Id)
            {
                MessageBox.Show("You cannot delete your own account.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string userName = $"{dgvUsers.SelectedRows[0].Cells["FirstName"].Value} {dgvUsers.SelectedRows[0].Cells["LastName"].Value}";

            if (MessageBox.Show($"Are you sure you want to delete user '{userName}'? This action cannot be undone.",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    _userRepository.Delete(userId);
                    MessageBox.Show("User deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUserData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting user: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRefreshUsers_Click(object sender, EventArgs e)
        {
            txtUserSearch.Clear();
            LoadUserData();
        }

        private void TxtUserSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string searchTerm = txtUserSearch.Text.Trim();
                if (string.IsNullOrEmpty(searchTerm))
                {
                    LoadUserData();
                    return;
                }

                var users = _userRepository.GetAll().Where(u =>
                    u.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.IdentificationNumber.Contains(searchTerm)).ToList();

                DisplayUserData(users);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching users: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}

