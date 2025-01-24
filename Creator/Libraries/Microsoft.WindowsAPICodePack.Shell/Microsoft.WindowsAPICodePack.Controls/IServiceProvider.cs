using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Controls
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6d5140c1-7436-11ce-8034-00aa006009fa")]
	internal interface IServiceProvider
	{
		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall)]
		HResult QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject);
	}
}
