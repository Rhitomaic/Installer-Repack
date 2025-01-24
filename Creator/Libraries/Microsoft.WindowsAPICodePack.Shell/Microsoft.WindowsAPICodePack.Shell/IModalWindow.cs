using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B4DB1657-70D7-485E-8E3E-6FCB5A5C1802")]
	internal interface IModalWindow
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		int Show([In] IntPtr parent);
	}
}
