using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using System.ComponentModel;

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
        private System.Windows.Forms.Label lblLoading;

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

        // Background workers for async operations
        private BackgroundWorker _contentLoader;
        private BackgroundWorker _myListLoader;
        private BackgroundWorker _recentlyWatchedLoader;
        private BackgroundWorker _ratingsLoader;

        // Cache for content images to avoid reloading
        private Dictionary<string, Image> _imageCache = new Dictionary<string, Image>();

        public MainForm(User currentUser)
        {
            Debug.WriteLine("MainForm constructor started");
            try
            {
                Debug.WriteLine("Iniciando MainForm...");
                _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser), "User cannot be null");
                Debug.WriteLine($"Usuario: {_currentUser?.FirstName} {_currentUser?.LastName}, ID: {_currentUser?.Id}");

                // Inicializar repositorios
                Debug.WriteLine("Inicializando contexto de base de datos...");
                var context = new Data.EntityFramework.NetWatchDbContext();
                _contentRepository = new ContentRepository(context);
                _ratingRepository = new RatingRepository(context);
                _viewingHistoryRepository = new ViewingHistoryRepository(context);
                _userRepository = new UserRepository(context);

                // Initialize background workers
                InitializeBackgroundWorkers();

                Debug.WriteLine("Llamando a InitializeComponent...");
                InitializeComponent();

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

                Debug.WriteLine("MainForm inicializado correctamente");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error crítico al inicializar MainForm: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                // Show more detailed error and ensure it's visible to the user
                MessageBox.Show($"Error crítico al inicializar la ventana principal.\n\nDetalles: {ex.Message}\n\nStack trace: {ex.StackTrace}",
                    "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Try to ensure form is still visible despite the error
                try
                {
                    this.Show();
                    this.BringToFront();
                }
                catch { /* Last resort - ignore if this fails too */ }
            }
        }

        private void InitializeBackgroundWorkers()
        {
            // Content loader
            _contentLoader = new BackgroundWorker();
            _contentLoader.DoWork += ContentLoader_DoWork;
            _contentLoader.RunWorkerCompleted += ContentLoader_RunWorkerCompleted;
            _contentLoader.WorkerSupportsCancellation = true;

            // My list loader
            _myListLoader = new BackgroundWorker();
            _myListLoader.DoWork += MyListLoader_DoWork;
            _myListLoader.RunWorkerCompleted += MyListLoader_RunWorkerCompleted;
            _myListLoader.WorkerSupportsCancellation = true;

            // Recently watched loader
            _recentlyWatchedLoader = new BackgroundWorker();
            _recentlyWatchedLoader.DoWork += RecentlyWatchedLoader_DoWork;
            _recentlyWatchedLoader.RunWorkerCompleted += RecentlyWatchedLoader_RunWorkerCompleted;
            _recentlyWatchedLoader.WorkerSupportsCancellation = true;

            // Ratings loader
            _ratingsLoader = new BackgroundWorker();
            _ratingsLoader.DoWork += RatingsLoader_DoWork;
            _ratingsLoader.RunWorkerCompleted += RatingsLoader_RunWorkerCompleted;
            _ratingsLoader.WorkerSupportsCancellation = true;
        }

        private void InitializeComponent()
        {
            Debug.WriteLine("InitializeComponent started");
            try
            {
                Debug.WriteLine("Iniciando InitializeComponent...");

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
                this.lblLoading = new System.Windows.Forms.Label();

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

                ((System.ComponentModel.ISupportInitialize)(this.dgvMyRatings)).BeginInit();
                this.SuspendLayout();

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

                // lblLoading
                this.lblLoading.AutoSize = true;
                this.lblLoading.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
                this.lblLoading.Location = new System.Drawing.Point(350, 250);
                this.lblLoading.Name = "lblLoading";
                this.lblLoading.Size = new System.Drawing.Size(200, 28);
                this.lblLoading.Text = "Loading content...";
                this.lblLoading.Visible = false;

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
                this.tabBrowse.Controls.Add(this.lblLoading);

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

                ((System.ComponentModel.ISupportInitialize)(this.dgvMyRatings)).EndInit();
                this.ResumeLayout(false);

                Debug.WriteLine("InitializeComponent completado");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en InitializeComponent: {ex.Message}");
                MessageBox.Show($"Error al inicializar componentes: {ex.Message}\n\n{ex.StackTrace}",
                    "Error de Inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Debug.WriteLine("MainForm_Load started");
            Debug.WriteLine($"Current user in MainForm_Load: {_currentUser?.Id} - {_currentUser?.FirstName} {_currentUser?.LastName}");
            try
            {
                Debug.WriteLine("MainForm_Load iniciando...");

                // Set up user profile information
                lblUserName.Text = $"{_currentUser.FirstName} {_currentUser.LastName}";
                lblEmail.Text = $"Email: {_currentUser.Email}";
                lblRegistrationDate.Text = $"Member since: {_currentUser.RegistrationDate.ToShortDateString()}";

                // Load filter options
                LoadFilterOptions();

                // Show welcome message
                MessageBox.Show($"Welcome, {_currentUser.FirstName}! The content is loading, please wait a moment.",
                    "NetWatch App", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Start loading content asynchronously
                StartContentLoading();

                Debug.WriteLine("MainForm_Load completado");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en MainForm_Load: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error al cargar datos: {ex.Message}\n\n{ex.StackTrace}",
                    "Error de Carga", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // Refresh data when switching tabs
                switch (tabControl.SelectedIndex)
                {
                    case 0: // Browse tab
                        StartContentLoading();
                        break;
                    case 1: // My List tab
                        StartMyListLoading();
                        break;
                    case 2: // Profile tab
                        StartRecentlyWatchedLoading();
                        StartRatingsLoading();
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en TabControl_SelectedIndexChanged: {ex.Message}");
                MessageBox.Show($"Error al cambiar de pestaña: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Background Worker Methods

        private void StartContentLoading()
        {
            try
            {
                // Cancel any ongoing operation
                if (_contentLoader.IsBusy)
                {
                    _contentLoader.CancelAsync();
                    return;
                }

                // Show loading indicator
                flpContent.Controls.Clear();
                lblLoading.Visible = true;
                flpContent.Controls.Add(lblLoading);

                // Start background worker
                _contentLoader.RunWorkerAsync(new ContentLoadParameters
                {
                    SearchTerm = txtSearch.Text.Trim(),
                    Genre = cmbGenre.SelectedIndex > 0 ? cmbGenre.SelectedItem.ToString() : null,
                    Type = cmbType.SelectedIndex > 0 ? cmbType.SelectedItem.ToString() : null
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error starting content loading: {ex.Message}");
                lblLoading.Visible = false;
            }
        }

        private void ContentLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var parameters = e.Argument as ContentLoadParameters;
                if (parameters == null) return;

                // Get filtered content
                var contents = _contentRepository.Search(parameters.SearchTerm, parameters.Genre, parameters.Type);
                Debug.WriteLine($"Contenido encontrado: {contents.Count} elementos");

                // Check if operation was cancelled
                if (_contentLoader.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                e.Result = contents;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ContentLoader_DoWork: {ex.Message}");
                e.Result = ex;
            }
        }

        private void ContentLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Hide loading indicator
                lblLoading.Visible = false;
                flpContent.Controls.Clear();

                if (e.Cancelled)
                {
                    Debug.WriteLine("Content loading was cancelled");
                    return;
                }

                if (e.Error != null)
                {
                    Debug.WriteLine($"Error in content loading: {e.Error.Message}");
                    MessageBox.Show($"Error loading content: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (e.Result is Exception ex)
                {
                    Debug.WriteLine($"Exception in content loading: {ex.Message}");
                    MessageBox.Show($"Error loading content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var contents = e.Result as List<Content>;
                if (contents == null || contents.Count == 0)
                {
                    var noContentLabel = new Label
                    {
                        Text = "No content found matching your criteria",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 12),
                        Margin = new Padding(10),
                        ForeColor = System.Drawing.Color.Red
                    };
                    flpContent.Controls.Add(noContentLabel);
                    Debug.WriteLine("No se encontró contenido con los criterios actuales");
                    return;
                }

                // Create content cards in batches to improve performance
                const int batchSize = 10;
                for (int i = 0; i < contents.Count; i += batchSize)
                {
                    var batch = contents.Skip(i).Take(batchSize).ToList();
                    foreach (var content in batch)
                    {
                        var contentCard = CreateContentCard(content);
                        flpContent.Controls.Add(contentCard);
                    }

                    // Allow UI to update between batches
                    Application.DoEvents();
                }

                Debug.WriteLine("Content loading completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ContentLoader_RunWorkerCompleted: {ex.Message}");
                MessageBox.Show($"Error displaying content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartMyListLoading()
        {
            try
            {
                // Cancel any ongoing operation
                if (_myListLoader.IsBusy)
                {
                    _myListLoader.CancelAsync();
                    return;
                }

                // Show loading indicator
                flpMyList.Controls.Clear();
                lblNoContent.Text = "Loading your list...";
                lblNoContent.Visible = true;

                // Start background worker
                _myListLoader.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error starting my list loading: {ex.Message}");
                lblNoContent.Visible = false;
            }
        }

        private void MyListLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Get user's viewing history
                var viewingHistories = _viewingHistoryRepository.GetByUser(_currentUser.Id);
                Debug.WriteLine($"Historial de visualización: {viewingHistories.Count} elementos");

                // Check if operation was cancelled
                if (_myListLoader.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                e.Result = viewingHistories;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in MyListLoader_DoWork: {ex.Message}");
                e.Result = ex;
            }
        }

        private void MyListLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                flpMyList.Controls.Clear();

                if (e.Cancelled)
                {
                    Debug.WriteLine("My list loading was cancelled");
                    return;
                }

                if (e.Error != null)
                {
                    Debug.WriteLine($"Error in my list loading: {e.Error.Message}");
                    MessageBox.Show($"Error loading your list: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (e.Result is Exception ex)
                {
                    Debug.WriteLine($"Exception in my list loading: {ex.Message}");
                    MessageBox.Show($"Error loading your list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var viewingHistories = e.Result as List<ViewingHistory>;
                if (viewingHistories == null || viewingHistories.Count == 0)
                {
                    lblNoContent.Text = "No content in your list yet";
                    lblNoContent.Visible = true;
                    Debug.WriteLine("No content in user's list");
                    return;
                }

                // Create content cards in batches to improve performance
                const int batchSize = 5;
                for (int i = 0; i < viewingHistories.Count; i += batchSize)
                {
                    var batch = viewingHistories.Skip(i).Take(batchSize).ToList();
                    foreach (var history in batch)
                    {
                        var contentCard = CreateContentCard(history.Content);
                        flpMyList.Controls.Add(contentCard);
                    }

                    // Allow UI to update between batches
                    Application.DoEvents();
                }

                lblNoContent.Visible = false;
                Debug.WriteLine("My list loading completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in MyListLoader_RunWorkerCompleted: {ex.Message}");
                MessageBox.Show($"Error displaying your list: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartRecentlyWatchedLoading()
        {
            try
            {
                // Cancel any ongoing operation
                if (_recentlyWatchedLoader.IsBusy)
                {
                    _recentlyWatchedLoader.CancelAsync();
                    return;
                }

                // Show loading indicator
                flpRecentlyWatched.Controls.Clear();
                var loadingLabel = new Label
                {
                    Text = "Loading recently watched...",
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10),
                    Margin = new Padding(10)
                };
                flpRecentlyWatched.Controls.Add(loadingLabel);

                // Start background worker
                _recentlyWatchedLoader.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error starting recently watched loading: {ex.Message}");
            }
        }

        private void RecentlyWatchedLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Get recently watched content
                var recentlyWatched = _viewingHistoryRepository.GetRecentlyWatchedContent(_currentUser.Id, 5);
                Debug.WriteLine($"Contenido visto recientemente: {recentlyWatched.Count} elementos");

                // Check if operation was cancelled
                if (_recentlyWatchedLoader.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                e.Result = recentlyWatched;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RecentlyWatchedLoader_DoWork: {ex.Message}");
                e.Result = ex;
            }
        }

        private void RecentlyWatchedLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                flpRecentlyWatched.Controls.Clear();

                if (e.Cancelled)
                {
                    Debug.WriteLine("Recently watched loading was cancelled");
                    return;
                }

                if (e.Error != null)
                {
                    Debug.WriteLine($"Error in recently watched loading: {e.Error.Message}");
                    MessageBox.Show($"Error loading recently watched: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (e.Result is Exception ex)
                {
                    Debug.WriteLine($"Exception in recently watched loading: {ex.Message}");
                    MessageBox.Show($"Error loading recently watched: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var recentlyWatched = e.Result as List<Content>;
                if (recentlyWatched == null || recentlyWatched.Count == 0)
                {
                    var noContentLabel = new Label
                    {
                        Text = "No recently watched content",
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10),
                        Margin = new Padding(10)
                    };
                    flpRecentlyWatched.Controls.Add(noContentLabel);
                    Debug.WriteLine("No recently watched content");
                    return;
                }

                // Create content cards
                foreach (var content in recentlyWatched)
                {
                    var contentCard = CreateContentCard(content);
                    flpRecentlyWatched.Controls.Add(contentCard);

                    // Allow UI to update
                    Application.DoEvents();
                }

                Debug.WriteLine("Recently watched loading completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RecentlyWatchedLoader_RunWorkerCompleted: {ex.Message}");
                MessageBox.Show($"Error displaying recently watched: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartRatingsLoading()
        {
            try
            {
                // Cancel any ongoing operation
                if (_ratingsLoader.IsBusy)
                {
                    _ratingsLoader.CancelAsync();
                    return;
                }

                // Set up ratings grid if not already done
                if (dgvMyRatings.Columns.Count == 0)
                {
                    SetupRatingsGrid();
                }

                // Show loading indicator
                dgvMyRatings.DataSource = null;

                // Start background worker
                _ratingsLoader.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error starting ratings loading: {ex.Message}");
            }
        }

        private void RatingsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // Get user's ratings
                var ratings = _ratingRepository.GetByUser(_currentUser.Id);
                Debug.WriteLine($"Calificaciones del usuario: {ratings.Count} elementos");

                // Check if operation was cancelled
                if (_ratingsLoader.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                // Create a list of rating view models
                var ratingViewModels = ratings.Select(r => new
                {
                    ContentTitle = r.Content.Title,
                    r.Score,
                    r.Comment,
                    RatingDate = r.RatingDate.ToShortDateString()
                }).ToList();

                e.Result = ratingViewModels;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RatingsLoader_DoWork: {ex.Message}");
                e.Result = ex;
            }
        }

        private void RatingsLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    Debug.WriteLine("Ratings loading was cancelled");
                    return;
                }

                if (e.Error != null)
                {
                    Debug.WriteLine($"Error in ratings loading: {e.Error.Message}");
                    MessageBox.Show($"Error loading ratings: {e.Error.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (e.Result is Exception ex)
                {
                    Debug.WriteLine($"Exception in ratings loading: {ex.Message}");
                    MessageBox.Show($"Error loading ratings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                dgvMyRatings.DataSource = e.Result;
                Debug.WriteLine("Ratings loading completed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in RatingsLoader_RunWorkerCompleted: {ex.Message}");
                MessageBox.Show($"Error displaying ratings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Browse Tab Methods

        private void LoadFilterOptions()
        {
            try
            {
                Debug.WriteLine("LoadFilterOptions iniciando...");

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

                Debug.WriteLine("LoadFilterOptions completado");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en LoadFilterOptions: {ex.Message}");
                MessageBox.Show($"Error loading filter options: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Panel CreateContentCard(Content content)
        {
            try
            {
                // Create a panel for the content card with a more attractive design
                var panel = new Panel
                {
                    Width = 160,
                    Height = 240,
                    Margin = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle,
                    BackColor = System.Drawing.Color.WhiteSmoke,
                    Cursor = Cursors.Hand // Change cursor to indicate clickable
                };

                // Create a picture box for the content image
                var pictureBox = new PictureBox
                {
                    Width = 150,
                    Height = 150,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Location = new Point(5, 5),
                    BorderStyle = BorderStyle.FixedSingle
                };

                // Load image if available - use cached image if possible
                if (!string.IsNullOrEmpty(content.ImagePath))
                {
                    try
                    {
                        if (_imageCache.ContainsKey(content.ImagePath))
                        {
                            pictureBox.Image = _imageCache[content.ImagePath];
                        }
                        else
                        {
                            // Use placeholder image while loading
                            pictureBox.BackColor = Color.LightGray;

                            // Load image asynchronously
                            var imageUrl = content.ImagePath;
                            Task.Run(() => {
                                try
                                {
                                    using (var httpClient = new System.Net.Http.HttpClient())
                                    {
                                        httpClient.Timeout = TimeSpan.FromSeconds(5); // Set timeout to avoid hanging
                                        var imageData = httpClient.GetByteArrayAsync(imageUrl).Result;
                                        using (var ms = new System.IO.MemoryStream(imageData))
                                        {
                                            var image = Image.FromStream(ms);

                                            // Cache the image
                                            if (!_imageCache.ContainsKey(imageUrl))
                                            {
                                                _imageCache[imageUrl] = image;
                                            }

                                            // Update UI on the main thread
                                            this.Invoke((MethodInvoker)delegate {
                                                if (!pictureBox.IsDisposed)
                                                {
                                                    pictureBox.Image = image;
                                                }
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine($"Error loading image for {content.Title}: {ex.Message}");
                                }
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error loading image for {content.Title}: {ex.Message}");
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

                // Add rating information
                double averageRating = _ratingRepository.GetAverageRatingForContent(content.Id);
                var ratingLabel = new Label
                {
                    Text = $"★ {averageRating:F1}/5.0",
                    AutoSize = false,
                    Width = 150,
                    Height = 20,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 8, FontStyle.Italic),
                    Location = new Point(5, 220),
                    ForeColor = System.Drawing.Color.DarkBlue
                };

                // Add controls to the panel
                panel.Controls.Add(pictureBox);
                panel.Controls.Add(titleLabel);
                panel.Controls.Add(infoLabel);
                panel.Controls.Add(ratingLabel);

                // Add click event to open content details
                panel.Click += (sender, e) => OpenContentDetails(content);
                pictureBox.Click += (sender, e) => OpenContentDetails(content);
                titleLabel.Click += (sender, e) => OpenContentDetails(content);
                infoLabel.Click += (sender, e) => OpenContentDetails(content);
                ratingLabel.Click += (sender, e) => OpenContentDetails(content);

                // Add tooltip to show description on hover
                var toolTip = new ToolTip();
                toolTip.SetToolTip(panel, content.Description);
                toolTip.SetToolTip(pictureBox, content.Description);
                toolTip.SetToolTip(titleLabel, content.Description);

                return panel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en CreateContentCard: {ex.Message}");

                // Return a simple error panel as fallback
                var errorPanel = new Panel
                {
                    Width = 160,
                    Height = 240,
                    Margin = new Padding(10),
                    BorderStyle = BorderStyle.FixedSingle
                };

                var errorLabel = new Label
                {
                    Text = content.Title + "\n(Error loading)",
                    AutoSize = false,
                    Width = 150,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(5, 100)
                };

                errorPanel.Controls.Add(errorLabel);
                return errorPanel;
            }
        }

        private void OpenContentDetails(Content content)
        {
            try
            {
                Debug.WriteLine($"Abriendo detalles para contenido: {content.Title} (ID: {content.Id})");

                // Make sure we have the latest content data
                var refreshedContent = _contentRepository.GetById(content.Id);
                if (refreshedContent == null)
                {
                    MessageBox.Show("Content details could not be loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (var contentDetailsForm = new ContentDetailsForm(refreshedContent, _currentUser))
                {
                    contentDetailsForm.ShowDialog();

                    // Refresh data after viewing details - only refresh the current tab
                    switch (tabControl.SelectedIndex)
                    {
                        case 0: // Browse tab
                            StartContentLoading();
                            break;
                        case 1: // My List tab
                            StartMyListLoading();
                            break;
                        case 2: // Profile tab
                            StartRecentlyWatchedLoading();
                            StartRatingsLoading();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en OpenContentDetails: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Error opening content details: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            StartContentLoading();
        }

        private void FilterContent(object sender, EventArgs e)
        {
            StartContentLoading();
        }

        private void BtnClearFilters_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            cmbGenre.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            StartContentLoading();
        }

        #endregion

        #region Profile Tab Methods

        private void SetupRatingsGrid()
        {
            try
            {
                Debug.WriteLine("SetupRatingsGrid iniciando...");

                dgvMyRatings.AutoGenerateColumns = false;
                dgvMyRatings.Columns.Clear();

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

                Debug.WriteLine("SetupRatingsGrid completado");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en SetupRatingsGrid: {ex.Message}");
                MessageBox.Show($"Error setting up ratings grid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditProfile_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en BtnEditProfile_Click: {ex.Message}");
                MessageBox.Show($"Error editing profile: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnChangePassword_Click(object sender, EventArgs e)
        {
            try
            {
                using (var changePasswordForm = new ChangePasswordForm(_currentUser))
                {
                    changePasswordForm.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en BtnChangePassword_Click: {ex.Message}");
                MessageBox.Show($"Error changing password: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Helper Classes

        private class ContentLoadParameters
        {
            public string SearchTerm { get; set; }
            public string Genre { get; set; }
            public string Type { get; set; }
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose cached images
                foreach (var image in _imageCache.Values)
                {
                    image.Dispose();
                }
                _imageCache.Clear();

                // Cancel any running background workers
                if (_contentLoader.IsBusy) _contentLoader.CancelAsync();
                if (_myListLoader.IsBusy) _myListLoader.CancelAsync();
                if (_recentlyWatchedLoader.IsBusy) _recentlyWatchedLoader.CancelAsync();
                if (_ratingsLoader.IsBusy) _ratingsLoader.CancelAsync();
            }
            base.Dispose(disposing);
        }
    }
}

