using System;
using System.Windows.Forms;

namespace DSSampler
{
    /// <summary>
    /// This class represents the required configuration for the sampler.
    /// </summary>
    public class InteractiveDSConfig
    {
        private ISampleProvider prov;
        private bool preview;
        private bool buffer;
        private Control owner;
        private bool stats;

        /// <summary>
        /// Indicates wether the statistics should be enabled
        /// </summary>
        public bool Statistics
        {
            get
            {
                return stats;
            }
            set
            {
                stats = value;
            }
        }

        /// <summary>
        /// Holds the provider that should be used.
        /// </summary>
        public ISampleProvider Provider
        {
            get
            {
                return prov;
            }
            set
            {
                prov = value;
            }
        }

        /// <summary>
        /// Indicates wether a preview is requested.
        /// </summary>
        public bool Preview
        {
            get
            {
                return preview;
            }
            set
            {
                preview = value;
            }
        }

        /// <summary>
        /// Indicates, wether a buffer should be used.
        /// </summary>
        public bool Buffered
        {
            get
            {
                return buffer;
            }
            set
            {
                buffer = value;
            }
        }

        /// <summary>
        /// Holds the designated owner of the preview window.
        /// </summary>
        public Control Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }

    }
}
