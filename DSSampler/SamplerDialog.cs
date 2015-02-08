using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DSSampler
{
    public partial class SamplerDialog : Form
    {
        private Dictionary<string, ISampleProvider> providertab;
        private DsSampler sampler;
        public delegate void ConfigDone(InteractiveDSConfig config);

        /// <summary>
        /// Launched when all the necessary information has been gathered.
        /// </summary>
        public event ConfigDone SetupComplete;

        public const string version = "2.2.0";        

        public SamplerDialog(DsSampler smp, IEnumerable<ISampleProvider> providers)
        {
            sampler = smp;
            InitializeComponent();

            Text = Text + " " + version;

            providertab = new Dictionary<string, ISampleProvider>();
            List<string> items = new List<string>();

            foreach(ISampleProvider prov in providers)
                try
                {
                    string name = prov.Name; //Throws exception if prov is null
                    if (!prov.Applicable)
                        continue;
                    providertab.Add(name, prov); //Throws exception if the name is duplicate
                    items.Add(name); // if everything is ok, the item will be listed
                }
                catch(Exception){/*Takes care of null providers, and duplicates*/}

            providerBox.Items.Clear();
            providerBox.Items.AddRange(items.ToArray());
        }

        private void updateTimer_Tick(object sender, EventArgs e)
        {
            // Updates stats every 100 ms
            SamplerStats s = sampler.Statistics;
            if (s != null)
            {
                numFrames.Text = s.NumFrames.ToString();
                numRequests.Text = s.NumRequests.ToString();
                numCopies.Text = s.NumCopies.ToString();
                response.Text = s.CurrentResponse.ToString("G3") + " ms";
                fps.Text = s.CurrentFps.ToString("G3");
                avgFps.Text = s.AvgFps.ToString("G3");
                avgResponse.Text = s.AvgResponse.ToString("G3") + " ms";
                if(s.FrameServerMode)
                    skipFrames.Text = s.SkippedFrames.ToString();
                else
                    skipFrames.Text = "-";
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            string value = (string)providerBox.SelectedItem;

            // Sanity check
            if (value == null)
            {
                MessageBox.Show("Specify input source!");
                return;    
            }

            //This has sense only if somebody is listening
            if (sampler != null && SetupComplete != null)
            {
                startButton.Visible = false;
                bufferedMode.Enabled = false;
                providerBox.Enabled = false;
                statistics.Enabled = false;
                closeWindow.Enabled = false;
                Text = "Active sampler" + " " + version;

                InteractiveDSConfig config = new InteractiveDSConfig();
                config.Buffered = bufferedMode.Checked;
                config.Statistics = statistics.Checked;
                config.Preview = true;
                config.Provider = providertab[value];
                config.Owner = videoPanel;

                sampler.MediaOnline += new DsSampler.MediaStartup(sampler_MediaOnline);
                SetupComplete(config);
            }
        }

        void sampler_MediaOnline(DSConfig config)
        {
            sampler.MediaOnline -= new DsSampler.MediaStartup(sampler_MediaOnline);
            imgDimLabel.Enabled = true;
            imgDim.Text = sampler.Configuration.ImageWidth + "x" + sampler.Configuration.ImageHeight;

            if (sampler.Configuration.Statistics)
            {
                //starting stats
                updateTimer.Enabled = true;

                numFramesLabel.Enabled = true;
                numRequestsLabel.Enabled = true;
                numCopiesLabel.Enabled = true;
                responseLabel.Enabled = true;
                fpsLabel.Enabled = true;
                avgFpsLabel.Enabled = true;
                avgResponseLabel.Enabled = true;
                skipFramesLabel.Enabled = true;
            }
            if (closeWindow.Checked)
                Hide();
        }

        private void SamplerDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}