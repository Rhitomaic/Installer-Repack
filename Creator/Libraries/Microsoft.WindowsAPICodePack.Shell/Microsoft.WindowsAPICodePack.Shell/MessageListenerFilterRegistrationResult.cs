using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class MessageListenerFilterRegistrationResult
	{
		public IntPtr WindowHandle { get; private set; }

		public uint Message { get; private set; }

		internal MessageListenerFilterRegistrationResult(IntPtr handle, uint msg)
		{
			WindowHandle = handle;
			Message = msg;
		}
	}
}
