using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[ComImport]
	[Guid("A99400F4-3D84-4557-94BA-1242FB2CC9A6")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyEnumTypeList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCount(out uint pctypes);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAt([In] uint itype, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyEnumType ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetConditionAt([In] uint index, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void FindMatchingIndex([In] PropVariant propvarCmp, out uint pnIndex);
	}
}
