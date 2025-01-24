#define DEBUG
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Markup;
using Microsoft.WindowsAPICodePack.Shell.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	[ContentProperty("Items")]
	public class CommonFileDialogComboBox : CommonFileDialogProminentControl, ICommonFileDialogIndexedControls
	{
		private readonly Collection<CommonFileDialogComboBoxItem> items = new Collection<CommonFileDialogComboBoxItem>();

		private int selectedIndex = -1;

		public Collection<CommonFileDialogComboBoxItem> Items => items;

		public int SelectedIndex
		{
			get
			{
				return selectedIndex;
			}
			set
			{
				if (selectedIndex == value)
				{
					return;
				}
				if (base.HostingDialog == null)
				{
					selectedIndex = value;
					return;
				}
				if (value >= 0 && value < items.Count)
				{
					selectedIndex = value;
					ApplyPropertyChange("SelectedIndex");
					return;
				}
				throw new IndexOutOfRangeException(LocalizedMessages.ComboBoxIndexOutsideBounds);
			}
		}

		public event EventHandler SelectedIndexChanged = delegate
		{
		};

		public CommonFileDialogComboBox()
		{
		}

		public CommonFileDialogComboBox(string name)
			: base(name, string.Empty)
		{
		}

		void ICommonFileDialogIndexedControls.RaiseSelectedIndexChangedEvent()
		{
			if (base.Enabled)
			{
				this.SelectedIndexChanged(this, EventArgs.Empty);
			}
		}

		internal override void Attach(IFileDialogCustomize dialog)
		{
			Debug.Assert(dialog != null, "CommonFileDialogComboBox.Attach: dialog parameter can not be null");
			dialog.AddComboBox(base.Id);
			for (int i = 0; i < items.Count; i++)
			{
				dialog.AddControlItem(base.Id, i, items[i].Text);
			}
			if (selectedIndex >= 0 && selectedIndex < items.Count)
			{
				dialog.SetSelectedControlItem(base.Id, selectedIndex);
			}
			else if (selectedIndex != -1)
			{
				throw new IndexOutOfRangeException(LocalizedMessages.ComboBoxIndexOutsideBounds);
			}
			if (base.IsProminent)
			{
				dialog.MakeProminent(base.Id);
			}
			SyncUnmanagedProperties();
		}
	}
}
