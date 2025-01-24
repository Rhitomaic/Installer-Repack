using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("00000100-0000-0000-C000-000000000046")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IEnumUnknown
	{
		[PreserveSig]
		HResult Next(uint requestedNumber, ref IntPtr buffer, ref uint fetchedNumber);

		[PreserveSig]
		HResult Skip(uint number);

		[PreserveSig]
		HResult Reset();

		[PreserveSig]
		HResult Clone(out IEnumUnknown result);
	}
}
