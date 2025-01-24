using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	[ComImport]
	[Guid("6332DEBF-87B5-4670-90C0-5E57B408A49E")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface ICustomDestinationList
	{
		void SetAppID([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		[PreserveSig]
		HResult BeginList(out uint cMaxSlots, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

		[PreserveSig]
		HResult AppendCategory([MarshalAs(UnmanagedType.LPWStr)] string pszCategory, [MarshalAs(UnmanagedType.Interface)] IObjectArray poa);

		void AppendKnownCategory([MarshalAs(UnmanagedType.I4)] KnownDestinationCategory category);

		[PreserveSig]
		HResult AddUserTasks([MarshalAs(UnmanagedType.Interface)] IObjectArray poa);

		void CommitList();

		void GetRemovedDestinations(ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppvObject);

		void DeleteList([MarshalAs(UnmanagedType.LPWStr)] string pszAppID);

		void AbortList();
	}
}
