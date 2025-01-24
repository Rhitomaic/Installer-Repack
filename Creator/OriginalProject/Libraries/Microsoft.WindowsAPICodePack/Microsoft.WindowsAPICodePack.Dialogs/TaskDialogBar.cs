namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class TaskDialogBar : TaskDialogControl
	{
		private TaskDialogProgressBarState state;

		public TaskDialogProgressBarState State
		{
			get
			{
				return state;
			}
			set
			{
				CheckPropertyChangeAllowed("State");
				state = value;
				ApplyPropertyChange("State");
			}
		}

		public TaskDialogBar()
		{
		}

		protected TaskDialogBar(string name)
			: base(name)
		{
		}

		protected internal virtual void Reset()
		{
			state = TaskDialogProgressBarState.Normal;
		}
	}
}
