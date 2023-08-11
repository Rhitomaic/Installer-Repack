#define DEBUG
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Markup;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	[ContentProperty("Items")]
	public class CommonFileDialogMenu : CommonFileDialogProminentControl
	{
		private Collection<CommonFileDialogMenuItem> items = new Collection<CommonFileDialogMenuItem>();

		public Collection<CommonFileDialogMenuItem> Items => items;

		public CommonFileDialogMenu()
		{
		}

		public CommonFileDialogMenu(string text)
			: base(text)
		{
		}

		public CommonFileDialogMenu(string name, string text)
			: base(name, text)
		{
		}

		internal override void Attach(IFileDialogCustomize dialog)
		{
			Debug.Assert(dialog != null, "CommonFileDialogMenu.Attach: dialog parameter can not be null");
			dialog.AddMenu(base.Id, Text);
			foreach (CommonFileDialogMenuItem item in items)
			{
				dialog.AddControlItem(base.Id, item.Id, item.Text);
			}
			if (base.IsProminent)
			{
				dialog.MakeProminent(base.Id);
			}
			SyncUnmanagedProperties();
		}
	}
}
