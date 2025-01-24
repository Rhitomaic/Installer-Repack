using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[ComImport]
	[Guid("1F9FC1D0-C39B-4B26-817F-011967D3440E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyDescriptionList
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCount(out uint pcElem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAt([In] uint iElem, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IPropertyDescription ppv);
	}
}
