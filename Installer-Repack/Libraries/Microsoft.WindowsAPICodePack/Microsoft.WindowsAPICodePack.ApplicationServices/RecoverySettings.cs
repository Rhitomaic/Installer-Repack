using System.Globalization;
using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.ApplicationServices
{
	public class RecoverySettings
	{
		private RecoveryData recoveryData;

		private uint pingInterval;

		public RecoveryData RecoveryData => recoveryData;

		public uint PingInterval => pingInterval;

		public RecoverySettings(RecoveryData data, uint interval)
		{
			recoveryData = data;
			pingInterval = interval;
		}

		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, LocalizedMessages.RecoverySettingsFormatString, recoveryData.Callback.Method.ToString(), recoveryData.State.ToString(), PingInterval);
		}
	}
}
