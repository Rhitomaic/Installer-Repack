using System;
using System.Windows;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class ThumbnailButtonClickedEventArgs : EventArgs
	{
		public IntPtr WindowHandle { get; private set; }

		public UIElement WindowsControl { get; private set; }

		public ThumbnailToolBarButton ThumbnailButton { get; private set; }

		public ThumbnailButtonClickedEventArgs(IntPtr windowHandle, ThumbnailToolBarButton button)
		{
			ThumbnailButton = button;
			WindowHandle = windowHandle;
			WindowsControl = null;
		}

		public ThumbnailButtonClickedEventArgs(UIElement windowsControl, ThumbnailToolBarButton button)
		{
			ThumbnailButton = button;
			WindowHandle = IntPtr.Zero;
			WindowsControl = windowsControl;
		}
	}
}
