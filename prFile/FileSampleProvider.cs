using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Threading;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using DSSampler.DShowNET;
using DSSampler.DShowNET.Device;

namespace DSSampler
{
	/// <summary>
	/// FileSampleProvider is an ISampleProvider, that can render and
	/// sample media files. However only the builtin graphbuilder is used.
	/// This is usualy sufficent, but may require additional filters to be
	/// installed by the user for some specific media file types. 
    /// version changes:
    ///     1.2.0 - Fixed frame drop bug.
	/// </summary>
    public class FileSampleProvider : ISampleProvider
	{

		public const string	version	= "1.2.1";

        private OpenFileDialog inputMediaDialog;
        private bool preview;
        private ISampleGrabberCB sampleGrabber;
        private VideoInfoHeader videoInfoHeader;

		/// <summary> graph builder interface. </summary>
		private IGraphBuilder			graphBuilder;

		/// <summary> capture graph builder interface. </summary>
		private ISampleGrabber			sampGrabber;

		/// <summary> control interface. </summary>
		private IMediaControl			mediaCtrl;

		/// <summary> video window interface. </summary>
		private IVideoWindow			videoWin;

		/// <summary> grabber filter interface. </summary>
		private IBaseFilter				baseGrabFlt;
		private IBaseFilter				smartTee;

		public FileSampleProvider()
		{
			inputMediaDialog = new OpenFileDialog();
			inputMediaDialog.Filter = "Media files|*.avi;*.wmv;*.asf;*.mpg;*.vob;*.ogm|All files|*.*";
			inputMediaDialog.RestoreDirectory = true;
			inputMediaDialog.Title = "Select Input Media File";
			inputMediaDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.inputMediaDialog_FileOk);
		}

		bool StartUpFileVideo( string fileName )
		{
			int hr;
			try 
			{

				GetFilePlayInterfaces();

				SetupPlaybackGraph(fileName);
			
				hr = mediaCtrl.Run();
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				return true;
			}
			catch( Exception ee )
			{
				throw new Exception("Could not start video stream\r\n" + ee.Message);
			}
		}

		void SetupPlaybackGraph(string fname)
		{
			int hr;
			try 
			{
				hr = graphBuilder.RenderFile(fname,null);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				AMMediaType media = new AMMediaType();
				media.majorType	= MediaType.Video;
				media.subType	= MediaSubType.RGB24;
				media.formatType = FormatType.VideoInfo;		// ???
				hr = sampGrabber.SetMediaType( media );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				hr = graphBuilder.AddFilter( baseGrabFlt, "Ds.NET Grabber" );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				hr = graphBuilder.AddFilter(smartTee,"smartTee");
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				IBaseFilter renderer;
				hr = graphBuilder.FindFilterByName("Video Renderer",out renderer);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				IPin inPin;
				IPin srcPin;

				hr = DsUtils.GetPin(renderer,PinDirection.Input,out inPin,0);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				hr = inPin.ConnectedTo(out srcPin);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				hr = srcPin.Disconnect();
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				hr = graphBuilder.RemoveFilter(renderer);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );
				Marshal.ReleaseComObject(renderer);
				Marshal.ReleaseComObject(inPin);

				hr = DsUtils.GetPin(smartTee,PinDirection.Input,out inPin,0);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

                hr = graphBuilder.Connect(srcPin,inPin);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );
				Marshal.ReleaseComObject(srcPin);
				Marshal.ReleaseComObject(inPin);
				srcPin = inPin = null;

