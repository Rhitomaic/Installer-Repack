using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class JumpListLink : JumpListTask, IJumpListItem, IDisposable
	{
		internal static PropertyKey PKEY_Title = SystemProperties.System.Title;

		private string title;

		private string path;

		private IPropertyStore nativePropertyStore;

		private IShellLinkW nativeShellLink;

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value", LocalizedMessages.JumpListLinkTitleRequired);
				}
				title = value;
			}
		}

		public string Path
		{
			get
			{
				return path;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value", LocalizedMessages.JumpListLinkTitleRequired);
				}
				path = value;
			}
		}

		public IconReference IconReference { get; set; }

		public string Arguments { get; set; }

		public string WorkingDirectory { get; set; }

		public WindowShowCommand ShowCommand { get; set; }

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
				nativeShellLink.SetPath(Path);
				if (!string.IsNullOrEmpty(IconReference.ModuleName))
				{
					nativeShellLink.SetIconLocation(IconReference.ModuleName, IconReference.ResourceId);
				}
				if (!string.IsNullOrEmpty(Arguments))
				{
					nativeShellLink.SetArguments(Arguments);
				}
				if (!string.IsNullOrEmpty(WorkingDirectory))
				{
					nativeShellLink.SetWorkingDirectory(WorkingDirectory);
				}
				nativeShellLink.SetShowCmd((uint)ShowCommand);
				using (PropVariant pv = new PropVariant(Title))
				{
					HResult result = nativePropertyStore.SetValue(ref PKEY_Title, pv);
					if (!CoreErrorHelper.Succeeded(result))
					{
						throw new ShellException(result);
					}
					nativePropertyStore.Commit();
				}
				return nativeShellLink;
			}
		}

		public JumpListLink(string pathValue, string titleValue)
		{
			if (string.IsNullOrEmpty(pathValue))
			{
				throw new ArgumentNullException("pathValue", LocalizedMessages.JumpListLinkPathRequired);
			}
			if (string.IsNullOrEmpty(titleValue))
			{
				throw new ArgumentNullException("titleValue", LocalizedMessages.JumpListLinkTitleRequired);
			}
			Path = pathValue;
			Title = titleValue;
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				title = null;
			}
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

		~JumpListLink()
		{
			Dispose(false);
		}
	}
}
