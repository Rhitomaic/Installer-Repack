namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellNonFileSystemFolder : ShellFolder
	{
		internal ShellNonFileSystemFolder()
		{
		}

		internal ShellNonFileSystemFolder(IShellItem2 shellItem)
		{
			nativeShellItem = shellItem;
		}
	}
}
