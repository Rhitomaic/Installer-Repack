using System;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	internal static class PowerManagementNativeMethods
	{
		[StructLayout(LayoutKind.Sequential, Pack = 4)]
		public struct PowerBroadcastSetting
		{
			public Guid PowerSetting;

			public int DataLength;
		}

		public struct SystemPowerCapabilities
		{
			[MarshalAs(UnmanagedType.I1)]
			public bool PowerButtonPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool SleepButtonPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool LidPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS1;

			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS2;

			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS3;

			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS4;

			[MarshalAs(UnmanagedType.I1)]
			public bool SystemS5;

			[MarshalAs(UnmanagedType.I1)]
			public bool HiberFilePresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool FullWake;

			[MarshalAs(UnmanagedType.I1)]
			public bool VideoDimPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool ApmPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool UpsPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool ThermalControl;

			[MarshalAs(UnmanagedType.I1)]
			public bool ProcessorThrottle;

			public byte ProcessorMinimumThrottle;

			public byte ProcessorMaximumThrottle;

			[MarshalAs(UnmanagedType.I1)]
			public bool FastSystemS4;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public byte[] spare2;

			[MarshalAs(UnmanagedType.I1)]
			public bool DiskSpinDown;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
			public byte[] spare3;

			[MarshalAs(UnmanagedType.I1)]
			public bool SystemBatteriesPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool BatteriesAreShortTerm;

			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
			public BatteryReportingScale[] BatteryScale;

			public SystemPowerState AcOnlineWake;

			public SystemPowerState SoftLidWake;

			public SystemPowerState RtcWake;

			public SystemPowerState MinimumDeviceWakeState;

			public SystemPowerState DefaultLowLatencyWake;
		}

		public enum PowerInformationLevel
		{
			SystemPowerPolicyAc,
			SystemPowerPolicyDc,
			VerifySystemPolicyAc,
			VerifySystemPolicyDc,
			SystemPowerCapabilities,
			SystemBatteryState,
			SystemPowerStateHandler,
			ProcessorStateHandler,
			SystemPowerPolicyCurrent,
			AdministratorPowerPolicy,
			SystemReserveHiberFile,
			ProcessorInformation,
			SystemPowerInformation,
			ProcessorStateHandler2,
			LastWakeTime,
			LastSleepTime,
			SystemExecutionState,
			SystemPowerStateNotifyHandler,
			ProcessorPowerPolicyAc,
			ProcessorPowerPolicyDc,
			VerifyProcessorPowerPolicyAc,
			VerifyProcessorPowerPolicyDc,
			ProcessorPowerPolicyCurrent,
			SystemPowerStateLogging,
			SystemPowerLoggingEntry,
			SetPowerSettingValue,
			NotifyUserPowerSetting,
			PowerInformationLevelUnused0,
			PowerInformationLevelUnused1,
			SystemVideoState,
			TraceApplicationPowerMessage,
			TraceApplicationPowerMessageEnd,
			ProcessorPerfStates,
			ProcessorIdleStates,
			ProcessorCap,
			SystemWakeSource,
			SystemHiberFileInformation,
			TraceServicePowerMessage,
			ProcessorLoad,
			PowerShutdownNotification,
			MonitorCapabilities,
			SessionPowerInit,
			SessionDisplayState,
			PowerRequestCreate,
			PowerRequestAction,
			GetPowerRequestList,
			ProcessorInformationEx,
			NotifyUserModeLegacyPowerEvent,
			GroupPark,
			ProcessorIdleDomains,
			WakeTimerList,
			SystemHiberFileSize,
			PowerInformationLevelMaximum
		}

		public struct BatteryReportingScale
		{
			public uint Granularity;

			public uint Capacity;
		}

		public enum SystemPowerState
		{
			Unspecified,
			Working,
			Sleeping1,
			Sleeping2,
			Sleeping3,
			Hibernate,
			Shutdown,
			Maximum
		}

		public struct SystemBatteryState
		{
			[MarshalAs(UnmanagedType.I1)]
			public bool AcOnLine;

			[MarshalAs(UnmanagedType.I1)]
			public bool BatteryPresent;

			[MarshalAs(UnmanagedType.I1)]
			public bool Charging;

			[MarshalAs(UnmanagedType.I1)]
			public bool Discharging;

			public byte Spare1;

			public byte Spare2;

			public byte Spare3;

			public byte Spare4;

			public uint MaxCapacity;

			public uint RemainingCapacity;

			public uint Rate;

			public uint EstimatedTime;

			public uint DefaultAlert1;

			public uint DefaultAlert2;
		}

		internal const uint PowerBroadcastMessage = 536u;

		internal const uint PowerSettingChangeMessage = 32787u;

		internal const uint ScreenSaverSetActive = 17u;

		internal const uint UpdateInFile = 1u;

		internal const uint SendChange = 2u;

		[DllImport("powrprof.dll")]
		internal static extern uint CallNtPowerInformation(PowerInformationLevel informationLevel, IntPtr inputBuffer, uint inputBufferSize, out SystemPowerCapabilities outputBuffer, uint outputBufferSize);

		[DllImport("powrprof.dll")]
		internal static extern uint CallNtPowerInformation(PowerInformationLevel informationLevel, IntPtr inputBuffer, uint inputBufferSize, out SystemBatteryState outputBuffer, uint outputBufferSize);

		[DllImport("powrprof.dll")]
		internal static extern void PowerGetActiveScheme(IntPtr rootPowerKey, [MarshalAs(UnmanagedType.LPStruct)] out Guid activePolicy);

		[DllImport("User32", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		internal static extern int RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, int Flags);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern ExecutionStates SetThreadExecutionState(ExecutionStates esFlags);
	}
}
