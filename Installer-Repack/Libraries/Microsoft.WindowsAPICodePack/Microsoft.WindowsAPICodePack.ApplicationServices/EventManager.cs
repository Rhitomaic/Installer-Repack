using System;
using System.Threading;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	internal static class EventManager
	{
		internal static AutoResetEvent monitorOnReset = new AutoResetEvent(false);

		internal static readonly Guid PowerPersonalityChange = new Guid(610108737, 14659, 17442, 176, 37, 19, 167, 132, 246, 121, 183);

		internal static readonly Guid PowerSourceChange = new Guid(1564383833u, 59861, 19200, 166, 189, byte.MaxValue, 52, byte.MaxValue, 81, 101, 72);

		internal static readonly Guid BatteryCapacityChange = new Guid(2813165633u, 46170, 19630, 135, 163, 238, 203, 180, 104, 169, 225);

		internal static readonly Guid BackgroundTaskNotification = new Guid(1364996568u, 63284, 5693, 160, 253, 17, 160, 140, 145, 232, 241);

		internal static readonly Guid MonitorPowerStatus = new Guid(41095189, 17680, 17702, 153, 230, 229, 161, 126, 189, 26, 234);

		private static bool personalityCaught;

		private static bool powerSrcCaught;

		private static bool batteryLifeCaught;

		private static bool monitorOnCaught;

		internal static bool IsMessageCaught(Guid eventGuid)
		{
			bool result = false;
			if (eventGuid == BatteryCapacityChange)
			{
				if (!batteryLifeCaught)
				{
					batteryLifeCaught = true;
					result = true;
				}
			}
			else if (eventGuid == MonitorPowerStatus)
			{
				if (!monitorOnCaught)
				{
					monitorOnCaught = true;
					result = true;
				}
			}
			else if (eventGuid == PowerPersonalityChange)
			{
				if (!personalityCaught)
				{
					personalityCaught = true;
					result = true;
				}
			}
			else if (eventGuid == PowerSourceChange && !powerSrcCaught)
			{
				powerSrcCaught = true;
				result = true;
			}
			return result;
		}
	}
}
