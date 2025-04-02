using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class SettingsForm : Form
    {
        private readonly User _currentUser;
        private readonly UserRepository _userRepository;

        public SettingsForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRepository = new UserRepository(new NetWatchDbContext());

            // Fill form with user data
            txtIdentificationNumber.Text = _currentUser.IdentificationNumber;
            txtFirstName.Text = _currentUser.FirstName;
            txtLastName.Text = _currentUser.LastName;
            txtEmail.Text = _currentUser.Email;

            // Set up event handlers
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnChangePassword.Click += BtnChangePassword_Click;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblIdentificationNumber = new System.Windows.Forms.Label();
            this.txtIdentificationNumber = new System.Windows.Forms.TextBox();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.btnChangePassword = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(100, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 37);
            this.lblTitle.Text = "Account Settings";

            // lblIdentificationNumber
            this.lblIdentificationNumber.AutoSize = true;
            this.lblIdentificationNumber.Location = new System.Drawing.Point(30, 80);
            this.lblIdentificationNumber.Name = "lblIdentificationNumber";
            this.lblIdentificationNumber.Size = new System.Drawing.Size(150, 20);
            this.lblIdentificationNumber.Text = "Identification Number:";

            // txtIdentificationNumber
            this.txtIdentificationNumber.Location = new System.Drawing.Point(180, 80);
            this.txtIdentificationNumber.Name = "txtIdentificationNumber";
            this.txtIdentificationNumber.Size = new System.Drawing.Size(200, 27);

            // lblFirstName
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(30, 120);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(80, 20);
            this.lblFirstName.Text = "First Name:";

            // txtFirstName
            this.txtFirstName.Location = new System.Drawing.Point(180, 120);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(200, 27);

            // lblLastName
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(30, 160);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(79, 20);
            this.lblLastName.Text = "Last Name:";

            // txtLastName
            this.txtLastName.Location = new System.Drawing.Point(180, 160);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(200, 27);

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(30, 200);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(46, 20);
            this.lblEmail.Text = "Email:";

            // txtEmail
            this.txtEmail.Location = new System.Drawing.Point(180, 200);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(200, 27);

            // btnChangePassword
            this.btnChangePassword.Location = new System.Drawing.Point(180, 240);
            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Size = new System.Drawing.Size(150, 35);
            this.btnChangePassword.Text = "Change Password";
            this.btnChangePassword.UseVisualStyleBackColor = true;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(180, 290);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(290, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // SettingsForm
            this.ClientSize = new System.Drawing.Size(420, 350);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblIdentificationNumber);
            this.Controls.Add(this.txtIdentificationNumber);
            this.Controls.Add(this.lblFirstName);
            this.Controls.Add(this.txtFirstName);
            this.Controls.Add(this.lblLastName);
            this.Controls.Add(this.txtLastName);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.btnChangePassword);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Account Settings";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    // Update user properties
                    _currentUser.IdentificationNumber = txtIdentificationNumber.Text.Trim();
                    _currentUser.FirstName = txtFirstName.Text.Trim();
                    _currentUser.LastName = txtLastName.Text.Trim();
                    _currentUser.Email = txtEmail.Text.Trim();

                    // Save changes
                    _userRepository.Update(_currentUser);

                    MessageBox.Show("Settings saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtIdentificationNumber.Text))
            {
                MessageBox.Show("Please enter your identification number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Please enter your first name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Please enter your last name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please enter your email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            using (var changePasswordForm = new ChangePasswordForm(_currentUser))
            {
                changePasswordForm.ShowDialog();
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    public partial class ChangePasswordForm : Form
    {
        private readonly User _currentUser;
        private readonly UserRepository _userRepository;

        public ChangePasswordForm(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _userRepository = new UserRepository(new NetWatchDbContext());

            // Set up event handlers
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblCurrentPassword = new System.Windows.Forms.Label();
            this.txtCurrentPassword = new System.Windows.Forms.TextBox();
            this.lblNewPassword = new System.Windows.Forms.Label();
            this.txtNewPassword = new System.Windows.Forms.TextBox();
            this.lblConfirmPassword = new System.Windows.Forms.Label();
            this.txtConfirmPassword = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(80, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 32);
            this.lblTitle.Text = "Change Password";

            // lblCurrentPassword
            this.lblCurrentPassword.AutoSize = true;
            this.lblCurrentPassword.Location = new System.Drawing.Point(30, 80);
            this.lblCurrentPassword.Name = "lblCurrentPassword";
            this.lblCurrentPassword.Size = new System.Drawing.Size(122, 20);
            this.lblCurrentPassword.Text = "Current Password:";

            // txtCurrentPassword
            this.txtCurrentPassword.Location = new System.Drawing.Point(160, 80);
            this.txtCurrentPassword.Name = "txtCurrentPassword";
            this.txtCurrentPassword.PasswordChar = '*';
            this.txtCurrentPassword.Size = new System.Drawing.Size(200, 27);

            // lblNewPassword
            this.lblNewPassword.AutoSize = true;
            this.lblNewPassword.Location = new System.Drawing.Point(30, 120);
            this.lblNewPassword.Name = "lblNewPassword";
            this.lblNewPassword.Size = new System.Drawing.Size(104, 20);
            this.lblNewPassword.Text = "New Password:";

            // txtNewPassword
            this.txtNewPassword.Location = new System.Drawing.Point(160, 120);
            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.PasswordChar = '*';
            this.txtNewPassword.Size = new System.Drawing.Size(200, 27);

            // lblConfirmPassword
            this.lblConfirmPassword.AutoSize = true;
            this.lblConfirmPassword.Location = new System.Drawing.Point(30, 160);
            this.lblConfirmPassword.Name = "lblConfirmPassword";
            this.lblConfirmPassword.Size = new System.Drawing.Size(132, 20);
            this.lblConfirmPassword.Text = "Confirm Password:";

            // txtConfirmPassword
            this.txtConfirmPassword.Location = new System.Drawing.Point(160, 160);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '*';
            this.txtConfirmPassword.Size = new System.Drawing.Size(200, 27);

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(160, 210);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(270, 210);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // ChangePasswordForm
            this.ClientSize = new System.Drawing.Size(400, 270);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblCurrentPassword);
            this.Controls.Add(this.txtCurrentPassword);
            this.Controls.Add(this.lblNewPassword);
            this.Controls.Add(this.txtNewPassword);
            this.Controls.Add(this.lblConfirmPassword);
            this.Controls.Add(this.txtConfirmPassword);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChangePasswordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Change Password";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    // Update user password
                    _currentUser.Password = txtNewPassword.Text;

                    // Save changes
                    _userRepository.Update(_currentUser);

                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtCurrentPassword.Text))
            {
                MessageBox.Show("Please enter your current password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtCurrentPassword.Text != _currentUser.Password)
            {
                MessageBox.Show("Current password is incorrect.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                MessageBox.Show("Please enter a new password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtNewPassword.Text.Length < 6)
            {
                MessageBox.Show("New password must be at least 6 characters long.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("New passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
