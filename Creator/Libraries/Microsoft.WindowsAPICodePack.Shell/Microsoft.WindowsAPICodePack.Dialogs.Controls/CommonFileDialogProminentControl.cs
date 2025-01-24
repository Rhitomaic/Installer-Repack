using System.Windows.Markup;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	[ContentProperty("Items")]
	public abstract class CommonFileDialogProminentControl : CommonFileDialogControl
	{
		private bool isProminent;

		public bool IsProminent
		{
			get
			{
				return isProminent;
			}
			set
			{
				isProminent = value;
			}
		}

		protected CommonFileDialogProminentControl()
		{
		}

		protected CommonFileDialogProminentControl(string text)
			: base(text)
		{
		}

		protected CommonFileDialogProminentControl(string name, string text)
			: base(name, text)
		{
		}
	}
}
