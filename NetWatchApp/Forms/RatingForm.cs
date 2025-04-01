using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class RatingForm : Form
    {
        private readonly Content _content;
        private readonly User _currentUser;
        private readonly RatingRepository _ratingRepository;
        private int _selectedRating = 0;

        public RatingForm(Content content, User currentUser)
        {
            InitializeComponent();
            _content = content;
            _currentUser = currentUser;
            _ratingRepository = new RatingRepository(new NetWatchDbContext());

            lblTitle.Text = $"Rate: {_content.Title}";
            LoadExistingRating();
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();
            this.pnlStars = new System.Windows.Forms.Panel();
            this.star1 = new System.Windows.Forms.PictureBox();
            this.star2 = new System.Windows.Forms.PictureBox();
            this.star3 = new System.Windows.Forms.PictureBox();
            this.star4 = new System.Windows.Forms.PictureBox();
            this.star5 = new System.Windows.Forms.PictureBox();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlStars.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.star1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.star2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.star3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.star4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.star5)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Rate: Title";

            // lblRating
            this.lblRating.AutoSize = true;
            this.lblRating.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRating.Location = new System.Drawing.Point(20, 70);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(100, 19);
            this.lblRating.TabIndex = 1;
            this.lblRating.Text = "Your Rating:";

            // pnlStars
            this.pnlStars.Location = new System.Drawing.Point(20, 100);
            this.pnlStars.Name = "pnlStars";
            this.pnlStars.Size = new System.Drawing.Size(250, 50);
            this.pnlStars.TabIndex = 2;
            this.pnlStars.Controls.Add(this.star1);
            this.pnlStars.Controls.Add(this.star2);
            this.pnlStars.Controls.Add(this.star3);
            this.pnlStars.Controls.Add(this.star4);
            this.pnlStars.Controls.Add(this.star5);

            // star1
            this.star1.Location = new System.Drawing.Point(0, 0);
            this.star1.Name = "star1";
            this.star1.Size = new System.Drawing.Size(40, 40);
            this.star1.TabIndex = 0;
            this.star1.TabStop = false;
            this.star1.BackColor = System.Drawing.Color.Gray;
            this.star1.Click += new System.EventHandler(this.star1_Click);

            // star2
            this.star2.Location = new System.Drawing.Point(50, 0);
            this.star2.Name = "star2";
            this.star2.Size = new System.Drawing.Size(40, 40);
            this.star2.TabIndex = 1;
            this.star2.TabStop = false;
            this.star2.BackColor = System.Drawing.Color.Gray;
            this.star2.Click += new System.EventHandler(this.star2_Click);

            // star3
            this.star3.Location = new System.Drawing.Point(100, 0);
            this.star3.Name = "star3";
            this.star3.Size = new System.Drawing.Size(40, 40);
            this.star3.TabIndex = 2;
            this.star3.TabStop = false;
            this.star3.BackColor = System.Drawing.Color.Gray;
            this.star3.Click += new System.EventHandler(this.star3_Click);

            // star4
            this.star4.Location = new System.Drawing.Point(150, 0);
            this.star4.Name = "star4";
            this.star4.Size = new System.Drawing.Size(40, 40);
            this.star4.TabIndex = 3;
            this.star4.TabStop = false;
            this.star4.BackColor = System.Drawing.Color.Gray;
            this.star4.Click += new System.EventHandler(this.star4_Click);

            // star5
            this.star5.Location = new System.Drawing.Point(200, 0);
            this.star5.Name = "star5";
            this.star5.Size = new System.Drawing.Size(40, 40);
            this.star5.TabIndex = 4;
            this.star5.TabStop = false;
            this.star5.BackColor = System.Drawing.Color.Gray;
            this.star5.Click += new System.EventHandler(this.star5_Click);

            // lblComment
            this.lblComment.AutoSize = true;
            this.lblComment.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblComment.Location = new System.Drawing.Point(20, 170);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(100, 19);
            this.lblComment.TabIndex = 3;
            this.lblComment.Text = "Comment:";

            // txtComment
            this.txtComment.Location = new System.Drawing.Point(20, 200);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(360, 100);
            this.txtComment.TabIndex = 4;

            // btnSubmit
            this.btnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmit.ForeColor = System.Drawing.Color.White;
            this.btnSubmit.Location = new System.Drawing.Point(20, 320);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(150, 40);
            this.btnSubmit.TabIndex = 5;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);

            // btnCancel
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(230, 320);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 40);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // RatingForm
            this.ClientSize = new System.Drawing.Size(400, 380);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.pnlStars);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "RatingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rate Content";
            this.pnlStars.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.star1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.star2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.star3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.star4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.star5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblRating;
        private System.Windows.Forms.Panel pnlStars;
        private System.Windows.Forms.PictureBox star1;
        private System.Windows.Forms.PictureBox star2;
        private System.Windows.Forms.PictureBox star3;
        private System.Windows.Forms.PictureBox star4;
        private System.Windows.Forms.PictureBox star5;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;

        private async void LoadExistingRating()
        {
            var ratings = await _ratingRepository.GetByUserIdAsync(_currentUser.Id);
            var existingRating = ratings.FirstOrDefault(r => r.ContentId == _content.Id);

            if (existingRating != null)
            {
                _selectedRating = existingRating.Score;
                txtComment.Text = existingRating.Comment;
                UpdateStars();
            }
        }

        private void UpdateStars()
        {
            star1.BackColor = _selectedRating >= 1 ? System.Drawing.Color.Gold : System.Drawing.Color.Gray;
            star2.BackColor = _selectedRating >= 2 ? System.Drawing.Color.Gold : System.Drawing.Color.Gray;
            star3.BackColor = _selectedRating >= 3 ? System.Drawing.Color.Gold : System.Drawing.Color.Gray;
            star4.BackColor = _selectedRating >= 4 ? System.Drawing.Color.Gold : System.Drawing.Color.Gray;
            star5.BackColor = _selectedRating >= 5 ? System.Drawing.Color.Gold : System.Drawing.Color.Gray;
        }

        private void star1_Click(object sender, EventArgs e)
        {
            _selectedRating = 1;
            UpdateStars();
        }

        private void star2_Click(object sender, EventArgs e)
        {
            _selectedRating = 2;
            UpdateStars();
        }

        private void star3_Click(object sender, EventArgs e)
        {
            _selectedRating = 3;
            UpdateStars();
        }

        private void star4_Click(object sender, EventArgs e)
        {
            _selectedRating = 4;
            UpdateStars();
        }

        private void star5_Click(object sender, EventArgs e)
        {
            _selectedRating = 5;
            UpdateStars();
        }

        private async void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_selectedRating == 0)
            {
                MessageBox.Show("Please select a rating before submitting.", "Rating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var rating = new Rating
            {
                UserId = _currentUser.Id,
                ContentId = _content.Id,
                Score = _selectedRating,
                Comment = txtComment.Text,
                RatingDate = DateTime.Now
            };

            bool success = await _ratingRepository.AddAsync(rating);

            if (success)
            {
                MessageBox.Show("Thank you for your rating!", "Rating Submitted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("An error occurred while submitting your rating. Please try again.", "Rating Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}