#define DEBUG
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	internal class NativeTaskDialog : IDisposable
	{
		private TaskDialogNativeMethods.TaskDialogConfiguration nativeDialogConfig;

		private NativeTaskDialogSettings settings;

		private IntPtr hWndDialog;

		private TaskDialog outerDialog;

		private IntPtr[] updatedStrings = new IntPtr[Enum.GetNames(typeof(TaskDialogNativeMethods.TaskDialogElements)).Length];

		private IntPtr buttonArray;

		private IntPtr radioButtonArray;

		private bool firstRadioButtonClicked = true;

		private bool disposed;

		public DialogShowState ShowState { get; private set; }

		public int SelectedButtonId { get; private set; }

		public int SelectedRadioButtonId { get; private set; }

		public bool CheckBoxChecked { get; private set; }

		internal NativeTaskDialog(NativeTaskDialogSettings settings, TaskDialog outerDialog)
		{
			nativeDialogConfig = settings.NativeConfiguration;
			this.settings = settings;
			nativeDialogConfig.callback = DialogProc;
			ShowState = DialogShowState.PreShow;
			this.outerDialog = outerDialog;
		}

		internal void NativeShow()
		{
			if (settings == null)
			{
				throw new InvalidOperationException(LocalizedMessages.NativeTaskDialogConfigurationError);
			}
			MarshalDialogControlStructs();
			try
			{
				ShowState = DialogShowState.Showing;
				int button;
				int radioButton;
				bool verificationFlagChecked;
				HResult hResult = TaskDialogNativeMethods.TaskDialogIndirect(nativeDialogConfig, out button, out radioButton, out verificationFlagChecked);
				if (CoreErrorHelper.Failed(hResult))
				{
					string message;
					switch (hResult)
					{
					case HResult.InvalidArguments:
						message = LocalizedMessages.NativeTaskDialogInternalErrorArgs;
						break;
					case HResult.OutOfMemory:
						message = LocalizedMessages.NativeTaskDialogInternalErrorComplex;
						break;
					default:
						message = string.Format(CultureInfo.InvariantCulture, LocalizedMessages.NativeTaskDialogInternalErrorUnexpected, hResult);
						break;
					}
					Exception exceptionForHR = Marshal.GetExceptionForHR((int)hResult);
					throw new Win32Exception(message, exceptionForHR);
				}
				SelectedButtonId = button;
				SelectedRadioButtonId = radioButton;
				CheckBoxChecked = verificationFlagChecked;
			}
			catch (EntryPointNotFoundException innerException)
			{
				throw new NotSupportedException(LocalizedMessages.NativeTaskDialogVersionError, innerException);
			}
			finally
			{
				ShowState = DialogShowState.Closed;
			}
		}

		internal void NativeClose(TaskDialogResult result)
		{
			ShowState = DialogShowState.Closing;
			int wparam;
			switch (result)
			{
			case TaskDialogResult.Close:
				wparam = 8;
				break;
			case TaskDialogResult.CustomButtonClicked:
				wparam = 9;
				break;
			case TaskDialogResult.No:
				wparam = 7;
				break;
			case TaskDialogResult.Ok:
				wparam = 1;
				break;
			case TaskDialogResult.Retry:
				wparam = 4;
				break;
			case TaskDialogResult.Yes:
				wparam = 6;
				break;
			default:
				wparam = 2;
				break;
			}
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.ClickButton, wparam, 0L);
		}

		private int DialogProc(IntPtr windowHandle, uint message, IntPtr wparam, IntPtr lparam, IntPtr referenceData)
		{
			hWndDialog = windowHandle;
			switch ((TaskDialogNativeMethods.TaskDialogNotifications)message)
			{
			case TaskDialogNativeMethods.TaskDialogNotifications.Created:
			{
				int result = PerformDialogInitialization();
				outerDialog.RaiseOpenedEvent();
				return result;
			}
			case TaskDialogNativeMethods.TaskDialogNotifications.ButtonClicked:
				return HandleButtonClick((int)wparam);
			case TaskDialogNativeMethods.TaskDialogNotifications.RadioButtonClicked:
				return HandleRadioButtonClick((int)wparam);
			case TaskDialogNativeMethods.TaskDialogNotifications.HyperlinkClicked:
				return HandleHyperlinkClick(lparam);
			case TaskDialogNativeMethods.TaskDialogNotifications.Help:
				return HandleHelpInvocation();
			case TaskDialogNativeMethods.TaskDialogNotifications.Timer:
				return HandleTick((int)wparam);
			case TaskDialogNativeMethods.TaskDialogNotifications.Destroyed:
				return PerformDialogCleanup();
			default:
				return 0;
			}
		}

		private int PerformDialogInitialization()
		{
			if (IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar))
			{
				UpdateProgressBarRange();
				UpdateProgressBarState(settings.ProgressBarState);
				UpdateProgressBarValue(settings.ProgressBarValue);
				UpdateProgressBarValue(settings.ProgressBarValue);
			}
			else if (IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar))
			{
				SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarMarquee, 1, 0L);
				UpdateProgressBarState(settings.ProgressBarState);
			}
			if (settings.ElevatedButtons != null && settings.ElevatedButtons.Count > 0)
			{
				foreach (int elevatedButton in settings.ElevatedButtons)
				{
					UpdateElevationIcon(elevatedButton, true);
				}
			}
			return 0;
		}

		private int HandleButtonClick(int id)
		{
			if (ShowState != DialogShowState.Closing)
			{
				outerDialog.RaiseButtonClickEvent(id);
			}
			if (id < 9)
			{
				return outerDialog.RaiseClosingEvent(id);
			}
			return 1;
		}

		private int HandleRadioButtonClick(int id)
		{
			if (firstRadioButtonClicked && !IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton))
			{
				firstRadioButtonClicked = false;
			}
			else
			{
				outerDialog.RaiseButtonClickEvent(id);
			}
			return 0;
		}

		private int HandleHyperlinkClick(IntPtr href)
		{
			string link = Marshal.PtrToStringUni(href);
			outerDialog.RaiseHyperlinkClickEvent(link);
			return 0;
		}

		private int HandleTick(int ticks)
		{
			outerDialog.RaiseTickEvent(ticks);
			return 0;
		}

		private int HandleHelpInvocation()
		{
			outerDialog.RaiseHelpInvokedEvent();
			return 0;
		}

		private int PerformDialogCleanup()
		{
			firstRadioButtonClicked = true;
			return 0;
		}

		internal void UpdateProgressBarValue(int i)
		{
			AssertCurrentlyShowing();
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarPosition, i, 0L);
		}

		internal void UpdateProgressBarRange()
		{
			AssertCurrentlyShowing();
			long lparam = MakeLongLParam(settings.ProgressBarMaximum, settings.ProgressBarMinimum);
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarRange, 0, lparam);
		}

		internal void UpdateProgressBarState(TaskDialogProgressBarState state)
		{
			AssertCurrentlyShowing();
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetProgressBarState, (int)state, 0L);
		}

		internal void UpdateText(string text)
		{
			UpdateTextCore(text, TaskDialogNativeMethods.TaskDialogElements.Content);
		}

		internal void UpdateInstruction(string instruction)
		{
			UpdateTextCore(instruction, TaskDialogNativeMethods.TaskDialogElements.MainInstruction);
		}

		internal void UpdateFooterText(string footerText)
		{
			UpdateTextCore(footerText, TaskDialogNativeMethods.TaskDialogElements.Footer);
		}

		internal void UpdateExpandedText(string expandedText)
		{
			UpdateTextCore(expandedText, TaskDialogNativeMethods.TaskDialogElements.ExpandedInformation);
		}

		private void UpdateTextCore(string s, TaskDialogNativeMethods.TaskDialogElements element)
		{
			AssertCurrentlyShowing();
			FreeOldString(element);
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetElementText, (int)element, (long)MakeNewString(s, element));
		}

		internal void UpdateMainIcon(TaskDialogStandardIcon mainIcon)
		{
			UpdateIconCore(mainIcon, TaskDialogNativeMethods.TaskDialogIconElement.Main);
		}

		internal void UpdateFooterIcon(TaskDialogStandardIcon footerIcon)
		{
			UpdateIconCore(footerIcon, TaskDialogNativeMethods.TaskDialogIconElement.Footer);
		}

		private void UpdateIconCore(TaskDialogStandardIcon icon, TaskDialogNativeMethods.TaskDialogIconElement element)
		{
			AssertCurrentlyShowing();
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.UpdateIcon, (int)element, (long)icon);
		}

		internal void UpdateCheckBoxChecked(bool cbc)
		{
			AssertCurrentlyShowing();
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.ClickVerification, cbc ? 1 : 0, 1L);
		}

		internal void UpdateElevationIcon(int buttonId, bool showIcon)
		{
			AssertCurrentlyShowing();
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.SetButtonElevationRequiredState, buttonId, Convert.ToInt32(showIcon));
		}

		internal void UpdateButtonEnabled(int buttonID, bool enabled)
		{
			AssertCurrentlyShowing();
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.EnableButton, buttonID, enabled ? 1 : 0);
		}

		internal void UpdateRadioButtonEnabled(int buttonID, bool enabled)
		{
			AssertCurrentlyShowing();
			SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages.EnableRadioButton, buttonID, enabled ? 1 : 0);
		}

		internal void AssertCurrentlyShowing()
		{
			Debug.Assert(ShowState == DialogShowState.Showing, "Update*() methods should only be called while native dialog is showing");
		}

		private int SendMessageHelper(TaskDialogNativeMethods.TaskDialogMessages message, int wparam, long lparam)
		{
			Debug.Assert(true, "HWND for dialog is null during SendMessage");
			return (int)CoreNativeMethods.SendMessage(hWndDialog, (uint)message, (IntPtr)wparam, new IntPtr(lparam));
		}

		private bool IsOptionSet(TaskDialogNativeMethods.TaskDialogOptions flag)
		{
			return (nativeDialogConfig.taskDialogFlags & flag) == flag;
		}

		private IntPtr MakeNewString(string text, TaskDialogNativeMethods.TaskDialogElements element)
		{
			IntPtr intPtr = Marshal.StringToHGlobalUni(text);
			updatedStrings[(int)element] = intPtr;
			return intPtr;
		}

		private void FreeOldString(TaskDialogNativeMethods.TaskDialogElements element)
		{
			if (updatedStrings[(int)element] != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(updatedStrings[(int)element]);
				ref IntPtr reference = ref updatedStrings[(int)element];
				reference = IntPtr.Zero;
			}
		}

		private static long MakeLongLParam(int a, int b)
		{
			return (a << 16) + b;
		}

		private void MarshalDialogControlStructs()
		{
			if (settings.Buttons != null && settings.Buttons.Length > 0)
			{
				buttonArray = AllocateAndMarshalButtons(settings.Buttons);
				settings.NativeConfiguration.buttons = buttonArray;
				settings.NativeConfiguration.buttonCount = (uint)settings.Buttons.Length;
			}
			if (settings.RadioButtons != null && settings.RadioButtons.Length > 0)
			{
				radioButtonArray = AllocateAndMarshalButtons(settings.RadioButtons);
				settings.NativeConfiguration.radioButtons = radioButtonArray;
				settings.NativeConfiguration.radioButtonCount = (uint)settings.RadioButtons.Length;
			}
		}

		private static IntPtr AllocateAndMarshalButtons(TaskDialogNativeMethods.TaskDialogButton[] structs)
		{
			IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TaskDialogNativeMethods.TaskDialogButton)) * structs.Length);
			IntPtr intPtr2 = intPtr;
			foreach (TaskDialogNativeMethods.TaskDialogButton taskDialogButton in structs)
			{
				Marshal.StructureToPtr(taskDialogButton, intPtr2, false);
				intPtr2 = (IntPtr)((int)intPtr2 + Marshal.SizeOf(taskDialogButton));
			}
			return intPtr;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~NativeTaskDialog()
		{
			Dispose(false);
		}

		protected void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
			if (ShowState == DialogShowState.Showing)
			{
				NativeClose(TaskDialogResult.Cancel);
			}
			if (updatedStrings != null)
			{
				for (int i = 0; i < updatedStrings.Length; i++)
				{
					if (updatedStrings[i] != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(updatedStrings[i]);
						ref IntPtr reference = ref updatedStrings[i];
						reference = IntPtr.Zero;
					}
				}
			}
			if (buttonArray != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(buttonArray);
				buttonArray = IntPtr.Zero;
			}
			if (radioButtonArray != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(radioButtonArray);
				radioButtonArray = IntPtr.Zero;
			}
			if (!disposing)
			{
			}
		}
	}
}
