using System;
using System.Runtime.InteropServices;

namespace Sebb767.DLC
{
	/// <summary>
	/// Loader for Windows.
	/// </summary>
	internal sealed class LoaderWindows : Loader
	{
		[DllImport("kernel32.dll")]
		static extern int LoadLibrary(
			[MarshalAs(UnmanagedType.LPStr)] string lpLibFileName);

		[DllImport("kernel32.dll")]
		static extern IntPtr GetProcAddress(int hModule,
			[MarshalAs(UnmanagedType.LPStr)] string procName);

		[DllImport("kernel32.dll")]
		static extern bool FreeLibrary(int hModule);

		internal LoaderWindows(string lib)
			: base(lib)
		{
			hModule = LoadLibrary (lib);

			if (hModule == 0)
				throw new ClassLoadingException ("LoadLibrary returned 0 (error) when loading `" + lib + "`.");

		}

		internal override IntPtr getProcedure(string procName)
		{
			if (disposed)
				throw new ObjectDisposedException ("The library is already unloaded!");

			IntPtr r = GetProcAddress (hModule, procName);

			if (r == IntPtr.Zero)
				throw new ClassLoadingException ("GetProcAddress returned 0 (error) when loading `" + lib + "`.");

			return r;
		}

		protected override void Dispose(bool Disposing)
		{
			if (this.disposed)
				return;

			base.Dispose (true);

			if (!FreeLibrary(hModule))
				throw new ClassLoadingException ("FreeLibrary failed with `" + lib + "`.");
		}
	}
}

