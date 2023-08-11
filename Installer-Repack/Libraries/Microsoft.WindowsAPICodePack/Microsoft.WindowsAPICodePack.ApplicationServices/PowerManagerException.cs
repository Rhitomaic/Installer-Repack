using System;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	[Serializable]
	public class PowerManagerException : Exception
	{
		public PowerManagerException()
		{
		}

		public PowerManagerException(string message)
			: base(message)
		{
		}

		public PowerManagerException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		protected PowerManagerException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
