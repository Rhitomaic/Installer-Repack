using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class TabbedThumbnailManager
	{
		private Dictionary<IntPtr, TabbedThumbnail> _tabbedThumbnailCache;

		private Dictionary<UIElement, TabbedThumbnail> _tabbedThumbnailCacheWPF;

		internal TabbedThumbnailManager()
		{
			_tabbedThumbnailCache = new Dictionary<IntPtr, TabbedThumbnail>();
			_tabbedThumbnailCacheWPF = new Dictionary<UIElement, TabbedThumbnail>();
		}

		public void AddThumbnailPreview(TabbedThumbnail preview)
		{
			if (preview == null)
			{
				throw new ArgumentNullException("preview");
			}
			if (preview.WindowHandle == IntPtr.Zero)
			{
				if (_tabbedThumbnailCacheWPF.ContainsKey(preview.WindowsControl))
				{
					throw new ArgumentException(LocalizedMessages.ThumbnailManagerPreviewAdded, "preview");
				}
				_tabbedThumbnailCacheWPF.Add(preview.WindowsControl, preview);
			}
			else
			{
				if (_tabbedThumbnailCache.ContainsKey(preview.WindowHandle))
				{
					throw new ArgumentException(LocalizedMessages.ThumbnailManagerPreviewAdded, "preview");
				}
				_tabbedThumbnailCache.Add(preview.WindowHandle, preview);
			}
			TaskbarWindowManager.AddTabbedThumbnail(preview);
			preview.InvalidatePreview();
		}

		public TabbedThumbnail GetThumbnailPreview(IntPtr windowHandle)
		{
			if (windowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailManagerInvalidHandle, "windowHandle");
			}
			TabbedThumbnail value;
			return _tabbedThumbnailCache.TryGetValue(windowHandle, out value) ? value : null;
		}

		public TabbedThumbnail GetThumbnailPreview(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return GetThumbnailPreview(control.Handle);
		}

		public TabbedThumbnail GetThumbnailPreview(UIElement windowsControl)
		{
			if (windowsControl == null)
			{
				throw new ArgumentNullException("windowsControl");
			}
			TabbedThumbnail value;
			return _tabbedThumbnailCacheWPF.TryGetValue(windowsControl, out value) ? value : null;
		}

		public void RemoveThumbnailPreview(TabbedThumbnail preview)
		{
			if (preview == null)
			{
				throw new ArgumentNullException("preview");
			}
			if (_tabbedThumbnailCache.ContainsKey(preview.WindowHandle))
			{
				RemoveThumbnailPreview(preview.WindowHandle);
			}
			else if (_tabbedThumbnailCacheWPF.ContainsKey(preview.WindowsControl))
			{
				RemoveThumbnailPreview(preview.WindowsControl);
			}
		}

		public void RemoveThumbnailPreview(IntPtr windowHandle)
		{
			if (!_tabbedThumbnailCache.ContainsKey(windowHandle))
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailManagerControlNotAdded, "windowHandle");
			}
			TaskbarWindowManager.UnregisterTab(_tabbedThumbnailCache[windowHandle].TaskbarWindow);
			_tabbedThumbnailCache.Remove(windowHandle);
			TaskbarWindow taskbarWindow = TaskbarWindowManager.GetTaskbarWindow(windowHandle, TaskbarProxyWindowType.TabbedThumbnail);
			if (taskbarWindow != null)
			{
				if (TaskbarWindowManager._taskbarWindowList.Contains(taskbarWindow))
				{
					TaskbarWindowManager._taskbarWindowList.Remove(taskbarWindow);
				}
				taskbarWindow.Dispose();
				taskbarWindow = null;
			}
		}

		public void RemoveThumbnailPreview(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			IntPtr handle = control.Handle;
			RemoveThumbnailPreview(handle);
		}

		public void RemoveThumbnailPreview(UIElement windowsControl)
		{
			if (windowsControl == null)
			{
				throw new ArgumentNullException("windowsControl");
			}
			if (!_tabbedThumbnailCacheWPF.ContainsKey(windowsControl))
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailManagerControlNotAdded, "windowsControl");
			}
			TaskbarWindowManager.UnregisterTab(_tabbedThumbnailCacheWPF[windowsControl].TaskbarWindow);
			_tabbedThumbnailCacheWPF.Remove(windowsControl);
			TaskbarWindow taskbarWindow = TaskbarWindowManager.GetTaskbarWindow(windowsControl, TaskbarProxyWindowType.TabbedThumbnail);
			if (taskbarWindow != null)
			{
				if (TaskbarWindowManager._taskbarWindowList.Contains(taskbarWindow))
				{
					TaskbarWindowManager._taskbarWindowList.Remove(taskbarWindow);
				}
				taskbarWindow.Dispose();
				taskbarWindow = null;
			}
		}

		public void SetActiveTab(TabbedThumbnail preview)
		{
			if (preview == null)
			{
				throw new ArgumentNullException("preview");
			}
			if (preview.WindowHandle != IntPtr.Zero)
			{
				if (!_tabbedThumbnailCache.ContainsKey(preview.WindowHandle))
				{
					throw new ArgumentException(LocalizedMessages.ThumbnailManagerPreviewNotAdded, "preview");
				}
				TaskbarWindowManager.SetActiveTab(_tabbedThumbnailCache[preview.WindowHandle].TaskbarWindow);
			}
			else if (preview.WindowsControl != null)
			{
				if (!_tabbedThumbnailCacheWPF.ContainsKey(preview.WindowsControl))
				{
					throw new ArgumentException(LocalizedMessages.ThumbnailManagerPreviewNotAdded, "preview");
				}
				TaskbarWindowManager.SetActiveTab(_tabbedThumbnailCacheWPF[preview.WindowsControl].TaskbarWindow);
			}
		}

		public void SetActiveTab(IntPtr windowHandle)
		{
			if (!_tabbedThumbnailCache.ContainsKey(windowHandle))
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailManagerPreviewNotAdded, "windowHandle");
			}
			TaskbarWindowManager.SetActiveTab(_tabbedThumbnailCache[windowHandle].TaskbarWindow);
		}

		public void SetActiveTab(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			SetActiveTab(control.Handle);
		}

		public void SetActiveTab(UIElement windowsControl)
		{
			if (windowsControl == null)
			{
				throw new ArgumentNullException("windowsControl");
			}
			if (!_tabbedThumbnailCacheWPF.ContainsKey(windowsControl))
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailManagerPreviewNotAdded, "windowsControl");
			}
			TaskbarWindowManager.SetActiveTab(_tabbedThumbnailCacheWPF[windowsControl].TaskbarWindow);
		}

		public bool IsThumbnailPreviewAdded(TabbedThumbnail preview)
		{
			if (preview == null)
			{
				throw new ArgumentNullException("preview");
			}
			if (preview.WindowHandle != IntPtr.Zero && _tabbedThumbnailCache.ContainsKey(preview.WindowHandle))
			{
				return true;
			}
			if (preview.WindowsControl != null && _tabbedThumbnailCacheWPF.ContainsKey(preview.WindowsControl))
			{
				return true;
			}
			return false;
		}

		public bool IsThumbnailPreviewAdded(IntPtr windowHandle)
		{
			if (windowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailManagerInvalidHandle, "windowHandle");
			}
			return _tabbedThumbnailCache.ContainsKey(windowHandle);
		}

		public bool IsThumbnailPreviewAdded(Control control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return _tabbedThumbnailCache.ContainsKey(control.Handle);
		}

		public bool IsThumbnailPreviewAdded(UIElement control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return _tabbedThumbnailCacheWPF.ContainsKey(control);
		}

		public void InvalidateThumbnails()
		{
			foreach (TabbedThumbnail value in _tabbedThumbnailCache.Values)
			{
				TaskbarWindowManager.InvalidatePreview(value.TaskbarWindow);
				value.SetImage(IntPtr.Zero);
			}
			foreach (TabbedThumbnail value2 in _tabbedThumbnailCacheWPF.Values)
			{
				TaskbarWindowManager.InvalidatePreview(value2.TaskbarWindow);
				value2.SetImage(IntPtr.Zero);
			}
		}

		public static void ClearThumbnailClip(IntPtr windowHandle)
		{
			TaskbarList.Instance.SetThumbnailClip(windowHandle, IntPtr.Zero);
		}

		public void SetThumbnailClip(IntPtr windowHandle, Rectangle? clippingRectangle)
		{
			if (!clippingRectangle.HasValue)
			{
				ClearThumbnailClip(windowHandle);
				return;
			}
			NativeRect nativeRect = default(NativeRect);
			nativeRect.Left = clippingRectangle.Value.Left;
			nativeRect.Top = clippingRectangle.Value.Top;
			nativeRect.Right = clippingRectangle.Value.Right;
			nativeRect.Bottom = clippingRectangle.Value.Bottom;
			IntPtr intPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(nativeRect));
			try
			{
				Marshal.StructureToPtr(nativeRect, intPtr, true);
				TaskbarList.Instance.SetThumbnailClip(windowHandle, intPtr);
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
		}

		public static void SetTabOrder(TabbedThumbnail previewToChange, TabbedThumbnail insertBeforePreview)
		{
			if (previewToChange == null)
			{
				throw new ArgumentNullException("previewToChange");
			}
			IntPtr windowToTellTaskbarAbout = previewToChange.TaskbarWindow.WindowToTellTaskbarAbout;
			if (insertBeforePreview == null)
			{
				TaskbarList.Instance.SetTabOrder(windowToTellTaskbarAbout, IntPtr.Zero);
				return;
			}
			IntPtr windowToTellTaskbarAbout2 = insertBeforePreview.TaskbarWindow.WindowToTellTaskbarAbout;
			TaskbarList.Instance.SetTabOrder(windowToTellTaskbarAbout, windowToTellTaskbarAbout2);
		}
	}
}
