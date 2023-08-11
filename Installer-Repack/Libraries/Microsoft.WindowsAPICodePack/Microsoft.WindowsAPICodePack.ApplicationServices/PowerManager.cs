using System;
using System.ComponentModel;
using System.Security.Permissions;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	public static class PowerManager
	{
		private static bool? isMonitorOn;

		private static bool monitorRequired;

		private static bool requestBlockSleep;

		private static readonly object monitoronlock = new object();

		public static bool MonitorRequired
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return monitorRequired;
			}
			[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
			set
			{
				CoreHelpers.ThrowIfNotXP();
				if (value)
				{
					SetThreadExecutionState(ExecutionStates.Continuous | ExecutionStates.DisplayRequired);
				}
				else
				{
					SetThreadExecutionState(ExecutionStates.Continuous);
				}
				monitorRequired = value;
			}
		}

		public static bool RequestBlockSleep
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return requestBlockSleep;
			}
			[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
			set
			{
				CoreHelpers.ThrowIfNotXP();
				if (value)
				{
					SetThreadExecutionState(ExecutionStates.Continuous | ExecutionStates.SystemRequired);
				}
				else
				{
					SetThreadExecutionState(ExecutionStates.Continuous);
				}
				requestBlockSleep = value;
			}
		}

		public static bool IsBatteryPresent
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return Power.GetSystemBatteryState().BatteryPresent;
			}
		}

		public static bool IsBatteryShortTerm
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				return Power.GetSystemPowerCapabilities().BatteriesAreShortTerm;
			}
		}

		public static bool IsUpsPresent
		{
			get
			{
				CoreHelpers.ThrowIfNotXP();
				PowerManagementNativeMethods.SystemPowerCapabilities systemPowerCapabilities = Power.GetSystemPowerCapabilities();
				return systemPowerCapabilities.BatteriesAreShortTerm && systemPowerCapabilities.SystemBatteriesPresent;
			}
		}

		public static PowerPersonality PowerPersonality
		{
			get
			{
				PowerManagementNativeMethods.PowerGetActiveScheme(IntPtr.Zero, out var activePolicy);
				try
				{
					return PowerPersonalityGuids.GuidToEnum(activePolicy);
				}
				finally
				{
					CoreNativeMethods.LocalFree(ref activePolicy);
				}
			}
		}

		public static int BatteryLifePercent
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				if (!Power.GetSystemBatteryState().BatteryPresent)
				{
					throw new InvalidOperationException(LocalizedMessages.PowerManagerBatteryNotPresent);
				}
				PowerManagementNativeMethods.SystemBatteryState systemBatteryState = Power.GetSystemBatteryState();
				return (int)Math.Round((double)systemBatteryState.RemainingCapacity / (double)systemBatteryState.MaxCapacity * 100.0, 0);
			}
		}

		public static bool IsMonitorOn
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				lock (monitoronlock)
				{
					if (!isMonitorOn.HasValue)
					{
						EventHandler value = delegate
						{
						};
						IsMonitorOnChanged += value;
						EventManager.monitorOnReset.WaitOne();
					}
				}
				return isMonitorOn.Value;
			}
			internal set
			{
				isMonitorOn = value;
			}
		}

		public static PowerSource PowerSource
		{
			get
			{
				CoreHelpers.ThrowIfNotVista();
				if (IsUpsPresent)
				{
					return PowerSource.Ups;
				}
				if (!IsBatteryPresent || GetCurrentBatteryState().ACOnline)
				{
					return PowerSource.AC;
				}
				return PowerSource.Battery;
			}
		}

		public static event EventHandler PowerPersonalityChanged
		{
			add
			{
				MessageManager.RegisterPowerEvent(EventManager.PowerPersonalityChange, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.PowerPersonalityChange, value);
			}
		}

		public static event EventHandler PowerSourceChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.PowerSourceChange, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.PowerSourceChange, value);
			}
		}

		public static event EventHandler BatteryLifePercentChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.BatteryCapacityChange, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.BatteryCapacityChange, value);
			}
		}

		public static event EventHandler IsMonitorOnChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.MonitorPowerStatus, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.MonitorPowerStatus, value);
			}
		}

		public static event EventHandler SystemBusyChanged
		{
			add
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.RegisterPowerEvent(EventManager.BackgroundTaskNotification, value);
			}
			remove
			{
				CoreHelpers.ThrowIfNotVista();
				MessageManager.UnregisterPowerEvent(EventManager.BackgroundTaskNotification, value);
			}
		}

		public static BatteryState GetCurrentBatteryState()
		{
			CoreHelpers.ThrowIfNotXP();
			return new BatteryState();
		}

		public static void SetThreadExecutionState(ExecutionStates executionStateOptions)
		{
			if (PowerManagementNativeMethods.SetThreadExecutionState(executionStateOptions) == ExecutionStates.None)
			{
				throw new Win32Exception(LocalizedMessages.PowerExecutionStateFailed);
			}
		}
	}
}
