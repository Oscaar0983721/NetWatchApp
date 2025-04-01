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

        public AdminPanelForm()
        {
            InitializeComponent();
            _contentRepository = new ContentRepository(new NetWatchDbContext());
            _userRepository = new UserRepository(new NetWatchDbContext());

            // Load initial data
            LoadContentTab();
        }

        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabContent = new System.Windows.Forms.TabPage();
            this.tabUsers = new System.Windows.Forms.TabPage();
            this.tabReports = new System.Windows.Forms.TabPage();

            this.pnlContentList = new System.Windows.Forms.Panel();
            this.dgvContent = new System.Windows.Forms.DataGridView();
            this.btnAddContent = new System.Windows.Forms.Button();
            this.btnEditContent = new System.Windows.Forms.Button();
            this.btnDeleteContent = new System.Windows.Forms.Button();

            this.pnlUserList = new System.Windows.Forms.Panel();
            this.dgvUsers = new System.Windows.Forms.DataGridView();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnDeleteUser = new System.Windows.Forms.Button();

            this.pnlReports = new System.Windows.Forms.Panel();
            this.btnPopularContent = new System.Windows.Forms.Button();
            this.btnSubscriptions = new System.Windows.Forms.Button();
            this.btnWatchTime = new System.Windows.Forms.Button();
            this.rtbReportOutput = new System.Windows.Forms.RichTextBox();

            this.tabControl.SuspendLayout();
            this.tabContent.SuspendLayout();
            this.tabUsers.SuspendLayout();
            this.tabReports.SuspendLayout();
            this.pnlContentList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContent)).BeginInit();
            this.pnlUserList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).BeginInit();
            this.pnlReports.SuspendLayout();
            this.SuspendLayout();

            // tabControl
            this.tabControl.Controls.Add(this.tabContent);
            this.tabControl.Controls.Add(this.tabUsers);
            this.tabControl.Controls.Add(this.tabReports);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(800, 600);
            this.tabControl.TabIndex = 0;

            // tabContent
            this.tabContent.Controls.Add(this.pnlContentList);
            this.tabContent.Location = new System.Drawing.Point(4, 24);
            this.tabContent.Name = "tabContent";
            this.tabContent.Padding = new System.Windows.Forms.Padding(3);
            this.tabContent.Size = new System.Drawing.Size(792, 572);
            this.tabContent.TabIndex = 0;
            this.tabContent.Text = "Content Management";
            this.tabContent.UseVisualStyleBackColor = true;

            // pnlContentList
            this.pnlContentList.Controls.Add(this.dgvContent);
            this.pnlContentList.Controls.Add(this.btnAddContent);
            this.pnlContentList.Controls.Add(this.btnEditContent);
            this.pnlContentList.Controls.Add(this.btnDeleteContent);
            this.pnlContentList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContentList.Location = new System.Drawing.Point(3, 3);
            this.pnlContentList.Name = "pnlContentList";
            this.pnlContentList.Size = new System.Drawing.Size(786, 566);
            this.pnlContentList.TabIndex = 0;

            // dgvContent
            this.dgvContent.AllowUserToAddRows = false;
            this.dgvContent.AllowUserToDeleteRows = false;
            this.dgvContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvContent.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvContent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvContent.Location = new System.Drawing.Point(0, 0);
            this.dgvContent.MultiSelect = false;
            this.dgvContent.Name = "dgvContent";
            this.dgvContent.ReadOnly = true;
            this.dgvContent.RowTemplate.Height = 25;
            this.dgvContent.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContent.Size = new System.Drawing.Size(786, 520);
            this.dgvContent.TabIndex = 0;

            // btnAddContent
            this.btnAddContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAddContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddContent.ForeColor = System.Drawing.Color.White;
            this.btnAddContent.Location = new System.Drawing.Point(0, 526);
            this.btnAddContent.Name = "btnAddContent";
            this.btnAddContent.Size = new System.Drawing.Size(150, 40);
            this.btnAddContent.TabIndex = 1;
            this.btnAddContent.Text = "Add Content";
            this.btnAddContent.UseVisualStyleBackColor = false;
            this.btnAddContent.Click += new System.EventHandler(this.btnAddContent_Click);

            // btnEditContent
            this.btnEditContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnEditContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditContent.ForeColor = System.Drawing.Color.White;
            this.btnEditContent.Location = new System.Drawing.Point(160, 526);
            this.btnEditContent.Name = "btnEditContent";
            this.btnEditContent.Size = new System.Drawing.Size(150, 40);
            this.btnEditContent.TabIndex = 2;
            this.btnEditContent.Text = "Edit Content";
            this.btnEditContent.UseVisualStyleBackColor = false;
            this.btnEditContent.Click += new System.EventHandler(this.btnEditContent_Click);

            // btnDeleteContent
            this.btnDeleteContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDeleteContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteContent.ForeColor = System.Drawing.Color.White;
            this.btnDeleteContent.Location = new System.Drawing.Point(320, 526);
            this.btnDeleteContent.Name = "btnDeleteContent";
            this.btnDeleteContent.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteContent.TabIndex = 3;
            this.btnDeleteContent.Text = "Delete Content";
            this.btnDeleteContent.UseVisualStyleBackColor = false;
            this.btnDeleteContent.Click += new System.EventHandler(this.btnDeleteContent_Click);

            // tabUsers
            this.tabUsers.Controls.Add(this.pnlUserList);
            this.tabUsers.Location = new System.Drawing.Point(4, 24);
            this.tabUsers.Name = "tabUsers";
            this.tabUsers.Padding = new System.Windows.Forms.Padding(3);
            this.tabUsers.Size = new System.Drawing.Size(792, 572);
            this.tabUsers.TabIndex = 1;
            this.tabUsers.Text = "User Management";
            this.tabUsers.UseVisualStyleBackColor = true;

            // pnlUserList
            this.pnlUserList.Controls.Add(this.dgvUsers);
            this.pnlUserList.Controls.Add(this.btnAddUser);
            this.pnlUserList.Controls.Add(this.btnEditUser);
            this.pnlUserList.Controls.Add(this.btnDeleteUser);
            this.pnlUserList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlUserList.Location = new System.Drawing.Point(3, 3);
            this.pnlUserList.Name = "pnlUserList";
            this.pnlUserList.Size = new System.Drawing.Size(786, 566);
            this.pnlUserList.TabIndex = 1;

            // dgvUsers
            this.dgvUsers.AllowUserToAddRows = false;
            this.dgvUsers.AllowUserToDeleteRows = false;
            this.dgvUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvUsers.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUsers.Location = new System.Drawing.Point(0, 0);
            this.dgvUsers.MultiSelect = false;
            this.dgvUsers.Name = "dgvUsers";
            this.dgvUsers.ReadOnly = true;
            this.dgvUsers.RowTemplate.Height = 25;
            this.dgvUsers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsers.Size = new System.Drawing.Size(786, 520);
            this.dgvUsers.TabIndex = 0;

            // btnAddUser
            this.btnAddUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAddUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddUser.ForeColor = System.Drawing.Color.White;
            this.btnAddUser.Location = new System.Drawing.Point(0, 526);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(150, 40);
            this.btnAddUser.TabIndex = 1;
            this.btnAddUser.Text = "Add User";
            this.btnAddUser.UseVisualStyleBackColor = false;
            this.btnAddUser.Click += new System.EventHandler(this.btnAddUser_Click);

            // btnEditUser
            this.btnEditUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnEditUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditUser.ForeColor = System.Drawing.Color.White;
            this.btnEditUser.Location = new System.Drawing.Point(160, 526);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(150, 40);
            this.btnEditUser.TabIndex = 2;
            this.btnEditUser.Text = "Edit User";
            this.btnEditUser.UseVisualStyleBackColor = false;
            this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);

            // btnDeleteUser
            this.btnDeleteUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteUser.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnDeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteUser.ForeColor = System.Drawing.Color.White;
            this.btnDeleteUser.Location = new System.Drawing.Point(320, 526);
            this.btnDeleteUser.Name = "btnDeleteUser";
            this.btnDeleteUser.Size = new System.Drawing.Size(150, 40);
            this.btnDeleteUser.TabIndex = 3;
            this.btnDeleteUser.Text = "Delete User";
            this.btnDeleteUser.UseVisualStyleBackColor = false;
            this.btnDeleteUser.Click += new System.EventHandler(this.btnDeleteUser_Click);

            // tabReports
            this.tabReports.Controls.Add(this.pnlReports);
            this.tabReports.Location = new System.Drawing.Point(4, 24);
            this.tabReports.Name = "tabReports";
            this.tabReports.Padding = new System.Windows.Forms.Padding(3);
            this.tabReports.Size = new System.Drawing.Size(792, 572);
            this.tabReports.TabIndex = 2;
            this.tabReports.Text = "Reports";
            this.tabReports.UseVisualStyleBackColor = true;

            // pnlReports
            this.pnlReports.Controls.Add(this.btnPopularContent);
            this.pnlReports.Controls.Add(this.btnSubscriptions);
            this.pnlReports.Controls.Add(this.btnWatchTime);
            this.pnlReports.Controls.Add(this.rtbReportOutput);
            this.pnlReports.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlReports.Location = new System.Drawing.Point(3, 3);
            this.pnlReports.Name = "pnlReports";
            this.pnlReports.Size = new System.Drawing.Size(786, 566);
            this.pnlReports.TabIndex = 0;

            // btnPopularContent
            this.btnPopularContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnPopularContent.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPopularContent.ForeColor = System.Drawing.Color.White;
            this.btnPopularContent.Location = new System.Drawing.Point(10, 10);
            this.btnPopularContent.Name = "btnPopularContent";
            this.btnPopularContent.Size = new System.Drawing.Size(150, 40);
            this.btnPopularContent.TabIndex = 0;
            this.btnPopularContent.Text = "Popular Content";
            this.btnPopularContent.UseVisualStyleBackColor = false;
            this.btnPopularContent.Click += new System.EventHandler(this.btnPopularContent_Click);

            // btnSubscriptions
            this.btnSubscriptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSubscriptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubscriptions.ForeColor = System.Drawing.Color.White;
            this.btnSubscriptions.Location = new System.Drawing.Point(170, 10);
            this.btnSubscriptions.Name = "btnSubscriptions";
            this.btnSubscriptions.Size = new System.Drawing.Size(150, 40);
            this.btnSubscriptions.TabIndex = 1;
            this.btnSubscriptions.Text = "Subscriptions";
            this.btnSubscriptions.UseVisualStyleBackColor = false;
            this.btnSubscriptions.Click += new System.EventHandler(this.btnSubscriptions_Click);

            // btnWatchTime
            this.btnWatchTime.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnWatchTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWatchTime.ForeColor = System.Drawing.Color.White;
            this.btnWatchTime.Location = new System.Drawing.Point(330, 10);
            this.btnWatchTime.Name = "btnWatchTime";
            this.btnWatchTime.Size = new System.Drawing.Size(150, 40);
            this.btnWatchTime.TabIndex = 2;
            this.btnWatchTime.Text = "Watch Time";
            this.btnWatchTime.UseVisualStyleBackColor = false;
            this.btnWatchTime.Click += new System.EventHandler(this.btnWatchTime_Click);

            // rtbReportOutput
            this.rtbReportOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbReportOutput.Location = new System.Drawing.Point(10, 60);
            this.rtbReportOutput.Name = "rtbReportOutput";
            this.rtbReportOutput.ReadOnly = true;
            this.rtbReportOutput.Size = new System.Drawing.Size(766, 496);
            this.rtbReportOutput.TabIndex = 3;
            this.rtbReportOutput.Text = "";

            // AdminPanelForm
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.tabControl);
            this.Name = "AdminPanelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Admin Panel";
            this.tabControl.ResumeLayout(false);
            this.tabContent.ResumeLayout(false);
            this.tabUsers.ResumeLayout(false);
            this.tabReports.ResumeLayout(false);
            this.pnlContentList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContent)).EndInit();
            this.pnlUserList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUsers)).EndInit();
            this.pnlReports.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabContent;
        private System.Windows.Forms.TabPage tabUsers;
        private System.Windows.Forms.TabPage tabReports;
        private System.Windows.Forms.Panel pnlContentList;
        private System.Windows.Forms.DataGridView dgvContent;
        private System.Windows.Forms.Button btnAddContent;
        private System.Windows.Forms.Button btnEditContent;
        private System.Windows.Forms.Button btnDeleteContent;
        private System.Windows.Forms.Panel pnlUserList;
        private System.Windows.Forms.DataGridView dgvUsers;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnDeleteUser;
        private System.Windows.Forms.Panel pnlReports;
        private System.Windows.Forms.Button btnPopularContent;
        private System.Windows.Forms.Button btnSubscriptions;
        private System.Windows.Forms.Button btnWatchTime;
        private System.Windows.Forms.RichTextBox rtbReportOutput;

        private async void LoadContentTab()
        {
            var contents = await _contentRepository.GetAllAsync();

            // Configure DataGridView
            dgvContent.DataSource = contents.Select(c => new
            {
                c.Id,
                c.Title,
                Type = c.Type.ToString(),
                c.Genre,
                c.Platform,
                c.ReleaseYear
            }).ToList();

            // Add tab selection event
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
        }

        private async void LoadUsersTab()
        {
            var users = await _userRepository.GetAllAsync();

            // Configure DataGridView
            dgvUsers.DataSource = users.Select(u => new
            {
                u.Id,
                u.IdentificationNumber,
                u.Name,
                u.Email,
                Role = u.IsAdmin ? "Admin" : "User",
                u.RegistrationDate
            }).ToList();
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl.SelectedTab == tabUsers)
            {
                LoadUsersTab();
            }
        }

        private void btnAddContent_Click(object sender, EventArgs e)
        {
            // Open content add form
            MessageBox.Show("Content add functionality would be implemented here.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a content to edit.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells["Id"].Value);

            // Open content edit form
            MessageBox.Show($"Content edit functionality for ID {contentId} would be implemented here.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnDeleteContent_Click(object sender, EventArgs e)
        {
            if (dgvContent.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a content to delete.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int contentId = Convert.ToInt32(dgvContent.SelectedRows[0].Cells["Id"].Value);
            string contentTitle = dgvContent.SelectedRows[0].Cells["Title"].Value.ToString();

            DialogResult result = MessageBox.Show($"Are you sure you want to delete '{contentTitle}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = await _contentRepository.DeleteAsync(contentId);

                if (success)
                {
                    MessageBox.Show("Content deleted successfully.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadContentTab();
                }
                else
                {
                    MessageBox.Show("An error occurred while deleting the content.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            // Open user add form
            MessageBox.Show("User add functionality would be implemented here.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to edit.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);

            // Open user edit form
            MessageBox.Show($"User edit functionality for ID {userId} would be implemented here.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user to delete.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int userId = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["Id"].Value);
            string userName = dgvUsers.SelectedRows[0].Cells["Name"].Value.ToString();

            DialogResult result = MessageBox.Show($"Are you sure you want to delete user '{userName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                bool success = await _userRepository.DeleteAsync(userId);

                if (success)
                {
                    MessageBox.Show("User deleted successfully.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadUsersTab();
                }
                else
                {
                    MessageBox.Show("An error occurred while deleting the user.", "Admin Panel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnPopularContent_Click(object sender, EventArgs e)
        {
            rtbReportOutput.Clear();
            rtbReportOutput.AppendText("Popular Content Report\n");
            rtbReportOutput.AppendText("======================\n\n");
            rtbReportOutput.AppendText("This report would show the most popular content based on views and ratings.\n\n");
            rtbReportOutput.AppendText("Top 10 Most Watched Content:\n");
            rtbReportOutput.AppendText("1. Series 5 - 245 views\n");
            rtbReportOutput.AppendText("2. Movie 12 - 198 views\n");
            rtbReportOutput.AppendText("3. Series 2 - 176 views\n");
            rtbReportOutput.AppendText("4. Movie 7 - 154 views\n");
            rtbReportOutput.AppendText("5. Series 9 - 132 views\n");
            rtbReportOutput.AppendText("6. Movie 3 - 121 views\n");
            rtbReportOutput.AppendText("7. Series 1 - 119 views\n");
            rtbReportOutput.AppendText("8. Movie 22 - 98 views\n");
            rtbReportOutput.AppendText("9. Series 15 - 87 views\n");
            rtbReportOutput.AppendText("10. Movie 18 - 76 views\n\n");
            rtbReportOutput.AppendText("Top 10 Highest Rated Content:\n");
            rtbReportOutput.AppendText("1. Series 3 - 4.9/5\n");
            rtbReportOutput.AppendText("2. Movie 5 - 4.8/5\n");
            rtbReportOutput.AppendText("3. Series 7 - 4.7/5\n");
            rtbReportOutput.AppendText("4. Movie 9 - 4.6/5\n");
            rtbReportOutput.AppendText("5. Series 12 - 4.5/5\n");
        }

        private void btnSubscriptions_Click(object sender, EventArgs e)
        {
            rtbReportOutput.Clear();
            rtbReportOutput.AppendText("Subscriptions Report\n");
            rtbReportOutput.AppendText("===================\n\n");
            rtbReportOutput.AppendText("This report would show subscription statistics for the last month.\n\n");
            rtbReportOutput.AppendText("New Users in the Last Month: 45\n");
            rtbReportOutput.AppendText("Active Users: 132\n");
            rtbReportOutput.AppendText("Inactive Users: 18\n\n");
            rtbReportOutput.AppendText("User Growth by Month:\n");
            rtbReportOutput.AppendText("January: 32 new users\n");
            rtbReportOutput.AppendText("February: 28 new users\n");
            rtbReportOutput.AppendText("March: 37 new users\n");
            rtbReportOutput.AppendText("April: 45 new users\n");
        }

        private void btnWatchTime_Click(object sender, EventArgs e)
        {
            rtbReportOutput.Clear();
            rtbReportOutput.AppendText("Watch Time Report\n");
            rtbReportOutput.AppendText("================\n\n");
            rtbReportOutput.AppendText("This report would show watch time statistics.\n\n");
            rtbReportOutput.AppendText("Total Watch Time: 12,450 hours\n");
            rtbReportOutput.AppendText("Average Watch Time per User: 83 hours\n\n");
            rtbReportOutput.AppendText("Watch Time by Content Type:\n");
            rtbReportOutput.AppendText("Movies: 5,230 hours (42%)\n");
            rtbReportOutput.AppendText("Series: 7,220 hours (58%)\n\n");
            rtbReportOutput.AppendText("Watch Time by Genre:\n");
            rtbReportOutput.AppendText("Action: 2,890 hours (23%)\n");
            rtbReportOutput.AppendText("Comedy: 2,245 hours (18%)\n");
            rtbReportOutput.AppendText("Drama: 1,980 hours (16%)\n");
            rtbReportOutput.AppendText("Sci-Fi: 1,745 hours (14%)\n");
            rtbReportOutput.AppendText("Horror: 1,120 hours (9%)\n");
            rtbReportOutput.AppendText("Other: 2,470 hours (20%)\n");
        }
    }
}