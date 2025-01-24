using System;
using System.Windows;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class TabbedThumbnailClosedEventArgs : TabbedThumbnailEventArgs
	{
		public bool Cancel { get; set; }

		public TabbedThumbnailClosedEventArgs(IntPtr windowHandle)
			: base(windowHandle)
		{
		}

		public TabbedThumbnailClosedEventArgs(UIElement windowsControl)
			: base(windowsControl)
		{
		}
	}
}
