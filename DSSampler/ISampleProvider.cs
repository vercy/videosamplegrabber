using System;

using DSSampler.DShowNET;

namespace DSSampler
{
	/// <summary>
	/// ISampleProvider is an interface for objects that wish to
	/// provide samples from a stream of pictures.
	/// </summary>
	public interface ISampleProvider: IDisposable
	{
		/// <summary>
		/// The name of the SampleProvider. This method should
		/// return a displyable name for it might be displayed
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Checks if the provider can be initialized on the curent
		/// machine. This is crucial, because SampleProviders may
		/// have special hardware requirements. This method should
		/// check all of the necessary components' availability.
		/// </summary>
		/// <returns>
		/// A single boolean indicating if the provider meets all requirements
		/// </returns>
		bool Applicable
		{
			get;
		}

		/// <summary>
		/// Starts the Sample provider. This usually consists of building
		/// a filtergraph, and running the associated media content.
		/// Notice that starting the stream may require additional user
		/// interaction depending on the actuall provider.
		/// </summary>
		/// /// <param name="preview">indicates if a preview should be rendered</param>
        VideoInfoHeader StartProvider(bool preview, ISampleGrabberCB sampleGrabber, IntPtr owner);

        /// <summary>
        /// This part of the interface is quite a mess. This method is used for resizing
        /// the preview of the provider. However there is no such criteria that a provider
        /// must support previews, the current design allow only such providers.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="w">The width of the window</param>
        /// <param name="h">The height of the window</param>
        void ResizeVideo(int x, int y, int w, int h);

		/// <summary>
		/// Stops and closes the stream. The reusability feature will not
        /// be added, until the architecture has been proven to be usable.
		/// </summary>
		void CloseStream();
	}
}
