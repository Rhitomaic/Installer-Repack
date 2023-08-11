using System;
using System.Windows;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class TabbedThumbnailBitmapRequestedEventArgs : TabbedThumbnailEventArgs
	{
		public bool Handled { get; set; }

		public TabbedThumbnailBitmapRequestedEventArgs(IntPtr windowHandle)
			: base(windowHandle)
		{
		}

		public TabbedThumbnailBitmapRequestedEventArgs(UIElement windowsControl)
			: base(windowsControl)
		{
		}
	}
}
