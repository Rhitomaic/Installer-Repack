using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	public class ThumbnailToolBarManager
	{
		internal ThumbnailToolBarManager()
		{
		}

		public void AddButtons(IntPtr windowHandle, params ThumbnailToolBarButton[] buttons)
		{
			if (windowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailManagerInvalidHandle, "windowHandle");
			}
			VerifyButtons(buttons);
			TaskbarWindowManager.AddThumbnailButtons(windowHandle, buttons);
		}

		public void AddButtons(UIElement control, params ThumbnailToolBarButton[] buttons)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			VerifyButtons(buttons);
			TaskbarWindowManager.AddThumbnailButtons(control, buttons);
		}

		private static void VerifyButtons(params ThumbnailToolBarButton[] buttons)
		{
			if (buttons != null && buttons.Length == 0)
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailToolbarManagerNullEmptyArray, "buttons");
			}
			if (buttons.Length > 7)
			{
				throw new ArgumentException(LocalizedMessages.ThumbnailToolbarManagerMaxButtons, "buttons");
			}
		}
	}
}
