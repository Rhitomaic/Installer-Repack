using System.ComponentModel;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class CommonFileDialogFolderChangeEventArgs : CancelEventArgs
	{
		public string Folder { get; set; }

		public CommonFileDialogFolderChangeEventArgs(string folder)
		{
			Folder = folder;
		}
	}
}
