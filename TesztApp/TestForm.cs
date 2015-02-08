using System;
using System.Threading;
using System.Drawing;
using System.Windows.Forms;
using DSSampler;

namespace TestApp
{
    public partial class TestForm : Form
    {
        private DsSampler grabber;

        public TestForm()
        {
            
            InitializeComponent();
        }

        private void captureButton_Click(object sender, EventArgs e)
        {
            if (grabber == null)
                return;

            Image old = pictureBox.Image;
            pictureBox.Image = null;
            if (old != null)
                old.Dispose();

            captureButton.Enabled = false;
            frameServerButton.Enabled = false;
            saveButton.Enabled = false;
            grabber.RequestFrame();
        }

        private void TestForm_Shown(object sender, EventArgs e)
        {
            grabber = new DsSampler(this);
            grabber.FrameComplete += new DsSampler.CaptureDone(grabber_FrameComplete);
            grabber.MediaOnline += new DsSampler.MediaStartup(grabber_MediaOnline);
            grabber.InteractiveStartup();
        }

        void grabber_FrameComplete(Bitmap bmp)
        {
            //Thread.Sleep(333);
            pictureBox.Image = bmp;
            saveButton.Enabled = true;
            if (!grabber.FrameServerIsRunning)
            {
                captureButton.Enabled = true;
                frameServerButton.Enabled = true;
            }
        }

        void grabber_MediaOnline(DSConfig config)
        {
            Size s = this.Size;
            Size ps = pictureBox.Size;
            s.Height = s.Height - ps.Height + config.ImageHeight;
            s.Width = s.Width - ps.Width + config.ImageWidth;
            this.Size = s;
            captureButton.Enabled = true;
            frameServerButton.Enabled = true;
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Bitmap bmp = (Bitmap)pictureBox.Image;
            if (bmp != null && savedlg.ShowDialog() == DialogResult.OK)
                bmp.Save(savedlg.FileName);
        }

        private void frameServerMode_Click(object sender, EventArgs e)
        {
            if (grabber.FrameServerIsRunning)
            {
                frameServerButton.Text = "Start Server";
                captureButton.Enabled = true;
                grabber.StopFrameServer();
            }
            else
            {
                frameServerButton.Text = "Stop Server";
                captureButton.Enabled = false;
                grabber.StartFrameServer();
            }
            
        }

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            grabber.Dispose();
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            grabber.ShowConfigPanel();
        }
    }
}