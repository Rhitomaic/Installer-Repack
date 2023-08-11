using System;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Controls
{
	public class NavigationPendingEventArgs : EventArgs
	{
		public ShellObject PendingLocation { get; set; }

		public bool Cancel { get; set; }
	}
}
