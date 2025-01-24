using System;
using System.Drawing;
using Microsoft.WindowsAPICodePack.Shell;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public sealed class ThumbnailToolBarButton : IDisposable
	{
		private static uint nextId = 101u;

		private ThumbButton win32ThumbButton;

		private bool internalUpdate = false;

		private Icon icon;

		private string tooltip;

		private bool visible = true;

		private bool enabled = true;

		private bool dismissOnClick;

		private bool isInteractive = true;

		internal uint Id { get; set; }

		public Icon Icon
		{
			get
			{
				return icon;
			}
			set
			{
				if (icon != value)
				{
					icon = value;
					UpdateThumbnailButton();
				}
			}
		}

		public string Tooltip
		{
			get
			{
				return tooltip;
			}
			set
			{
				if (tooltip != value)
				{
					tooltip = value;
					UpdateThumbnailButton();
				}
			}
		}

		public bool Visible
		{
			get
			{
				return (Flags & ThumbButtonOptions.Hidden) == 0;
			}
			set
			{
				if (visible != value)
				{
					visible = value;
					if (value)
					{
						Flags &= ~ThumbButtonOptions.Hidden;
					}
					else
					{
						Flags |= ThumbButtonOptions.Hidden;
					}
					UpdateThumbnailButton();
				}
			}
		}

		public bool Enabled
		{
			get
			{
				return (Flags & ThumbButtonOptions.Disabled) == 0;
			}
			set
			{
				if (value != enabled)
				{
					enabled = value;
					if (value)
					{
						Flags &= ~ThumbButtonOptions.Disabled;
					}
					else
					{
						Flags |= ThumbButtonOptions.Disabled;
					}
					UpdateThumbnailButton();
				}
			}
		}

		public bool DismissOnClick
		{
			get
			{
				return (Flags & ThumbButtonOptions.DismissOnClick) == 0;
			}
			set
			{
				if (value != dismissOnClick)
				{
					dismissOnClick = value;
					if (value)
					{
						Flags |= ThumbButtonOptions.DismissOnClick;
					}
					else
					{
						Flags &= ~ThumbButtonOptions.DismissOnClick;
					}
					UpdateThumbnailButton();
				}
			}
		}

		public bool IsInteractive
		{
			get
			{
				return (Flags & ThumbButtonOptions.NonInteractive) == 0;
			}
			set
			{
				if (value != isInteractive)
				{
					isInteractive = value;
					if (value)
					{
						Flags &= ~ThumbButtonOptions.NonInteractive;
					}
					else
					{
						Flags |= ThumbButtonOptions.NonInteractive;
					}
					UpdateThumbnailButton();
				}
			}
		}

		internal ThumbButtonOptions Flags { get; set; }

		internal ThumbButton Win32ThumbButton
		{
			get
			{
				win32ThumbButton.Id = Id;
				win32ThumbButton.Tip = Tooltip;
				win32ThumbButton.Icon = ((Icon != null) ? Icon.Handle : IntPtr.Zero);
				win32ThumbButton.Flags = Flags;
				win32ThumbButton.Mask = ThumbButtonMask.THB_FLAGS;
				if (Tooltip != null)
				{
					win32ThumbButton.Mask |= ThumbButtonMask.Tooltip;
				}
				if (Icon != null)
				{
					win32ThumbButton.Mask |= ThumbButtonMask.Icon;
				}
				return win32ThumbButton;
			}
		}

		internal IntPtr WindowHandle { get; set; }

		internal bool AddedToTaskbar { get; set; }

		public event EventHandler<ThumbnailButtonClickedEventArgs> Click;

		public ThumbnailToolBarButton(Icon icon, string tooltip)
		{
			internalUpdate = true;
			Id = nextId;
			if (nextId == int.MaxValue)
			{
				nextId = 101u;
			}
			else
			{
				nextId++;
			}
			Icon = icon;
			Tooltip = tooltip;
			Enabled = true;
			win32ThumbButton = default(ThumbButton);
			internalUpdate = false;
		}

		internal void FireClick(TaskbarWindow taskbarWindow)
		{
			if (this.Click != null && taskbarWindow != null)
			{
				if (taskbarWindow.UserWindowHandle != IntPtr.Zero)
				{
					this.Click(this, new ThumbnailButtonClickedEventArgs(taskbarWindow.UserWindowHandle, this));
				}
				else if (taskbarWindow.WindowsControl != null)
				{
					this.Click(this, new ThumbnailButtonClickedEventArgs(taskbarWindow.WindowsControl, this));
				}
			}
		}

		internal void UpdateThumbnailButton()
		{
			if (!internalUpdate && AddedToTaskbar)
			{
				ThumbButton[] pButtons = new ThumbButton[1] { Win32ThumbButton };
				HResult result = TaskbarList.Instance.ThumbBarUpdateButtons(WindowHandle, 1u, pButtons);
				if (!CoreErrorHelper.Succeeded(result))
				{
					throw new ShellException(result);
				}
			}
		}

		~ThumbnailToolBarButton()
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
				Icon.Dispose();
				tooltip = null;
			}
		}
	}
}
