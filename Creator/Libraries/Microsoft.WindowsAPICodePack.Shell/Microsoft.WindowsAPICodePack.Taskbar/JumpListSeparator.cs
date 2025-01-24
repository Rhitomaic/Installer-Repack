using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class JumpListSeparator : JumpListTask, IDisposable
	{
		internal static PropertyKey PKEY_AppUserModel_IsDestListSeparator = SystemProperties.System.AppUserModel.IsDestinationListSeparator;

		private IPropertyStore nativePropertyStore;

		private IShellLinkW nativeShellLink;

		internal override IShellLinkW NativeShellLink
		{
			get
			{
				if (nativeShellLink != null)
				{
					Marshal.ReleaseComObject(nativeShellLink);
					nativeShellLink = null;
				}
				nativeShellLink = (IShellLinkW)new CShellLink();
				if (nativePropertyStore != null)
				{
					Marshal.ReleaseComObject(nativePropertyStore);
					nativePropertyStore = null;
				}
				nativePropertyStore = (IPropertyStore)nativeShellLink;
				using (PropVariant pv = new PropVariant(true))
				{
					HResult result = nativePropertyStore.SetValue(ref PKEY_AppUserModel_IsDestListSeparator, pv);
					if (!CoreErrorHelper.Succeeded(result))
					{
						throw new ShellException(result);
					}
					nativePropertyStore.Commit();
				}
				return nativeShellLink;
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (nativePropertyStore != null)
			{
				Marshal.ReleaseComObject(nativePropertyStore);
				nativePropertyStore = null;
			}
			if (nativeShellLink != null)
			{
				Marshal.ReleaseComObject(nativeShellLink);
				nativeShellLink = null;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~JumpListSeparator()
		{
			Dispose(false);
		}
	}
}
