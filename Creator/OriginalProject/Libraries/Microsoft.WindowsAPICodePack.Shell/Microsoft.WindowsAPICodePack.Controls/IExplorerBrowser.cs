using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("DFD3B6B5-C10C-4BE9-85F6-A66969F402F6")]
	internal interface IExplorerBrowser
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Initialize(IntPtr hwndParent, [In] ref NativeRect prc, [In] FolderSettings pfs);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Destroy();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetRect([In][Out] ref IntPtr phdwp, NativeRect rcBrowser);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetPropertyBag([MarshalAs(UnmanagedType.LPWStr)] string pszPropertyBag);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetEmptyText([MarshalAs(UnmanagedType.LPWStr)] string pszEmptyText);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult SetFolderSettings(FolderSettings pfs);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Advise(IntPtr psbe, out uint pdwCookie);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult Unadvise([In] uint dwCookie);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetOptions([In] ExplorerBrowserOptions dwFlag);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetOptions(out ExplorerBrowserOptions pdwFlag);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void BrowseToIDList(IntPtr pidl, uint uFlags);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult BrowseToObject([MarshalAs(UnmanagedType.IUnknown)] object punk, uint uFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void FillFromObject([MarshalAs(UnmanagedType.IUnknown)] object punk, int dwFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RemoveAll();

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetCurrentView(ref Guid riid, out IntPtr ppv);
	}
}
