#define DEBUG
using System;
using System.Diagnostics;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public class CommonFileDialogButton : CommonFileDialogProminentControl
	{
		public event EventHandler Click = delegate
		{
		};

		public CommonFileDialogButton()
			: base(string.Empty)
		{
		}

		public CommonFileDialogButton(string text)
			: base(text)
		{
		}

		public CommonFileDialogButton(string name, string text)
			: base(name, text)
		{
		}

		internal override void Attach(IFileDialogCustomize dialog)
		{
			Debug.Assert(dialog != null, "CommonFileDialogButton.Attach: dialog parameter can not be null");
			dialog.AddPushButton(base.Id, Text);
			if (base.IsProminent)
			{
				dialog.MakeProminent(base.Id);
			}
			SyncUnmanagedProperties();
		}

		internal void RaiseClickEvent()
		{
			if (base.Enabled)
			{
				this.Click(this, EventArgs.Empty);
			}
		}
	}
}
