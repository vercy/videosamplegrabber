namespace DSSampler
{
    partial class SamplerDialog
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
            this.components = new System.ComponentModel.Container();
            this.providerBox = new System.Windows.Forms.ComboBox();
            this.startButton = new System.Windows.Forms.Button();
            this.numFramesLabel = new System.Windows.Forms.Label();
            this.numRequestsLabel = new System.Windows.Forms.Label();
            this.numCopiesLabel = new System.Windows.Forms.Label();
            this.responseLabel = new System.Windows.Forms.Label();
            this.bufferedMode = new System.Windows.Forms.CheckBox();
            this.numFrames = new System.Windows.Forms.Label();
            this.numRequests = new System.Windows.Forms.Label();
            this.numCopies = new System.Windows.Forms.Label();
            this.response = new System.Windows.Forms.Label();
            this.updateTimer = new System.Windows.Forms.Timer(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.imgDimLabel = new System.Windows.Forms.Label();
            this.imgDim = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.fps = new System.Windows.Forms.Label();
            this.videoPanel = new System.Windows.Forms.Panel();
            this.avgResponseLabel = new System.Windows.Forms.Label();
            this.avgResponse = new System.Windows.Forms.Label();
            this.avgFpsLabel = new System.Windows.Forms.Label();
            this.avgFps = new System.Windows.Forms.Label();
            this.statgroup = new System.Windows.Forms.GroupBox();
            this.statistics = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.closeWindow = new System.Windows.Forms.CheckBox();
            this.skipFramesLabel = new System.Windows.Forms.Label();
            this.skipFrames = new System.Windows.Forms.Label();
            this.statgroup.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // providerBox
            // 
            this.providerBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.providerBox.Location = new System.Drawing.Point(8, 25);
            this.providerBox.Name = "providerBox";
            this.providerBox.Size = new System.Drawing.Size(167, 21);
            this.providerBox.TabIndex = 0;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(181, 153);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(155, 24);
            this.startButton.TabIndex = 2;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // numFramesLabel
            // 
            this.numFramesLabel.AutoSize = true;
            this.numFramesLabel.Enabled = false;
            this.numFramesLabel.Location = new System.Drawing.Point(6, 16);
            this.numFramesLabel.Name = "numFramesLabel";
            this.numFramesLabel.Size = new System.Drawing.Size(63, 13);
            this.numFramesLabel.TabIndex = 3;
            this.numFramesLabel.Text = "Num frames";
            // 
            // numRequestsLabel
            // 
            this.numRequestsLabel.AutoSize = true;
            this.numRequestsLabel.Enabled = false;
            this.numRequestsLabel.Location = new System.Drawing.Point(6, 29);
            this.numRequestsLabel.Name = "numRequestsLabel";
            this.numRequestsLabel.Size = new System.Drawing.Size(72, 13);
            this.numRequestsLabel.TabIndex = 4;
            this.numRequestsLabel.Text = "Num requests";
            // 
            // numCopiesLabel
            // 
            this.numCopiesLabel.AutoSize = true;
            this.numCopiesLabel.Enabled = false;
            this.numCopiesLabel.Location = new System.Drawing.Point(6, 42);
            this.numCopiesLabel.Name = "numCopiesLabel";
            this.numCopiesLabel.Size = new System.Drawing.Size(64, 13);
            this.numCopiesLabel.TabIndex = 5;
            this.numCopiesLabel.Text = "Num Copies";
            // 
            // responseLabel
            // 
            this.responseLabel.AutoSize = true;
            this.responseLabel.Enabled = false;
            this.responseLabel.Location = new System.Drawing.Point(170, 42);
            this.responseLabel.Name = "responseLabel";
            this.responseLabel.Size = new System.Drawing.Size(55, 13);
            this.responseLabel.TabIndex = 6;
            this.responseLabel.Text = "Response";
            // 
            // bufferedMode
            // 
            this.bufferedMode.AutoSize = true;
            this.bufferedMode.Location = new System.Drawing.Point(8, 57);
            this.bufferedMode.Name = "bufferedMode";
            this.bufferedMode.Size = new System.Drawing.Size(130, 17);
            this.bufferedMode.TabIndex = 7;
            this.bufferedMode.Text = "Enable buffered mode";
            this.bufferedMode.UseVisualStyleBackColor = true;
            // 
            // numFrames
            // 
            this.numFrames.AutoSize = true;
            this.numFrames.Location = new System.Drawing.Point(98, 16);
            this.numFrames.Name = "numFrames";
            this.numFrames.Size = new System.Drawing.Size(0, 13);
            this.numFrames.TabIndex = 8;
            // 
            // numRequests
            // 
            this.numRequests.AutoSize = true;
            this.numRequests.Location = new System.Drawing.Point(98, 29);
            this.numRequests.Name = "numRequests";
            this.numRequests.Size = new System.Drawing.Size(0, 13);
            this.numRequests.TabIndex = 9;
            // 
            // numCopies
            // 
            this.numCopies.AutoSize = true;
            this.numCopies.Location = new System.Drawing.Point(98, 42);
            this.numCopies.Name = "numCopies";
            this.numCopies.Size = new System.Drawing.Size(0, 13);
            this.numCopies.TabIndex = 10;
            // 
            // response
            // 
            this.response.AutoSize = true;
            this.response.Location = new System.Drawing.Point(262, 42);
            this.response.Name = "response";
            this.response.Size = new System.Drawing.Size(0, 13);
            this.response.TabIndex = 11;
            // 
            // updateTimer
            // 
            this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Input source:";
            // 
            // imgDimLabel
            // 
            this.imgDimLabel.AutoSize = true;
            this.imgDimLabel.Enabled = false;
            this.imgDimLabel.Location = new System.Drawing.Point(6, 16);
            this.imgDimLabel.Name = "imgDimLabel";
            this.imgDimLabel.Size = new System.Drawing.Size(86, 13);
            this.imgDimLabel.TabIndex = 13;
            this.imgDimLabel.Text = "Image dimension";
            // 
            // imgDim
            // 
            this.imgDim.AutoSize = true;
            this.imgDim.Location = new System.Drawing.Point(100, 16);
            this.imgDim.Name = "imgDim";
            this.imgDim.Size = new System.Drawing.Size(0, 13);
            this.imgDim.TabIndex = 14;
            // 
            // fpsLabel
            // 
            this.fpsLabel.AutoSize = true;
            this.fpsLabel.Enabled = false;
            this.fpsLabel.Location = new System.Drawing.Point(170, 16);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(24, 13);
            this.fpsLabel.TabIndex = 15;
            this.fpsLabel.Text = "Fps";
            // 
            // fps
            // 
            this.fps.AutoSize = true;
            this.fps.Location = new System.Drawing.Point(262, 16);
            this.fps.Name = "fps";
            this.fps.Size = new System.Drawing.Size(0, 13);
            this.fps.TabIndex = 16;
            // 
            // videoPanel
            // 
            this.videoPanel.BackColor = System.Drawing.Color.Black;
            this.videoPanel.Location = new System.Drawing.Point(181, 9);
            this.videoPanel.Name = "videoPanel";
            this.videoPanel.Size = new System.Drawing.Size(157, 138);
            this.videoPanel.TabIndex = 17;
            // 
            // avgResponseLabel
            // 
            this.avgResponseLabel.AutoSize = true;
            this.avgResponseLabel.Enabled = false;
            this.avgResponseLabel.Location = new System.Drawing.Point(170, 55);
            this.avgResponseLabel.Name = "avgResponseLabel";
            this.avgResponseLabel.Size = new System.Drawing.Size(80, 13);
            this.avgResponseLabel.TabIndex = 18;
            this.avgResponseLabel.Text = "Avg. Response";
            // 
            // avgResponse
            // 
            this.avgResponse.AutoSize = true;
            this.avgResponse.Location = new System.Drawing.Point(262, 55);
            this.avgResponse.Name = "avgResponse";
            this.avgResponse.Size = new System.Drawing.Size(0, 13);
            this.avgResponse.TabIndex = 19;
            // 
            // avgFpsLabel
            // 
            this.avgFpsLabel.AutoSize = true;
            this.avgFpsLabel.Enabled = false;
            this.avgFpsLabel.Location = new System.Drawing.Point(170, 29);
            this.avgFpsLabel.Name = "avgFpsLabel";
            this.avgFpsLabel.Size = new System.Drawing.Size(49, 13);
            this.avgFpsLabel.TabIndex = 20;
            this.avgFpsLabel.Text = "Avg. Fps";
            // 
            // avgFps
            // 
            this.avgFps.AutoSize = true;
            this.avgFps.Location = new System.Drawing.Point(262, 29);
            this.avgFps.Name = "avgFps";
            this.avgFps.Size = new System.Drawing.Size(0, 13);
            this.avgFps.TabIndex = 21;
            // 
            // statgroup
            // 
            this.statgroup.Controls.Add(this.skipFrames);
            this.statgroup.Controls.Add(this.skipFramesLabel);
            this.statgroup.Controls.Add(this.avgFps);
            this.statgroup.Controls.Add(this.numFramesLabel);
            this.statgroup.Controls.Add(this.avgFpsLabel);
            this.statgroup.Controls.Add(this.numRequestsLabel);
            this.statgroup.Controls.Add(this.avgResponse);
            this.statgroup.Controls.Add(this.numCopiesLabel);
            this.statgroup.Controls.Add(this.avgResponseLabel);
            this.statgroup.Controls.Add(this.responseLabel);
            this.statgroup.Controls.Add(this.numFrames);
            this.statgroup.Controls.Add(this.fps);
            this.statgroup.Controls.Add(this.numRequests);
            this.statgroup.Controls.Add(this.fpsLabel);
            this.statgroup.Controls.Add(this.numCopies);
            this.statgroup.Controls.Add(this.response);
            this.statgroup.Location = new System.Drawing.Point(8, 178);
            this.statgroup.Name = "statgroup";
            this.statgroup.Size = new System.Drawing.Size(330, 75);
            this.statgroup.TabIndex = 22;
            this.statgroup.TabStop = false;
            this.statgroup.Text = "Statistics";
            // 
            // statistics
            // 
            this.statistics.AutoSize = true;
            this.statistics.Checked = true;
            this.statistics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.statistics.Location = new System.Drawing.Point(8, 80);
            this.statistics.Name = "statistics";
            this.statistics.Size = new System.Drawing.Size(102, 17);
            this.statistics.TabIndex = 23;
            this.statistics.Text = "Enable statistics";
            this.statistics.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.imgDimLabel);
            this.groupBox2.Controls.Add(this.imgDim);
            this.groupBox2.Location = new System.Drawing.Point(8, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(167, 40);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Image info";
            // 
            // closeWindow
            // 
            this.closeWindow.AutoSize = true;
            this.closeWindow.Checked = true;
            this.closeWindow.CheckState = System.Windows.Forms.CheckState.Checked;
            this.closeWindow.Location = new System.Drawing.Point(8, 103);
            this.closeWindow.Name = "closeWindow";
            this.closeWindow.Size = new System.Drawing.Size(110, 17);
            this.closeWindow.TabIndex = 25;
            this.closeWindow.Text = "Close this window";
            this.closeWindow.UseVisualStyleBackColor = true;
            // 
            // skipFramesLabel
            // 
            this.skipFramesLabel.AutoSize = true;
            this.skipFramesLabel.Enabled = false;
            this.skipFramesLabel.Location = new System.Drawing.Point(4, 55);
            this.skipFramesLabel.Name = "skipFramesLabel";
            this.skipFramesLabel.Size = new System.Drawing.Size(83, 13);
            this.skipFramesLabel.TabIndex = 22;
            this.skipFramesLabel.Text = "Skipped Frames";
            // 
            // skipFrames
            // 
            this.skipFrames.AutoSize = true;
            this.skipFrames.Location = new System.Drawing.Point(98, 55);
            this.skipFrames.Name = "skipFrames";
            this.skipFrames.Size = new System.Drawing.Size(0, 13);
            this.skipFrames.TabIndex = 23;
            // 
            // SamplerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 262);
            this.Controls.Add(this.closeWindow);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statistics);
            this.Controls.Add(this.statgroup);
            this.Controls.Add(this.videoPanel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bufferedMode);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.providerBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SamplerDialog";
            this.Text = "Sampler Initialization";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SamplerDialog_FormClosing);
            this.statgroup.ResumeLayout(false);
            this.statgroup.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox providerBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label numFramesLabel;
        private System.Windows.Forms.Label numRequestsLabel;
        private System.Windows.Forms.Label numCopiesLabel;
        private System.Windows.Forms.Label responseLabel;
        private System.Windows.Forms.CheckBox bufferedMode;
        private System.Windows.Forms.Label numFrames;
        private System.Windows.Forms.Label numRequests;
        private System.Windows.Forms.Label numCopies;
        private System.Windows.Forms.Label response;
        private System.Windows.Forms.Timer updateTimer;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label imgDimLabel;
        private System.Windows.Forms.Label imgDim;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Label fps;
        private System.Windows.Forms.Panel videoPanel;
        private System.Windows.Forms.Label avgResponseLabel;
        private System.Windows.Forms.Label avgResponse;
        private System.Windows.Forms.Label avgFpsLabel;
        private System.Windows.Forms.Label avgFps;
        private System.Windows.Forms.GroupBox statgroup;
        private System.Windows.Forms.CheckBox statistics;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox closeWindow;
        private System.Windows.Forms.Label skipFrames;
        private System.Windows.Forms.Label skipFramesLabel;
    }
}