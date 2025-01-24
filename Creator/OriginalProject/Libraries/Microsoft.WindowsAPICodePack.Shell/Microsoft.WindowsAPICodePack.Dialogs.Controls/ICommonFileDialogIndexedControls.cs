using System;

namespace Microsoft.WindowsAPICodePack.Dialogs.Controls
{
	internal interface ICommonFileDialogIndexedControls
	{
		int SelectedIndex { get; set; }

		event EventHandler SelectedIndexChanged;

		void RaiseSelectedIndexChangedEvent();
	}
}
