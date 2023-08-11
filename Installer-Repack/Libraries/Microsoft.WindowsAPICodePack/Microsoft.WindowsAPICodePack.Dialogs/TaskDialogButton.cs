namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public class TaskDialogButton : TaskDialogButtonBase
	{
		private bool useElevationIcon;

		public bool UseElevationIcon
		{
			get
			{
				return useElevationIcon;
			}
			set
			{
				CheckPropertyChangeAllowed("ShowElevationIcon");
				useElevationIcon = value;
				ApplyPropertyChange("ShowElevationIcon");
			}
		}

		public TaskDialogButton()
		{
		}

		public TaskDialogButton(string name, string text)
			: base(name, text)
		{
		}
	}
}
