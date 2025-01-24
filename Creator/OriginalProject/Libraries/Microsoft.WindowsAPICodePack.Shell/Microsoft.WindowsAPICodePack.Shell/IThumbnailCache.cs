using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("F676C15D-596A-4ce2-8234-33996F445DB1")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IThumbnailCache
	{
		void GetThumbnail([In] IShellItem pShellItem, [In] uint cxyRequestedThumbSize, [In] ShellNativeMethods.ThumbnailOptions flags, out ISharedBitmap ppvThumb, out ShellNativeMethods.ThumbnailCacheOptions pOutFlags, [Out] ShellNativeMethods.ThumbnailId pThumbnailID);

		void GetThumbnailByID([In] ShellNativeMethods.ThumbnailId thumbnailID, [In] uint cxyRequestedThumbSize, out ISharedBitmap ppvThumb, out ShellNativeMethods.ThumbnailCacheOptions pOutFlags);
	}
}
