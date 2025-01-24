using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[Serializable]
	public class CommonControlException : COMException
	{
		public CommonControlException()
		{
		}

		public CommonControlException(string message)
			: base(message)
		{
		}

		public CommonControlException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public CommonControlException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		internal CommonControlException(string message, HResult errorCode)
			: this(message, (int)errorCode)
		{
		}

		protected CommonControlException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
