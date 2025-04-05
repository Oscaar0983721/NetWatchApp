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
        private System.Windows.Forms.Label lblStatus;
        private readonly UserRepository _userRepository;

        public LoginForm()
        {
            try
            {
                Debug.WriteLine("Iniciando LoginForm...");
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

                Debug.WriteLine("LoginForm inicializado correctamente");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al inicializar LoginForm: {ex.Message}");
                MessageBox.Show($"Error al inicializar la ventana de inicio de sesión: {ex.Message}\n\n{ex.StackTrace}",
                    "Error de Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            this.lblStatus = new System.Windows.Forms.Label();

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

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblStatus.Location = new System.Drawing.Point(100, 250);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(200, 20);
            this.lblStatus.Text = "";
            this.lblStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // LoginForm
            this.ClientSize = new System.Drawing.Size(400, 280);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnRegister);
            this.Controls.Add(this.lblStatus);
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

                Debug.WriteLine($"Intentando iniciar sesión con email: {email}");

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both email and password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Disable login button and show status
                btnLogin.Enabled = false;
                lblStatus.Text = "Logging in...";
                lblStatus.Visible = true;
                Application.DoEvents(); // Force UI update

                var user = _userRepository.Authenticate(email, password);

                if (user != null)
                {
                    // Successful login
                    Debug.WriteLine($"Inicio de sesión exitoso para: {user.FirstName} {user.LastName}");
                    lblStatus.Text = "Login successful!";

                    // Open main form based on user type
                    if (user.IsAdmin)
                    {
                        Debug.WriteLine("Abriendo AdminDashboardForm...");
                        MessageBox.Show($"Welcome, {user.FirstName}! Logging in as Administrator.", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        var adminForm = new AdminDashboardForm(user);
                        this.Hide();
                        adminForm.ShowDialog();
                        this.Show();
                    }
                    else
                    {
                        Debug.WriteLine("Abriendo MainForm para usuario regular...");
                        try
                        {
                            // Show a welcome message before showing the main form
                            MessageBox.Show($"Welcome, {user.FirstName}! Please wait while the content loads.", "Login Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Create the main form
                            var mainForm = new MainForm(user);
                            Debug.WriteLine("MainForm instanciado correctamente");

                            // Hide the login form
                            this.Hide();
                            Debug.WriteLine("LoginForm oculto, mostrando MainForm...");

                            // Show the main form as a dialog to ensure proper modal behavior
                            mainForm.ShowDialog();

                            // Show login form again when main form closes
                            this.Show();
                            Debug.WriteLine("MainForm cerrado, mostrando LoginForm nuevamente");
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error crítico al abrir MainForm: {ex.Message}");
                            Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                            MessageBox.Show($"Error al abrir la ventana principal:\n\n{ex.Message}\n\n{ex.StackTrace}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Show(); // Ensure login form is visible again if MainForm fails
                        }
                    }
                }
                else
                {
                    // Failed login
                    Debug.WriteLine("Inicio de sesión fallido: credenciales inválidas");
                    lblStatus.Text = "Invalid credentials";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    MessageBox.Show("Invalid email or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Re-enable login button
                btnLogin.Enabled = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error durante el inicio de sesión: {ex.Message}");
                lblStatus.Text = "Error logging in";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show($"Error during login: {ex.Message}\n\n{ex.StackTrace}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Re-enable login button
                btnLogin.Enabled = true;
            }
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                Debug.WriteLine("Abriendo formulario de registro...");
                var registerForm = new RegisterForm();
                if (registerForm.ShowDialog() == DialogResult.OK)
                {
                    // If registration was successful, pre-fill the email
                    txtEmail.Text = registerForm.RegisteredEmail;
                    txtPassword.Focus();
                    lblStatus.Text = "Registration successful! Please login.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    lblStatus.Visible = true;
                    Debug.WriteLine($"Registro exitoso para: {registerForm.RegisteredEmail}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al abrir el formulario de registro: {ex.Message}");
                MessageBox.Show($"Error opening registration form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

