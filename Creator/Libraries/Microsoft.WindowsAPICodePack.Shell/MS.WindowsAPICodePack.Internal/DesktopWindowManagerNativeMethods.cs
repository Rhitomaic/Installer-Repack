using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.WindowsAPICodePack.Shell;

namespace MS.WindowsAPICodePack.Internal
{
	[SuppressUnmanagedCodeSecurity]
	internal static class DesktopWindowManagerNativeMethods
	{
		[DllImport("DwmApi.dll")]
		internal static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref Margins m);

		[DllImport("DwmApi.dll", PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DwmIsCompositionEnabled();

		[DllImport("DwmApi.dll")]
		internal static extern int DwmEnableComposition(CompositionEnable compositionAction);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowRect(IntPtr hwnd, out NativeRect rect);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetClientRect(IntPtr hwnd, out NativeRect rect);
	}
}
