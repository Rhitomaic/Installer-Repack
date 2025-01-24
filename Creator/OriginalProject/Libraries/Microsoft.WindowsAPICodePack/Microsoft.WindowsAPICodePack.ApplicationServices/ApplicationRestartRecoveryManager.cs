using System;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	public static class ApplicationRestartRecoveryManager
	{
		public static void RegisterForApplicationRecovery(RecoverySettings settings)
		{
			CoreHelpers.ThrowIfNotVista();
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			GCHandle gCHandle = GCHandle.Alloc(settings.RecoveryData);
			HResult hResult = AppRestartRecoveryNativeMethods.RegisterApplicationRecoveryCallback(AppRestartRecoveryNativeMethods.InternalCallback, (IntPtr)gCHandle, settings.PingInterval, 0u);
			if (!CoreErrorHelper.Succeeded(hResult))
			{
				if (hResult == HResult.InvalidArguments)
				{
					throw new ArgumentException(LocalizedMessages.ApplicationRecoveryBadParameters, "settings");
				}
				throw new ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToRegister);
			}
		}

		public static void UnregisterApplicationRecovery()
		{
			CoreHelpers.ThrowIfNotVista();
			HResult result = AppRestartRecoveryNativeMethods.UnregisterApplicationRecoveryCallback();
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToUnregister);
			}
		}

		public static void UnregisterApplicationRestart()
		{
			CoreHelpers.ThrowIfNotVista();
			HResult result = AppRestartRecoveryNativeMethods.UnregisterApplicationRestart();
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new ApplicationRecoveryException(LocalizedMessages.ApplicationRecoveryFailedToUnregisterForRestart);
			}
		}

		public static bool ApplicationRecoveryInProgress()
		{
			CoreHelpers.ThrowIfNotVista();
			bool canceled = false;
			HResult result = AppRestartRecoveryNativeMethods.ApplicationRecoveryInProgress(out canceled);
			if (!CoreErrorHelper.Succeeded(result))
			{
				throw new InvalidOperationException(LocalizedMessages.ApplicationRecoveryMustBeCalledFromCallback);
			}
			return canceled;
		}

		public static void ApplicationRecoveryFinished(bool success)
		{
			CoreHelpers.ThrowIfNotVista();
			AppRestartRecoveryNativeMethods.ApplicationRecoveryFinished(success);
		}

		public static void RegisterForApplicationRestart(RestartSettings settings)
		{
			CoreHelpers.ThrowIfNotVista();
			if (settings == null)
			{
				throw new ArgumentNullException("settings");
			}
			switch (AppRestartRecoveryNativeMethods.RegisterApplicationRestart(settings.Command, settings.Restrictions))
			{
			case HResult.Fail:
				throw new InvalidOperationException(LocalizedMessages.ApplicationRecoveryFailedToRegisterForRestart);
			case HResult.InvalidArguments:
				throw new ArgumentException(LocalizedMessages.ApplicationRecoverFailedToRegisterForRestartBadParameters);
			}
		}
	}
}
