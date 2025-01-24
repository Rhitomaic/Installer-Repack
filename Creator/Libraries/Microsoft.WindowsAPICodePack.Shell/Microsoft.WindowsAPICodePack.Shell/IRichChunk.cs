using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("4FDEF69C-DBC9-454e-9910-B34F3C64B510")]
	internal interface IRichChunk
	{
		[PreserveSig]
		HResult GetData();
	}
}
