using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("B63EA76D-1F85-456F-A19C-48159EFA858B")]
	internal interface IShellItemArray
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult BindToHandler([In][MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, out IntPtr ppvOut);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetPropertyStore([In] int Flags, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetPropertyDescriptionList([In] ref PropertyKey keyType, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetAttributes([In] ShellNativeMethods.ShellItemAttributeOptions dwAttribFlags, [In] ShellNativeMethods.ShellFileGetAttributesOptions sfgaoMask, out ShellNativeMethods.ShellFileGetAttributesOptions psfgaoAttribs);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetCount(out uint pdwNumItems);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
	}
}
