using System;
using Microsoft.WindowsAPICodePack.Shell.Interop;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class WindowMessageEventArgs : EventArgs
	{
		public Message Message { get; private set; }

		internal WindowMessageEventArgs(Message msg)
		{
			Message = msg;
		}
	}
}
