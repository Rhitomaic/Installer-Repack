using System.Globalization;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellFileSystemFolder : ShellFolder
	{
		public virtual string Path => ParsingName;

		internal ShellFileSystemFolder()
		{
		}

		internal ShellFileSystemFolder(IShellItem2 shellItem)
		{
			nativeShellItem = shellItem;
		}

		public static ShellFileSystemFolder FromFolderPath(string path)
		{
			string absolutePath = ShellHelper.GetAbsolutePath(path);
			if (!Directory.Exists(absolutePath))
			{
				throw new DirectoryNotFoundException(string.Format(CultureInfo.InvariantCulture, LocalizedMessages.FilePathNotExist, path));
			}
			ShellFileSystemFolder shellFileSystemFolder = new ShellFileSystemFolder();
			try
			{
				shellFileSystemFolder.ParsingName = absolutePath;
				return shellFileSystemFolder;
			}
			catch
			{
				shellFileSystemFolder.Dispose();
				throw;
			}
		}
	}
}
