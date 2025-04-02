using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class LoginForm : Form
    {
        private readonly UserRepository _userRepository;

        public LoginForm()
        {
            InitializeComponent();
            _userRepository = new UserRepository(new NetWatchDbContext());

            // Set up event handlers
            btnLogin.Click += BtnLogin_Click;
            btnRegister.Click += BtnRegister_Click;
            lnkForgotPassword.LinkClicked += LnkForgotPassword_LinkClicked;
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
            this.lnkForgotPassword = new System.Windows.Forms.LinkLabel();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(100, 30);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 37);
            this.lblTitle.Text = "NetWatch Login";

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
            this.btnLogin.Location = new System.Drawing.Point(150, 200);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(100, 35);
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;

            // btnRegister
            this.btnRegister.Location = new System.Drawing.Point(260, 200);
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(100, 35);
            this.btnRegister.Text = "Register";
            this.btnRegister.UseVisualStyleBackColor = true;

            // lnkForgotPassword
            this.lnkForgotPassword.AutoSize = true;
            this.lnkForgotPassword.Location = new System.Drawing.Point(150, 250);
            this.lnkForgotPassword.Name = "lnkForgotPassword";
            this.lnkForgotPassword.Size = new System.Drawing.Size(124, 20);
            this.lnkForgotPassword.TabIndex = 0;
            this.lnkForgotPassword.TabStop = true;
            this.lnkForgotPassword.Text = "Forgot Password?";

            // LoginForm
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.lnkForgotPassword);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NetWatch - Login";
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string email = txtEmail.Text.Trim();
                string password = txtPassword.Text;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var user = _userRepository.Authenticate(email, password);
                if (user != null)
                {
                    // Login successful
                    this.Hide();

                    // Open main form
                    using (var mainForm = new MainForm(user))
                    {
                        mainForm.ShowDialog();
                    }

                    // Show login form again when main form is closed
                    this.Show();
                    txtPassword.Clear();
                }
                else
                {
                    MessageBox.Show("Invalid email or password.", "Authentication Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during login: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // Open registration form
            using (var registerForm = new RegisterForm())
            {
                registerForm.ShowDialog();
            }
        }

        private void LnkForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("Please contact the administrator to reset your password.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
