using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[Serializable]
	public class ShellException : ExternalException
	{
		public ShellException()
		{
		}

		internal ShellException(HResult result)
			: this((int)result)
		{
		}

		public ShellException(string message)
			: base(message)
		{
		}

		public ShellException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ShellException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		internal ShellException(string message, HResult errorCode)
			: this(message, (int)errorCode)
		{
		}

		public ShellException(int errorCode)
			: base(LocalizedMessages.ShellExceptionDefaultText, errorCode)
		{
		}

		protected ShellException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
