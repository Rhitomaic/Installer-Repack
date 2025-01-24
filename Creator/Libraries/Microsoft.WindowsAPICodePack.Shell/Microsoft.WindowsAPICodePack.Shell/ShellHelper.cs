using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class ShellHelper
	{
		internal static PropertyKey ItemTypePropertyKey = new PropertyKey(new Guid("28636AA6-953D-11D2-B5D6-00C04FD918D0"), 11);

		internal static string GetParsingName(IShellItem shellItem)
		{
			if (shellItem == null)
			{
				return null;
			}
			string result = null;
			IntPtr ppszName = IntPtr.Zero;
			HResult displayName = shellItem.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.DesktopAbsoluteParsing, out ppszName);
			if (displayName != 0 && displayName != HResult.InvalidArguments)
			{
				throw new ShellException(LocalizedMessages.ShellHelperGetParsingNameFailed, displayName);
			}
			if (ppszName != IntPtr.Zero)
			{
				result = Marshal.PtrToStringAuto(ppszName);
				Marshal.FreeCoTaskMem(ppszName);
				ppszName = IntPtr.Zero;
			}
			return result;
		}

		internal static string GetAbsolutePath(string path)
		{
			if (Uri.IsWellFormedUriString(path, UriKind.Absolute))
			{
				return path;
			}
			return Path.GetFullPath(path);
		}

		internal static string GetItemType(IShellItem2 shellItem)
		{
			if (shellItem != null)
			{
				string ppsz = null;
				if (shellItem.GetString(ref ItemTypePropertyKey, out ppsz) == HResult.Ok)
				{
					return ppsz;
				}
			}
			return null;
		}

		internal static IntPtr PidlFromParsingName(string name)
		{
			IntPtr ppidl;
			ShellNativeMethods.ShellFileGetAttributesOptions psfgaoOut;
			int result = ShellNativeMethods.SHParseDisplayName(name, IntPtr.Zero, out ppidl, (ShellNativeMethods.ShellFileGetAttributesOptions)0, out psfgaoOut);
			return CoreErrorHelper.Succeeded(result) ? ppidl : IntPtr.Zero;
		}

		internal static IntPtr PidlFromShellItem(IShellItem nativeShellItem)
		{
			IntPtr iUnknownForObject = Marshal.GetIUnknownForObject(nativeShellItem);
			return PidlFromUnknown(iUnknownForObject);
		}

		internal static IntPtr PidlFromUnknown(IntPtr unknown)
		{
			IntPtr ppidl;
			int result = ShellNativeMethods.SHGetIDListFromObject(unknown, out ppidl);
			return CoreErrorHelper.Succeeded(result) ? ppidl : IntPtr.Zero;
		}
	}
}
