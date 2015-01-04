using System;

namespace Sebb767.DLC
{
	/// <summary>
	/// Thrown when an error occurs while loading a libary.
	/// </summary>
	public class ClassLoadingException : Exception
	{
		public ClassLoadingException (string message)
			: base(message)
		{
			// empty ... :/
		}
	}
}

