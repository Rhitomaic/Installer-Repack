using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
	internal struct ThumbButton
	{
		internal const int Clicked = 6144;

		[MarshalAs(UnmanagedType.U4)]
		internal ThumbButtonMask Mask;

		internal uint Id;

		internal uint Bitmap;

		internal IntPtr Icon;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
		internal string Tip;

		[MarshalAs(UnmanagedType.U4)]
		internal ThumbButtonOptions Flags;
	}
}
