using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[ComImport]
	[Guid("9B6E051C-5DDD-4321-9070-FE2ACB55E794")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyEnumType2 : IPropertyEnumType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetEnumType(out PropEnumType penumtype);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetValue([Out] PropVariant ppropvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetRangeMinValue([Out] PropVariant ppropvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetRangeSetValue([Out] PropVariant ppropvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		new void GetDisplayText([MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetImageReference([MarshalAs(UnmanagedType.LPWStr)] out string ppszImageRes);
	}
}
