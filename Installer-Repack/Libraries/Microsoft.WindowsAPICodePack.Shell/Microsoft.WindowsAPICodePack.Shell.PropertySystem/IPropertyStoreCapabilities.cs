using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell.PropertySystem
{
	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c8e2d566-186e-4d49-bf41-6909ead56acc")]
	internal interface IPropertyStoreCapabilities
	{
		HResult IsPropertyWritable([In] ref PropertyKey propertyKey);
	}
}
