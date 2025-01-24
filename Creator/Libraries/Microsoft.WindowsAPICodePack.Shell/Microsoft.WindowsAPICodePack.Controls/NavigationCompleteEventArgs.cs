using System;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Controls
{
	public class NavigationCompleteEventArgs : EventArgs
	{
		public ShellObject NewLocation { get; set; }
	}
}
