#define DEBUG
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class ShellObjectFactory
	{
		internal static ShellObject Create(IShellItem nativeShellItem)
		{
			Debug.Assert(nativeShellItem != null, "nativeShellItem should not be null");
			if (!CoreHelpers.RunningOnVista)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.ShellObjectFactoryPlatformNotSupported);
			}
			IShellItem2 shellItem = nativeShellItem as IShellItem2;
			string text = ShellHelper.GetItemType(shellItem);
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToUpperInvariant();
			}
			shellItem.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.Folder | ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem, out var psfgaoAttribs);
			bool flag = (psfgaoAttribs & ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem) != 0;
			bool flag2 = (psfgaoAttribs & ShellNativeMethods.ShellFileGetAttributesOptions.Folder) != 0;
			ShellLibrary shellLibrary = null;
			if (text == ".lnk")
			{
				return new ShellLink(shellItem);
			}
			if (flag2)
			{
				if (text == ".library-ms" && (shellLibrary = ShellLibrary.FromShellItem(shellItem, true)) != null)
				{
					return shellLibrary;
				}
				if (text == ".searchconnector-ms")
				{
					return new ShellSearchConnector(shellItem);
				}
				if (text == ".search-ms")
				{
					return new ShellSavedSearchCollection(shellItem);
				}
				if (flag)
				{
					if (!IsVirtualKnownFolder(shellItem))
					{
						return new FileSystemKnownFolder(shellItem);
					}
					return new ShellFileSystemFolder(shellItem);
				}
				if (IsVirtualKnownFolder(shellItem))
				{
					return new NonFileSystemKnownFolder(shellItem);
				}
				return new ShellNonFileSystemFolder(shellItem);
			}
			if (flag)
			{
				return new ShellFile(shellItem);
			}
			return new ShellNonFileSystemItem(shellItem);
		}

		private static bool IsVirtualKnownFolder(IShellItem2 nativeShellItem2)
		{
			IntPtr pidl = IntPtr.Zero;
			try
			{
				IKnownFolderNative nativeFolder = null;
				KnownFoldersSafeNativeMethods.NativeFolderDefinition definition = default(KnownFoldersSafeNativeMethods.NativeFolderDefinition);
				object padlock = new object();
				lock (padlock)
				{
					IntPtr unknown = Marshal.GetIUnknownForObject(nativeShellItem2);
					ThreadPool.QueueUserWorkItem(delegate
					{
						lock (padlock)
						{
							pidl = ShellHelper.PidlFromUnknown(unknown);
							new KnownFolderManagerClass().FindFolderFromIDList(pidl, out nativeFolder);
							if (nativeFolder != null)
							{
								nativeFolder.GetFolderDefinition(out definition);
							}
							Monitor.Pulse(padlock);
						}
					});
					Monitor.Wait(padlock);
				}
				return nativeFolder != null && definition.category == FolderCategory.Virtual;
			}
			finally
			{
				ShellNativeMethods.ILFree(pidl);
			}
		}

		internal static ShellObject Create(string parsingName)
		{
			if (string.IsNullOrEmpty(parsingName))
			{
				throw new ArgumentNullException("parsingName");
			}
			Guid riid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
			IShellItem2 shellItem;
			int num = ShellNativeMethods.SHCreateItemFromParsingName(parsingName, IntPtr.Zero, ref riid, out shellItem);
			if (!CoreErrorHelper.Succeeded(num))
			{
				throw new ShellException(LocalizedMessages.ShellObjectFactoryUnableToCreateItem, Marshal.GetExceptionForHR(num));
			}
			return Create(shellItem);
		}

		internal static ShellObject Create(IntPtr idListPtr)
		{
			CoreHelpers.ThrowIfNotVista();
			Guid riid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
			IShellItem2 ppv;
			int result = ShellNativeMethods.SHCreateItemFromIDList(idListPtr, ref riid, out ppv);
			if (!CoreErrorHelper.Succeeded(result))
			{
				return null;
			}
			return Create(ppv);
		}

		internal static ShellObject Create(IntPtr idListPtr, ShellContainer parent)
		{
			IShellItem ppsi;
			int result = ShellNativeMethods.SHCreateShellItem(IntPtr.Zero, parent.NativeShellFolder, idListPtr, out ppsi);
			if (!CoreErrorHelper.Succeeded(result))
			{
				return null;
			}
			return Create(ppsi);
		}
	}
}
