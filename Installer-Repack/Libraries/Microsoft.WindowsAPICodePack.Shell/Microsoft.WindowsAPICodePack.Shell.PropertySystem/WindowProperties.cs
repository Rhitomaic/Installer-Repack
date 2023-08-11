using System;
using System.Windows;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	public static class WindowProperties
	{
		public static void SetWindowProperty(IntPtr windowHandle, PropertyKey propKey, string value)
		{
			TaskbarNativeMethods.SetWindowProperty(windowHandle, propKey, value);
		}

		public static void SetWindowProperty(Window window, PropertyKey propKey, string value)
		{
			TaskbarNativeMethods.SetWindowProperty(new WindowInteropHelper(window).Handle, propKey, value);
		}
	}
}
