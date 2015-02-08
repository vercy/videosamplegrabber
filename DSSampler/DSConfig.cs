using System;
using System.Collections.Generic;
using System.Text;

namespace DSSampler
{
    /// <summary>
    /// Config holder class for the DsSampler. These are the actual settings.
    /// </summary>
    public class DSConfig
    {
        private int width;
        private int height;
        private ISampleProvider provider;
        private bool preview;
        private bool stats;

        /// <summary>
        /// Creates a new config object
        /// </summary>
        /// <param name="p">Provider</param>
        /// <param name="w">Width of the image</param>
        /// <param name="h">Height of the image</param>
        /// <param name="pr">Preview</param>
        /// /// <param name="st">Statistics</param>
        public DSConfig(ISampleProvider p, int w, int h, bool pr, bool st)
        {
            stats = st;
            provider = p;
            width = w;
            height = h;
            preview = pr;
        }

        /// <summary>
        /// Indicates wether the statistics is enabled
        /// </summary>
        public bool Statistics
        {
            get
            {
                return stats;
            }
        }

        /// <summary>
        /// The width of the image, that the Sampler serves
        /// </summary>
        public int ImageWidth
        {
            get
            {
                return width;
            }
        }

        /// <summary>
        /// The height of the image, that the Sampler serves
        /// </summary>
        public int ImageHeight
        {
            get
            {
                return height;
            }
        }

        /// <summary>
        /// The provider that is used to grab images
        /// </summary>
        public ISampleProvider Provider
        {
            get
            {
                return provider;
            }
        }

        /// <summary>
        /// Indicates wether a preview is shown
        /// </summary>
        public bool Preview
        {
            get
            {
                return preview;
            }
        }
    }
}
