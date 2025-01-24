using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1af3a467-214f-4298-908e-06b03e0b39f9")]
	internal interface IFolderView2 : IFolderView
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new HResult GetCurrentViewMode(out uint pViewMode);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void SetCurrentViewMode(uint ViewMode);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetFolder(ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void Item(int iItemIndex, out IntPtr ppidl);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new HResult ItemCount(uint uFlags, out int pcItems);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new HResult Items(uint uFlags, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetSelectionMarkedItem(out int piItem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetFocusedItem(out int piItem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetItemPosition(IntPtr pidl, out NativePoint ppt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetSpacing(out NativePoint ppt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetDefaultSpacing(out NativePoint ppt);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetAutoArrange();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void SelectItem(int iItem, uint dwFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void SelectAndPositionItems(uint cidl, IntPtr apidl, ref NativePoint apt, uint dwFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetGroupBy(IntPtr key, bool fAscending);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetGroupBy(ref IntPtr pkey, ref bool pfAscending);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetViewProperty(IntPtr pidl, IntPtr propkey, object propvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetViewProperty(IntPtr pidl, IntPtr propkey, out object ppropvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetTileViewProperties(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetExtendedTileViewProperties(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] string pszPropList);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetText(int iType, [MarshalAs(UnmanagedType.LPWStr)] string pwszText);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetCurrentFolderFlags(uint dwMask, uint dwFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetCurrentFolderFlags(out uint pdwFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSortColumnCount(out int pcColumns);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetSortColumns(IntPtr rgSortColumns, int cColumns);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSortColumns(out IntPtr rgSortColumns, int cColumns);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetItem(int iItem, ref Guid riid, [MarshalAs(UnmanagedType.IUnknown)] out object ppv);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetVisibleItem(int iStart, bool fPrevious, out int piItem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSelectedItem(int iStart, out int piItem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSelection(bool fNoneImpliesFolder, out IShellItemArray ppsia);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetSelectionState(IntPtr pidl, out uint pdwFlags);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void InvokeVerbOnSelection([In][MarshalAs(UnmanagedType.LPWStr)] string pszVerb);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult SetViewModeAndIconSize(int uViewMode, int iImageSize);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetViewModeAndIconSize(out int puViewMode, out int piImageSize);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetGroupSubsetCount(uint cVisibleRows);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetGroupSubsetCount(out uint pcVisibleRows);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetRedraw(bool fRedrawOn);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void IsMoveInSameFolder();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void DoRename();
	}
}
