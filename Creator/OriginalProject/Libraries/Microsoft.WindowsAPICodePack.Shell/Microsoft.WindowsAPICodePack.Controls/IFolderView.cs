using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("cde725b0-ccc9-4519-917e-325d72fab4ce")]
	internal interface IFolderView
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCurrentViewMode(out uint pViewMode);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetCurrentViewMode(uint ViewMode);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFolder(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Item(int iItemIndex, out IntPtr ppidl);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ItemCount(uint uFlags, out int pcItems);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Items(uint uFlags, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSelectionMarkedItem(out int piItem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFocusedItem(out int piItem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetItemPosition(IntPtr pidl, out NativePoint ppt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSpacing(out NativePoint ppt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetDefaultSpacing(out NativePoint ppt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetAutoArrange();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SelectItem(int iItem, uint dwFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SelectAndPositionItems(uint cidl, IntPtr apidl, ref NativePoint apt, uint dwFlags);
	}
}
