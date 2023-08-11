using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class TaskDialogHyperlinkClickedEventArgs : EventArgs
	{
		public string LinkText { get; set; }

		public TaskDialogHyperlinkClickedEventArgs(string linkText)
		{
			LinkText = linkText;
		}
	}
}
