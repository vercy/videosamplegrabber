using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using DSSampler;
using DSSampler.DShowNET;
using DSSampler.DShowNET.Device;

namespace Providers.General
{
	/// <summary>
	/// Summary description for CaptureSampleProvider.
	/// </summary>
	public class CaptureProvider : ISampleProvider//,ISampleGrabberCB
	{
		public const string	version	= "1.2.1";

		/// <summary> base filter of the actually used video devices. </summary>
		private IBaseFilter				capFilter;

		/// <summary> graph builder interface. </summary>
		private IGraphBuilder			graphBuilder;

		/// <summary> capture graph builder interface. </summary>
		private ICaptureGraphBuilder2	capGraph;
		private ISampleGrabber			sampGrabber;

		/// <summary> control interface. </summary>
		private IMediaControl			mediaCtrl;

		/// <summary> video window interface. </summary>
		private IVideoWindow			videoWin;

		/// <summary> grabber filter interface. </summary>
		private IBaseFilter				baseGrabFlt;

		/// <summary> list of installed video devices. </summary>
		private ArrayList				capDevices;

		public CaptureProvider()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary> build the capture graph for grabber. </summary>
		/*
		 * Ha nem akar magától felépülni, szükség lehet egy manuális beszúrásra.
		 * Ez pl a gyártónak jó, hogy ne menjen a cucc, csak az õ programjával. LoL
		 * Ekkor minden kamerához ki kell választani a megfelelõ, rejtett splittert,
		 * és utána újra próbálkozni. Ekkor már menni fog. Érdekes "védelem".
		 */
		VideoInfoHeader SetupGraph(bool preview,ISampleGrabberCB samplegrabber)
		{
			int hr;
			try 
			{
				hr = capGraph.SetFiltergraph( graphBuilder );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				hr = graphBuilder.AddFilter( capFilter, "Ds.NET Video Capture Device" );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				//DsUtils.ShowCapPinDialog( capGraph, capFilter, IntPtr.Zero );

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

				Guid cat;
				Guid med;
				if(preview)
				{
					cat = PinCategory.Preview;
					med = MediaType.Video;
					hr = capGraph.RenderStream( ref cat, ref med, capFilter, null, null ); // preview 
					if( hr < 0 )
						Marshal.ThrowExceptionForHR( hr );
				}
				
				cat = PinCategory.Capture;
				med = MediaType.Video;
				hr = capGraph.RenderStream( ref cat, ref med, capFilter, null, baseGrabFlt ); // baseGrabFlt 

				// Ha itt száll el gáz van!!!
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

				media = new AMMediaType();
				hr = sampGrabber.GetConnectedMediaType( media );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );
				if( (media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero) )
					throw new NotSupportedException( "Unknown Grabber Media Format" );

				VideoInfoHeader videoInfoHeader = (VideoInfoHeader) Marshal.PtrToStructure( media.formatPtr, typeof(VideoInfoHeader) );
				Marshal.FreeCoTaskMem( media.formatPtr );
				media.formatPtr = IntPtr.Zero;

                //Modified according to the platform SDK, to capture the buffer
				hr = sampGrabber.SetBufferSamples( false );
				if( hr == 0 )
					hr = sampGrabber.SetOneShot( false );
				if( hr == 0 )
					hr = sampGrabber.SetCallback( samplegrabber, 1 );
				if( hr < 0 )
					Marshal.ThrowExceptionForHR( hr );

                //this is needed for external processing
                return videoInfoHeader;
			}
			catch( Exception ee )
			{
				throw new Exception("Could not setup graph\r\n" + ee.Message);
			}
		}

		/// <summary> create the used COM components and get the interfaces. </summary>
		void GetInterfaces()
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

				Guid clsid = Clsid.CaptureGraphBuilder2;
				Guid riid = typeof(ICaptureGraphBuilder2).GUID;
				comObj = DsBugWO.CreateDsInstance( ref clsid, ref riid );
				capGraph = (ICaptureGraphBuilder2) comObj; comObj = null;

				comType = Type.GetTypeFromCLSID( Clsid.SampleGrabber );
				if( comType == null )
					throw new NotImplementedException( @"DirectShow SampleGrabber not installed/registered!" );
				comObj = Activator.CreateInstance( comType );
				sampGrabber = (ISampleGrabber) comObj; comObj = null;

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

