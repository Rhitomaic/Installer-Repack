using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal sealed class TabbedThumbnailProxyWindow : Form, IDisposable
	{
		internal TabbedThumbnail TabbedThumbnail { get; private set; }

		internal UIElement WindowsControl { get; private set; }

		internal IntPtr WindowToTellTaskbarAbout => base.Handle;

		internal TabbedThumbnailProxyWindow(TabbedThumbnail preview)
		{
			TabbedThumbnail = preview;
			base.Size = new System.Drawing.Size(1, 1);
			if (!string.IsNullOrEmpty(preview.Title))
			{
				Text = preview.Title;
			}
			if (preview.WindowsControl != null)
			{
				WindowsControl = preview.WindowsControl;
			}
		}

		protected override void WndProc(ref Message m)
		{
			bool flag = false;
			if (TabbedThumbnail != null)
			{
				flag = TaskbarWindowManager.DispatchMessage(ref m, TabbedThumbnail.TaskbarWindow);
			}
			if (m.Msg == 2 || m.Msg == 130 || (m.Msg == 274 && (int)m.WParam == 61536))
			{
				base.WndProc(ref m);
			}
			else if (!flag)
			{
				base.WndProc(ref m);
			}
		}

		~TabbedThumbnailProxyWindow()
		{
			Dispose(false);
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (TabbedThumbnail != null)
				{
					TabbedThumbnail.Dispose();
				}
				TabbedThumbnail = null;
				WindowsControl = null;
			}
			base.Dispose(disposing);
		}
	}
}
