using System;

/* Dynamic library(function) Caller
 * 
 * A simple library to call functions in unmanaged code.
 * System independend + lightweight
 * 
 * (c) Sebb767, 2014
 **/
 
namespace Sebb767.DLC
{
	/// <summary>
	/// Class to load functions from unmanaged librarys.
	/// Use one instance per library version.
	/// </summary>
	public sealed class DLC : IDisposable
	{
		public string library {
			get;
			private set;
		}
		private Loader loader; // class to load the library

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Sebb767.DLC.DLC"/> class.
		/// </summary>
		/// <param name="library">The library to load.</param>
		public DLC (string library)
		{
            initialize(library);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sebb767.DLC.DLC"/> class.
		/// </summary>
		/// <param name="libraryWindows">The library to load on windows.</param>
		/// <param name="libraryLinux">The library to load on linux.</param>
	    public DLC (string libraryWindows, string libraryLinux)
		{
			initialize (isLinux () ? libraryLinux : libraryWindows);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Sebb767.DLC.DLC"/> class.
		/// </summary>
		/// <param name="libraryWindows32">The library to load on windows x86.</param>
		/// <param name="libraryWindows64">The library to load on windows x64.</param>
		/// <param name="libraryLinux32">The library to load on linux x86.</param>
		/// <param name="libraryLinux64">The library to load on linux x64.</param>
	    public DLC (string libraryWindows32, string libraryWindows64, string libraryLinux32, string libraryLinux64)
		{
			initialize(x64() ?
				(isLinux () ? libraryLinux64 : libraryWindows64) :
				(isLinux () ? libraryLinux32 : libraryWindows32) );
		}

		#endregion

		/// <summary>
		/// Initializes the class with the given library path.
		/// </summary>
		/// <param name="lib">The actual library.</param>
		private void initialize(string lib)
		{
			this.library = lib;
			this.loader = isLinux() ? (Loader)new LoaderLinux(lib) : (Loader)new LoaderWindows(lib);
		}

		#region dispose pattern

		public void Dispose()
		{
			this.Dispose (true);
			GC.SuppressFinalize (this);
		}

		~DLC ()
		{
			this.Dispose (false);
		}

		private void Dispose(bool disposing)
		{
			if(disposing && loader != null)
			{
				loader.Dispose ();
				loader = null;
			}
		}

		#endregion
		
		#region Helpers

		/// <summary>
		/// Returns wether we're on Linux.
		/// </summary>
		/// <returns><c>true</c>, if running on Linux, <c>false</c> otherwise.</returns>
		public static bool isLinux()
		{
    		int p = (int) Environment.OSVersion.Platform;
            return (p == 4) || (p == 6) || (p == 128);
		}

		/// <summary>
		/// Returns wether we're on an 64 bit OS.
		/// </summary>
		/// <returns><c>true</c>, if running on an 64-bit OS, <c>false</c> otherwise.</returns>
		public static bool x64()
		{
		    return IntPtr.Size == 8;
		}
		
		#endregion
	}
}

