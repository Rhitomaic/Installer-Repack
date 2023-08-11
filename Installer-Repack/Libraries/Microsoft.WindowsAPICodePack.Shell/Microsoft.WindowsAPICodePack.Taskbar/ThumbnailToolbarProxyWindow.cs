using System;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal class ThumbnailToolbarProxyWindow : NativeWindow, IDisposable
	{
		private ThumbnailToolBarButton[] _thumbnailButtons;

		private IntPtr _internalWindowHandle;

		internal UIElement WindowsControl { get; set; }

		internal IntPtr WindowToTellTaskbarAbout => (_internalWindowHandle != IntPtr.Zero) ? _internalWindowHandle : base.Handle;

		internal TaskbarWindow TaskbarWindow { get; set; }

		internal ThumbnailToolbarProxyWindow(IntPtr windowHandle, ThumbnailToolBarButton[] buttons)
		{
			if (windowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.CommonFileDialogInvalidHandle, "windowHandle");
			}
			if (buttons != null && buttons.Length == 0)
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailToolbarManagerNullEmptyArray, "buttons");
			}
			_internalWindowHandle = windowHandle;
			_thumbnailButtons = buttons;
			Array.ForEach(_thumbnailButtons, UpdateHandle);
			AssignHandle(windowHandle);
		}

		internal ThumbnailToolbarProxyWindow(UIElement windowsControl, ThumbnailToolBarButton[] buttons)
		{
			if (windowsControl == null)
			{
				throw new ArgumentNullException("windowsControl");
			}
			if (buttons != null && buttons.Length == 0)
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailToolbarManagerNullEmptyArray, "buttons");
			}
			_internalWindowHandle = IntPtr.Zero;
			WindowsControl = windowsControl;
			_thumbnailButtons = buttons;
			Array.ForEach(_thumbnailButtons, UpdateHandle);
		}

		private void UpdateHandle(ThumbnailToolBarButton button)
		{
			button.WindowHandle = _internalWindowHandle;
			button.AddedToTaskbar = false;
		}

		protected override void WndProc(ref Message m)
		{
			bool flag = false;
			flag = TaskbarWindowManager.DispatchMessage(ref m, TaskbarWindow);
			if (m.Msg == 2 || m.Msg == 130 || (m.Msg == 274 && (int)m.WParam == 61536))
			{
				base.WndProc(ref m);
			}
			else if (!flag)
			{
				base.WndProc(ref m);
			}
		}

		~ThumbnailToolbarProxyWindow()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				_thumbnailButtons = null;
			}
		}
	}
}
