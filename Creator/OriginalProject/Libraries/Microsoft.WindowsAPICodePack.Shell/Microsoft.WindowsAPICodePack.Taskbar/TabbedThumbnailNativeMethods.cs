using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal static class TabbedThumbnailNativeMethods
	{
		internal const int DisplayFrame = 1;

		internal const int ForceIconicRepresentation = 7;

		internal const int HasIconicBitmap = 10;

		internal const uint WmDwmSendIconicThumbnail = 803u;

		internal const uint WmDwmSendIconicLivePreviewBitmap = 806u;

		internal const uint WaActive = 1u;

		internal const uint WaClickActive = 2u;

		internal const int ScClose = 61536;

		internal const int ScMaximize = 61488;

		internal const int ScMinimize = 61472;

		internal const uint MsgfltAdd = 1u;

		internal const uint MsgfltRemove = 2u;

		[DllImport("dwmapi.dll")]
		internal static extern int DwmSetIconicThumbnail(IntPtr hwnd, IntPtr hbitmap, uint flags);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmInvalidateIconicBitmaps(IntPtr hwnd);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbitmap, ref NativePoint ptClient, uint flags);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmSetIconicLivePreviewBitmap(IntPtr hwnd, IntPtr hbitmap, IntPtr ptClient, uint flags);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmSetWindowAttribute(IntPtr hwnd, uint dwAttributeToSet, IntPtr pvAttributeValue, uint cbAttribute);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowRect(IntPtr hwnd, ref NativeRect rect);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetClientRect(IntPtr hwnd, ref NativeRect rect);

		internal static bool GetClientSize(IntPtr hwnd, out Size size)
		{
			NativeRect rect = default(NativeRect);
			if (!GetClientRect(hwnd, ref rect))
			{
				size = new Size(-1, -1);
				return false;
			}
			size = new Size(rect.Right, rect.Bottom);
			return true;
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ClientToScreen(IntPtr hwnd, ref NativePoint point);

		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool StretchBlt(IntPtr hDestDC, int destX, int destY, int destWidth, int destHeight, IntPtr hSrcDC, int srcX, int srcY, int srcWidth, int srcHeight, uint operation);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetWindowDC(IntPtr hwnd);

		[DllImport("user32.dll")]
		internal static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr ChangeWindowMessageFilter(uint message, uint dwFlag);

		internal static void SetIconicThumbnail(IntPtr hwnd, IntPtr hBitmap)
		{
			int num = DwmSetIconicThumbnail(hwnd, hBitmap, 1u);
			if (num != 0)
			{
				throw Marshal.GetExceptionForHR(num);
			}
		}

		internal static void SetPeekBitmap(IntPtr hwnd, IntPtr bitmap, bool displayFrame)
		{
			int num = DwmSetIconicLivePreviewBitmap(hwnd, bitmap, IntPtr.Zero, displayFrame ? 1u : 0u);
			if (num != 0)
			{
				throw Marshal.GetExceptionForHR(num);
			}
		}

		internal static void SetPeekBitmap(IntPtr hwnd, IntPtr bitmap, Point offset, bool displayFrame)
		{
			NativePoint ptClient = new NativePoint(offset.X, offset.Y);
			int num = DwmSetIconicLivePreviewBitmap(hwnd, bitmap, ref ptClient, displayFrame ? 1u : 0u);
			if (num != 0)
			{
				Exception exceptionForHR = Marshal.GetExceptionForHR(num);
				if (!(exceptionForHR is ArgumentException))
				{
					throw exceptionForHR;
				}
			}
		}

		internal static void EnableCustomWindowPreview(IntPtr hwnd, bool enable)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(4);
			Marshal.WriteInt32(intPtr, enable ? 1 : 0);
			try
			{
				int num = DwmSetWindowAttribute(hwnd, 10u, intPtr, 4u);
				if (num != 0)
				{
					throw Marshal.GetExceptionForHR(num);
				}
				num = DwmSetWindowAttribute(hwnd, 7u, intPtr, 4u);
				if (num != 0)
				{
					throw Marshal.GetExceptionForHR(num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}
	}
}
