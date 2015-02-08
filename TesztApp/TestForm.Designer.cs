namespace TestApp
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.captureButton = new System.Windows.Forms.ToolStripButton();
            this.frameServerButton = new System.Windows.Forms.ToolStripButton();
            this.saveButton = new System.Windows.Forms.ToolStripButton();
            this.setupButton = new System.Windows.Forms.ToolStripButton();
            this.savedlg = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.captureButton,
            this.frameServerButton,
            this.saveButton,
            this.setupButton});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(440, 36);
            this.toolStrip.TabIndex = 1;
            // 
            // captureButton
            // 
            this.captureButton.Enabled = false;
            this.captureButton.Image = ((System.Drawing.Image)(resources.GetObject("captureButton.Image")));
            this.captureButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.captureButton.Name = "captureButton";
            this.captureButton.Size = new System.Drawing.Size(50, 33);
            this.captureButton.Text = "Capture";
            this.captureButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.captureButton.ToolTipText = "Captures a new image from th eunderlying stream";
            this.captureButton.Click += new System.EventHandler(this.captureButton_Click);
            // 
            // frameServerButton
            // 
            this.frameServerButton.Enabled = false;
            this.frameServerButton.Image = ((System.Drawing.Image)(resources.GetObject("frameServerButton.Image")));
            this.frameServerButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.frameServerButton.Name = "frameServerButton";
            this.frameServerButton.Size = new System.Drawing.Size(70, 33);
            this.frameServerButton.Text = "Start Server";
            this.frameServerButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.frameServerButton.Click += new System.EventHandler(this.frameServerMode_Click);
            // 
            // saveButton
            // 
            this.saveButton.Enabled = false;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(35, 33);
            this.saveButton.Text = "Save";
            this.saveButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // setupButton
            // 
            this.setupButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.setupButton.Image = ((System.Drawing.Image)(resources.GetObject("setupButton.Image")));
            this.setupButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.setupButton.Name = "setupButton";
            this.setupButton.Size = new System.Drawing.Size(39, 33);
            this.setupButton.Text = "Setup";
            this.setupButton.Click += new System.EventHandler(this.setupButton_Click);
            // 
            // savedlg
            // 
            this.savedlg.DefaultExt = "png";
            this.savedlg.Filter = "Portable Network Graphics (png)|*.png";
            this.savedlg.RestoreDirectory = true;
            // 
            // pictureBox
            // 
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 36);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(440, 254);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox.TabIndex = 5;
            this.pictureBox.TabStop = false;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 290);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.toolStrip);
            this.Name = "TestForm";
            this.Text = "Sample Grabber Test Application v2.1";
            this.Shown += new System.EventHandler(this.TestForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TestForm_FormClosing);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton captureButton;
        private System.Windows.Forms.ToolStripButton saveButton;
        private System.Windows.Forms.SaveFileDialog savedlg;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripButton frameServerButton;
        private System.Windows.Forms.ToolStripButton setupButton;


    }
}

