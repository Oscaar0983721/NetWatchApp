using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class RatingForm : Form
    {
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.TrackBar trkRating;
        private System.Windows.Forms.Label lblRatingValue;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        private readonly Content _content;
        private readonly User _currentUser;
        private readonly RatingRepository _ratingRepository;
        private Rating _existingRating;

        public RatingForm(Content content, User currentUser)
        {
            InitializeComponent();
            _content = content;
            _currentUser = currentUser;
            _ratingRepository = new RatingRepository(new Data.EntityFramework.NetWatchDbContext());

            // Set content title
            lblTitle.Text = $"Rate: {_content.Title}";

            // Load existing rating if any
            LoadExistingRating();

            // Set up event handlers
            trkRating.ValueChanged += TrkRating_ValueChanged;
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();
            this.trkRating = new System.Windows.Forms.TrackBar();
            this.lblRatingValue = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblTitle
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(200, 28);
            this.lblTitle.Text = "Rate: Content Title";

            // lblRating
            this.lblRating.AutoSize = true;
            this.lblRating.Location = new System.Drawing.Point(20, 60);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(52, 20);
            this.lblRating.Text = "Rating:";

            // trkRating
            this.trkRating.Location = new System.Drawing.Point(80, 60);
            this.trkRating.Maximum = 5;
            this.trkRating.Minimum = 1;
            this.trkRating.Name = "trkRating";
            this.trkRating.Size = new System.Drawing.Size(200, 45);
            this.trkRating.TabIndex = 0;
            this.trkRating.Value = 3;

            // lblRatingValue
            this.lblRatingValue.AutoSize = true;
            this.lblRatingValue.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblRatingValue.Location = new System.Drawing.Point(290, 60);
            this.lblRatingValue.Name = "lblRatingValue";
            this.lblRatingValue.Size = new System.Drawing.Size(24, 28);
            this.lblRatingValue.Text = "3";

            // lblComment
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(20, 110);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(74, 20);
            this.lblComment.Text = "Comment:";

            // txtComment
            this.txtComment.Location = new System.Drawing.Point(20, 140);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(360, 100);
            this.txtComment.TabIndex = 1;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(100, 260);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(220, 260);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // RatingForm
            this.ClientSize = new System.Drawing.Size(400, 310);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.trkRating);
            this.Controls.Add(this.lblRatingValue);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RatingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rate Content";
        }

        private void LoadExistingRating()
        {
            _existingRating = _ratingRepository.GetByUserAndContent(_currentUser.Id, _content.Id);

            if (_existingRating != null)
            {
                trkRating.Value = _existingRating.Score;
                txtComment.Text = _existingRating.Comment;
                lblRatingValue.Text = _existingRating.Score.ToString();
            }
        }

        private void TrkRating_ValueChanged(object sender, EventArgs e)
        {
            lblRatingValue.Text = trkRating.Value.ToString();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (_existingRating == null)
                {
                    // Create new rating
                    var rating = new Rating
                    {
                        UserId = _currentUser.Id,
                        ContentId = _content.Id,
                        Score = trkRating.Value,
                        Comment = txtComment.Text.Trim(),
                        RatingDate = DateTime.Now
                    };

                    _ratingRepository.Add(rating);
                }
                else
                {
                    // Update existing rating
                    _existingRating.Score = trkRating.Value;
                    _existingRating.Comment = txtComment.Text.Trim();
                    _existingRating.RatingDate = DateTime.Now;

                    _ratingRepository.Update(_existingRating);
                }

                MessageBox.Show("Rating saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving rating: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

