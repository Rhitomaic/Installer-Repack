namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellNonFileSystemItem : ShellObject
	{
		internal ShellNonFileSystemItem(IShellItem2 shellItem)
		{
			nativeShellItem = shellItem;
		}
	}
}
