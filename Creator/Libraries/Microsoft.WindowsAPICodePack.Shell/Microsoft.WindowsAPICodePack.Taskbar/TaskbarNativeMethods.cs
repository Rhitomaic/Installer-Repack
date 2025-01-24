using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Taskbar
{
	internal static class TaskbarNativeMethods
	{
		internal static class TaskbarGuids
		{
			internal static Guid IObjectArray = new Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9");

			internal static Guid IUnknown = new Guid("00000000-0000-0000-C000-000000000046");
		}

		internal const int WmCommand = 273;

		internal const uint WmDwmSendIconThumbnail = 803u;

		internal const uint WmDwmSendIconicLivePreviewBitmap = 806u;

		internal static readonly uint WmTaskbarButtonCreated = RegisterWindowMessage("TaskbarButtonCreated");

		[DllImport("shell32.dll")]
		internal static extern void SetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] string AppID);

		[DllImport("shell32.dll")]
		internal static extern void GetCurrentProcessExplicitAppUserModelID([MarshalAs(UnmanagedType.LPWStr)] out string AppID);

		[DllImport("shell32.dll")]
		internal static extern void SHAddToRecentDocs(ShellAddToRecentDocs flags, [MarshalAs(UnmanagedType.LPWStr)] string path);

		internal static void SHAddToRecentDocs(string path)
		{
			SHAddToRecentDocs(ShellAddToRecentDocs.PathW, path);
		}

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

		[DllImport("shell32.dll")]
		public static extern int SHGetPropertyStoreForWindow(IntPtr hwnd, ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out IPropertyStore propertyStore);

		internal static void SetWindowAppId(IntPtr hwnd, string appId)
		{
			SetWindowProperty(hwnd, SystemProperties.System.AppUserModel.ID, appId);
		}

		internal static void SetWindowProperty(IntPtr hwnd, PropertyKey propkey, string value)
		{
			IPropertyStore windowPropertyStore = GetWindowPropertyStore(hwnd);
			using (PropVariant pv = new PropVariant(value))
			{
				HResult result = windowPropertyStore.SetValue(ref propkey, pv);
				if (!CoreErrorHelper.Succeeded(result))
				{
					throw new ShellException(result);
				}
			}
			Marshal.ReleaseComObject(windowPropertyStore);
		}

		internal static IPropertyStore GetWindowPropertyStore(IntPtr hwnd)
		{
			Guid iid = new Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99");
			IPropertyStore propertyStore;
			int num = SHGetPropertyStoreForWindow(hwnd, ref iid, out propertyStore);
			if (num != 0)
			{
				throw Marshal.GetExceptionForHR(num);
			}
			return propertyStore;
		}
	}
}
