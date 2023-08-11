#define DEBUG
using System.Diagnostics;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	public class CommonFileDialogSeparator : CommonFileDialogControl
	{
		internal override void Attach(IFileDialogCustomize dialog)
		{
			Debug.Assert(dialog != null, "CommonFileDialogSeparator.Attach: dialog parameter can not be null");
			dialog.AddSeparator(base.Id);
			SyncUnmanagedProperties();
		}
	}
}
