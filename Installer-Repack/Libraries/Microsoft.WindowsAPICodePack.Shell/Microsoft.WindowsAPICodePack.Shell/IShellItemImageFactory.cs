using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("bcc18b79-ba16-442f-80c4-8a59c30c463b")]
	internal interface IShellItemImageFactory
	{
		[PreserveSig]
		HResult GetImage([In][MarshalAs(UnmanagedType.Struct)] CoreNativeMethods.Size size, [In] ShellNativeMethods.SIIGBF flags, out IntPtr phbm);
	}
}
