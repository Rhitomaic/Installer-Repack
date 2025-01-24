using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	[ComImport]
	[Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IObjectArray
	{
		void GetCount(out uint cObjects);

		void GetAt(uint iIndex, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);
	}
}
