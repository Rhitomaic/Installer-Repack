using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("68284fAA-6A48-11D0-8c78-00C04fd918b4")]
	internal interface IInputObject
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult UIActivateIO(bool fActivate, ref Message pMsg);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult HasFocusIO();

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult TranslateAcceleratorIO(ref Message pMsg);
	}
}
