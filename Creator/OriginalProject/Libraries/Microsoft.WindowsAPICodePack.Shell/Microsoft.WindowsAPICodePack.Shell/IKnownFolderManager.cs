using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("8BE2D872-86AA-4d47-B776-32CCA40C7018")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IKnownFolderManager
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void FolderIdFromCsidl(int csidl, out Guid knownFolderID);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void FolderIdToCsidl([In][MarshalAs(UnmanagedType.LPStruct)] Guid id, out int csidl);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFolderIds(out IntPtr folders, out uint count);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult GetFolder([In][MarshalAs(UnmanagedType.LPStruct)] Guid id, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetFolderByName(string canonicalName, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void RegisterFolder([In][MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid, [In] ref KnownFoldersSafeNativeMethods.NativeFolderDefinition knownFolderDefinition);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void UnregisterFolder([In][MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void FindFolderFromPath([In][MarshalAs(UnmanagedType.LPWStr)] string path, [In] int mode, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		HResult FindFolderFromIDList(IntPtr pidl, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Redirect();
	}
}
