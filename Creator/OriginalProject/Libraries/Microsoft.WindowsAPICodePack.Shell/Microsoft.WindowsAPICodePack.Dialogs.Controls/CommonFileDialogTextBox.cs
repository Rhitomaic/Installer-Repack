#define DEBUG
using System.Diagnostics;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public class CommonFileDialogTextBox : CommonFileDialogControl
	{
		private IFileDialogCustomize customizedDialog;

		internal bool Closed { get; set; }

		public override string Text
		{
			get
			{
				if (!Closed)
				{
					SyncValue();
				}
				return base.Text;
			}
			set
			{
				if (customizedDialog != null)
				{
					customizedDialog.SetEditBoxText(base.Id, value);
				}
				base.Text = value;
			}
		}

		public CommonFileDialogTextBox()
			: base(string.Empty)
		{
		}

		public CommonFileDialogTextBox(string text)
			: base(text)
		{
		}

		public CommonFileDialogTextBox(string name, string text)
			: base(name, text)
		{
		}

		internal override void Attach(IFileDialogCustomize dialog)
		{
			Debug.Assert(dialog != null, "CommonFileDialogTextBox.Attach: dialog parameter can not be null");
			dialog.AddEditBox(base.Id, Text);
			customizedDialog = dialog;
			SyncUnmanagedProperties();
			Closed = false;
		}

		internal void SyncValue()
		{
			if (customizedDialog != null)
			{
				customizedDialog.GetEditBoxText(base.Id, out var ppszText);
				base.Text = ppszText;
			}
		}
	}
}
