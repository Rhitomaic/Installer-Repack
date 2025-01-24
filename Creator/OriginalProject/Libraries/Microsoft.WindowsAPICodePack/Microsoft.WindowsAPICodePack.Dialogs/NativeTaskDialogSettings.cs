using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	internal class NativeTaskDialogSettings
	{
		public int ProgressBarMinimum { get; set; }

		public int ProgressBarMaximum { get; set; }

		public int ProgressBarValue { get; set; }

		public TaskDialogProgressBarState ProgressBarState { get; set; }

		public bool InvokeHelp { get; set; }

		public TaskDialogNativeMethods.TaskDialogConfiguration NativeConfiguration { get; private set; }

		public TaskDialogNativeMethods.TaskDialogButton[] Buttons { get; set; }

		public TaskDialogNativeMethods.TaskDialogButton[] RadioButtons { get; set; }

		public List<int> ElevatedButtons { get; set; }

		internal NativeTaskDialogSettings()
		{
			NativeConfiguration = new TaskDialogNativeMethods.TaskDialogConfiguration();
			NativeConfiguration.size = (uint)Marshal.SizeOf(NativeConfiguration);
			NativeConfiguration.parentHandle = IntPtr.Zero;
			NativeConfiguration.instance = IntPtr.Zero;
			NativeConfiguration.taskDialogFlags = TaskDialogNativeMethods.TaskDialogOptions.AllowCancel;
			NativeConfiguration.commonButtons = TaskDialogNativeMethods.TaskDialogCommonButtons.Ok;
			NativeConfiguration.mainIcon = new TaskDialogNativeMethods.IconUnion(0);
			NativeConfiguration.footerIcon = new TaskDialogNativeMethods.IconUnion(0);
			NativeConfiguration.width = 0u;
			NativeConfiguration.buttonCount = 0u;
			NativeConfiguration.radioButtonCount = 0u;
			NativeConfiguration.buttons = IntPtr.Zero;
			NativeConfiguration.radioButtons = IntPtr.Zero;
			NativeConfiguration.defaultButtonIndex = 0;
			NativeConfiguration.defaultRadioButtonIndex = 0;
			NativeConfiguration.windowTitle = TaskDialogDefaults.Caption;
			NativeConfiguration.mainInstruction = TaskDialogDefaults.MainInstruction;
			NativeConfiguration.content = TaskDialogDefaults.Content;
			NativeConfiguration.verificationText = null;
			NativeConfiguration.expandedInformation = null;
			NativeConfiguration.expandedControlText = null;
			NativeConfiguration.collapsedControlText = null;
			NativeConfiguration.footerText = null;
		}
	}
}
