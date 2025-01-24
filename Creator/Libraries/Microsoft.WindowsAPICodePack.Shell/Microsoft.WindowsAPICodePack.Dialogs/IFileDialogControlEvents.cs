using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	[ComImport]
	[Guid("36116642-D713-4B97-9B83-7484A9D00433")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IFileDialogControlEvents
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnItemSelected([In][MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl, [In] int dwIDItem);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnButtonClicked([In][MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnCheckButtonToggled([In][MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl, [In] bool bChecked);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void OnControlActivating([In][MarshalAs(UnmanagedType.Interface)] IFileDialogCustomize pfdc, [In] int dwIDCtl);
	}
}
