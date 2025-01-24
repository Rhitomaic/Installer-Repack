using System.Globalization;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public class ShellFile : ShellObject
	{
		public virtual string Path => ParsingName;

		internal ShellFile(string path)
		{
			string absolutePath = ShellHelper.GetAbsolutePath(path);
			if (!File.Exists(absolutePath))
			{
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, LocalizedMessages.FilePathNotExist, path));
			}
			ParsingName = absolutePath;
		}

		internal ShellFile(IShellItem2 shellItem)
		{
			nativeShellItem = shellItem;
		}

		public static ShellFile FromFilePath(string path)
		{
			return new ShellFile(path);
		}
	}
}
