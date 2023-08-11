#define DEBUG
using System;
using System.Diagnostics;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public class CommonFileDialogCheckBox : CommonFileDialogProminentControl
	{
		private bool isChecked;

		public bool IsChecked
		{
			get
			{
				return isChecked;
			}
			set
			{
				if (isChecked != value)
				{
					isChecked = value;
					ApplyPropertyChange("IsChecked");
				}
			}
		}

		public event EventHandler CheckedChanged = delegate
		{
		};

		public CommonFileDialogCheckBox()
		{
		}

		public CommonFileDialogCheckBox(string text)
			: base(text)
		{
		}

		public CommonFileDialogCheckBox(string name, string text)
			: base(name, text)
		{
		}

		public CommonFileDialogCheckBox(string text, bool isChecked)
			: base(text)
		{
			this.isChecked = isChecked;
		}

		public CommonFileDialogCheckBox(string name, string text, bool isChecked)
			: base(name, text)
		{
			this.isChecked = isChecked;
		}

		internal void RaiseCheckedChangedEvent()
		{
			if (base.Enabled)
			{
				this.CheckedChanged(this, EventArgs.Empty);
			}
		}

		internal override void Attach(IFileDialogCustomize dialog)
		{
			Debug.Assert(dialog != null, "CommonFileDialogCheckBox.Attach: dialog parameter can not be null");
			dialog.AddCheckButton(base.Id, Text, isChecked);
			if (base.IsProminent)
			{
				dialog.MakeProminent(base.Id);
			}
			ApplyPropertyChange("IsChecked");
			SyncUnmanagedProperties();
		}
	}
}
