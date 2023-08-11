using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public abstract class ShellContainer : ShellObject, IEnumerable<ShellObject>, IEnumerable, IDisposable
	{
		private IShellFolder desktopFolderEnumeration;

		private IShellFolder nativeShellFolder;

		internal IShellFolder NativeShellFolder
		{
			get
			{
				if (nativeShellFolder == null)
				{
					Guid riid = new Guid("000214E6-0000-0000-C000-000000000046");
					Guid bhid = new Guid("3981e224-f559-11d3-8e3a-00c04f6837d5");
					HResult result = NativeShellItem.BindToHandler(IntPtr.Zero, ref bhid, ref riid, out nativeShellFolder);
					if (CoreErrorHelper.Failed(result))
					{
						string parsingName = ShellHelper.GetParsingName(NativeShellItem);
						if (parsingName != null && parsingName != Environment.GetFolderPath(Environment.SpecialFolder.Desktop))
						{
							throw new ShellException(result);
						}
					}
				}
				return nativeShellFolder;
			}
		}

		internal ShellContainer()
		{
		}

		internal ShellContainer(IShellItem2 shellItem)
			: base(shellItem)
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (nativeShellFolder != null)
			{
				Marshal.ReleaseComObject(nativeShellFolder);
				nativeShellFolder = null;
			}
			if (desktopFolderEnumeration != null)
			{
				Marshal.ReleaseComObject(desktopFolderEnumeration);
				desktopFolderEnumeration = null;
			}
			base.Dispose(disposing);
		}

		public IEnumerator<ShellObject> GetEnumerator()
		{
			if (NativeShellFolder == null)
			{
				if (desktopFolderEnumeration == null)
				{
					ShellNativeMethods.SHGetDesktopFolder(out desktopFolderEnumeration);
				}
				nativeShellFolder = desktopFolderEnumeration;
			}
			return new ShellFolderItems(this);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return new ShellFolderItems(this);
		}
	}
}
