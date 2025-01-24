using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class StockIconsNativeMethods
	{
		[Flags]
		internal enum StockIconOptions
		{
			Large = 0,
			Small = 1,
			ShellSize = 4,
			Handle = 0x100,
			SystemIndex = 0x4000,
			LinkOverlay = 0x8000,
			Selected = 0x10000
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct StockIconInfo
		{
			internal uint StuctureSize;

			internal IntPtr Handle;

			internal int ImageIndex;

			internal int Identifier;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
			internal string Path;
		}

		[DllImport("Shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		internal static extern HResult SHGetStockIconInfo(StockIconIdentifier identifier, StockIconOptions flags, ref StockIconInfo info);
	}
}
