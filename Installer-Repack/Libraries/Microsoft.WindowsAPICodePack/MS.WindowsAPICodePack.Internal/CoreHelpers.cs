using System;
using System.Globalization;
using System.Text;
using Microsoft.WindowsAPICodePack.Resources;

namespace MS.WindowsAPICodePack.Internal
{
	public static class CoreHelpers
	{
		public static bool RunningOnXP => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major >= 5;

		public static bool RunningOnVista => Environment.OSVersion.Version.Major >= 6;

		public static bool RunningOnWin7 => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.CompareTo(new Version(6, 1)) >= 0;

		public static void ThrowIfNotXP()
		{
			if (!RunningOnXP)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.CoreHelpersRunningOnXp);
			}
		}

		public static void ThrowIfNotVista()
		{
			if (!RunningOnVista)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.CoreHelpersRunningOnVista);
			}
		}

		public static void ThrowIfNotWin7()
		{
			if (!RunningOnWin7)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.CoreHelpersRunningOn7);
			}
		}

		public static string GetStringResource(string resourceId)
		{
			if (string.IsNullOrEmpty(resourceId))
			{
				return string.Empty;
			}
			resourceId = resourceId.Replace("shell32,dll", "shell32.dll");
			string[] array = resourceId.Split(',');
			string text = array[0];
			text = text.Replace("@", string.Empty);
			text = Environment.ExpandEnvironmentVariables(text);
			IntPtr instanceHandle = CoreNativeMethods.LoadLibrary(text);
			array[1] = array[1].Replace("-", string.Empty);
			int id = int.Parse(array[1], CultureInfo.InvariantCulture);
			StringBuilder stringBuilder = new StringBuilder(255);
			return (CoreNativeMethods.LoadString(instanceHandle, id, stringBuilder, 255) != 0) ? stringBuilder.ToString() : null;
		}
	}
}
