using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal static class TaskbarWindowManager
	{
		internal static List<TaskbarWindow> _taskbarWindowList = new List<TaskbarWindow>();

		private static bool _buttonsAdded;

		internal static void AddThumbnailButtons(IntPtr userWindowHandle, params ThumbnailToolBarButton[] buttons)
		{
			TaskbarWindow taskbarWindow = GetTaskbarWindow(userWindowHandle, TaskbarProxyWindowType.ThumbnailToolbar);
			TaskbarWindow taskbarWindow2 = null;
			try
			{
				AddThumbnailButtons(taskbarWindow ?? (taskbarWindow2 = new TaskbarWindow(userWindowHandle, buttons)), taskbarWindow == null, buttons);
			}
			catch
			{
				taskbarWindow2?.Dispose();
				throw;
			}
		}

		internal static void AddThumbnailButtons(UIElement control, params ThumbnailToolBarButton[] buttons)
		{
			TaskbarWindow taskbarWindow = GetTaskbarWindow(control, TaskbarProxyWindowType.ThumbnailToolbar);
			TaskbarWindow taskbarWindow2 = null;
			try
			{
				AddThumbnailButtons(taskbarWindow ?? (taskbarWindow2 = new TaskbarWindow(control, buttons)), taskbarWindow == null, buttons);
			}
			catch
			{
				taskbarWindow2?.Dispose();
				throw;
			}
		}

		private static void AddThumbnailButtons(TaskbarWindow taskbarWindow, bool add, params ThumbnailToolBarButton[] buttons)
		{
			if (add)
			{
				_taskbarWindowList.Add(taskbarWindow);
				return;
			}
			if (taskbarWindow.ThumbnailButtons == null)
			{
				taskbarWindow.ThumbnailButtons = buttons;
				return;
			}
			throw new InvalidOperationException(LocalizedMessages.TaskbarWindowManagerButtonsAlreadyAdded);
		}

		internal static void AddTabbedThumbnail(TabbedThumbnail preview)
		{
			TaskbarWindow taskbarWindow = null;
			taskbarWindow = ((!(preview.WindowHandle == IntPtr.Zero)) ? GetTaskbarWindow(preview.WindowHandle, TaskbarProxyWindowType.TabbedThumbnail) : GetTaskbarWindow(preview.WindowsControl, TaskbarProxyWindowType.TabbedThumbnail));
			if (taskbarWindow == null)
			{
				taskbarWindow = new TaskbarWindow(preview);
				_taskbarWindowList.Add(taskbarWindow);
			}
			else if (taskbarWindow.TabbedThumbnail == null)
			{
				taskbarWindow.TabbedThumbnail = preview;
			}
			preview.TitleChanged += thumbnailPreview_TitleChanged;
			preview.TooltipChanged += thumbnailPreview_TooltipChanged;
			IntPtr windowToTellTaskbarAbout = taskbarWindow.WindowToTellTaskbarAbout;
			TaskbarList.Instance.RegisterTab(windowToTellTaskbarAbout, preview.ParentWindowHandle);
			TaskbarList.Instance.SetTabOrder(windowToTellTaskbarAbout, IntPtr.Zero);
			TaskbarList.Instance.SetTabActive(windowToTellTaskbarAbout, preview.ParentWindowHandle, 0u);
			TabbedThumbnailNativeMethods.ChangeWindowMessageFilter(803u, 1u);
			TabbedThumbnailNativeMethods.ChangeWindowMessageFilter(806u, 1u);
			TabbedThumbnailNativeMethods.EnableCustomWindowPreview(windowToTellTaskbarAbout, true);
			thumbnailPreview_TitleChanged(preview, EventArgs.Empty);
			thumbnailPreview_TooltipChanged(preview, EventArgs.Empty);
			preview.AddedToTaskbar = true;
		}

		internal static TaskbarWindow GetTaskbarWindow(UIElement windowsControl, TaskbarProxyWindowType taskbarProxyWindowType)
		{
			if (windowsControl == null)
			{
				throw new ArgumentNullException("windowsControl");
			}
			TaskbarWindow taskbarWindow = _taskbarWindowList.FirstOrDefault((TaskbarWindow window) => (window.TabbedThumbnail != null && window.TabbedThumbnail.WindowsControl == windowsControl) || (window.ThumbnailToolbarProxyWindow != null && window.ThumbnailToolbarProxyWindow.WindowsControl == windowsControl));
			if (taskbarWindow != null)
			{
				switch (taskbarProxyWindowType)
				{
				case TaskbarProxyWindowType.ThumbnailToolbar:
					taskbarWindow.EnableThumbnailToolbars = true;
					break;
				case TaskbarProxyWindowType.TabbedThumbnail:
					taskbarWindow.EnableTabbedThumbnails = true;
					break;
				}
			}
			return taskbarWindow;
		}

		internal static TaskbarWindow GetTaskbarWindow(IntPtr userWindowHandle, TaskbarProxyWindowType taskbarProxyWindowType)
		{
			if (userWindowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.CommonFileDialogInvalidHandle, "userWindowHandle");
			}
			TaskbarWindow taskbarWindow = _taskbarWindowList.FirstOrDefault((TaskbarWindow window) => window.UserWindowHandle == userWindowHandle);
			if (taskbarWindow != null)
			{
				switch (taskbarProxyWindowType)
				{
				case TaskbarProxyWindowType.ThumbnailToolbar:
					taskbarWindow.EnableThumbnailToolbars = true;
					break;
				case TaskbarProxyWindowType.TabbedThumbnail:
					taskbarWindow.EnableTabbedThumbnails = true;
					break;
				}
			}
			return taskbarWindow;
		}

		private static void DispatchTaskbarButtonMessages(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (m.Msg == (int)TaskbarNativeMethods.WmTaskbarButtonCreated)
			{
				AddButtons(taskbarWindow);
				return;
			}
			if (!_buttonsAdded)
			{
				AddButtons(taskbarWindow);
			}
			if (m.Msg != 273 || CoreNativeMethods.GetHiWord(m.WParam.ToInt64(), 16) != 6144)
			{
				return;
			}
			int buttonId = CoreNativeMethods.GetLoWord(m.WParam.ToInt64());
			IEnumerable<ThumbnailToolBarButton> enumerable = taskbarWindow.ThumbnailButtons.Where((ThumbnailToolBarButton b) => b.Id == buttonId);
			foreach (ThumbnailToolBarButton item in enumerable)
			{
				item.FireClick(taskbarWindow);
			}
		}

		private static bool DispatchActivateMessage(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (m.Msg == 6)
			{
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailActivated();
				SetActiveTab(taskbarWindow);
				return true;
			}
			return false;
		}

		private static bool DispatchSendIconThumbnailMessage(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (m.Msg == 803)
			{
				int width = (int)((long)m.LParam >> 16);
				int height = (int)((long)m.LParam & 0xFFFF);
				System.Drawing.Size size = new System.Drawing.Size(width, height);
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailBitmapRequested();
				IntPtr zero = IntPtr.Zero;
				System.Drawing.Size size2 = new System.Drawing.Size(200, 200);
				if (taskbarWindow.TabbedThumbnail.WindowHandle != IntPtr.Zero)
				{
					TabbedThumbnailNativeMethods.GetClientSize(taskbarWindow.TabbedThumbnail.WindowHandle, out size2);
				}
				else if (taskbarWindow.TabbedThumbnail.WindowsControl != null)
				{
					size2 = new System.Drawing.Size(Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Width), Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Height));
				}
				if (size2.Height == -1 && size2.Width == -1)
				{
					int width2 = (size2.Height = 199);
					size2.Width = width2;
				}
				if (taskbarWindow.TabbedThumbnail.ClippingRectangle.HasValue && taskbarWindow.TabbedThumbnail.ClippingRectangle.Value != Rectangle.Empty)
				{
					zero = ((!(taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)) ? taskbarWindow.TabbedThumbnail.CurrentHBitmap : GrabBitmap(taskbarWindow, size2));
					Bitmap bitmap = Image.FromHbitmap(zero);
					Rectangle value = taskbarWindow.TabbedThumbnail.ClippingRectangle.Value;
					if (value.Height > size.Height)
					{
						value.Height = size.Height;
					}
					if (value.Width > size.Width)
					{
						value.Width = size.Width;
					}
					bitmap = bitmap.Clone(value, bitmap.PixelFormat);
					if (zero != IntPtr.Zero && taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
					{
						ShellNativeMethods.DeleteObject(zero);
					}
					zero = bitmap.GetHbitmap();
					bitmap.Dispose();
				}
				else
				{
					zero = taskbarWindow.TabbedThumbnail.CurrentHBitmap;
					if (zero == IntPtr.Zero)
					{
						zero = GrabBitmap(taskbarWindow, size2);
					}
				}
				if (zero != IntPtr.Zero)
				{
					Bitmap bitmap2 = TabbedThumbnailScreenCapture.ResizeImageWithAspect(zero, size.Width, size.Height, true);
					if (taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
					{
						ShellNativeMethods.DeleteObject(zero);
					}
					zero = bitmap2.GetHbitmap();
					TabbedThumbnailNativeMethods.SetIconicThumbnail(taskbarWindow.WindowToTellTaskbarAbout, zero);
					bitmap2.Dispose();
				}
				if (taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
				{
					ShellNativeMethods.DeleteObject(zero);
				}
				return true;
			}
			return false;
		}

		private static bool DispatchLivePreviewBitmapMessage(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (m.Msg == 806)
			{
				int num = (int)((long)m.LParam >> 16);
				int num2 = (int)((long)m.LParam & 0xFFFF);
				System.Drawing.Size size = new System.Drawing.Size(200, 200);
				if (taskbarWindow.TabbedThumbnail.WindowHandle != IntPtr.Zero)
				{
					TabbedThumbnailNativeMethods.GetClientSize(taskbarWindow.TabbedThumbnail.WindowHandle, out size);
				}
				else if (taskbarWindow.TabbedThumbnail.WindowsControl != null)
				{
					size = new System.Drawing.Size(Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Width), Convert.ToInt32(taskbarWindow.TabbedThumbnail.WindowsControl.RenderSize.Height));
				}
				if (num <= 0)
				{
					num = size.Width;
				}
				if (num2 <= 0)
				{
					num2 = size.Height;
				}
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailBitmapRequested();
				IntPtr intPtr = ((taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero) ? GrabBitmap(taskbarWindow, size) : taskbarWindow.TabbedThumbnail.CurrentHBitmap);
				if (taskbarWindow.TabbedThumbnail.ParentWindowHandle != IntPtr.Zero && taskbarWindow.TabbedThumbnail.WindowHandle != IntPtr.Zero)
				{
					System.Drawing.Point offset = default(System.Drawing.Point);
					offset = (taskbarWindow.TabbedThumbnail.PeekOffset.HasValue ? new System.Drawing.Point(Convert.ToInt32(taskbarWindow.TabbedThumbnail.PeekOffset.Value.X), Convert.ToInt32(taskbarWindow.TabbedThumbnail.PeekOffset.Value.Y)) : WindowUtilities.GetParentOffsetOfChild(taskbarWindow.TabbedThumbnail.WindowHandle, taskbarWindow.TabbedThumbnail.ParentWindowHandle));
					if (intPtr != IntPtr.Zero && offset.X >= 0 && offset.Y >= 0)
					{
						TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, intPtr, offset, taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap);
					}
					if (taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
					{
						ShellNativeMethods.DeleteObject(intPtr);
					}
					return true;
				}
				if (taskbarWindow.TabbedThumbnail.ParentWindowHandle != IntPtr.Zero && taskbarWindow.TabbedThumbnail.WindowsControl != null)
				{
					System.Windows.Point point;
					if (!taskbarWindow.TabbedThumbnail.PeekOffset.HasValue)
					{
						GeneralTransform generalTransform = taskbarWindow.TabbedThumbnail.WindowsControl.TransformToVisual(taskbarWindow.TabbedThumbnail.WindowsControlParentWindow);
						point = generalTransform.Transform(new System.Windows.Point(0.0, 0.0));
					}
					else
					{
						point = new System.Windows.Point(taskbarWindow.TabbedThumbnail.PeekOffset.Value.X, taskbarWindow.TabbedThumbnail.PeekOffset.Value.Y);
					}
					if (intPtr != IntPtr.Zero)
					{
						if (point.X >= 0.0 && point.Y >= 0.0)
						{
							TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, intPtr, new System.Drawing.Point((int)point.X, (int)point.Y), taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap);
						}
						else
						{
							TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, intPtr, taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap);
						}
					}
					if (taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
					{
						ShellNativeMethods.DeleteObject(intPtr);
					}
					return true;
				}
				bool flag = 1 == 0;
				TabbedThumbnailNativeMethods.SetPeekBitmap(taskbarWindow.WindowToTellTaskbarAbout, intPtr, taskbarWindow.TabbedThumbnail.DisplayFrameAroundBitmap);
				if (taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
				{
					ShellNativeMethods.DeleteObject(intPtr);
				}
				return true;
			}
			return false;
		}

		private static bool DispatchDestroyMessage(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (m.Msg == 2)
			{
				TaskbarList.Instance.UnregisterTab(taskbarWindow.WindowToTellTaskbarAbout);
				taskbarWindow.TabbedThumbnail.RemovedFromTaskbar = true;
				return true;
			}
			return false;
		}

		private static bool DispatchNCDestroyMessage(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (m.Msg == 130)
			{
				taskbarWindow.TabbedThumbnail.OnTabbedThumbnailClosed();
				if (_taskbarWindowList.Contains(taskbarWindow))
				{
					_taskbarWindowList.Remove(taskbarWindow);
				}
				taskbarWindow.Dispose();
				return true;
			}
			return false;
		}

		private static bool DispatchSystemCommandMessage(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (m.Msg == 274)
			{
				if ((int)m.WParam == 61536)
				{
					if (taskbarWindow.TabbedThumbnail.OnTabbedThumbnailClosed())
					{
						if (_taskbarWindowList.Contains(taskbarWindow))
						{
							_taskbarWindowList.Remove(taskbarWindow);
						}
						taskbarWindow.Dispose();
						taskbarWindow = null;
					}
				}
				else if ((int)m.WParam == 61488)
				{
					taskbarWindow.TabbedThumbnail.OnTabbedThumbnailMaximized();
				}
				else if ((int)m.WParam == 61472)
				{
					taskbarWindow.TabbedThumbnail.OnTabbedThumbnailMinimized();
				}
				return true;
			}
			return false;
		}

		internal static bool DispatchMessage(ref Message m, TaskbarWindow taskbarWindow)
		{
			if (taskbarWindow.EnableThumbnailToolbars)
			{
				DispatchTaskbarButtonMessages(ref m, taskbarWindow);
			}
			if (taskbarWindow.EnableTabbedThumbnails)
			{
				if (taskbarWindow.TabbedThumbnail == null || taskbarWindow.TabbedThumbnail.RemovedFromTaskbar)
				{
					return false;
				}
				if (DispatchActivateMessage(ref m, taskbarWindow))
				{
					return true;
				}
				if (DispatchSendIconThumbnailMessage(ref m, taskbarWindow))
				{
					return true;
				}
				if (DispatchLivePreviewBitmapMessage(ref m, taskbarWindow))
				{
					return true;
				}
				if (DispatchDestroyMessage(ref m, taskbarWindow))
				{
					return true;
				}
				if (DispatchNCDestroyMessage(ref m, taskbarWindow))
				{
					return true;
				}
				if (DispatchSystemCommandMessage(ref m, taskbarWindow))
				{
					return true;
				}
			}
			return false;
		}

		private static IntPtr GrabBitmap(TaskbarWindow taskbarWindow, System.Drawing.Size requestedSize)
		{
			IntPtr result = IntPtr.Zero;
			if (taskbarWindow.TabbedThumbnail.WindowHandle != IntPtr.Zero)
			{
				if (taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
				{
					using (Bitmap bitmap = TabbedThumbnailScreenCapture.GrabWindowBitmap(taskbarWindow.TabbedThumbnail.WindowHandle, requestedSize))
					{
						result = bitmap.GetHbitmap();
					}
				}
				else
				{
					using (Image original = Image.FromHbitmap(taskbarWindow.TabbedThumbnail.CurrentHBitmap))
					{
						using (Bitmap bitmap = new Bitmap(original, requestedSize))
						{
							result = bitmap?.GetHbitmap() ?? IntPtr.Zero;
						}
					}
				}
			}
			else if (taskbarWindow.TabbedThumbnail.WindowsControl != null)
			{
				if (taskbarWindow.TabbedThumbnail.CurrentHBitmap == IntPtr.Zero)
				{
					Bitmap bitmap2 = TabbedThumbnailScreenCapture.GrabWindowBitmap(taskbarWindow.TabbedThumbnail.WindowsControl, 96, 96, requestedSize.Width, requestedSize.Height);
					if (bitmap2 != null)
					{
						result = bitmap2.GetHbitmap();
						bitmap2.Dispose();
					}
				}
				else
				{
					using (Image original = Image.FromHbitmap(taskbarWindow.TabbedThumbnail.CurrentHBitmap))
					{
						using (Bitmap bitmap = new Bitmap(original, requestedSize))
						{
							result = bitmap?.GetHbitmap() ?? IntPtr.Zero;
						}
					}
				}
			}
			return result;
		}

		internal static void SetActiveTab(TaskbarWindow taskbarWindow)
		{
			if (taskbarWindow != null)
			{
				TaskbarList.Instance.SetTabActive(taskbarWindow.WindowToTellTaskbarAbout, taskbarWindow.TabbedThumbnail.ParentWindowHandle, 0u);
			}
		}

		internal static void UnregisterTab(TaskbarWindow taskbarWindow)
		{
			if (taskbarWindow != null)
			{
				TaskbarList.Instance.UnregisterTab(taskbarWindow.WindowToTellTaskbarAbout);
			}
		}

		internal static void InvalidatePreview(TaskbarWindow taskbarWindow)
		{
			if (taskbarWindow != null)
			{
				TabbedThumbnailNativeMethods.DwmInvalidateIconicBitmaps(taskbarWindow.WindowToTellTaskbarAbout);
			}
		}

		private static void AddButtons(TaskbarWindow taskbarWindow)
		{
			ThumbButton[] pButtons = taskbarWindow.ThumbnailButtons.Select((ThumbnailToolBarButton thumbButton) => thumbButton.Win32ThumbButton).ToArray();
			HResult result = TaskbarList.Instance.ThumbBarAddButtons(taskbarWindow.WindowToTellTaskbarAbout, (uint)taskbarWindow.ThumbnailButtons.Length, pButtons);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ShellException(result);
			}
			_buttonsAdded = true;
			ThumbnailToolBarButton[] thumbnailButtons = taskbarWindow.ThumbnailButtons;
			foreach (ThumbnailToolBarButton thumbnailToolBarButton in thumbnailButtons)
			{
				thumbnailToolBarButton.AddedToTaskbar = _buttonsAdded;
			}
		}

		private static void thumbnailPreview_TooltipChanged(object sender, EventArgs e)
		{
			TabbedThumbnail tabbedThumbnail = sender as TabbedThumbnail;
			TaskbarWindow taskbarWindow = null;
			taskbarWindow = ((!(tabbedThumbnail.WindowHandle == IntPtr.Zero)) ? GetTaskbarWindow(tabbedThumbnail.WindowHandle, TaskbarProxyWindowType.TabbedThumbnail) : GetTaskbarWindow(tabbedThumbnail.WindowsControl, TaskbarProxyWindowType.TabbedThumbnail));
			if (taskbarWindow != null)
			{
				TaskbarList.Instance.SetThumbnailTooltip(taskbarWindow.WindowToTellTaskbarAbout, tabbedThumbnail.Tooltip);
			}
		}

		private static void thumbnailPreview_TitleChanged(object sender, EventArgs e)
		{
			TabbedThumbnail tabbedThumbnail = sender as TabbedThumbnail;
			TaskbarWindow taskbarWindow = null;
			((!(tabbedThumbnail.WindowHandle == IntPtr.Zero)) ? GetTaskbarWindow(tabbedThumbnail.WindowHandle, TaskbarProxyWindowType.TabbedThumbnail) : GetTaskbarWindow(tabbedThumbnail.WindowsControl, TaskbarProxyWindowType.TabbedThumbnail))?.SetTitle(tabbedThumbnail.Title);
		}
	}
}
