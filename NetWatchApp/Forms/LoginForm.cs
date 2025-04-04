using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace NetWatchApp.Forms
{
    public partial class LoginForm : Form
    {
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.Label lblTitle;
        private readonly UserRepository _userRepository;

        public LoginForm()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new Data.EntityFramework.NetWatchDbContext());

            // Set up event handlers
            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;

            // For testing purposes, pre-fill admin credentials
#if DEBUG
            txtEmail.Text = "admin@netwatch.com";
            txtPassword.Text = "admin123";
#endif
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnRegister = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(100, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 37);
            this.lblTitle.Text = "NetWatch App";

            // lblEmail
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(50, 100);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(46, 20);
            this.lblEmail.Text = "Email:";

            // txtEmail
            this.txtEmail.Location = new System.Drawing.Point(150, 100);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(200, 27);

            // lblPassword
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(50, 150);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(73, 20);
            this.lblPassword.Text = "Password:";

            // txtPassword
            this.txtPassword.Location = new System.Drawing.Point(150, 150);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 27);

            // btnLogin
            this.btnLogin.Location = new System.Drawing.Point(100, 200);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;

            // btnRegister
            this.btnRegister.Location = new System.Drawing.Point(220, 200);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(100, 35);
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;

            // LoginForm
            this.ClientSize = new System.Drawing.Size(400, 280);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnRegister);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetWatch App - Login";
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Depuración para ver la consulta SQL
                Debug.WriteLine($"Attempting login with email: {email}");

                var user = _userRepository.Authenticate(email, password);

                if (user != null)
                {
                    // Successful login
                    MessageBox.Show($"Welcome, {user.FirstName}!", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Open main form based on user type
                    if (user.IsAdmin)
                    {
                        var adminForm = new AdminDashboardForm(user);
                        this.Hide();
                        adminForm.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        var mainForm = new MainForm(user);
                        this.Hide();
                        mainForm.ShowDialog();
                        this.Show();
                    }
                }
                else
                {
                    // Failed login
                    MessageBox.Show("Invalid email or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            var registerForm = new RegisterForm();
            if (registerForm.ShowDialog() == DialogResult.OK)
            {
                // If registration was successful, pre-fill the email
                txtEmail.Text = registerForm.RegisteredEmail;
                txtPassword.Focus();
            }
        }
    }
}

