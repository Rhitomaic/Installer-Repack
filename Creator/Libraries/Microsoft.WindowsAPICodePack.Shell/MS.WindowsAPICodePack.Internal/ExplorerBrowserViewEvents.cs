using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Controls.WindowsForms;

namespace MS.WindowsAPICodePack.Internal
{
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	public class ExplorerBrowserViewEvents : IDisposable
	{
		private uint viewConnectionPointCookie;

		private object viewDispatch;

		private IntPtr nullPtr = IntPtr.Zero;

		private Guid IID_DShellFolderViewEvents = new Guid("62112AA2-EBE4-11cf-A5FB-0020AFE7292D");

		private Guid IID_IDispatch = new Guid("00020400-0000-0000-C000-000000000046");

		private ExplorerBrowser parent;

		public ExplorerBrowserViewEvents()
			: this(null)
		{
		}

		internal ExplorerBrowserViewEvents(ExplorerBrowser parent)
		{
			this.parent = parent;
		}

		internal void ConnectToView(IShellView psv)
		{
			DisconnectFromView();
			if (psv.GetItemObject(ShellViewGetItemObject.Background, ref IID_IDispatch, out viewDispatch) == HResult.Ok && ExplorerBrowserNativeMethods.ConnectToConnectionPoint(this, ref IID_DShellFolderViewEvents, true, viewDispatch, ref viewConnectionPointCookie, ref nullPtr) != 0)
			{
				Marshal.ReleaseComObject(viewDispatch);
			}
		}

		internal void DisconnectFromView()
		{
			if (viewDispatch != null)
			{
				ExplorerBrowserNativeMethods.ConnectToConnectionPoint(IntPtr.Zero, ref IID_DShellFolderViewEvents, false, viewDispatch, ref viewConnectionPointCookie, ref nullPtr);
				Marshal.ReleaseComObject(viewDispatch);
				viewDispatch = null;
				viewConnectionPointCookie = 0u;
			}
		}

		[DispId(200)]
		public void ViewSelectionChanged()
		{
			parent.FireSelectionChanged();
		}

		[DispId(207)]
		public void ViewContentsChanged()
		{
			parent.FireContentChanged();
		}

		[DispId(201)]
		public void ViewFileListEnumDone()
		{
			parent.FireContentEnumerationComplete();
		}

		[DispId(220)]
		public void ViewSelectedItemChanged()
		{
			parent.FireSelectedItemChanged();
		}

		~ExplorerBrowserViewEvents()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposed)
		{
			if (disposed)
			{
				DisconnectFromView();
			}
		}
	}
}
