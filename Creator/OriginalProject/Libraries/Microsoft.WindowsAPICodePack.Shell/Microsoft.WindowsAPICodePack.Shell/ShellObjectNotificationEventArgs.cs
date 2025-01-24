using System;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellObjectNotificationEventArgs : EventArgs
	{
		public ShellObjectChangeTypes ChangeType { get; private set; }

		public bool FromSystemInterrupt { get; private set; }

		internal ShellObjectNotificationEventArgs(ChangeNotifyLock notifyLock)
		{
			ChangeType = notifyLock.ChangeType;
			FromSystemInterrupt = notifyLock.FromSystemInterrupt;
		}
	}
}
