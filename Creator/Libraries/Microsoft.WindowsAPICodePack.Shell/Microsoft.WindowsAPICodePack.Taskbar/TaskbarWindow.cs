using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal class TaskbarWindow : IDisposable
	{
		private TabbedThumbnail _tabbedThumbnailPreview;

		private ThumbnailToolBarButton[] _thumbnailButtons;

		internal TabbedThumbnailProxyWindow TabbedThumbnailProxyWindow { get; set; }

		internal ThumbnailToolbarProxyWindow ThumbnailToolbarProxyWindow { get; set; }

		internal bool EnableTabbedThumbnails { get; set; }

		internal bool EnableThumbnailToolbars { get; set; }

		internal IntPtr UserWindowHandle { get; set; }

		internal UIElement WindowsControl { get; set; }

		internal TabbedThumbnail TabbedThumbnail
		{
			get
			{
				return _tabbedThumbnailPreview;
			}
			set
			{
				if (_tabbedThumbnailPreview != null)
				{
					throw new InvalidOperationException(LocalizedMessages.TaskbarWindowValueSet);
				}
				TabbedThumbnailProxyWindow = new TabbedThumbnailProxyWindow(value);
				_tabbedThumbnailPreview = value;
				_tabbedThumbnailPreview.TaskbarWindow = this;
			}
		}

		internal ThumbnailToolBarButton[] ThumbnailButtons
		{
			get
			{
				return _thumbnailButtons;
			}
			set
			{
				_thumbnailButtons = value;
				UpdateHandles();
			}
		}

		internal IntPtr WindowToTellTaskbarAbout
		{
			get
			{
				if (EnableThumbnailToolbars && !EnableTabbedThumbnails && ThumbnailToolbarProxyWindow != null)
				{
					return ThumbnailToolbarProxyWindow.WindowToTellTaskbarAbout;
				}
				if (!EnableThumbnailToolbars && EnableTabbedThumbnails && TabbedThumbnailProxyWindow != null)
				{
					return TabbedThumbnailProxyWindow.WindowToTellTaskbarAbout;
				}
				if (EnableTabbedThumbnails && EnableThumbnailToolbars && TabbedThumbnailProxyWindow != null)
				{
					return TabbedThumbnailProxyWindow.WindowToTellTaskbarAbout;
				}
				throw new InvalidOperationException();
			}
		}

		private void UpdateHandles()
		{
			ThumbnailToolBarButton[] thumbnailButtons = _thumbnailButtons;
			foreach (ThumbnailToolBarButton thumbnailToolBarButton in thumbnailButtons)
			{
				thumbnailToolBarButton.WindowHandle = WindowToTellTaskbarAbout;
				thumbnailToolBarButton.AddedToTaskbar = false;
			}
		}

		internal void SetTitle(string title)
		{
			if (TabbedThumbnailProxyWindow == null)
			{
				throw new InvalidOperationException(LocalizedMessages.TasbarWindowProxyWindowSet);
			}
			TabbedThumbnailProxyWindow.Text = title;
		}

		internal TaskbarWindow(IntPtr userWindowHandle, params ThumbnailToolBarButton[] buttons)
		{
			if (userWindowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.CommonFileDialogInvalidHandle, "userWindowHandle");
			}
			if (buttons == null || buttons.Length == 0)
			{
				throw new ArgumentException(LocalizedMessages.TaskbarWindowEmptyButtonArray, "buttons");
			}
			ThumbnailToolbarProxyWindow = new ThumbnailToolbarProxyWindow(userWindowHandle, buttons);
			ThumbnailToolbarProxyWindow.TaskbarWindow = this;
			EnableThumbnailToolbars = true;
			EnableTabbedThumbnails = false;
			ThumbnailButtons = buttons;
			UserWindowHandle = userWindowHandle;
			WindowsControl = null;
		}

		internal TaskbarWindow(UIElement windowsControl, params ThumbnailToolBarButton[] buttons)
		{
			if (windowsControl == null)
			{
				throw new ArgumentNullException("windowsControl");
			}
			if (buttons == null || buttons.Length == 0)
			{
				throw new ArgumentException(LocalizedMessages.TaskbarWindowEmptyButtonArray, "buttons");
			}
			ThumbnailToolbarProxyWindow = new ThumbnailToolbarProxyWindow(windowsControl, buttons);
			ThumbnailToolbarProxyWindow.TaskbarWindow = this;
			EnableThumbnailToolbars = true;
			EnableTabbedThumbnails = false;
			ThumbnailButtons = buttons;
			UserWindowHandle = IntPtr.Zero;
			WindowsControl = windowsControl;
		}

		internal TaskbarWindow(TabbedThumbnail preview)
		{
			if (preview == null)
			{
				throw new ArgumentNullException("preview");
			}
			TabbedThumbnailProxyWindow = new TabbedThumbnailProxyWindow(preview);
			EnableThumbnailToolbars = false;
			EnableTabbedThumbnails = true;
			UserWindowHandle = preview.WindowHandle;
			WindowsControl = preview.WindowsControl;
			TabbedThumbnail = preview;
		}

		~TaskbarWindow()
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
				if (_tabbedThumbnailPreview != null)
				{
					_tabbedThumbnailPreview.Dispose();
				}
				_tabbedThumbnailPreview = null;
				if (ThumbnailToolbarProxyWindow != null)
				{
					ThumbnailToolbarProxyWindow.Dispose();
				}
				ThumbnailToolbarProxyWindow = null;
				if (TabbedThumbnailProxyWindow != null)
				{
					TabbedThumbnailProxyWindow.Dispose();
				}
				TabbedThumbnailProxyWindow = null;
				_thumbnailButtons = null;
			}
		}
	}
}
