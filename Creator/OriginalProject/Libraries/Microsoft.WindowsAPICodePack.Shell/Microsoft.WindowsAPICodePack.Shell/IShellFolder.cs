using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("000214E6-0000-0000-C000-000000000046")]
	[ComConversionLoss]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IShellFolder
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ParseDisplayName(IntPtr hwnd, [In][MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In][MarshalAs(UnmanagedType.LPWStr)] string pszDisplayName, [In][Out] ref uint pchEaten, [Out] IntPtr ppidl, [In][Out] ref uint pdwAttributes);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult EnumObjects([In] IntPtr hwnd, [In] ShellNativeMethods.ShellFolderEnumerationOptions grfFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumIDList ppenumIDList);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult BindToObject([In] IntPtr pidl, IntPtr pbc, [In] ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellFolder ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void BindToStorage([In] ref IntPtr pidl, [In][MarshalAs(UnmanagedType.Interface)] IBindCtx pbc, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CompareIDs([In] IntPtr lParam, [In] ref IntPtr pidl1, [In] ref IntPtr pidl2);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void CreateViewObject([In] IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAttributesOf([In] uint cidl, [In] IntPtr apidl, [In][Out] ref uint rgfInOut);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetUIObjectOf([In] IntPtr hwndOwner, [In] uint cidl, [In] IntPtr apidl, [In] ref Guid riid, [In][Out] ref uint rgfReserved, out IntPtr ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetDisplayNameOf([In] ref IntPtr pidl, [In] uint uFlags, out IntPtr pName);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetNameOf([In] IntPtr hwnd, [In] ref IntPtr pidl, [In][MarshalAs(UnmanagedType.LPWStr)] string pszName, [In] uint uFlags, [Out] IntPtr ppidlOut);
	}
}
