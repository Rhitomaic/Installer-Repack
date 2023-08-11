using System;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	internal static class TaskDialogNativeMethods
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		internal class TaskDialogConfiguration
		{
			internal uint size;

			internal IntPtr parentHandle;

			internal IntPtr instance;

			internal TaskDialogOptions taskDialogFlags;

			internal TaskDialogCommonButtons commonButtons;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string windowTitle;

			internal IconUnion mainIcon;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string mainInstruction;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string content;

			internal uint buttonCount;

			internal IntPtr buttons;

			internal int defaultButtonIndex;

			internal uint radioButtonCount;

			internal IntPtr radioButtons;

			internal int defaultRadioButtonIndex;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string verificationText;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string expandedInformation;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string expandedControlText;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string collapsedControlText;

			internal IconUnion footerIcon;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string footerText;

			internal TaskDialogCallback callback;

			internal IntPtr callbackData;

			internal uint width;
		}

		[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
		internal struct IconUnion
		{
			[FieldOffset(0)]
			private int mainIcon;

			[FieldOffset(0)]
			private IntPtr spacer;

			public int MainIcon => mainIcon;

			internal IconUnion(int i)
			{
				mainIcon = i;
				spacer = IntPtr.Zero;
			}
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
		internal struct TaskDialogButton
		{
			internal int buttonId;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string buttonText;

			public TaskDialogButton(int buttonId, string text)
			{
				this.buttonId = buttonId;
				buttonText = text;
			}
		}

		[Flags]
		internal enum TaskDialogCommonButtons
		{
			Ok = 1,
			Yes = 2,
			No = 4,
			Cancel = 8,
			Retry = 0x10,
			Close = 0x20
		}

		internal enum TaskDialogCommonButtonReturnIds
		{
			Ok = 1,
			Cancel,
			Abort,
			Retry,
			Ignore,
			Yes,
			No,
			Close
		}

		internal enum TaskDialogElements
		{
			Content,
			ExpandedInformation,
			Footer,
			MainInstruction
		}

		internal enum TaskDialogIconElement
		{
			Main,
			Footer
		}

		[Flags]
		internal enum TaskDialogOptions
		{
			None = 0,
			EnableHyperlinks = 1,
			UseMainIcon = 2,
			UseFooterIcon = 4,
			AllowCancel = 8,
			UseCommandLinks = 0x10,
			UseNoIconCommandLinks = 0x20,
			ExpandFooterArea = 0x40,
			ExpandedByDefault = 0x80,
			CheckVerificationFlag = 0x100,
			ShowProgressBar = 0x200,
			ShowMarqueeProgressBar = 0x400,
			UseCallbackTimer = 0x800,
			PositionRelativeToWindow = 0x1000,
			RightToLeftLayout = 0x2000,
			NoDefaultRadioButton = 0x4000
		}

		internal enum TaskDialogMessages
		{
			NavigatePage = 1125,
			ClickButton = 1126,
			SetMarqueeProgressBar = 1127,
			SetProgressBarState = 1128,
			SetProgressBarRange = 1129,
			SetProgressBarPosition = 1130,
			SetProgressBarMarquee = 1131,
			SetElementText = 1132,
			ClickRadioButton = 1134,
			EnableButton = 1135,
			EnableRadioButton = 1136,
			ClickVerification = 1137,
			UpdateElementText = 1138,
			SetButtonElevationRequiredState = 1139,
			UpdateIcon = 1140
		}

		internal enum TaskDialogNotifications
		{
			Created,
			Navigated,
			ButtonClicked,
			HyperlinkClicked,
			Timer,
			Destroyed,
			RadioButtonClicked,
			Constructed,
			VerificationClicked,
			Help,
			ExpandButtonClicked
		}

		internal delegate int TaskDialogCallback(IntPtr hwnd, uint message, IntPtr wparam, IntPtr lparam, IntPtr referenceData);

		internal enum ProgressBarState
		{
			Normal = 1,
			Error,
			Paused
		}

		internal enum TaskDialogIcons
		{
			Warning = 65535,
			Error = 65534,
			Information = 65533,
			Shield = 65532
		}

		internal const int TaskDialogIdealWidth = 0;

		internal const int TaskDialogButtonShieldIcon = 1;

		internal const int NoDefaultButtonSpecified = 0;

		[DllImport("Comctl32.dll", SetLastError = true)]
		internal static extern HResult TaskDialogIndirect([In] TaskDialogConfiguration taskConfig, out int button, out int radioButton, [MarshalAs(UnmanagedType.Bool)] out bool verificationFlagChecked);
	}
}
