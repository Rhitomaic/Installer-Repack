using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellItem
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult BindToHandler([In] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetDisplayName([In] ShellNativeMethods.ShellItemDesignNameOptions sigdnName, out IntPtr ppszName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAttributes([In] ShellNativeMethods.ShellFileGetAttributesOptions sfgaoMask, out ShellNativeMethods.ShellFileGetAttributesOptions psfgaoAttribs);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Compare([In][MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] SICHINTF hint, out int piOrder);
	}
}
