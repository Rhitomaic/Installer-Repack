namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellObjectRenamedEventArgs : ShellObjectChangedEventArgs
	{
		public string NewPath { get; private set; }

		internal ShellObjectRenamedEventArgs(ChangeNotifyLock notifyLock)
			: base(notifyLock)
		{
			NewPath = notifyLock.ItemName2;
		}
	}
}
