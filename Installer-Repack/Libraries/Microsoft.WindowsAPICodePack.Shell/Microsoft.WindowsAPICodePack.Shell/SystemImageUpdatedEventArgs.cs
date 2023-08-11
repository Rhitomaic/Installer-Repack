namespace Microsoft.WindowsAPICodePack.Shell
{
	public class SystemImageUpdatedEventArgs : ShellObjectNotificationEventArgs
	{
		public int ImageIndex { get; private set; }

		internal SystemImageUpdatedEventArgs(ChangeNotifyLock notifyLock)
			: base(notifyLock)
		{
			ImageIndex = notifyLock.ImageIndex;
		}
	}
}
