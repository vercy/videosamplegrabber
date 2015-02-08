using System;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

using DSSampler.DShowNET;
using DSSampler.DShowNET.Device;

namespace DSSampler
{
	/// <summary>
    /// DsSampler is the core of the frame capture API. It has the basic functionality to
    /// handle arbitrary number and type of plug-ins that can be used to retrieve frames
    /// from video streams. As of version 4.0 many new features have been introduced. Enjoy!
    /// 
    /// Version 4.1 - Added Frame server mode
    /// 
    /// Version 4.0 - Synchronized buffered mode: reduced response time
    ///             - MediaOnline event: the sampler tells you when its ready
    ///             - Configuration handling: various information is available to the public throug config
    ///             - Statistics: modular stats for the tweakers. Stats can be disabled to improve performance.
    /// 
    /// Version 3.5 - removed built in providers
    ///             - added dynamic loader (external plugin architecture)
    ///             - changed event signatures
    ///             - NEW buffered mode
    /// 
    /// Version 3.0 - Hopefully thread safe.
    ///             - Provides an ISampleCB implementation
	/// </summary>
    public class DsSampler : IDisposable, ISampleGrabberCB  
	{
        /// <summary>
        /// Represents the control whose message queue (thread)
        /// should be used for tramsmitting the frames.
        /// </summary>
        private Control mQueue;

        private List<ISampleProvider> providers;
        private ISampleProvider provider;

        private bool request;
        private bool requestFrame;
        private bool BMPbuffered;
        private bool BMPreqWhileEmpty;
        private bool frameServerIsRunnig;

        private Bitmap BMPbuffer;
        private byte[] savedArray;

        private int imageWidth;
        private int imageHeight;
        private int imageStride;
        private int imageOffset;
        private VideoInfoHeader videoInfoHeader;

        private bool mediaOnline;
        private DSConfig configuration;
        private SamplerStats stats;
        private SamplerDialog configPane;
        public delegate void MediaStartup(DSConfig config);
		public delegate	void CaptureDone(Bitmap bmp);

		/// <summary>
		/// Launched when the latest request can be satisfied.
		/// </summary>
		public event CaptureDone FrameComplete;

        /// <summary>
        /// Launched, when the Sampler can accept requests.
        /// </summary>
        public event MediaStartup MediaOnline;

        /// <summary>
        /// The Current configuration of the sampler. It is not null after the MediaOnline event.
        /// </summary>
        public DSConfig Configuration
        {
            get
            {
                return configuration;
            }
        }

        /// <summary>
        /// The current statistics describing the sampler. Not null if the configuration says so.
        /// </summary>
        public SamplerStats Statistics
        {
            get
            {
                return stats;
            }
        }

        /// <summary>
        /// Returns if the frame server is running.
        /// </summary>
        public bool FrameServerIsRunning
        {
            get
            {
                return frameServerIsRunnig;
            }
        }

		/// <summary>
		/// Creates a new sampler. The sampler has been disegned to support a simple plugin
        /// architecture. With this approach it is more flexible than ever.
		/// </summary>
		public DsSampler(Control ctrl)
		{
            // State
            frameServerIsRunnig = false;
            BMPreqWhileEmpty = false;
            mediaOnline = false;
            request = false;
            mQueue = ctrl;
            providers = new List<ISampleProvider>();
            stats = new SamplerStats();

            // Loader
            List<string> ass = new List<string>();

			foreach(string f in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory,@"pr*.dll"))
			{
				string aname = f.Substring(f.LastIndexOf(Path.DirectorySeparatorChar)+1);
				aname = aname.Substring(0,aname.Length-4);
				ass.Add(aname);
			}

			foreach(string assemblyName in ass)
				try
				{
					Assembly assembly =  AppDomain.CurrentDomain.Load(assemblyName);
					foreach (Type t in assembly.GetTypes())
						foreach(Type it in t.GetInterfaces())
							if(it.Equals(typeof(ISampleProvider)))
								try
								{
                                    providers.Add((ISampleProvider)Activator.CreateInstance(t));
									break; // The current class is done
								}
								catch (MissingMethodException e)
								{
									Console.WriteLine(e.Message);
									continue;
								}
				}
				catch(Exception e)
				{
					Console.WriteLine("Error loading from '"+assemblyName+"': "+e.Message);
				}
		}

