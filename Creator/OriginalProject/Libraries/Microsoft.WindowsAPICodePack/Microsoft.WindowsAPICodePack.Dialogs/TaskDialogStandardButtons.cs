using System;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	[Flags]
	public enum TaskDialogStandardButtons
	{
		None = 0,
		Ok = 1,
		Yes = 2,
		No = 4,
		Cancel = 8,
		Retry = 0x10,
		Close = 0x20
	}
}