		void CreateCaptureDevice( UCOMIMoniker mon )
		{
			object capObj = null;
			try 
			{
				Guid gbf = typeof( IBaseFilter ).GUID;
				mon.BindToObject( null, null, ref gbf, out capObj );
				capFilter = (IBaseFilter) capObj;
                capObj = null;
			}
			catch( Exception ee )
			{
				throw new Exception("Could not create capture device\r\n" + ee.Message);
			}
			finally
			{
				if( capObj != null )
					Marshal.ReleaseComObject( capObj ); capObj = null;
			}

		}

		public void CloseInterfaces()
		{
			int hr;
			try 
			{

				if( mediaCtrl != null )
				{
					hr = mediaCtrl.Stop();
					mediaCtrl = null;
				}

                if (videoWin != null)
                {
                    hr = videoWin.put_Visible(DsHlp.OAFALSE);
                    hr = videoWin.put_Owner(IntPtr.Zero);
                    videoWin = null;
                }

				baseGrabFlt = null;
				if( sampGrabber != null )
					Marshal.ReleaseComObject( sampGrabber ); sampGrabber = null;

				if( capGraph != null )
					Marshal.ReleaseComObject( capGraph ); capGraph = null;

				if( graphBuilder != null )
					Marshal.ReleaseComObject( graphBuilder ); graphBuilder = null;

				if( capFilter != null )
					Marshal.ReleaseComObject( capFilter ); capFilter = null;
			
				if( capDevices != null )
				{
					foreach( DsDevice d in capDevices )
						d.Dispose();
					capDevices = null;
				}
			}
			catch( Exception )
			{}
		}

		#region ISampleProvider Members

		public string Name
		{
			get
			{
				return "General Sample Provider "+version;
			}
		}

		public bool Applicable
		{
			get
			{
				if( ! DsUtils.IsCorrectDirectXVersion() )
					return false;

				if( ! DsDev.GetDevicesOfCat( FilterCategory.VideoInputDevice, out capDevices ) )
					return false;

                // At least one capture device is present
                return capDevices.Count > 0;
			}
		}

        public VideoInfoHeader StartProvider(bool preview, ISampleGrabberCB samplegrabber, IntPtr owner)
		{
			int hr;
            VideoInfoHeader rv;
            try
            {
                if (!DsUtils.IsCorrectDirectXVersion())
                    throw new Exception("At least DirectX 8.1 is required!");

                if (!DsDev.GetDevicesOfCat(FilterCategory.VideoInputDevice, out capDevices))
                    throw new Exception("Could not retrieve the list of capture devices!");

                if (capDevices.Count == 0)
                    throw new Exception("Capture Sample Provider is not applicable!");

                DsDevice device = null;
                if (capDevices.Count == 1)
                    device = capDevices[0] as DsDevice;
                else
                {
                    DeviceSelector selector = new DeviceSelector(capDevices);
                    selector.ShowDialog();
                    device = selector.SelectedDevice;
                }

                if (device == null)
                    throw new Exception("No video capture device could be selected!");


                CreateCaptureDevice(device.Mon);

                GetInterfaces();

                rv = SetupGraph(preview, samplegrabber);

                SetupVideoWindow(owner);

                hr = mediaCtrl.Run();
                if (hr < 0)
                    Marshal.ThrowExceptionForHR(hr);
            }
            catch (Exception ee)
            {
                throw new Exception("Could not start video stream\r\n" + ee.Message);
            }
            return rv;
		}

		public void CloseStream()
		{
			int hr;
			try 
			{

				if( mediaCtrl != null )
				{
					hr = mediaCtrl.Stop();
					mediaCtrl = null;
				}

				baseGrabFlt = null;
				if( sampGrabber != null )
					Marshal.ReleaseComObject( sampGrabber ); sampGrabber = null;

				if( capGraph != null )
					Marshal.ReleaseComObject( capGraph ); capGraph = null;

				if( graphBuilder != null )
					Marshal.ReleaseComObject( graphBuilder ); graphBuilder = null;

				if( capFilter != null )
					Marshal.ReleaseComObject( capFilter ); capFilter = null;
			
				if( capDevices != null )
				{
					foreach( DsDevice d in capDevices )
						d.Dispose();
					capDevices = null;
				}
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
