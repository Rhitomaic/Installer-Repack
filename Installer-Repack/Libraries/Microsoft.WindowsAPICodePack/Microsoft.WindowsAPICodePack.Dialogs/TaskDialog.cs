#define DEBUG
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.WindowsAPICodePack.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public sealed class TaskDialog : IDialogControlHost, IDisposable
	{
		private static TaskDialog staticDialog;

		private NativeTaskDialog nativeDialog;

		private List<TaskDialogButtonBase> buttons = new List<TaskDialogButtonBase>();

		private List<TaskDialogButtonBase> radioButtons = new List<TaskDialogButtonBase>();

		private List<TaskDialogButtonBase> commandLinks = new List<TaskDialogButtonBase>();

		private IntPtr ownerWindow;

		private string text;

		private string instructionText;

		private string caption;

		private string footerText;

		private string checkBoxText;

		private string detailsExpandedText;

		private bool detailsExpanded;

		private string detailsExpandedLabel;

		private string detailsCollapsedLabel;

		private bool cancelable;

		private TaskDialogStandardIcon icon;

		private TaskDialogStandardIcon footerIcon;

		private TaskDialogStandardButtons standardButtons = TaskDialogStandardButtons.None;

		private DialogControlCollection<TaskDialogControl> controls;

		private bool hyperlinksEnabled;

		private bool? footerCheckBoxChecked = null;

		private TaskDialogExpandedDetailsLocation expansionMode;

		private TaskDialogStartupLocation startupLocation;

		private TaskDialogProgressBar progressBar;

		private bool disposed;

		public IntPtr OwnerWindowHandle
		{
			get
			{
				return ownerWindow;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.OwnerCannotBeChanged);
				ownerWindow = value;
			}
		}

		public string Text
		{
			get
			{
				return text;
			}
			set
			{
				text = value;
				if (NativeDialogShowing)
				{
					nativeDialog.UpdateText(text);
				}
			}
		}

		public string InstructionText
		{
			get
			{
				return instructionText;
			}
			set
			{
				instructionText = value;
				if (NativeDialogShowing)
				{
					nativeDialog.UpdateInstruction(instructionText);
				}
			}
		}

		public string Caption
		{
			get
			{
				return caption;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.CaptionCannotBeChanged);
				caption = value;
			}
		}

		public string FooterText
		{
			get
			{
				return footerText;
			}
			set
			{
				footerText = value;
				if (NativeDialogShowing)
				{
					nativeDialog.UpdateFooterText(footerText);
				}
			}
		}

		public string FooterCheckBoxText
		{
			get
			{
				return checkBoxText;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.CheckBoxCannotBeChanged);
				checkBoxText = value;
			}
		}

		public string DetailsExpandedText
		{
			get
			{
				return detailsExpandedText;
			}
			set
			{
				detailsExpandedText = value;
				if (NativeDialogShowing)
				{
					nativeDialog.UpdateExpandedText(detailsExpandedText);
				}
			}
		}

		public bool DetailsExpanded
		{
			get
			{
				return detailsExpanded;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.ExpandingStateCannotBeChanged);
				detailsExpanded = value;
			}
		}

		public string DetailsExpandedLabel
		{
			get
			{
				return detailsExpandedLabel;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.ExpandedLabelCannotBeChanged);
				detailsExpandedLabel = value;
			}
		}

		public string DetailsCollapsedLabel
		{
			get
			{
				return detailsCollapsedLabel;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.CollapsedTextCannotBeChanged);
				detailsCollapsedLabel = value;
			}
		}

		public bool Cancelable
		{
			get
			{
				return cancelable;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.CancelableCannotBeChanged);
				cancelable = value;
			}
		}

		public TaskDialogStandardIcon Icon
		{
			get
			{
				return icon;
			}
			set
			{
				icon = value;
				if (NativeDialogShowing)
				{
					nativeDialog.UpdateMainIcon(icon);
				}
			}
		}

		public TaskDialogStandardIcon FooterIcon
		{
			get
			{
				return footerIcon;
			}
			set
			{
				footerIcon = value;
				if (NativeDialogShowing)
				{
					nativeDialog.UpdateFooterIcon(footerIcon);
				}
			}
		}

		public TaskDialogStandardButtons StandardButtons
		{
			get
			{
				return standardButtons;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.StandardButtonsCannotBeChanged);
				standardButtons = value;
			}
		}

		public DialogControlCollection<TaskDialogControl> Controls => controls;

		public bool HyperlinksEnabled
		{
			get
			{
				return hyperlinksEnabled;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.HyperlinksCannotBetSet);
				hyperlinksEnabled = value;
			}
		}

		public bool? FooterCheckBoxChecked
		{
			get
			{
				return footerCheckBoxChecked.GetValueOrDefault(false);
			}
			set
			{
				footerCheckBoxChecked = value;
				if (NativeDialogShowing)
				{
					nativeDialog.UpdateCheckBoxChecked(footerCheckBoxChecked.Value);
				}
			}
		}

		public TaskDialogExpandedDetailsLocation ExpansionMode
		{
			get
			{
				return expansionMode;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.ExpandedDetailsCannotBeChanged);
				expansionMode = value;
			}
		}

		public TaskDialogStartupLocation StartupLocation
		{
			get
			{
				return startupLocation;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.StartupLocationCannotBeChanged);
				startupLocation = value;
			}
		}

		public TaskDialogProgressBar ProgressBar
		{
			get
			{
				return progressBar;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.ProgressBarCannotBeChanged);
				if (value != null)
				{
					if (value.HostingDialog != null)
					{
						throw new InvalidOperationException(LocalizedMessages.ProgressBarCannotBeHostedInMultipleDialogs);
					}
					value.HostingDialog = this;
				}
				progressBar = value;
			}
		}

		private bool NativeDialogShowing => nativeDialog != null && (nativeDialog.ShowState == DialogShowState.Showing || nativeDialog.ShowState == DialogShowState.Closing);

		public static bool IsPlatformSupported => CoreHelpers.RunningOnVista;

		public event EventHandler<TaskDialogTickEventArgs> Tick;

		public event EventHandler<TaskDialogHyperlinkClickedEventArgs> HyperlinkClick;

		public event EventHandler<TaskDialogClosingEventArgs> Closing;

		public event EventHandler HelpInvoked;

		public event EventHandler Opened;

		public TaskDialog()
		{
			CoreHelpers.ThrowIfNotVista();
			controls = new DialogControlCollection<TaskDialogControl>(this);
		}

		public static TaskDialogResult Show(string text)
		{
			return ShowCoreStatic(text, TaskDialogDefaults.MainInstruction, TaskDialogDefaults.Caption);
		}

		public static TaskDialogResult Show(string text, string instructionText)
		{
			return ShowCoreStatic(text, instructionText, TaskDialogDefaults.Caption);
		}

		public static TaskDialogResult Show(string text, string instructionText, string caption)
		{
			return ShowCoreStatic(text, instructionText, caption);
		}

		public TaskDialogResult Show()
		{
			return ShowCore();
		}

		private static TaskDialogResult ShowCoreStatic(string text, string instructionText, string caption)
		{
			CoreHelpers.ThrowIfNotVista();
			if (staticDialog == null)
			{
				staticDialog = new TaskDialog();
			}
			staticDialog.text = text;
			staticDialog.instructionText = instructionText;
			staticDialog.caption = caption;
			return staticDialog.Show();
		}

		private TaskDialogResult ShowCore()
		{
			TaskDialogResult result;
			try
			{
				SortDialogControls();
				ValidateCurrentDialogSettings();
				NativeTaskDialogSettings settings = new NativeTaskDialogSettings();
				ApplyCoreSettings(settings);
				ApplySupplementalSettings(settings);
				nativeDialog = new NativeTaskDialog(settings, this);
				nativeDialog.NativeShow();
				result = ConstructDialogResult(nativeDialog);
				footerCheckBoxChecked = nativeDialog.CheckBoxChecked;
			}
			finally
			{
				CleanUp();
				nativeDialog = null;
			}
			return result;
		}

		private void ValidateCurrentDialogSettings()
		{
			if (footerCheckBoxChecked.HasValue && footerCheckBoxChecked.Value && string.IsNullOrEmpty(checkBoxText))
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogCheckBoxTextRequiredToEnableCheckBox);
			}
			if (progressBar != null && !progressBar.HasValidValues)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogProgressBarValueInRange);
			}
			if (buttons.Count > 0 && commandLinks.Count > 0)
			{
				throw new NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndLinks);
			}
			if (buttons.Count > 0 && standardButtons != 0)
			{
				throw new NotSupportedException(LocalizedMessages.TaskDialogSupportedButtonsAndButtons);
			}
		}

		private static TaskDialogResult ConstructDialogResult(NativeTaskDialog native)
		{
			Debug.Assert(native.ShowState == DialogShowState.Closed, "dialog result being constructed for unshown dialog.");
			TaskDialogStandardButtons taskDialogStandardButtons = MapButtonIdToStandardButton(native.SelectedButtonId);
			if (taskDialogStandardButtons == TaskDialogStandardButtons.None)
			{
				return TaskDialogResult.CustomButtonClicked;
			}
			return (TaskDialogResult)taskDialogStandardButtons;
		}

		public void Close()
		{
			if (!NativeDialogShowing)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing);
			}
			nativeDialog.NativeClose(TaskDialogResult.Cancel);
		}

		public void Close(TaskDialogResult closingResult)
		{
			if (!NativeDialogShowing)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogCloseNonShowing);
			}
			nativeDialog.NativeClose(closingResult);
		}

		private void ApplyCoreSettings(NativeTaskDialogSettings settings)
		{
			ApplyGeneralNativeConfiguration(settings.NativeConfiguration);
			ApplyTextConfiguration(settings.NativeConfiguration);
			ApplyOptionConfiguration(settings.NativeConfiguration);
			ApplyControlConfiguration(settings);
		}

		private void ApplyGeneralNativeConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
		{
			if (ownerWindow != IntPtr.Zero)
			{
				dialogConfig.parentHandle = ownerWindow;
			}
			dialogConfig.mainIcon = new TaskDialogNativeMethods.IconUnion((int)icon);
			dialogConfig.footerIcon = new TaskDialogNativeMethods.IconUnion((int)footerIcon);
			dialogConfig.commonButtons = (TaskDialogNativeMethods.TaskDialogCommonButtons)standardButtons;
		}

		private void ApplyTextConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
		{
			dialogConfig.content = text;
			dialogConfig.windowTitle = caption;
			dialogConfig.mainInstruction = instructionText;
			dialogConfig.expandedInformation = detailsExpandedText;
			dialogConfig.expandedControlText = detailsExpandedLabel;
			dialogConfig.collapsedControlText = detailsCollapsedLabel;
			dialogConfig.footerText = footerText;
			dialogConfig.verificationText = checkBoxText;
		}

		private void ApplyOptionConfiguration(TaskDialogNativeMethods.TaskDialogConfiguration dialogConfig)
		{
			TaskDialogNativeMethods.TaskDialogOptions taskDialogOptions = TaskDialogNativeMethods.TaskDialogOptions.None;
			if (cancelable)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.AllowCancel;
			}
			if (footerCheckBoxChecked.HasValue && footerCheckBoxChecked.Value)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.CheckVerificationFlag;
			}
			if (hyperlinksEnabled)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.EnableHyperlinks;
			}
			if (detailsExpanded)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.ExpandedByDefault;
			}
			if (this.Tick != null)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.UseCallbackTimer;
			}
			if (startupLocation == TaskDialogStartupLocation.CenterOwner)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.PositionRelativeToWindow;
			}
			if (expansionMode == TaskDialogExpandedDetailsLocation.ExpandFooter)
			{
				taskDialogOptions |= TaskDialogNativeMethods.TaskDialogOptions.ExpandFooterArea;
			}
			dialogConfig.taskDialogFlags = taskDialogOptions;
		}

		private void ApplyControlConfiguration(NativeTaskDialogSettings settings)
		{
			if (progressBar != null)
			{
				if (progressBar.State == TaskDialogProgressBarState.Marquee)
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.ShowMarqueeProgressBar;
				}
				else
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.ShowProgressBar;
				}
			}
			if (buttons.Count > 0 || commandLinks.Count > 0)
			{
				List<TaskDialogButtonBase> list = ((buttons.Count > 0) ? buttons : commandLinks);
				settings.Buttons = BuildButtonStructArray(list);
				if (commandLinks.Count > 0)
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.UseCommandLinks;
				}
				settings.NativeConfiguration.defaultButtonIndex = FindDefaultButtonId(list);
				ApplyElevatedIcons(settings, list);
			}
			if (radioButtons.Count > 0)
			{
				settings.RadioButtons = BuildButtonStructArray(radioButtons);
				int num = FindDefaultButtonId(radioButtons);
				settings.NativeConfiguration.defaultRadioButtonIndex = num;
				if (num == 0)
				{
					settings.NativeConfiguration.taskDialogFlags |= TaskDialogNativeMethods.TaskDialogOptions.NoDefaultRadioButton;
				}
			}
		}

		private static TaskDialogNativeMethods.TaskDialogButton[] BuildButtonStructArray(List<TaskDialogButtonBase> controls)
		{
			int count = controls.Count;
			TaskDialogNativeMethods.TaskDialogButton[] array = new TaskDialogNativeMethods.TaskDialogButton[count];
			for (int i = 0; i < count; i++)
			{
				TaskDialogButtonBase taskDialogButtonBase = controls[i];
				ref TaskDialogNativeMethods.TaskDialogButton reference = ref array[i];
				reference = new TaskDialogNativeMethods.TaskDialogButton(taskDialogButtonBase.Id, taskDialogButtonBase.ToString());
			}
			return array;
		}

		private static int FindDefaultButtonId(List<TaskDialogButtonBase> controls)
		{
			List<TaskDialogButtonBase> list = controls.FindAll((TaskDialogButtonBase control) => control.Default);
			if (list.Count == 1)
			{
				return list[0].Id;
			}
			if (list.Count > 1)
			{
				throw new InvalidOperationException(LocalizedMessages.TaskDialogOnlyOneDefaultControl);
			}
			return 0;
		}

		private static void ApplyElevatedIcons(NativeTaskDialogSettings settings, List<TaskDialogButtonBase> controls)
		{
			foreach (TaskDialogButton control in controls)
			{
				if (control.UseElevationIcon)
				{
					if (settings.ElevatedButtons == null)
					{
						settings.ElevatedButtons = new List<int>();
					}
					settings.ElevatedButtons.Add(control.Id);
				}
			}
		}

		private void ApplySupplementalSettings(NativeTaskDialogSettings settings)
		{
			if (progressBar != null && progressBar.State != TaskDialogProgressBarState.Marquee)
			{
				settings.ProgressBarMinimum = progressBar.Minimum;
				settings.ProgressBarMaximum = progressBar.Maximum;
				settings.ProgressBarValue = progressBar.Value;
				settings.ProgressBarState = progressBar.State;
			}
			if (this.HelpInvoked != null)
			{
				settings.InvokeHelp = true;
			}
		}

		private void SortDialogControls()
		{
			foreach (TaskDialogControl control in controls)
			{
				TaskDialogButtonBase taskDialogButtonBase = control as TaskDialogButtonBase;
				TaskDialogCommandLink taskDialogCommandLink = control as TaskDialogCommandLink;
				if (taskDialogButtonBase != null && string.IsNullOrEmpty(taskDialogButtonBase.Text) && taskDialogCommandLink != null && string.IsNullOrEmpty(taskDialogCommandLink.Instruction))
				{
					throw new InvalidOperationException(LocalizedMessages.TaskDialogButtonTextEmpty);
				}
				if (taskDialogCommandLink != null)
				{
					commandLinks.Add(taskDialogCommandLink);
				}
				else if (control is TaskDialogRadioButton item)
				{
					if (radioButtons == null)
					{
						radioButtons = new List<TaskDialogButtonBase>();
					}
					radioButtons.Add(item);
				}
				else if (taskDialogButtonBase != null)
				{
					if (buttons == null)
					{
						buttons = new List<TaskDialogButtonBase>();
					}
					buttons.Add(taskDialogButtonBase);
				}
				else
				{
					if (!(control is TaskDialogProgressBar taskDialogProgressBar))
					{
						throw new InvalidOperationException(LocalizedMessages.TaskDialogUnkownControl);
					}
					progressBar = taskDialogProgressBar;
				}
			}
		}

		private static TaskDialogStandardButtons MapButtonIdToStandardButton(int id)
		{
			switch ((TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds)id)
			{
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ok:
				return TaskDialogStandardButtons.Ok;
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Cancel:
				return TaskDialogStandardButtons.Cancel;
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Abort:
				return TaskDialogStandardButtons.None;
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Retry:
				return TaskDialogStandardButtons.Retry;
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Ignore:
				return TaskDialogStandardButtons.None;
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Yes:
				return TaskDialogStandardButtons.Yes;
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.No:
				return TaskDialogStandardButtons.No;
			case TaskDialogNativeMethods.TaskDialogCommonButtonReturnIds.Close:
				return TaskDialogStandardButtons.Close;
			default:
				return TaskDialogStandardButtons.None;
			}
		}

		private void ThrowIfDialogShowing(string message)
		{
			if (NativeDialogShowing)
			{
				throw new NotSupportedException(message);
			}
		}

		private TaskDialogButtonBase GetButtonForId(int id)
		{
			return (TaskDialogButtonBase)controls.GetControlbyId(id);
		}

		bool IDialogControlHost.IsCollectionChangeAllowed()
		{
			return !NativeDialogShowing;
		}

		void IDialogControlHost.ApplyCollectionChanged()
		{
			Debug.Assert(!NativeDialogShowing, "Collection changed notification received despite show state of dialog");
		}

		bool IDialogControlHost.IsControlPropertyChangeAllowed(string propertyName, DialogControl control)
		{
			Debug.Assert(control is TaskDialogControl, "Property changing for a control that is not a TaskDialogControl-derived type");
			Debug.Assert(propertyName != "Name", "Name changes at any time are not supported - public API should have blocked this");
			bool result = false;
			if (!NativeDialogShowing)
			{
				result = ((propertyName == null || !(propertyName == "Enabled")) ? true : false);
			}
			else
			{
				switch (propertyName)
				{
				case "Text":
				case "Default":
					result = false;
					break;
				case "ShowElevationIcon":
				case "Enabled":
					result = true;
					break;
				default:
					Debug.Assert(true, "Unknown property name coming through property changing handler");
					break;
				}
			}
			return result;
		}

		void IDialogControlHost.ApplyControlPropertyChange(string propertyName, DialogControl control)
		{
			if (!NativeDialogShowing)
			{
				return;
			}
			if (control is TaskDialogProgressBar)
			{
				if (!progressBar.HasValidValues)
				{
					throw new ArgumentException(LocalizedMessages.TaskDialogProgressBarValueInRange);
				}
				switch (propertyName)
				{
				case "State":
					nativeDialog.UpdateProgressBarState(progressBar.State);
					break;
				case "Value":
					nativeDialog.UpdateProgressBarValue(progressBar.Value);
					break;
				case "Minimum":
				case "Maximum":
					nativeDialog.UpdateProgressBarRange();
					break;
				default:
					Debug.Assert(true, "Unknown property being set");
					break;
				}
			}
			else if (control is TaskDialogButton taskDialogButton)
			{
				switch (propertyName)
				{
				case "ShowElevationIcon":
					nativeDialog.UpdateElevationIcon(taskDialogButton.Id, taskDialogButton.UseElevationIcon);
					break;
				case "Enabled":
					nativeDialog.UpdateButtonEnabled(taskDialogButton.Id, taskDialogButton.Enabled);
					break;
				default:
					Debug.Assert(true, "Unknown property being set");
					break;
				}
			}
			else if (control is TaskDialogRadioButton taskDialogRadioButton)
			{
				if (propertyName != null && propertyName == "Enabled")
				{
					nativeDialog.UpdateRadioButtonEnabled(taskDialogRadioButton.Id, taskDialogRadioButton.Enabled);
				}
				else
				{
					Debug.Assert(true, "Unknown property being set");
				}
			}
			else
			{
				Debug.Assert(true, "Control property changed notification not handled properly - being ignored");
			}
		}

		internal void RaiseButtonClickEvent(int id)
		{
			GetButtonForId(id)?.RaiseClickEvent();
		}

		internal void RaiseHyperlinkClickEvent(string link)
		{
			this.HyperlinkClick?.Invoke(this, new TaskDialogHyperlinkClickedEventArgs(link));
		}

		internal int RaiseClosingEvent(int id)
		{
			EventHandler<TaskDialogClosingEventArgs> closing = this.Closing;
			if (closing != null)
			{
				TaskDialogButtonBase taskDialogButtonBase = null;
				TaskDialogClosingEventArgs taskDialogClosingEventArgs = new TaskDialogClosingEventArgs();
				TaskDialogStandardButtons taskDialogStandardButtons = MapButtonIdToStandardButton(id);
				if (taskDialogStandardButtons == TaskDialogStandardButtons.None)
				{
					taskDialogButtonBase = GetButtonForId(id);
					if (taskDialogButtonBase == null)
					{
						throw new InvalidOperationException(LocalizedMessages.TaskDialogBadButtonId);
					}
					taskDialogClosingEventArgs.CustomButton = taskDialogButtonBase.Name;
					taskDialogClosingEventArgs.TaskDialogResult = TaskDialogResult.CustomButtonClicked;
				}
				else
				{
					taskDialogClosingEventArgs.TaskDialogResult = (TaskDialogResult)taskDialogStandardButtons;
				}
				closing(this, taskDialogClosingEventArgs);
				if (taskDialogClosingEventArgs.Cancel)
				{
					return 1;
				}
			}
			return 0;
		}

		internal void RaiseHelpInvokedEvent()
		{
			if (this.HelpInvoked != null)
			{
				this.HelpInvoked(this, EventArgs.Empty);
			}
		}

		internal void RaiseOpenedEvent()
		{
			if (this.Opened != null)
			{
				this.Opened(this, EventArgs.Empty);
			}
		}

		internal void RaiseTickEvent(int ticks)
		{
			if (this.Tick != null)
			{
				this.Tick(this, new TaskDialogTickEventArgs(ticks));
			}
		}

		private void CleanUp()
		{
			if (progressBar != null)
			{
				progressBar.Reset();
			}
			if (buttons != null)
			{
				buttons.Clear();
			}
			if (commandLinks != null)
			{
				commandLinks.Clear();
			}
			if (radioButtons != null)
			{
				radioButtons.Clear();
			}
			progressBar = null;
			if (nativeDialog != null)
			{
				nativeDialog.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		~TaskDialog()
		{
			Dispose(false);
		}

		public void Dispose(bool disposing)
		{
			if (disposed)
			{
				return;
			}
			disposed = true;
			if (disposing)
			{
				if (nativeDialog != null && nativeDialog.ShowState == DialogShowState.Showing)
				{
					nativeDialog.NativeClose(TaskDialogResult.Cancel);
				}
				buttons = null;
				radioButtons = null;
				commandLinks = null;
			}
			if (nativeDialog != null)
			{
				nativeDialog.Dispose();
				nativeDialog = null;
			}
			if (staticDialog != null)
			{
				staticDialog.Dispose();
				staticDialog = null;
			}
		}
	}
}