        /// <summary>
        /// The resize code to make the preview fit in any control.
        /// </summary>
        /// <param name="nSize">The size available to the preview</param>
        private void ResizeVideo(Size nSize)
        {
            if (provider != null)
            {
                int x, y, w, h;
                float aspect = (float)imageWidth / imageHeight;
                float naspect = (float)nSize.Width / nSize.Height;
                if (aspect > naspect)
                {
                    w = nSize.Width;
                    h = (int)(w / aspect);
                    x = 0;
                    y = (nSize.Height - h) / 2;
                }
                else
                {
                    h = nSize.Height;
                    w = (int)(h * aspect);
                    y = 0;
                    x = (nSize.Width - w) / 2;
                }
                provider.ResizeVideo(x,y,w,h);
            }
        }

        /// <summary>
        /// Shows a dialog box, that allows the user to interactively start up the sampler.
        /// Media cannot be considered running after this call. Wait for MediaOnline event.
        /// </summary>
        public void InteractiveStartup()
        {
            configPane = new SamplerDialog(this, providers);
            configPane.Show();
            configPane.SetupComplete += new SamplerDialog.ConfigDone(configPane_SetupComplete);
        }

        /// <summary>
        /// Hides the config panel, if possible.
        /// </summary>
        public void HideConfigPanel()
        {
            if (configPane != null)
                configPane.Hide();
        }

        /// <summary>
        /// Shows the config panel, if possible.
        /// </summary>
        public void ShowConfigPanel()
        {
            if (configPane != null)
                configPane.Show();
        }

        /// <summary>
        /// Parametrizes the provider and itself according to the required config.
        /// You can consider it as converting required -> actual config.
        /// </summary>
        /// <param name="config">The required configuration from the UI</param>
        private void configPane_SetupComplete(InteractiveDSConfig config)
        {
            // Copy the provider
            provider = config.Provider;

            // Enable stats if necessary
            stats.Enabled = config.Statistics;

            // Prepare the buffer
            BMPbuffer = null;
            BMPbuffered = config.Buffered;

            videoInfoHeader = provider.StartProvider(config.Preview, this, config.Owner.Handle);

            // Setup image storage
            imageWidth = videoInfoHeader.BmiHeader.Width;
            imageHeight = videoInfoHeader.BmiHeader.Height;
            imageStride = imageWidth * 3;
            imageOffset = (imageHeight - 1) * imageStride;
            savedArray = new byte[imageWidth * imageHeight * 4];

            // Adjust the preview
            ResizeVideo(config.Owner.Size);

            // Create the current configuration object
            configuration = new DSConfig(provider, imageWidth, imageHeight, config.Preview, config.Statistics);

            // Indicate that the preparations are complete
            mediaOnline = true;
            if (MediaOnline != null)
                MediaOnline(configuration);
        }

        /// <summary>
        /// Starts a the sampler in frame server mode. That is the sampler sends frames
        /// to the registered listeners automatically after the previous has been processed.
        /// </summary>
        public void StartFrameServer()
        {
            if (!mediaOnline)
                throw new Exception("The media is not online yet. Start the server after the MediaOnline event.");
            if (frameServerIsRunnig)
                throw new Exception("The frame server is already running.");
            if (requestFrame)
                throw new Exception("A single frame request is in progress. See RequestFrame for information.");
            

            // The frameserver can be started at this point
            frameServerIsRunnig = true;
            Thread t = new Thread(new ThreadStart(ServerCore));

            stats.StartFrameServerMode();
            t.Start();
        }

        private void ServerCore()
        {
            if (BMPbuffered)
            {
                while (frameServerIsRunnig)
                {
                    Thread.Sleep(0);
                    stats.NewRequest();
                    if (BMPbuffer == null)
                    {
                        // The media is online, however no frame has arrived yet (slow media or immediate call)
                        BMPreqWhileEmpty = true;
                        return;
                    }
                    Bitmap cpy = null;
                    lock (BMPbuffer)
                    {
                        cpy = new Bitmap(BMPbuffer);
                    }
                    stats.NewCopy();
                    SampleComplete(cpy);
                }
            }
            else
            {
                stats.NewRequest();
                request = true;
            }
        }

        /// <summary>
        /// Stops the frame server. No frames will be sent after the last frame in the
        /// pipeline has been processed. You can start the service with StartFrameServer.
        /// </summary>
        public void StopFrameServer()
        {
            if (!frameServerIsRunnig)
                throw new Exception("The frame server is not running.");

            frameServerIsRunnig = false;
            stats.StopFrameServerMode();
        }

