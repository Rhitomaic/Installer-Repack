#define DEBUG
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Markup;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	[ContentProperty("Items")]
	public class CommonFileDialogGroupBox : CommonFileDialogProminentControl
	{
		private Collection<DialogControl> items;

		public Collection<DialogControl> Items => items;

		public CommonFileDialogGroupBox()
			: base(string.Empty)
		{
			Initialize();
		}

		public CommonFileDialogGroupBox(string text)
			: base(text)
		{
			Initialize();
		}

		public CommonFileDialogGroupBox(string name, string text)
			: base(name, text)
		{
			Initialize();
		}

		private void Initialize()
		{
			items = new Collection<DialogControl>();
		}

		internal override void Attach(IFileDialogCustomize dialog)
		{
			Debug.Assert(dialog != null, "CommonFileDialogGroupBox.Attach: dialog parameter can not be null");
			dialog.StartVisualGroup(base.Id, Text);
			foreach (CommonFileDialogControl item in items)
			{
				item.HostingDialog = base.HostingDialog;
				item.Attach(dialog);
			}
			dialog.EndVisualGroup();
			if (base.IsProminent)
			{
				dialog.MakeProminent(base.Id);
			}
			SyncUnmanagedProperties();
		}
	}
}
