using System;
using System.Windows;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class TabbedThumbnailEventArgs : EventArgs
	{
		public IntPtr WindowHandle { get; private set; }

		public UIElement WindowsControl { get; private set; }

		public TabbedThumbnailEventArgs(IntPtr windowHandle)
		{
			WindowHandle = windowHandle;
			WindowsControl = null;
		}

		public TabbedThumbnailEventArgs(UIElement windowsControl)
		{
			WindowHandle = IntPtr.Zero;
			WindowsControl = windowsControl;
		}
	}
}