		/// <summary>
		/// Requests a single frame for processing.
        /// Conditions of acceptance:
        ///     - the media should be online
        ///     - no previous request can be in progress
        ///     - the method cannot be invoked in frame server mode.
		/// </summary>
		public void RequestFrame()
		{
            if (!mediaOnline)
                throw new Exception("The media is not online yet. Request frames after the MediaOnline event.");
            if (frameServerIsRunnig)
                throw new Exception("Single frame requests cannot be made in frame server mode.");
            if (requestFrame)
                throw new Exception("The previous request could not satisfied yet. See StartFrameServer for advanced functionality");
            

            // At this point the request can be accepted
            stats.NewRequest();
            // Close semaphor
            requestFrame = true;

            if (BMPbuffered)
            {
                if (BMPbuffer == null)
                {
                    // The media is online, however no frame has arrived yet (slow media or immediate call)
                    BMPreqWhileEmpty = true;
                    return;
                }
                Bitmap cpy = null;
                lock (BMPbuffer)
                {
                    cpy = new Bitmap(BMPbuffer);
                }
                stats.NewCopy();
                SampleComplete(cpy);
            }
            else
                request = true;
		}

		
        /// <summary>
        /// This method will be called when a request completes.
        /// It's purpose is to change the current thread.
        /// </summary>
        /// <param name="bmp">The new sample bitmap</param>
		private void SampleComplete(Bitmap bmp)
        {
            try
            {
                if (BMPbuffered)
                    mQueue.Invoke(new CaptureDone(InvokeOnThis), new object[] { bmp });
                else
                    mQueue.BeginInvoke(new CaptureDone(InvokeOnThis), new object[] { bmp });
            }
            catch (Exception) { }
		}

        /// <summary>
        /// Sends the event to the owner of the sampler.
        /// This method gets executed on the thread the DirectShow interfaces were created.
        /// </summary>
        private void InvokeOnThis(Bitmap bmp)
        {
            stats.FulfillRequest();
            if (FrameComplete != null)
                FrameComplete(bmp);

            if (frameServerIsRunnig)
            {
                if (!BMPbuffered)
                {
                    stats.NewRequest();
                    request = true;
                }
            }
            else
            {
                // open semaphor ;) for single frames
                requestFrame = false;
            }
            
        }

        #region ISampleGrabberCB Members

        int ISampleGrabberCB.SampleCB(double SampleTime, IMediaSample pSample)
        {
            return 0;
        }

        int ISampleGrabberCB.BufferCB(double SampleTime, System.IntPtr pBuffer, int BufferLen)
        {
            stats.NewFrame();
            if (BMPbuffered)
            {
                if ((pBuffer != IntPtr.Zero) && (BufferLen <= savedArray.Length))
                    Marshal.Copy(pBuffer, savedArray, 0, BufferLen);
                else
                    return 0;

                GCHandle handle = GCHandle.Alloc(savedArray, GCHandleType.Pinned);
                int scan0 = (int)handle.AddrOfPinnedObject();
                scan0 += imageOffset;
                BMPbuffer = new Bitmap(imageWidth, imageHeight, -imageStride,
                    PixelFormat.Format24bppRgb, (IntPtr)scan0);
                handle.Free();
                stats.NewCopy();

                if (BMPreqWhileEmpty)
                {
                    // There was a request, however the buffer was empty, this must be the first frame
                    Bitmap cpy = null;
                    lock(BMPbuffer)
                    {
                        cpy = new Bitmap(BMPbuffer);
                    }
                    stats.NewCopy();
                    BMPreqWhileEmpty = false;
                    SampleComplete(cpy);
                }       
            }
            else if(request)
            {
                request = false;

                if ((pBuffer != IntPtr.Zero) && (BufferLen <= savedArray.Length))
                    Marshal.Copy(pBuffer, savedArray, 0, BufferLen);
                else
                    return 0;

                GCHandle handle = GCHandle.Alloc(savedArray, GCHandleType.Pinned);
                int scan0 = (int)handle.AddrOfPinnedObject();
                scan0 += imageOffset;
                BMPbuffer = new Bitmap(imageWidth, imageHeight, -imageStride,
                    PixelFormat.Format24bppRgb, (IntPtr)scan0);
                handle.Free();
                stats.NewCopy();

                SampleComplete(BMPbuffer);
            }
            return 0;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose the Sampler. This means Closing of the underlying stream.
        /// </summary>
        public void Dispose()
        {
            if (frameServerIsRunnig)
                StopFrameServer();

            if (provider != null)
                provider.CloseStream();
        }

        #endregion

    }


}
