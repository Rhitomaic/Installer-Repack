using Microsoft.WindowsAPICodePack.Resources;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	internal static class TaskDialogDefaults
	{
		public const int ProgressBarMinimumValue = 0;

		public const int ProgressBarMaximumValue = 100;

		public const int IdealWidth = 0;

		public const int MinimumDialogControlId = 9;

		public static string Caption => LocalizedMessages.TaskDialogDefaultCaption;

		public static string MainInstruction => LocalizedMessages.TaskDialogDefaultMainInstruction;

		public static string Content => LocalizedMessages.TaskDialogDefaultContent;
	}
}
