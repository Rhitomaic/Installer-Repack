using System;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public class CommonFileDialogMenuItem : CommonFileDialogControl
	{
		public event EventHandler Click = delegate
		{
		};

		public CommonFileDialogMenuItem()
			: base(string.Empty)
		{
		}

		public CommonFileDialogMenuItem(string text)
			: base(text)
		{
		}

		internal void RaiseClickEvent()
		{
			if (base.Enabled)
			{
				this.Click(this, EventArgs.Empty);
			}
		}

		internal override void Attach(IFileDialogCustomize dialog)
		{
		}
	}
}
