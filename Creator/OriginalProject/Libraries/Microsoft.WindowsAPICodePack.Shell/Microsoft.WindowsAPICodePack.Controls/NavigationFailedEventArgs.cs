using System;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Controls
{
	public class NavigationFailedEventArgs : EventArgs
	{
		public ShellObject FailedLocation { get; set; }
	}
}
