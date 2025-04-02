using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class AddContentForm : Form
    {
        private readonly ContentRepository _contentRepository;
        private List<Episode> _episodes = new List<Episode>();
        private string _selectedImagePath = null;

        public AddContentForm()
        {
            InitializeComponent();
            _contentRepository = new ContentRepository(new Data.EntityFramework.NetWatchDbContext());

            // Populate genre combobox
            cmbGenre.Items.AddRange(new object[] {
                "Action", "Adventure", "Comedy", "Drama", "Fantasy",
                "Horror", "Mystery", "Romance", "Sci-Fi", "Thriller", "Documentary"
            });

            // Populate content type combobox
            cmbType.Items.AddRange(new object[] { "Movie", "Series" });
            cmbType.SelectedIndex = 0; // Default to Movie

            // Populate platform combobox
            cmbPlatform.Items.AddRange(new object[] {
                "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu", "Apple TV+", "Other"
            });

            // Set up episode panel visibility
            pnlEpisodes.Visible = false;

            // Set up event handlers
            cmbType.SelectedIndexChanged += CmbType_SelectedIndexChanged;
            btnAddEpisode.Click += BtnAddEpisode_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnSelectImage.Click += BtnSelectImage_Click;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblReleaseYear = new System.Windows.Forms.Label();
            this.numReleaseYear = new System.Windows.Forms.NumericUpDown();
            this.lblGenre = new System.Windows.Forms.Label();
            this.cmbGenre = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.cmbPlatform = new System.Windows.Forms.ComboBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.numDuration = new System.Windows.Forms.NumericUpDown();
            this.pnlEpisodes = new System.Windows.Forms.Panel();
            this.lblEpisodes = new System.Windows.Forms.Label();
            this.dgvEpisodes = new System.Windows.Forms.DataGridView();
            this.colEpisodeNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEpisodeDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAddEpisode = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblImage = new System.Windows.Forms.Label();
            this.picContentImage = new System.Windows.Forms.PictureBox();
            this.btnSelectImage = new System.Windows.Forms.Button();
            this.lblImagePath = new System.Windows.Forms.Label();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(38, 20);
            this.lblTitle.Text = "Title:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(150, 20);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(300, 27);

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(20, 60);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(85, 20);
            this.lblDescription.Text = "Description:";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(150, 60);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(300, 100);

            // lblReleaseYear
            this.lblReleaseYear.AutoSize = true;
            this.lblReleaseYear.Location = new System.Drawing.Point(20, 170);
            this.lblReleaseYear.Name = "lblReleaseYear";
            this.lblReleaseYear.Size = new System.Drawing.Size(93, 20);
            this.lblReleaseYear.Text = "Release Year:";

            // numReleaseYear
            this.numReleaseYear.Location = new System.Drawing.Point(150, 170);
            this.numReleaseYear.Maximum = 2100;
            this.numReleaseYear.Minimum = 1900;
            this.numReleaseYear.Name = "numReleaseYear";
            this.numReleaseYear.Size = new System.Drawing.Size(150, 27);
            this.numReleaseYear.Value = DateTime.Now.Year;

            // lblGenre
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(20, 210);
            this.lblGenre.Name = "lblGenre";
            this.lblGenre.Size = new System.Drawing.Size(48, 20);
            this.lblGenre.Text = "Genre:";

            // cmbGenre
            this.cmbGenre.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGenre.FormattingEnabled = true;
            this.cmbGenre.Location = new System.Drawing.Point(150, 210);
            this.cmbGenre.Name = "cmbGenre";
            this.cmbGenre.Size = new System.Drawing.Size(200, 28);

            // lblType
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(20, 250);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(43, 20);
            this.lblType.Text = "Type:";

            // cmbType
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(150, 250);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(200, 28);

            // lblPlatform
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Location = new System.Drawing.Point(20, 290);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(68, 20);
            this.lblPlatform.Text = "Platform:";

            // cmbPlatform
            this.cmbPlatform.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlatform.FormattingEnabled = true;
            this.cmbPlatform.Location = new System.Drawing.Point(150, 290);
            this.cmbPlatform.Name = "cmbPlatform";
            this.cmbPlatform.Size = new System.Drawing.Size(200, 28);

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(20, 330);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(124, 20);
            this.lblDuration.Text = "Duration (minutes):";

            // numDuration
            this.numDuration.Location = new System.Drawing.Point(150, 330);
            this.numDuration.Maximum = 1000;
            this.numDuration.Minimum = 1;
            this.numDuration.Name = "numDuration";
            this.numDuration.Size = new System.Drawing.Size(150, 27);
            this.numDuration.Value = 90;

            // lblImage
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(20, 370);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(51, 20);
            this.lblImage.Text = "Image:";

            // picContentImage
            this.picContentImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picContentImage.Location = new System.Drawing.Point(150, 370);
            this.picContentImage.Name = "picContentImage";
            this.picContentImage.Size = new System.Drawing.Size(150, 200);
            this.picContentImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picContentImage.TabIndex = 0;
            this.picContentImage.TabStop = false;

            // btnSelectImage
            this.btnSelectImage.Location = new System.Drawing.Point(310, 370);
            this.btnSelectImage.Name = "btnSelectImage";
            this.btnSelectImage.Size = new System.Drawing.Size(140, 30);
            this.btnSelectImage.Text = "Select Image";
            this.btnSelectImage.UseVisualStyleBackColor = true;

            // lblImagePath
            this.lblImagePath.AutoSize = true;
            this.lblImagePath.Location = new System.Drawing.Point(150, 580);
            this.lblImagePath.Name = "lblImagePath";
            this.lblImagePath.Size = new System.Drawing.Size(87, 20);
            this.lblImagePath.Text = "No image selected";
            this.lblImagePath.ForeColor = System.Drawing.Color.Gray;

            // pnlEpisodes
            this.pnlEpisodes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlEpisodes.Controls.Add(this.lblEpisodes);
            this.pnlEpisodes.Controls.Add(this.dgvEpisodes);
            this.pnlEpisodes.Controls.Add(this.btnAddEpisode);
            this.pnlEpisodes.Location = new System.Drawing.Point(20, 610);
            this.pnlEpisodes.Name = "pnlEpisodes";
            this.pnlEpisodes.Size = new System.Drawing.Size(430, 200);
            this.pnlEpisodes.Visible = false;

            // lblEpisodes
            this.lblEpisodes.AutoSize = true;
            this.lblEpisodes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblEpisodes.Location = new System.Drawing.Point(10, 10);
            this.lblEpisodes.Name = "lblEpisodes";
            this.lblEpisodes.Size = new System.Drawing.Size(70, 20);
            this.lblEpisodes.Text = "Episodes:";

            // dgvEpisodes
            this.dgvEpisodes.AllowUserToAddRows = false;
            this.dgvEpisodes.AllowUserToDeleteRows = false;
            this.dgvEpisodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEpisodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colEpisodeNumber,
                this.colEpisodeTitle,
                this.colEpisodeDuration
            });
            this.dgvEpisodes.Location = new System.Drawing.Point(10, 40);
            this.dgvEpisodes.Name = "dgvEpisodes";
            this.dgvEpisodes.ReadOnly = true;
            this.dgvEpisodes.RowHeadersWidth = 51;
            this.dgvEpisodes.Size = new System.Drawing.Size(410, 110);
            this.dgvEpisodes.TabIndex = 0;

            // colEpisodeNumber
            this.colEpisodeNumber.HeaderText = "Number";
            this.colEpisodeNumber.MinimumWidth = 6;
            this.colEpisodeNumber.Name = "colEpisodeNumber";
            this.colEpisodeNumber.ReadOnly = true;
            this.colEpisodeNumber.Width = 70;

            // colEpisodeTitle
            this.colEpisodeTitle.HeaderText = "Title";
            this.colEpisodeTitle.MinimumWidth = 6;
            this.colEpisodeTitle.Name = "colEpisodeTitle";
            this.colEpisodeTitle.ReadOnly = true;
            this.colEpisodeTitle.Width = 200;

            // colEpisodeDuration
            this.colEpisodeDuration.HeaderText = "Duration";
            this.colEpisodeDuration.MinimumWidth = 6;
            this.colEpisodeDuration.Name = "colEpisodeDuration";
            this.colEpisodeDuration.ReadOnly = true;
            this.colEpisodeDuration.Width = 80;

            // btnAddEpisode
            this.btnAddEpisode.Location = new System.Drawing.Point(310, 160);
            this.btnAddEpisode.Name = "btnAddEpisode";
            this.btnAddEpisode.Size = new System.Drawing.Size(110, 30);
            this.btnAddEpisode.Text = "Add Episode";
            this.btnAddEpisode.UseVisualStyleBackColor = true;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(150, 830);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(270, 830);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // AddContentForm
            this.ClientSize = new System.Drawing.Size(480, 880);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblReleaseYear);
            this.Controls.Add(this.numReleaseYear);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.cmbGenre);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbType);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.cmbPlatform);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.numDuration);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.picContentImage);
            this.Controls.Add(this.btnSelectImage);
            this.Controls.Add(this.lblImagePath);
            this.Controls.Add(this.pnlEpisodes);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddContentForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add New Content";
        }

        private void CmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isSeries = cmbType.SelectedItem.ToString() == "Series";
            pnlEpisodes.Visible = isSeries;
            lblDuration.Visible = !isSeries;
            numDuration.Visible = !isSeries;

            // Adjust form height based on content type
            this.ClientSize = new Size(480, isSeries ? 880 : 680);
            btnSave.Location = new Point(150, isSeries ? 830 : 630);
            btnCancel.Location = new Point(270, isSeries ? 830 : 630);
        }

        private void BtnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select Image";
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                openFileDialog.Multiselect = false;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _selectedImagePath = openFileDialog.FileName;
                        picContentImage.Image = Image.FromFile(_selectedImagePath);
                        lblImagePath.Text = Path.GetFileName(_selectedImagePath);
                        lblImagePath.ForeColor = Color.Black;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        _selectedImagePath = null;
                        picContentImage.Image = null;
                        lblImagePath.Text = "No image selected";
                        lblImagePath.ForeColor = Color.Gray;
                    }
                }
            }
        }

        private void BtnAddEpisode_Click(object sender, EventArgs e)
        {
            using (var episodeForm = new AddEpisodeForm(_episodes.Count + 1))
            {
                if (episodeForm.ShowDialog() == DialogResult.OK)
                {
                    _episodes.Add(episodeForm.Episode);
                    RefreshEpisodesGrid();
                }
            }
        }

        private void RefreshEpisodesGrid()
        {
            dgvEpisodes.Rows.Clear();
            foreach (var episode in _episodes)
            {
                dgvEpisodes.Rows.Add(episode.EpisodeNumber, episode.Title, episode.Duration);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                try
                {
                    string imagePath = null;

                    // Save image if one was selected
                    if (!string.IsNullOrEmpty(_selectedImagePath))
                    {
                        // Create images directory if it doesn't exist
                        string imagesDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
                        if (!Directory.Exists(imagesDirectory))
                        {
                            Directory.CreateDirectory(imagesDirectory);
                        }

                        // Generate unique filename
                        string fileName = $"{Guid.NewGuid()}{Path.GetExtension(_selectedImagePath)}";
                        string destinationPath = Path.Combine(imagesDirectory, fileName);

                        // Copy the image file
                        File.Copy(_selectedImagePath, destinationPath);

                        // Store relative path
                        imagePath = Path.Combine("Images", fileName);
                    }

                    var content = new Content
                    {
                        Title = txtTitle.Text.Trim(),
                        Description = txtDescription.Text.Trim(),
                        ReleaseYear = (int)numReleaseYear.Value,
                        Genre = cmbGenre.SelectedItem.ToString(),
                        Type = cmbType.SelectedItem.ToString(),
                        Platform = cmbPlatform.SelectedItem.ToString(),
                        Duration = cmbType.SelectedItem.ToString() == "Movie" ? (int)numDuration.Value : 0,
                        Episodes = cmbType.SelectedItem.ToString() == "Series" ? _episodes : new List<Episode>(),
                        ImagePath = imagePath
                    };

                    _contentRepository.Add(content);
                    MessageBox.Show("Content added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show("Please enter a description.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbGenre.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a genre.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbPlatform.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a platform.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (cmbType.SelectedItem.ToString() == "Series" && _episodes.Count == 0)
            {
                MessageBox.Show("Please add at least one episode for the series.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

