using System;

namespace DSSampler
{
    /// <summary>
    /// Statistics sampler object. The properties hold the stats, while
    /// the collection is done via the sampler methods. Sampling can be disabled.
    /// </summary>
    public class SamplerStats
    {
        private int numFrames;
        private int numRequests;
        private int numCopies;
        private int skipFrame;

        private double curResponse;
        private double avgResponse;
        private double curFps;
        private double avgFps;

        private int numFramesServerMode;
        private int numProcessedInServerMode;
        private DateTime startRequest;
        private TimeSpan reqWait;
        private DateTime firstFrame;
        private DateTime prevFrame;
        private bool noFramesYet;
        private bool servermode;

        private bool enabled;

        /// <summary>
        /// Indicates wether the statistics are enabled.
        /// If the statistics are being enabled the values will be reset.
        /// </summary>
        public bool Enabled
        {
            get
            {
                return enabled;
            }
            set
            {
                
                if (value != enabled)
                {
                    // Reset only if state (false -> true)
                    // This will keep the latest stats until restart
                    if (value == true)
                        Reset();

                    enabled = value;
                }
                
            }
        }

        /// <summary>
        /// Returns the number of skipped frames since Frame Server startup.
        /// </summary>
        public int SkippedFrames
        {
            get
            {
                return skipFrame;
            }
        }

        /// <summary>
        /// Indicate wether Frameservermode is enabled.
        /// </summary>
        public bool FrameServerMode
        {
            get
            {
                return servermode;
            }
        }

        /// <summary>
        /// The aggregate number of frames
        /// </summary>
        public int NumFrames
        {
            get
            {
                return numFrames;
            }
        }

        /// <summary>
        /// The aggregate number of requests
        /// </summary>
        public int NumRequests
        {
            get
            {
                return numRequests;
            }
        }

        /// <summary>
        /// The aggregate number of copies.
        /// </summary>
        public int NumCopies
        {
            get
            {
                return numCopies;
            }
        }

        /// <summary>
        /// The response time of the last request in millis
        /// </summary>
        public double CurrentResponse
        {
            get
            {
                return curResponse;
            }
        }

        /// <summary>
        /// The measured fps based on new frames intervalls.
        /// </summary>
        public double CurrentFps
        {
            get
            {
                return curFps;
            }
        }

        /// <summary>
        /// The measured mean fps
        /// </summary>
        public double AvgFps
        {
            get
            {
                return avgFps;
            }
        }

        /// <summary>
        /// The measured mean response time
        /// </summary>
        public double AvgResponse
        {
            get
            {
                return avgResponse;
            }
        }

        /// <summary>
        /// Creates a new, empty statistics counter. The sampling is turned off by default.
        /// You can enable data collection via the Enabled property.
        /// </summary>
        public SamplerStats()
        {
            enabled = false;
            noFramesYet = true;
            servermode = false;
        }

        /// <summary>
        /// Resets the statistics to initial values
        /// </summary>
        private void Reset()
        {
            numFrames = 0;
            numRequests = 0;
            numCopies = 0;
            skipFrame = 0;
            numFramesServerMode = 0;
            numProcessedInServerMode = 0;

            curResponse = 0;
            curFps = 0;
            avgFps = 0;
            avgResponse = 0;
            servermode = false;
            noFramesYet = true;
            reqWait = new TimeSpan();
        }

        /// <summary>
        /// Should be called when a new request has been submitted
        /// </summary>
        public void NewRequest()
        {
            if (!enabled)
                return;

            startRequest = DateTime.Now;

            numRequests++;
        }

        /// <summary>
        /// Should be called when a request has been fulfilled
        /// </summary>
        public void FulfillRequest()
        {
            if (!enabled)
                return;

            TimeSpan ts = DateTime.Now.Subtract(startRequest);
            reqWait = reqWait.Add(ts);
            avgResponse = reqWait.TotalMilliseconds / numRequests;
            curResponse = ts.TotalMilliseconds;

            if (servermode)
            {
                numProcessedInServerMode++;
                skipFrame = numFramesServerMode - numProcessedInServerMode;
            }
        }

        /// <summary>
        /// Indicates the beginning of the frameserver mode.
        /// </summary>
        public void StartFrameServerMode()
        {
            if (!enabled)
                return;

            servermode = true;
            skipFrame = 0;
            numFramesServerMode = 0;
            numProcessedInServerMode = 0;
        }

        /// <summary>
        /// Indicates the end of franeserver mode.
        /// </summary>
        public void StopFrameServerMode()
        {
            if (!enabled)
                return;

            servermode = false;
        }

        /// <summary>
        /// Should be called when a new copy has been made of the target frame
        /// </summary>
        public void NewCopy()
        {
            if (!enabled)
                return;

            numCopies++;
        }

        /// <summary>
        /// Should be called when a new frame enterred the processing pipeline
        /// </summary>
        public void NewFrame()
        {
            if (!enabled)
                return;

            if (noFramesYet)
            {
                noFramesYet = false;
                firstFrame = DateTime.Now;
                prevFrame = firstFrame;
                numFrames++;
                return;
            }

            DateTime now = DateTime.Now;
            TimeSpan ts = now.Subtract(prevFrame);
            prevFrame = now;
            
            curFps = 1.0 / ts.TotalSeconds;
            avgFps = numFrames / now.Subtract(firstFrame).TotalSeconds;

            numFrames++;

            if (servermode)
            {
                numFramesServerMode++;
                skipFrame = numFramesServerMode - numProcessedInServerMode;
            }
        }
    }
}
