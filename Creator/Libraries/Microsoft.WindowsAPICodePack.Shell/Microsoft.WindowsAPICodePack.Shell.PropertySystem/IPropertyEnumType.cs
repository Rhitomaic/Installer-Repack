using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[ComImport]
	[Guid("11E1FBF9-2D56-4A6B-8DB3-7CD193A471F2")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IPropertyEnumType
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetEnumType(out PropEnumType penumtype);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetValue([Out] PropVariant ppropvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetRangeMinValue([Out] PropVariant ppropvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetRangeSetValue([Out] PropVariant ppropvar);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetDisplayText([MarshalAs(UnmanagedType.LPWStr)] out string ppszDisplay);
	}
}