				hr = DsUtils.GetPin(smartTee,PinDirection.Output,out srcPin,1);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				// grabber Input
				hr = DsUtils.GetPin(baseGrabFlt,PinDirection.Input,out inPin,0);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				// smartTee -> grabber
				hr = graphBuilder.Connect(srcPin,inPin);
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );
				Marshal.ReleaseComObject(srcPin);
				Marshal.ReleaseComObject(inPin);
				srcPin = inPin = null;

				
				if(preview)
				{
					
					// grabber Input
					hr = DsUtils.GetPin(smartTee,PinDirection.Output,out srcPin,0);
					if( hr < 0 )
						Marshal.ThrowExceptionForHR( hr );

					hr = graphBuilder.Render(srcPin);
					if( hr < 0 )
						Marshal.ThrowExceptionForHR( hr );
					Marshal.ReleaseComObject(srcPin);
					srcPin = null;
				}
				

				media = new AMMediaType();
				hr = sampGrabber.GetConnectedMediaType( media );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );
				if( (media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero) )
					throw new NotSupportedException( "Unknown Grabber Media Format" );

				videoInfoHeader = (VideoInfoHeader) Marshal.PtrToStructure( media.formatPtr, typeof(VideoInfoHeader) );
				Marshal.FreeCoTaskMem( media.formatPtr );
				media.formatPtr = IntPtr.Zero;

				//Modified according to the platform SDK, to capture the buffer
				hr = sampGrabber.SetBufferSamples( false );
				if( hr == 0 )
					hr = sampGrabber.SetOneShot( false );
				if( hr == 0 )
					hr = sampGrabber.SetCallback( sampleGrabber, 1 );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

			}
			catch( Exception ee )
			{
				throw new Exception("Could not setup graph\r\n" + ee.Message);
			}
		}

        private const int WS_CHILD = 0x40000000;	// attributes for video window
        private const int WS_CLIPCHILDREN = 0x02000000;

        public void ResizeVideo(int x, int y, int w, int h)
        {
            if (videoWin != null)
            {
                videoWin.SetWindowPosition(x, y, w, h);
            }
        }

        void SetupVideoWindow(IntPtr owner)
        {
            int hr;
            try
            {
                // Set the video window to be a child of the main window
                hr = videoWin.put_Owner(owner);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                // Set video window style
                hr = videoWin.put_WindowStyle(WS_CHILD | WS_CLIPCHILDREN);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);

                // Use helper function to position video window in client rect of owner window
                //ResizeVideoWindow();

                // Make the video window visible, now that it is properly positioned
                hr = videoWin.put_Visible(DsHlp.OATRUE);
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);
            }
            catch (Exception ee)
            {
                //MessageBox.Show(this, "Could not setup video window\r\n" + ee.Message, "DirectShow.NET", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
		
		void GetFilePlayInterfaces()
		{
			Type comType = null;
			object comObj = null;
			try 
			{
				comType = Type.GetTypeFromCLSID( Clsid.FilterGraph );
				if( comType == null )
					throw new NotImplementedException( @"DirectShow FilterGraph not installed/registered!" );
				comObj = Activator.CreateInstance( comType );
				graphBuilder = (IGraphBuilder) comObj; comObj = null;

				comType = Type.GetTypeFromCLSID( Clsid.SampleGrabber );
				if( comType == null )
					throw new NotImplementedException( @"DirectShow SampleGrabber not installed/registered!" );
				comObj = Activator.CreateInstance( comType );
				sampGrabber = (ISampleGrabber) comObj; comObj = null;

				comType = Type.GetTypeFromCLSID( Clsid.SmartTee );
				if( comType == null )
					throw new NotImplementedException( @"Video Renderer not installed/registered!" );
				comObj = Activator.CreateInstance( comType );
				smartTee = (IBaseFilter) comObj; comObj = null;

				mediaCtrl	= (IMediaControl)	graphBuilder;
				videoWin	= (IVideoWindow)	graphBuilder;
				baseGrabFlt	= (IBaseFilter)		sampGrabber;
			}
			catch( Exception ee )
			{
				throw new Exception("Could not get interfaces\r\n" + ee.Message);
			}
			finally
			{
				if( comObj != null )
					Marshal.ReleaseComObject( comObj ); comObj = null;
			}
		}

		
		private void inputMediaDialog_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
		{
			StartUpFileVideo(inputMediaDialog.FileName);
		}

		#region ISampleProvider Members

		public string Name
		{
			get
			{
				return "File Sample Provider "+version;
			}
		}

		public bool Applicable
		{
			get
			{
				return true;
			}
		}

        public VideoInfoHeader StartProvider(bool preview, ISampleGrabberCB samplegrabber, IntPtr owner)
		{
			this.preview = preview;
            this.sampleGrabber = samplegrabber;

			inputMediaDialog.ShowDialog();

            SetupVideoWindow(owner);
            return videoInfoHeader;
		}

		public void CloseStream()
		{
			try 
			{

				if( mediaCtrl != null )
				{
					mediaCtrl.Stop();
					mediaCtrl = null;
				}

                if (videoWin != null)
                {
                    videoWin.put_Visible(DsHlp.OAFALSE);
                    videoWin.put_Owner(IntPtr.Zero);
                    videoWin = null;
                }

				baseGrabFlt = null;
				if( sampGrabber != null )
					Marshal.ReleaseComObject( sampGrabber ); sampGrabber = null;

				if( smartTee != null )
					Marshal.ReleaseComObject( smartTee ); smartTee = null;

				if( graphBuilder != null )
					Marshal.ReleaseComObject( graphBuilder ); graphBuilder = null;
			}
			catch( Exception )
			{}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			CloseStream();
		}

		#endregion
    }
}
