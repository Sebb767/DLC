using System;
using System.Runtime.InteropServices;

namespace Sebb767.DLC
{
	/// <summary>
	/// Loader for Linux.
	/// </summary>
	internal sealed class LoaderLinux : Loader
	{
	    [DllImport("libdl.so")]
        static extern int dlopen(String fileName, int flags);
        
        [DllImport("libdl.so")]
        static extern IntPtr dlsym(int handle, String symbol);

        [DllImport("libdl.so")]
        static extern int dlclose(int handle);
	    
		internal LoaderLinux(string lib)
			: base(lib)
		{
			hModule = dlopen (lib, 2);

			if (hModule	 == 0)
				throw new ClassLoadingException ("dlopen returned 0 (error) when loading `" + lib + "`.");
		}

		internal override IntPtr getProcedure(string procName)
		{
			if (disposed)
				throw new ObjectDisposedException ("The library is already unloaded!");

			IntPtr r = dlsym (hModule, procName);

			if (r == IntPtr.Zero)
				throw new ClassLoadingException ("dlsym returned 0 (error) when loading `" + lib + "`.");

			return r;
		}

		protected override void Dispose(bool disposing)
		{
			if (this.disposed)
				return;

			base.Dispose (true);

			if (dlclose(hModule) != 0)
				throw new ClassLoadingException ("dlclose failed with `" + lib + "`.");
		}
	}
}

