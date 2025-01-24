using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	[ComImport]
	[Guid("5632B1A4-E38A-400A-928A-D4CD63230295")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IObjectCollection
	{
		[PreserveSig]
		void GetCount(out uint cObjects);

		[PreserveSig]
		void GetAt(uint iIndex, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

		void AddObject([MarshalAs(UnmanagedType.Interface)] object pvObject);

		void AddFromArray([MarshalAs(UnmanagedType.Interface)] IObjectArray poaSource);

		void RemoveObject(uint uiIndex);

		void Clear();
	}
}
