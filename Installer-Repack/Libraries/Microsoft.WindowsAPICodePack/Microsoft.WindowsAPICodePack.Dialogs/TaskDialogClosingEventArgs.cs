using System.ComponentModel;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class TaskDialogClosingEventArgs : CancelEventArgs
	{
		private TaskDialogResult taskDialogResult;

		private string customButton;

		public TaskDialogResult TaskDialogResult
		{
			get
			{
				return taskDialogResult;
			}
			set
			{
				taskDialogResult = value;
			}
		}

		public string CustomButton
		{
			get
			{
				return customButton;
			}
			set
			{
				customButton = value;
			}
		}
	}
}
