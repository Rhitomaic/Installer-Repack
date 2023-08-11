using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal class ShellFolderItems : IEnumerator<ShellObject>, IDisposable, IEnumerator
	{
		private IEnumIDList nativeEnumIdList;

		private ShellObject currentItem;

		private ShellContainer nativeShellFolder;

		public ShellObject Current => currentItem;

		object IEnumerator.Current => currentItem;

		internal ShellFolderItems(ShellContainer nativeShellFolder)
		{
			this.nativeShellFolder = nativeShellFolder;
			HResult hResult = nativeShellFolder.NativeShellFolder.EnumObjects(IntPtr.Zero, ShellNativeMethods.ShellFolderEnumerationOptions.Folders | ShellNativeMethods.ShellFolderEnumerationOptions.NonFolders, out nativeEnumIdList);
			if (!CoreErrorHelper.Succeeded(hResult))
			{
				if (hResult == HResult.Canceled)
				{
					throw new FileNotFoundException();
				}
				throw new ShellException(hResult);
			}
		}

		public void Dispose()
		{
			if (nativeEnumIdList != null)
			{
				Marshal.ReleaseComObject(nativeEnumIdList);
				nativeEnumIdList = null;
			}
		}

		public bool MoveNext()
		{
			if (nativeEnumIdList == null)
			{
				return false;
			}
			uint num = 1u;
			IntPtr rgelt;
			uint pceltFetched;
			HResult hResult = nativeEnumIdList.Next(num, out rgelt, out pceltFetched);
			if (pceltFetched < num || hResult != 0)
			{
				return false;
			}
			currentItem = ShellObjectFactory.Create(rgelt, nativeShellFolder);
			return true;
		}

		public void Reset()
		{
			if (nativeEnumIdList != null)
			{
				nativeEnumIdList.Reset();
			}
		}
	}
}
