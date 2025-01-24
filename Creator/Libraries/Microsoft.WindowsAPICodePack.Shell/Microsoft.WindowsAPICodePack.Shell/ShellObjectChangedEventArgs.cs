namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellObjectChangedEventArgs : ShellObjectNotificationEventArgs
	{
		public string Path { get; private set; }

		internal ShellObjectChangedEventArgs(ChangeNotifyLock notifyLock)
			: base(notifyLock)
		{
			Path = notifyLock.ItemName;
		}
	}
}
