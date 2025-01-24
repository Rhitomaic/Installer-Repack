using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	[Serializable]
	public class ApplicationRecoveryException : ExternalException
	{
		public ApplicationRecoveryException()
		{
		}

		public ApplicationRecoveryException(string message)
			: base(message)
		{
		}

		public ApplicationRecoveryException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public ApplicationRecoveryException(string message, int errorCode)
			: base(message, errorCode)
		{
		}

		protected ApplicationRecoveryException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
