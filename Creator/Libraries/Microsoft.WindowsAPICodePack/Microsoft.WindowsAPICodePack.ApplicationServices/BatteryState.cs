using System;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	public class BatteryState
	{
		public bool ACOnline { get; private set; }

		public int MaxCharge { get; private set; }

		public int CurrentCharge { get; private set; }

		public int ChargeRate { get; private set; }

		public TimeSpan EstimatedTimeRemaining { get; private set; }

		public int SuggestedCriticalBatteryCharge { get; private set; }

		public int SuggestedBatteryWarningCharge { get; private set; }

		internal BatteryState()
		{
			PowerManagementNativeMethods.SystemBatteryState systemBatteryState = Power.GetSystemBatteryState();
			if (!systemBatteryState.BatteryPresent)
			{
				throw new InvalidOperationException(LocalizedMessages.PowerManagerBatteryNotPresent);
			}
			ACOnline = systemBatteryState.AcOnLine;
			MaxCharge = (int)systemBatteryState.MaxCapacity;
			CurrentCharge = (int)systemBatteryState.RemainingCapacity;
			ChargeRate = (int)systemBatteryState.Rate;
			uint estimatedTime = systemBatteryState.EstimatedTime;
			if (estimatedTime != uint.MaxValue)
			{
				EstimatedTimeRemaining = new TimeSpan(0, 0, (int)estimatedTime);
			}
			else
			{
				EstimatedTimeRemaining = TimeSpan.MaxValue;
			}
			SuggestedCriticalBatteryCharge = (int)systemBatteryState.DefaultAlert1;
			SuggestedBatteryWarningCharge = (int)systemBatteryState.DefaultAlert2;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.BatteryStateStringRepresentation, Environment.NewLine, ACOnline, MaxCharge, CurrentCharge, ChargeRate, EstimatedTimeRemaining, SuggestedCriticalBatteryCharge, SuggestedBatteryWarningCharge);
		}
	}
}
