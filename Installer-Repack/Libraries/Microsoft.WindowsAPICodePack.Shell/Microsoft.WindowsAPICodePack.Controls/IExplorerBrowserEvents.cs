using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[ComImport]
	[Guid("361bbdc7-e6ee-4e13-be58-58e2240c810f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IExplorerBrowserEvents
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult OnNavigationPending(IntPtr pidlFolder);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult OnViewCreated([MarshalAs(UnmanagedType.IUnknown)] object psv);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult OnNavigationComplete(IntPtr pidlFolder);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult OnNavigationFailed(IntPtr pidlFolder);
	}
}
