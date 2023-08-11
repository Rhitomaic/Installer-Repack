using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MS.WindowsAPICodePack.Internal
{
	internal static class CoreNativeMethods
	{
		public struct Size
		{
			private int width;

			private int height;

			public int Width
			{
				get
				{
					return width;
				}
				set
				{
					width = value;
				}
			}

			public int Height
			{
				get
				{
					return height;
				}
				set
				{
					height = value;
				}
			}
		}

		public delegate int WNDPROC(IntPtr hWnd, uint uMessage, IntPtr wParam, IntPtr lParam);

		internal const int UserMessage = 1024;

		internal const int EnterIdleMessage = 289;

		internal const int FormatMessageFromSystem = 4096;

		internal const uint ResultFailed = 2147500037u;

		internal const uint ResultInvalidArgument = 2147942487u;

		internal const uint ResultFalse = 1u;

		internal const uint ResultNotFound = 2147943568u;

		internal const int DWMNCRP_USEWINDOWSTYLE = 0;

		internal const int DWMNCRP_DISABLED = 1;

		internal const int DWMNCRP_ENABLED = 2;

		internal const int DWMWA_NCRENDERING_ENABLED = 1;

		internal const int DWMWA_NCRENDERING_POLICY = 2;

		internal const int DWMWA_TRANSITIONS_FORCEDISABLED = 3;

		internal const uint StatusAccessDenied = 3221225506u;

		[DllImport("user32.dll", CharSet = CharSet.Auto, PreserveSig = false, SetLastError = true)]
		public static extern void PostMessage(IntPtr windowHandle, WindowMessage message, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, WindowMessage message, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, uint message, IntPtr wparam, IntPtr lparam);

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, uint message, IntPtr wparam, [MarshalAs(UnmanagedType.LPWStr)] string lparam);

		public static IntPtr SendMessage(IntPtr windowHandle, uint message, int wparam, string lparam)
		{
			return SendMessage(windowHandle, message, (IntPtr)wparam, lparam);
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr SendMessage(IntPtr windowHandle, uint message, ref int wparam, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lparam);

		[DllImport("kernel32.dll", BestFitMapping = false, SetLastError = true, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string fileName);

		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteObject(IntPtr graphicsObjectHandle);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int LoadString(IntPtr instanceHandle, int id, StringBuilder buffer, int bufferSize);

		[DllImport("Kernel32.dll")]
		internal static extern IntPtr LocalFree(ref Guid guid);

		[DllImport("user32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DestroyIcon(IntPtr hIcon);

		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		internal static extern int DestroyWindow(IntPtr handle);

		public static int GetHiWord(long value, int size)
		{
			return (short)(value >> size);
		}

		public static int GetLoWord(long value)
		{
			return (short)(value & 0xFFFF);
		}
	}
}
