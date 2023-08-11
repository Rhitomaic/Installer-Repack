using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class TabbedThumbnail : IDisposable
	{
		private TaskbarWindow _taskbarWindow;

		private bool _addedToTaskbar;

		private string _title = string.Empty;

		private string _tooltip = string.Empty;

		private Rectangle? _clippingRectangle;

		internal IntPtr WindowHandle { get; set; }

		internal IntPtr ParentWindowHandle { get; set; }

		internal UIElement WindowsControl { get; set; }

		internal Window WindowsControlParentWindow { get; set; }

		internal TaskbarWindow TaskbarWindow
		{
			get
			{
				return _taskbarWindow;
			}
			set
			{
				_taskbarWindow = value;
				if (_taskbarWindow != null && _taskbarWindow.TabbedThumbnailProxyWindow != null)
				{
					_taskbarWindow.TabbedThumbnailProxyWindow.Icon = Icon;
				}
			}
		}

		internal bool AddedToTaskbar
		{
			get
			{
				return _addedToTaskbar;
			}
			set
			{
				_addedToTaskbar = value;
				if (ClippingRectangle.HasValue)
				{
					TaskbarWindowManager.InvalidatePreview(TaskbarWindow);
				}
			}
		}

		internal bool RemovedFromTaskbar { get; set; }

		public string Title
		{
			get
			{
				return _title;
			}
			set
			{
				if (_title != value)
				{
					_title = value;
					if (this.TitleChanged != null)
					{
						this.TitleChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		public string Tooltip
		{
			get
			{
				return _tooltip;
			}
			set
			{
				if (_tooltip != value)
				{
					_tooltip = value;
					if (this.TooltipChanged != null)
					{
						this.TooltipChanged(this, EventArgs.Empty);
					}
				}
			}
		}

		public Rectangle? ClippingRectangle
		{
			get
			{
				return _clippingRectangle;
			}
			set
			{
				_clippingRectangle = value;
				TaskbarWindowManager.InvalidatePreview(TaskbarWindow);
			}
		}

		internal IntPtr CurrentHBitmap { get; set; }

		internal Icon Icon { get; private set; }

		public bool DisplayFrameAroundBitmap { get; set; }

		public Vector? PeekOffset { get; set; }

		public event EventHandler TitleChanged;

		public event EventHandler TooltipChanged;

		public event EventHandler<TabbedThumbnailClosedEventArgs> TabbedThumbnailClosed;

		public event EventHandler<TabbedThumbnailEventArgs> TabbedThumbnailMaximized;

		public event EventHandler<TabbedThumbnailEventArgs> TabbedThumbnailMinimized;

		public event EventHandler<TabbedThumbnailEventArgs> TabbedThumbnailActivated;

		public event EventHandler<TabbedThumbnailBitmapRequestedEventArgs> TabbedThumbnailBitmapRequested;

		public TabbedThumbnail(IntPtr parentWindowHandle, IntPtr windowHandle)
		{
			if (parentWindowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.TabbedThumbnailZeroParentHandle, "parentWindowHandle");
			}
			if (windowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.TabbedThumbnailZeroChildHandle, "windowHandle");
			}
			WindowHandle = windowHandle;
			ParentWindowHandle = parentWindowHandle;
		}

		public TabbedThumbnail(IntPtr parentWindowHandle, Control control)
		{
			if (parentWindowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.TabbedThumbnailZeroParentHandle, "parentWindowHandle");
			}
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			WindowHandle = control.Handle;
			ParentWindowHandle = parentWindowHandle;
		}

		public TabbedThumbnail(Window parentWindow, UIElement windowsControl, Vector peekOffset)
		{
			if (windowsControl == null)
			{
				throw new ArgumentNullException("windowsControl");
			}
			if (parentWindow == null)
			{
				throw new ArgumentNullException("parentWindow");
			}
			WindowHandle = IntPtr.Zero;
			WindowsControl = windowsControl;
			WindowsControlParentWindow = parentWindow;
			ParentWindowHandle = new WindowInteropHelper(parentWindow).Handle;
			PeekOffset = peekOffset;
		}

		public void SetWindowIcon(Icon icon)
		{
			Icon = icon;
			if (TaskbarWindow != null && TaskbarWindow.TabbedThumbnailProxyWindow != null)
			{
				TaskbarWindow.TabbedThumbnailProxyWindow.Icon = Icon;
			}
		}

		public void SetWindowIcon(IntPtr iconHandle)
		{
			Icon = ((iconHandle != IntPtr.Zero) ? Icon.FromHandle(iconHandle) : null);
			if (TaskbarWindow != null && TaskbarWindow.TabbedThumbnailProxyWindow != null)
			{
				TaskbarWindow.TabbedThumbnailProxyWindow.Icon = Icon;
			}
		}

		public void SetImage(Bitmap bitmap)
		{
			if (bitmap != null)
			{
				SetImage(bitmap.GetHbitmap());
			}
			else
			{
				SetImage(IntPtr.Zero);
			}
		}

		public void SetImage(BitmapSource bitmapSource)
		{
			if (bitmapSource == null)
			{
				SetImage(IntPtr.Zero);
				return;
			}
			BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
			bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource));
			using (MemoryStream memoryStream = new MemoryStream())
			{
				bmpBitmapEncoder.Save(memoryStream);
				memoryStream.Position = 0L;
				using (Bitmap bitmap = new Bitmap(memoryStream))
				{
					SetImage(bitmap.GetHbitmap());
				}
			}
		}

		internal void SetImage(IntPtr hBitmap)
		{
			if (CurrentHBitmap != IntPtr.Zero)
			{
				ShellNativeMethods.DeleteObject(CurrentHBitmap);
			}
			CurrentHBitmap = hBitmap;
			TaskbarWindowManager.InvalidatePreview(TaskbarWindow);
		}

		public void InvalidatePreview()
		{
			SetImage(IntPtr.Zero);
		}

		internal void OnTabbedThumbnailMaximized()
		{
			if (this.TabbedThumbnailMaximized != null)
			{
				this.TabbedThumbnailMaximized(this, GetTabbedThumbnailEventArgs());
			}
			else
			{
				CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.SystemCommand, new IntPtr(61488), IntPtr.Zero);
			}
		}

		internal void OnTabbedThumbnailMinimized()
		{
			if (this.TabbedThumbnailMinimized != null)
			{
				this.TabbedThumbnailMinimized(this, GetTabbedThumbnailEventArgs());
			}
			else
			{
				CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.SystemCommand, new IntPtr(61472), IntPtr.Zero);
			}
		}

		internal bool OnTabbedThumbnailClosed()
		{
			EventHandler<TabbedThumbnailClosedEventArgs> tabbedThumbnailClosed = this.TabbedThumbnailClosed;
			if (tabbedThumbnailClosed != null)
			{
				TabbedThumbnailClosedEventArgs tabbedThumbnailClosingEventArgs = GetTabbedThumbnailClosingEventArgs();
				tabbedThumbnailClosed(this, tabbedThumbnailClosingEventArgs);
				if (tabbedThumbnailClosingEventArgs.Cancel)
				{
					return false;
				}
			}
			else
			{
				CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.NCDestroy, IntPtr.Zero, IntPtr.Zero);
			}
			TaskbarManager.Instance.TabbedThumbnail.RemoveThumbnailPreview(this);
			return true;
		}

		internal void OnTabbedThumbnailActivated()
		{
			if (this.TabbedThumbnailActivated != null)
			{
				this.TabbedThumbnailActivated(this, GetTabbedThumbnailEventArgs());
			}
			else
			{
				CoreNativeMethods.SendMessage(ParentWindowHandle, WindowMessage.ActivateApplication, new IntPtr(1), new IntPtr(Thread.CurrentThread.GetHashCode()));
			}
		}

		internal void OnTabbedThumbnailBitmapRequested()
		{
			if (this.TabbedThumbnailBitmapRequested != null)
			{
				TabbedThumbnailBitmapRequestedEventArgs e = null;
				if (WindowHandle != IntPtr.Zero)
				{
					e = new TabbedThumbnailBitmapRequestedEventArgs(WindowHandle);
				}
				else if (WindowsControl != null)
				{
					e = new TabbedThumbnailBitmapRequestedEventArgs(WindowsControl);
				}
				this.TabbedThumbnailBitmapRequested(this, e);
			}
		}

		private TabbedThumbnailClosedEventArgs GetTabbedThumbnailClosingEventArgs()
		{
			TabbedThumbnailClosedEventArgs result = null;
			if (WindowHandle != IntPtr.Zero)
			{
				result = new TabbedThumbnailClosedEventArgs(WindowHandle);
			}
			else if (WindowsControl != null)
			{
				result = new TabbedThumbnailClosedEventArgs(WindowsControl);
			}
			return result;
		}

		private TabbedThumbnailEventArgs GetTabbedThumbnailEventArgs()
		{
			TabbedThumbnailEventArgs result = null;
			if (WindowHandle != IntPtr.Zero)
			{
				result = new TabbedThumbnailEventArgs(WindowHandle);
			}
			else if (WindowsControl != null)
			{
				result = new TabbedThumbnailEventArgs(WindowsControl);
			}
			return result;
		}

		~TabbedThumbnail()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				_taskbarWindow = null;
				if (Icon != null)
				{
					Icon.Dispose();
				}
				Icon = null;
				_title = null;
				_tooltip = null;
				WindowsControl = null;
			}
			if (CurrentHBitmap != IntPtr.Zero)
			{
				ShellNativeMethods.DeleteObject(CurrentHBitmap);
				CurrentHBitmap = IntPtr.Zero;
			}
		}
	}
}
