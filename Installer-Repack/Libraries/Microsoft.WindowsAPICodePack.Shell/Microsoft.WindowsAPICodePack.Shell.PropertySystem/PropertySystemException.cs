using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[Serializable]
	public class PropertySystemException : ExternalException
	{
		public PropertySystemException()
		{
		}

		public PropertySystemException(string message)
			: base(message)
		{
		}

		public PropertySystemException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public PropertySystemException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		protected PropertySystemException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
