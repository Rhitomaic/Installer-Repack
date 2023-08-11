using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class TaskbarManager
	{
		private static object _syncLock = new object();

		private static TaskbarManager _instance;

		private TabbedThumbnailManager _tabbedThumbnail;

		private ThumbnailToolBarManager _thumbnailToolBarManager;

		private IntPtr _ownerHandle;

		public static TaskbarManager Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_syncLock)
					{
						if (_instance == null)
						{
							_instance = new TaskbarManager();
						}
					}
				}
				return _instance;
			}
		}

		public TabbedThumbnailManager TabbedThumbnail
		{
			get
			{
				if (_tabbedThumbnail == null)
				{
					_tabbedThumbnail = new TabbedThumbnailManager();
				}
				return _tabbedThumbnail;
			}
		}

		public ThumbnailToolBarManager ThumbnailToolBars
		{
			get
			{
				if (_thumbnailToolBarManager == null)
				{
					_thumbnailToolBarManager = new ThumbnailToolBarManager();
				}
				return _thumbnailToolBarManager;
			}
		}

		public string ApplicationId
		{
			get
			{
				return GetCurrentProcessAppId();
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				SetCurrentProcessAppId(value);
				ApplicationIdSetProcessWide = true;
			}
		}

		internal IntPtr OwnerHandle
		{
			get
			{
				if (_ownerHandle == IntPtr.Zero)
				{
					Process currentProcess = Process.GetCurrentProcess();
					if (currentProcess == null || currentProcess.MainWindowHandle == IntPtr.Zero)
					{
						throw new InvalidOperationException(LocalizedMessages.TaskbarManagerValidWindowRequired);
					}
					_ownerHandle = currentProcess.MainWindowHandle;
				}
				return _ownerHandle;
			}
		}

		internal bool ApplicationIdSetProcessWide { get; private set; }

		public static bool IsPlatformSupported => CoreHelpers.RunningOnWin7;

		private TaskbarManager()
		{
			CoreHelpers.ThrowIfNotWin7();
		}

		public void SetOverlayIcon(Icon icon, string accessibilityText)
		{
			TaskbarList.Instance.SetOverlayIcon(OwnerHandle, icon?.Handle ?? IntPtr.Zero, accessibilityText);
		}

		public void SetOverlayIcon(IntPtr windowHandle, Icon icon, string accessibilityText)
		{
			TaskbarList.Instance.SetOverlayIcon(windowHandle, icon?.Handle ?? IntPtr.Zero, accessibilityText);
		}

		public void SetOverlayIcon(Window window, Icon icon, string accessibilityText)
		{
			TaskbarList.Instance.SetOverlayIcon(new WindowInteropHelper(window).Handle, icon?.Handle ?? IntPtr.Zero, accessibilityText);
		}

		public void SetProgressValue(int currentValue, int maximumValue)
		{
			TaskbarList.Instance.SetProgressValue(OwnerHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue));
		}

		public void SetProgressValue(int currentValue, int maximumValue, IntPtr windowHandle)
		{
			TaskbarList.Instance.SetProgressValue(windowHandle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue));
		}

		public void SetProgressValue(int currentValue, int maximumValue, Window window)
		{
			TaskbarList.Instance.SetProgressValue(new WindowInteropHelper(window).Handle, Convert.ToUInt32(currentValue), Convert.ToUInt32(maximumValue));
		}

		public void SetProgressState(TaskbarProgressBarState state)
		{
			TaskbarList.Instance.SetProgressState(OwnerHandle, (TaskbarProgressBarStatus)state);
		}

		public void SetProgressState(TaskbarProgressBarState state, IntPtr windowHandle)
		{
			TaskbarList.Instance.SetProgressState(windowHandle, (TaskbarProgressBarStatus)state);
		}

		public void SetProgressState(TaskbarProgressBarState state, Window window)
		{
			TaskbarList.Instance.SetProgressState(new WindowInteropHelper(window).Handle, (TaskbarProgressBarStatus)state);
		}

		public void SetApplicationIdForSpecificWindow(IntPtr windowHandle, string appId)
		{
			TaskbarNativeMethods.SetWindowAppId(windowHandle, appId);
		}

		public void SetApplicationIdForSpecificWindow(Window window, string appId)
		{
			TaskbarNativeMethods.SetWindowAppId(new WindowInteropHelper(window).Handle, appId);
		}

		private void SetCurrentProcessAppId(string appId)
		{
			TaskbarNativeMethods.SetCurrentProcessExplicitAppUserModelID(appId);
		}

		private string GetCurrentProcessAppId()
		{
			string AppID = string.Empty;
			TaskbarNativeMethods.GetCurrentProcessExplicitAppUserModelID(out AppID);
			return AppID;
		}
	}
}
