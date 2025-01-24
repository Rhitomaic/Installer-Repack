using System;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class WindowUtilities
	{
		internal static Point GetParentOffsetOfChild(IntPtr hwnd, IntPtr hwndParent)
		{
			NativePoint point = default(NativePoint);
			TabbedThumbnailNativeMethods.ClientToScreen(hwnd, ref point);
			NativePoint point2 = default(NativePoint);
			TabbedThumbnailNativeMethods.ClientToScreen(hwndParent, ref point2);
			return new Point(point.X - point2.X, point.Y - point2.Y);
		}

		internal static Size GetNonClientArea(IntPtr hwnd)
		{
			NativePoint point = default(NativePoint);
			TabbedThumbnailNativeMethods.ClientToScreen(hwnd, ref point);
			NativeRect rect = default(NativeRect);
			TabbedThumbnailNativeMethods.GetWindowRect(hwnd, ref rect);
			return new Size(point.X - rect.Left, point.Y - rect.Top);
		}
	}
}
