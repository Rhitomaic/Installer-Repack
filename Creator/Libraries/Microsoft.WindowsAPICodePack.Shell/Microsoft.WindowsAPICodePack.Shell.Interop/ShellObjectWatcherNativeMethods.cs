using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.Interop
{
	internal static class ShellObjectWatcherNativeMethods
	{
		public delegate int WndProcDelegate(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam);

		[DllImport("Ole32.dll")]
		public static extern HResult CreateBindCtx(int reserved, out IBindCtx bindCtx);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern uint RegisterClassEx(ref WindowClassEx windowClass);

		[DllImport("User32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern IntPtr CreateWindowEx(int extendedStyle, [MarshalAs(UnmanagedType.LPWStr)] string className, [MarshalAs(UnmanagedType.LPWStr)] string windowName, int style, int x, int y, int width, int height, IntPtr parentHandle, IntPtr menuHandle, IntPtr instanceHandle, IntPtr additionalData);

		[DllImport("User32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetMessage(out Message message, IntPtr windowHandle, uint filterMinMessage, uint filterMaxMessage);

		[DllImport("User32.dll")]
		public static extern int DefWindowProc(IntPtr hwnd, uint msg, IntPtr wparam, IntPtr lparam);

		[DllImport("User32.dll")]
		public static extern void DispatchMessage([In] ref Message message);
	}
}
