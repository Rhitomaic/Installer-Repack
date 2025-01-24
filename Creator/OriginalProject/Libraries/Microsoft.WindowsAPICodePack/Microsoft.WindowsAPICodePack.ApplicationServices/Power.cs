using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	internal static class Power
	{
		internal static PowerManagementNativeMethods.SystemPowerCapabilities GetSystemPowerCapabilities()
		{
			PowerManagementNativeMethods.SystemPowerCapabilities outputBuffer;
			uint num = PowerManagementNativeMethods.CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel.SystemPowerCapabilities, IntPtr.Zero, 0u, out outputBuffer, (uint)Marshal.SizeOf(typeof(PowerManagementNativeMethods.SystemPowerCapabilities)));
			if (num == 3221225506u)
			{
				throw new UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessCapabilities);
			}
			return outputBuffer;
		}

		internal static PowerManagementNativeMethods.SystemBatteryState GetSystemBatteryState()
		{
			PowerManagementNativeMethods.SystemBatteryState outputBuffer;
			uint num = PowerManagementNativeMethods.CallNtPowerInformation(PowerManagementNativeMethods.PowerInformationLevel.SystemBatteryState, IntPtr.Zero, 0u, out outputBuffer, (uint)Marshal.SizeOf(typeof(PowerManagementNativeMethods.SystemBatteryState)));
			if (num == 3221225506u)
			{
				throw new UnauthorizedAccessException(LocalizedMessages.PowerInsufficientAccessBatteryState);
			}
			return outputBuffer;
		}

		internal static int RegisterPowerSettingNotification(IntPtr handle, Guid powerSetting)
		{
			return PowerManagementNativeMethods.RegisterPowerSettingNotification(handle, ref powerSetting, 0);
		}
	}
}
