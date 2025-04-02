using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace NetWatchApp.Forms
{
    public partial class RatingForm : Form
    {
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
            lblContentTitle.Text = _content.Title;

            // Check if user already rated this content
            _existingRating = _ratingRepository.GetByUserAndContent(_currentUser.Id, _content.Id);
            if (_existingRating != null)
            {
                // Load existing rating
                for (int i = 0; i < ratingPanel.Controls.Count; i++)
                {
                    if (ratingPanel.Controls[i] is RadioButton radioButton)
                    {
                        int score = int.Parse(radioButton.Tag.ToString());
                        if (score == _existingRating.Score)
                        {
                            radioButton.Checked = true;
                            break;
                        }
                    }
                }

                txtComment.Text = _existingRating.Comment;
                btnSubmit.Text = "Update Rating";
            }

            // Set up event handlers
            btnSubmit.Click += BtnSubmit_Click;
            btnCancel.Click += BtnCancel_Click;
        }

        private void InitializeComponent()
        {
            this.lblContentTitle = new System.Windows.Forms.Label();
            this.lblRating = new System.Windows.Forms.Label();
            this.ratingPanel = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblContentTitle
            this.lblContentTitle.AutoSize = true;
            this.lblContentTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblContentTitle.Location = new System.Drawing.Point(20, 20);
            this.lblContentTitle.Name = "lblContentTitle";
            this.lblContentTitle.Size = new System.Drawing.Size(200, 28);
            this.lblContentTitle.Text = "Content Title";

            // lblRating
            this.lblRating.AutoSize = true;
            this.lblRating.Location = new System.Drawing.Point(20, 60);
            this.lblRating.Name = "lblRating";
            this.lblRating.Size = new System.Drawing.Size(52, 20);
            this.lblRating.Text = "Rating:";

            // ratingPanel
            this.ratingPanel.Location = new System.Drawing.Point(20, 90);
            this.ratingPanel.Name = "ratingPanel";
            this.ratingPanel.Size = new System.Drawing.Size(350, 40);
            this.ratingPanel.Controls.Add(this.radioButton1);
            this.ratingPanel.Controls.Add(this.radioButton2);
            this.ratingPanel.Controls.Add(this.radioButton3);
            this.ratingPanel.Controls.Add(this.radioButton4);
            this.ratingPanel.Controls.Add(this.radioButton5);

            // radioButton1
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(10, 10);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(37, 24);
            this.radioButton1.Text = "1";
            this.radioButton1.Tag = "1";

            // radioButton2
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(80, 10);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(37, 24);
            this.radioButton2.Text = "2";
            this.radioButton2.Tag = "2";

            // radioButton3
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(150, 10);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(37, 24);
            this.radioButton3.Text = "3";
            this.radioButton3.Tag = "3";

            // radioButton4
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(220, 10);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(37, 24);
            this.radioButton4.Text = "4";
            this.radioButton4.Tag = "4";

            // radioButton5
            this.radioButton5.AutoSize = true;
            this.radioButton5.Checked = true;
            this.radioButton5.Location = new System.Drawing.Point(290, 10);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(37, 24);
            this.radioButton5.Text = "5";
            this.radioButton5.Tag = "5";

            // lblComment
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(20, 140);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(74, 20);
            this.lblComment.Text = "Comment:";

            // txtComment
            this.txtComment.Location = new System.Drawing.Point(20, 170);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.Size = new System.Drawing.Size(350, 100);
            this.txtComment.TabIndex = 0;

            // btnSubmit
            this.btnSubmit.Location = new System.Drawing.Point(170, 290);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 35);
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(280, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // RatingForm
            this.ClientSize = new System.Drawing.Size(400, 350);
            this.Controls.Add(this.lblContentTitle);
            this.Controls.Add(this.lblRating);
            this.Controls.Add(this.ratingPanel);
            this.Controls.Add(this.lblComment);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RatingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rate Content";
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get selected rating
                int selectedRating = 5; // Default to 5
                foreach (Control control in ratingPanel.Controls)
                {
                    if (control is RadioButton radioButton && radioButton.Checked)
                    {
                        selectedRating = int.Parse(radioButton.Tag.ToString());
                        break;
                    }
                }

                if (_existingRating != null)
                {
                    // Update existing rating
                    _existingRating.Score = selectedRating;
                    _existingRating.Comment = txtComment.Text.Trim();
                    _existingRating.RatingDate = DateTime.Now;

                    _ratingRepository.Update(_existingRating);
                    MessageBox.Show("Rating updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Create new rating
                    var rating = new Rating
                    {
                        UserId = _currentUser.Id,
                        ContentId = _content.Id,
                        Score = selectedRating,
                        Comment = txtComment.Text.Trim(),
                        RatingDate = DateTime.Now
                    };

                    _ratingRepository.Add(rating);
                    MessageBox.Show("Rating submitted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting rating: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

