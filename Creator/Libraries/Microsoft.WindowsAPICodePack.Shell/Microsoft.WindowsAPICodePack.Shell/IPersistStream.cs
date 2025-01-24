using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("00000109-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPersistStream
	{
		[PreserveSig]
		void GetClassID(out Guid pClassID);

		[PreserveSig]
		HResult IsDirty();

		[PreserveSig]
		HResult Load([In][MarshalAs(UnmanagedType.Interface)] IStream stm);

		[PreserveSig]
		HResult Save([In][MarshalAs(UnmanagedType.Interface)] IStream stm, bool fRemember);

		[PreserveSig]
		HResult GetSizeMax(out ulong cbSize);
	}
}
