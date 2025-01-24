namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public class CommonFileDialogComboBoxItem
	{
		private string text = string.Empty;

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
			}
		}

		public CommonFileDialogComboBoxItem()
		{
		}

		public CommonFileDialogComboBoxItem(string text)
		{
			this.text = text;
		}
	}
}
