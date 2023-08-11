using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Taskbar;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("091162a4-bc96-411f-aae8-c5122cd03363")]
	internal interface ISharedBitmap
	{
		void GetSharedBitmap(out IntPtr phbm);

		void GetSize(out CoreNativeMethods.Size pSize);

		void GetFormat(out ThumbnailAlphaType pat);

		void InitializeBitmap([In] IntPtr hbm, [In] ThumbnailAlphaType wtsAT);

		void Detach(out IntPtr phbm);
	}
}
