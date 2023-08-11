#define DEBUG
using System;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public static class KnownFolderHelper
	{
		internal static IKnownFolderNative FromPIDL(IntPtr pidl)
		{
			KnownFolderManagerClass knownFolderManagerClass = new KnownFolderManagerClass();
			IKnownFolderNative knownFolder;
			return (knownFolderManagerClass.FindFolderFromIDList(pidl, out knownFolder) == HResult.Ok) ? knownFolder : null;
		}

		public static IKnownFolder FromKnownFolderId(Guid knownFolderId)
		{
			KnownFolderManagerClass knownFolderManagerClass = new KnownFolderManagerClass();
			IKnownFolderNative knownFolder;
			HResult folder = knownFolderManagerClass.GetFolder(knownFolderId, out knownFolder);
			if (folder != 0)
			{
				throw new ShellException(folder);
			}
			IKnownFolder knownFolder2 = GetKnownFolder(knownFolder);
			if (knownFolder2 == null)
			{
				throw new ArgumentException(LocalizedMessages.KnownFolderInvalidGuid, "knownFolderId");
			}
			return knownFolder2;
		}

		internal static IKnownFolder FromKnownFolderIdInternal(Guid knownFolderId)
		{
			IKnownFolderManager knownFolderManager = new KnownFolderManagerClass();
			IKnownFolderNative knownFolder;
			return (knownFolderManager.GetFolder(knownFolderId, out knownFolder) == HResult.Ok) ? GetKnownFolder(knownFolder) : null;
		}

		private static IKnownFolder GetKnownFolder(IKnownFolderNative knownFolderNative)
		{
			Debug.Assert(knownFolderNative != null, "Native IKnownFolder should not be null.");
			Guid interfaceGuid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
			IShellItem2 shellItem;
			HResult shellItem2 = knownFolderNative.GetShellItem(0, ref interfaceGuid, out shellItem);
			if (!CoreErrorHelper.Succeeded(shellItem2))
			{
				return null;
			}
			bool flag = false;
			if (shellItem != null)
			{
				shellItem.GetAttributes(ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem, out var psfgaoAttribs);
				flag = (psfgaoAttribs & ShellNativeMethods.ShellFileGetAttributesOptions.FileSystem) != 0;
			}
			if (flag)
			{
				return new FileSystemKnownFolder(knownFolderNative);
			}
			return new NonFileSystemKnownFolder(knownFolderNative);
		}

		public static IKnownFolder FromCanonicalName(string canonicalName)
		{
			IKnownFolderManager knownFolderManager = new KnownFolderManagerClass();
			knownFolderManager.GetFolderByName(canonicalName, out var knownFolder);
			IKnownFolder knownFolder2 = GetKnownFolder(knownFolder);
			if (knownFolder2 == null)
			{
				throw new ArgumentException(LocalizedMessages.ShellInvalidCanonicalName, "canonicalName");
			}
			return knownFolder2;
		}

		public static IKnownFolder FromPath(string path)
		{
			return FromParsingName(path);
		}

		public static IKnownFolder FromParsingName(string parsingName)
		{
			if (parsingName == null)
			{
				throw new ArgumentNullException("parsingName");
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			try
			{
				intPtr = ShellHelper.PidlFromParsingName(parsingName);
				if (intPtr == IntPtr.Zero)
				{
					throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName");
				}
				IKnownFolderNative knownFolderNative = FromPIDL(intPtr);
				if (knownFolderNative != null)
				{
					IKnownFolder knownFolder = GetKnownFolder(knownFolderNative);
					if (knownFolder == null)
					{
						throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName");
					}
					return knownFolder;
				}
				intPtr2 = ShellHelper.PidlFromParsingName(parsingName.PadRight(1, '\0'));
				if (intPtr2 == IntPtr.Zero)
				{
					throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName");
				}
				IKnownFolder knownFolder2 = GetKnownFolder(FromPIDL(intPtr));
				if (knownFolder2 == null)
				{
					throw new ArgumentException(LocalizedMessages.KnownFolderParsingName, "parsingName");
				}
				return knownFolder2;
			}
			finally
			{
				ShellNativeMethods.ILFree(intPtr);
				ShellNativeMethods.ILFree(intPtr2);
			}
		}
	}
}
