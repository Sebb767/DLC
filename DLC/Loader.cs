using System;

namespace Sebb767.DLC
{
	/* provides an interface for the system-dependent loader classes
	 * 
	 * Why an abstract class with funny methods?
	 * 
	 * At first I wanted to do this with an interface, but since
	 * these methods were static, an itf didn't work. Static in-
	 * heritance does not work with classes either, so I had to 
	 * create a non-static class (integration with the first class
	 * isn't possible because the system-dependend classes must 
	 * only be loaded on their specific platform). Since this
	 * class is created with load and disposed with unload, I
	 * converted this functions to ctor/dispose respectively.
	 * A proper dispose pattern had to be implemented to prevent
	 * library garbage from skyrocketing the memory usage.
	 **/
	internal abstract class Loader : IDisposable
	{
		protected string lib;
		protected int hModule; 
		protected bool disposed; // = false

		internal Loader(string Library) // = load
		{
			this.lib = Library;
		}

		internal abstract IntPtr getProcedure (string procName);

		protected virtual void Dispose (bool disposing) // = unload (override)
		{
			this.disposed = true;
		}

		internal virtual void Dispose()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}

		~Loader ()
		{
			Dispose (false);
		}
	}
}

