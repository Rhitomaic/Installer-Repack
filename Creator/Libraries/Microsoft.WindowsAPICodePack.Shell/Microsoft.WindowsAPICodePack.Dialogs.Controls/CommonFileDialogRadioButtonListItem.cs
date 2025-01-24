namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public class CommonFileDialogRadioButtonListItem
	{
		public string Text { get; set; }

		public CommonFileDialogRadioButtonListItem()
			: this(string.Empty)
		{
		}

		public CommonFileDialogRadioButtonListItem(string text)
		{
			Text = text;
		}
	}
}
