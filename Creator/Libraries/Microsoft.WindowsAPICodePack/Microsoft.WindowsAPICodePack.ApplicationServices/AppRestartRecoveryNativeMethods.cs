using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	internal static class AppRestartRecoveryNativeMethods
	{
		internal delegate uint InternalRecoveryCallback(IntPtr state);

		private static InternalRecoveryCallback internalCallback = InternalRecoveryHandler;

		internal static InternalRecoveryCallback InternalCallback => internalCallback;

		private static uint InternalRecoveryHandler(IntPtr parameter)
		{
			bool canceled = false;
			ApplicationRecoveryInProgress(out canceled);
			GCHandle gCHandle = GCHandle.FromIntPtr(parameter);
			RecoveryData recoveryData = gCHandle.Target as RecoveryData;
			recoveryData.Invoke();
			gCHandle.Free();
			return 0u;
		}

		[DllImport("kernel32.dll")]
		internal static extern void ApplicationRecoveryFinished([MarshalAs(UnmanagedType.Bool)] bool success);

		[DllImport("kernel32.dll")]
		internal static extern HResult ApplicationRecoveryInProgress([MarshalAs(UnmanagedType.Bool)] out bool canceled);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern HResult RegisterApplicationRecoveryCallback(InternalRecoveryCallback callback, IntPtr param, uint pingInterval, uint flags);

		[DllImport("kernel32.dll")]
		internal static extern HResult RegisterApplicationRestart([MarshalAs(UnmanagedType.BStr)] string commandLineArgs, RestartRestrictions flags);

		[DllImport("kernel32.dll")]
		internal static extern HResult UnregisterApplicationRecoveryCallback();

		[DllImport("kernel32.dll")]
		internal static extern HResult UnregisterApplicationRestart();
	}
}
