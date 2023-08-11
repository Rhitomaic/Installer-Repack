using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[ComImport]
	[Guid("e07010ec-bc17-44c0-97b0-46c7c95b9edc")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IExplorerPaneVisibility
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetPaneState(ref Guid explorerPane, out ExplorerPaneState peps);
	}
}
