using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("000214F2-0000-0000-C000-000000000046")]
	internal interface IEnumIDList
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Next(uint celt, out IntPtr rgelt, out uint pceltFetched);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Skip([In] uint celt);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Reset();

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Clone([MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenum);
	}
}
