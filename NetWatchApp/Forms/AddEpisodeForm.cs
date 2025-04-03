using NetWatchApp.Classes.Models;
using System;
using System.Windows.Forms;

namespace NetWatchApp.Forms
{
    public partial class AddEpisodeForm : Form
    {
        private System.Windows.Forms.Label lblEpisodeNumber;
        private System.Windows.Forms.NumericUpDown numEpisodeNumber;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.NumericUpDown numDuration;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        public Episode Episode { get; private set; }

        public AddEpisodeForm(int nextEpisodeNumber)
        {
            InitializeComponent();
            numEpisodeNumber.Value = nextEpisodeNumber;
        }

        private void InitializeComponent()
        {
            this.lblEpisodeNumber = new System.Windows.Forms.Label();
            this.numEpisodeNumber = new System.Windows.Forms.NumericUpDown();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.numDuration = new System.Windows.Forms.NumericUpDown();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            // lblEpisodeNumber
            this.lblEpisodeNumber.AutoSize = true;
            this.lblEpisodeNumber.Location = new System.Drawing.Point(20, 20);
            this.lblEpisodeNumber.Name = "lblEpisodeNumber";
            this.lblEpisodeNumber.Size = new System.Drawing.Size(65, 20);
            this.lblEpisodeNumber.Text = "Number:";

            // numEpisodeNumber
            this.numEpisodeNumber.Location = new System.Drawing.Point(120, 20);
            this.numEpisodeNumber.Maximum = 1000;
            this.numEpisodeNumber.Minimum = 1;
            this.numEpisodeNumber.Name = "numEpisodeNumber";
            this.numEpisodeNumber.Size = new System.Drawing.Size(150, 27);
            this.numEpisodeNumber.Value = 1;

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(20, 60);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(38, 20);
            this.lblTitle.Text = "Title:";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(120, 60);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(250, 27);

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(20, 100);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(124, 20);
            this.lblDuration.Text = "Duration (minutes):";

            // numDuration
            this.numDuration.Location = new System.Drawing.Point(150, 100);
            this.numDuration.Maximum = 300;
            this.numDuration.Minimum = 1;
            this.numDuration.Name = "numDuration";
            this.numDuration.Size = new System.Drawing.Size(120, 27);
            this.numDuration.Value = 30;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(100, 150);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 35);
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(220, 150);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 35);
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);

            // AddEpisodeForm
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.lblEpisodeNumber);
            this.Controls.Add(this.numEpisodeNumber);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.numDuration);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddEpisodeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Episode";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                Episode = new Episode
                {
                    EpisodeNumber = (int)numEpisodeNumber.Value,
                    Title = txtTitle.Text.Trim(),
                    Duration = (int)numDuration.Value
                };

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a title for the episode.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

