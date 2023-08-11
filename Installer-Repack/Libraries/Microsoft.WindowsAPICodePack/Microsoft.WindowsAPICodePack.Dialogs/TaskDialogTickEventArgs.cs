using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class TaskDialogTickEventArgs : EventArgs
	{
		public int Ticks { get; private set; }

		public TaskDialogTickEventArgs(int ticks)
		{
			Ticks = ticks;
		}
	}
}
