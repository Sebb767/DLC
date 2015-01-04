using System;

/* Dynamic library(function) Caller
 * 
 * A simple library to call functions in unmanaged code.
 * System independend + lightweight
 * 
 * (c) Sebb767, 2014
 **/
using System.Runtime.InteropServices;
 
namespace Sebb767.DLC
{
	/// <summary>
	/// Class to load functions from unmanaged libraries.
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

		/// <summary>
		/// Gets a pointer to the procedure by name.
		/// </summary>
		/// <returns>A IntPtr to the procedure.</returns>
		/// <param name="name">The procedures name.</param>
		public IntPtr getProc(string name)
		{
			return loader.getProcedure (name);
		}

		/// <summary>
		/// Returns a delegate to call the native function.
		/// </summary>
		/// <returns>An instance of the given delegate type.</returns>
		/// <param name="name">The procedures name.</param>
		/// <param name="del">The type of your delegate. Use typeof(myFunctionDelegate) .</param>
		public Delegate getProcDelegate(string name, Type del)
		{
			return Marshal.GetDelegateForFunctionPointer (loader.getProcedure (name), del);
		}

		#region dispose pattern

		/// <summary>
		/// Releases all resource used by the <see cref="Sebb767.DLC.DLC"/> object and unloads the corresponding library.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="Sebb767.DLC.DLC"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="Sebb767.DLC.DLC"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="Sebb767.DLC.DLC"/> so the garbage
		/// collector can reclaim the memory that the <see cref="Sebb767.DLC.DLC"/> was occupying.</remarks>
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

