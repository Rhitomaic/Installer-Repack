#define DEBUG
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Markup;
using Microsoft.WindowsAPICodePack.Controls;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	[ContentProperty("Controls")]
	public abstract class CommonFileDialog : IDialogControlHost, IDisposable
	{
		private class NativeDialogEventSink : IFileDialogEvents, IFileDialogControlEvents
		{
			private CommonFileDialog parent;

			private bool firstFolderChanged = true;

			public uint Cookie { get; set; }

			public NativeDialogEventSink(CommonFileDialog commonDialog)
			{
				parent = commonDialog;
			}

			public HResult OnFileOk(IFileDialog pfd)
			{
				CancelEventArgs cancelEventArgs = new CancelEventArgs();
				parent.OnFileOk(cancelEventArgs);
				if (!cancelEventArgs.Cancel && parent.Controls != null)
				{
					foreach (CommonFileDialogControl control in parent.Controls)
					{
						if (control is CommonFileDialogTextBox commonFileDialogTextBox)
						{
							commonFileDialogTextBox.SyncValue();
							commonFileDialogTextBox.Closed = true;
						}
						else
						{
							if (!(control is CommonFileDialogGroupBox commonFileDialogGroupBox))
							{
								continue;
							}
							foreach (CommonFileDialogControl item in commonFileDialogGroupBox.Items)
							{
								if (item is CommonFileDialogTextBox commonFileDialogTextBox2)
								{
									commonFileDialogTextBox2.SyncValue();
									commonFileDialogTextBox2.Closed = true;
								}
							}
						}
					}
				}
				return cancelEventArgs.Cancel ? HResult.False : HResult.Ok;
			}

			public HResult OnFolderChanging(IFileDialog pfd, IShellItem psiFolder)
			{
				CommonFileDialogFolderChangeEventArgs commonFileDialogFolderChangeEventArgs = new CommonFileDialogFolderChangeEventArgs(GetFileNameFromShellItem(psiFolder));
				if (!firstFolderChanged)
				{
					parent.OnFolderChanging(commonFileDialogFolderChangeEventArgs);
				}
				return commonFileDialogFolderChangeEventArgs.Cancel ? HResult.False : HResult.Ok;
			}

			public void OnFolderChange(IFileDialog pfd)
			{
				if (firstFolderChanged)
				{
					firstFolderChanged = false;
					parent.OnOpening(EventArgs.Empty);
				}
				else
				{
					parent.OnFolderChanged(EventArgs.Empty);
				}
			}

			public void OnSelectionChange(IFileDialog pfd)
			{
				parent.OnSelectionChanged(EventArgs.Empty);
			}

			public void OnShareViolation(IFileDialog pfd, IShellItem psi, out ShellNativeMethods.FileDialogEventShareViolationResponse pResponse)
			{
				pResponse = ShellNativeMethods.FileDialogEventShareViolationResponse.Accept;
			}

			public void OnTypeChange(IFileDialog pfd)
			{
				parent.OnFileTypeChanged(EventArgs.Empty);
			}

			public void OnOverwrite(IFileDialog pfd, IShellItem psi, out ShellNativeMethods.FileDialogEventOverwriteResponse pResponse)
			{
				pResponse = ShellNativeMethods.FileDialogEventOverwriteResponse.Default;
			}

			public void OnItemSelected(IFileDialogCustomize pfdc, int dwIDCtl, int dwIDItem)
			{
				DialogControl controlbyId = parent.controls.GetControlbyId(dwIDCtl);
				if (controlbyId is ICommonFileDialogIndexedControls commonFileDialogIndexedControls)
				{
					commonFileDialogIndexedControls.SelectedIndex = dwIDItem;
					commonFileDialogIndexedControls.RaiseSelectedIndexChangedEvent();
				}
				else
				{
					if (!(controlbyId is CommonFileDialogMenu commonFileDialogMenu))
					{
						return;
					}
					foreach (CommonFileDialogMenuItem item in commonFileDialogMenu.Items)
					{
						if (item.Id == dwIDItem)
						{
							item.RaiseClickEvent();
							break;
						}
					}
				}
			}

			public void OnButtonClicked(IFileDialogCustomize pfdc, int dwIDCtl)
			{
				DialogControl controlbyId = parent.controls.GetControlbyId(dwIDCtl);
				if (controlbyId is CommonFileDialogButton commonFileDialogButton)
				{
					commonFileDialogButton.RaiseClickEvent();
				}
			}

			public void OnCheckButtonToggled(IFileDialogCustomize pfdc, int dwIDCtl, bool bChecked)
			{
				DialogControl controlbyId = parent.controls.GetControlbyId(dwIDCtl);
				if (controlbyId is CommonFileDialogCheckBox commonFileDialogCheckBox)
				{
					commonFileDialogCheckBox.IsChecked = bChecked;
					commonFileDialogCheckBox.RaiseCheckedChangedEvent();
				}
			}

			public void OnControlActivating(IFileDialogCustomize pfdc, int dwIDCtl)
			{
			}
		}

		private Collection<string> filenames;

		internal readonly Collection<IShellItem> items;

		internal DialogShowState showState = DialogShowState.PreShow;

		private IFileDialog nativeDialog;

		private IFileDialogCustomize customize;

		private NativeDialogEventSink nativeEventSink;

		private bool? canceled;

		private bool resetSelections;

		private IntPtr parentWindow = IntPtr.Zero;

		private bool filterSet;

		private CommonFileDialogControlCollection<CommonFileDialogControl> controls;

		private CommonFileDialogFilterCollection filters;

		private string title;

		private bool ensureFileExists;

		private bool ensurePathExists;

		private bool ensureValidNames;

		private bool ensureReadOnly;

		private bool restoreDirectory;

		private bool showPlacesList = true;

		private bool addToMruList = true;

		private bool showHiddenItems;

		private bool allowPropertyEditing;

		private bool navigateToShortcut = true;

		private string initialDirectory;

		private ShellContainer initialDirectoryShellContainer;

		private string defaultDirectory;

		private ShellContainer defaultDirectoryShellContainer;

		private Guid cookieIdentifier;

		protected IEnumerable<string> FileNameCollection
		{
			get
			{
				foreach (string filename in filenames)
				{
					yield return filename;
				}
			}
		}

		public CommonFileDialogControlCollection<CommonFileDialogControl> Controls => controls;

		public CommonFileDialogFilterCollection Filters => filters;

		public string Title
		{
			get
			{
				return title;
			}
			set
			{
				title = value;
				if (NativeDialogShowing)
				{
					nativeDialog.SetTitle(value);
				}
			}
		}

		public bool EnsureFileExists
		{
			get
			{
				return ensureFileExists;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.EnsureFileExistsCannotBeChanged);
				ensureFileExists = value;
			}
		}

		public bool EnsurePathExists
		{
			get
			{
				return ensurePathExists;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.EnsurePathExistsCannotBeChanged);
				ensurePathExists = value;
			}
		}

		public bool EnsureValidNames
		{
			get
			{
				return ensureValidNames;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.EnsureValidNamesCannotBeChanged);
				ensureValidNames = value;
			}
		}

		public bool EnsureReadOnly
		{
			get
			{
				return ensureReadOnly;
			}
			set
			{
				// ThrowIfDialogShowing(LocalizedMessages.EnsureReadonlyCannotBeChanged);
				ensureReadOnly = value;
			}
		}

		public bool RestoreDirectory
		{
			get
			{
				return restoreDirectory;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.RestoreDirectoryCannotBeChanged);
				restoreDirectory = value;
			}
		}

		public bool ShowPlacesList
		{
			get
			{
				return showPlacesList;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.ShowPlacesListCannotBeChanged);
				showPlacesList = value;
			}
		}

		public bool AddToMostRecentlyUsedList
		{
			get
			{
				return addToMruList;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.AddToMostRecentlyUsedListCannotBeChanged);
				addToMruList = value;
			}
		}

		public bool ShowHiddenItems
		{
			get
			{
				return showHiddenItems;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.ShowHiddenItemsCannotBeChanged);
				showHiddenItems = value;
			}
		}

		public bool AllowPropertyEditing
		{
			get
			{
				return allowPropertyEditing;
			}
			set
			{
				allowPropertyEditing = value;
			}
		}

		public bool NavigateToShortcut
		{
			get
			{
				return navigateToShortcut;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.NavigateToShortcutCannotBeChanged);
				navigateToShortcut = value;
			}
		}

		public string DefaultExtension { get; set; }

		public int SelectedFileTypeIndex
		{
			get
			{
				if (nativeDialog != null)
				{
					nativeDialog.GetFileTypeIndex(out var piFileType);
					return (int)piFileType;
				}
				return -1;
			}
		}

		public string FileName
		{
			get
			{
				CheckFileNamesAvailable();
				if (filenames.Count > 1)
				{
					throw new InvalidOperationException(LocalizedMessages.CommonFileDialogMultipleFiles);
				}
				string text = filenames[0];
				if (!string.IsNullOrEmpty(DefaultExtension))
				{
					text = Path.ChangeExtension(text, DefaultExtension);
				}
				return text;
			}
		}

		public ShellObject FileAsShellObject
		{
			get
			{
				CheckFileItemsAvailable();
				if (items.Count > 1)
				{
					throw new InvalidOperationException(LocalizedMessages.CommonFileDialogMultipleItems);
				}
				if (items.Count == 0)
				{
					return null;
				}
				return ShellObjectFactory.Create(items[0]);
			}
		}

		public string InitialDirectory
		{
			get
			{
				return initialDirectory;
			}
			set
			{
				initialDirectory = value;
			}
		}

		public ShellContainer InitialDirectoryShellContainer
		{
			get
			{
				return initialDirectoryShellContainer;
			}
			set
			{
				initialDirectoryShellContainer = value;
			}
		}

		public string DefaultDirectory
		{
			get
			{
				return defaultDirectory;
			}
			set
			{
				defaultDirectory = value;
			}
		}

		public ShellContainer DefaultDirectoryShellContainer
		{
			get
			{
				return defaultDirectoryShellContainer;
			}
			set
			{
				defaultDirectoryShellContainer = value;
			}
		}

		public Guid CookieIdentifier
		{
			get
			{
				return cookieIdentifier;
			}
			set
			{
				cookieIdentifier = value;
			}
		}

		public string DefaultFileName { get; set; }

		private bool NativeDialogShowing => nativeDialog != null && (showState == DialogShowState.Showing || showState == DialogShowState.Closing);

		public static bool IsPlatformSupported => CoreHelpers.RunningOnVista;

		public event CancelEventHandler FileOk;

		public event EventHandler<CommonFileDialogFolderChangeEventArgs> FolderChanging;

		public event EventHandler FolderChanged;

		public event EventHandler SelectionChanged;

		public event EventHandler FileTypeChanged;

		public event EventHandler DialogOpening;

		protected CommonFileDialog()
		{
			if (!CoreHelpers.RunningOnVista)
			{
				throw new PlatformNotSupportedException(LocalizedMessages.CommonFileDialogRequiresVista);
			}
			filenames = new Collection<string>();
			filters = new CommonFileDialogFilterCollection();
			items = new Collection<IShellItem>();
			controls = new CommonFileDialogControlCollection<CommonFileDialogControl>(this);
		}

		protected CommonFileDialog(string title)
			: this()
		{
			this.title = title;
		}

		internal abstract void InitializeNativeFileDialog();

		internal abstract IFileDialog GetNativeFileDialog();

		internal abstract void PopulateWithFileNames(Collection<string> names);

		internal abstract void PopulateWithIShellItems(Collection<IShellItem> shellItems);

		internal abstract void CleanUpNativeFileDialog();

		internal abstract ShellNativeMethods.FileOpenOptions GetDerivedOptionFlags(ShellNativeMethods.FileOpenOptions flags);

		private void SyncFileTypeComboToDefaultExtension(IFileDialog dialog)
		{
			if (!(this is CommonSaveFileDialog) || DefaultExtension == null || filters.Count <= 0)
			{
				return;
			}
			CommonFileDialogFilter commonFileDialogFilter = null;
			for (uint num = 0u; num < filters.Count; num++)
			{
				commonFileDialogFilter = filters[(int)num];
				if (commonFileDialogFilter.Extensions.Contains(DefaultExtension))
				{
					dialog.SetFileTypeIndex(num + 1);
					break;
				}
			}
		}

		public void AddPlace(ShellContainer place, FileDialogAddPlaceLocation location)
		{
			if (place == null)
			{
				throw new ArgumentNullException("place");
			}
			if (nativeDialog == null)
			{
				InitializeNativeFileDialog();
				nativeDialog = GetNativeFileDialog();
			}
			if (nativeDialog != null)
			{
				nativeDialog.AddPlace(place.NativeShellItem, (ShellNativeMethods.FileDialogAddPlacement)location);
			}
		}

		public void AddPlace(string path, FileDialogAddPlaceLocation location)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}
			if (nativeDialog == null)
			{
				InitializeNativeFileDialog();
				nativeDialog = GetNativeFileDialog();
			}
			Guid riid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
			IShellItem2 shellItem;
			int num = ShellNativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, ref riid, out shellItem);
			if (!CoreErrorHelper.Succeeded(num))
			{
				throw new CommonControlException(LocalizedMessages.CommonFileDialogCannotCreateShellItem, Marshal.GetExceptionForHR(num));
			}
			if (nativeDialog != null)
			{
				nativeDialog.AddPlace(shellItem, (ShellNativeMethods.FileDialogAddPlacement)location);
			}
		}

		public CommonFileDialogResult ShowDialog(IntPtr ownerWindowHandle)
		{
			if (ownerWindowHandle == IntPtr.Zero)
			{
				throw new ArgumentException(LocalizedMessages.CommonFileDialogInvalidHandle, "ownerWindowHandle");
			}
			parentWindow = ownerWindowHandle;
			return ShowDialog();
		}

		public CommonFileDialogResult ShowDialog(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}
			parentWindow = new WindowInteropHelper(window).Handle;
			return ShowDialog();
		}

		public CommonFileDialogResult ShowDialog()
		{
			InitializeNativeFileDialog();
			nativeDialog = GetNativeFileDialog();
			ApplyNativeSettings(nativeDialog);
			InitializeEventSink(nativeDialog);
			if (resetSelections)
			{
				resetSelections = false;
			}
			showState = DialogShowState.Showing;
			int result = nativeDialog.Show(parentWindow);
			showState = DialogShowState.Closed;
			CommonFileDialogResult result2;
			if (CoreErrorHelper.Matches(result, 1223))
			{
				canceled = true;
				result2 = CommonFileDialogResult.Cancel;
				filenames.Clear();
			}
			else
			{
				canceled = false;
				result2 = CommonFileDialogResult.Ok;
				PopulateWithFileNames(filenames);
				PopulateWithIShellItems(items);
			}
			return result2;
		}

		public void ResetUserSelections()
		{
			resetSelections = true;
		}

		private void InitializeEventSink(IFileDialog nativeDlg)
		{
			if (this.FileOk != null || this.FolderChanging != null || this.FolderChanged != null || this.SelectionChanged != null || this.FileTypeChanged != null || this.DialogOpening != null || (controls != null && controls.Count > 0))
			{
				nativeEventSink = new NativeDialogEventSink(this);
				nativeDlg.Advise(nativeEventSink, out var pdwCookie);
				nativeEventSink.Cookie = pdwCookie;
			}
		}

		private void ApplyNativeSettings(IFileDialog dialog)
		{
			Debug.Assert(dialog != null, "No dialog instance to configure");
			if (parentWindow == IntPtr.Zero)
			{
				if (System.Windows.Application.Current != null && System.Windows.Application.Current.MainWindow != null)
				{
					parentWindow = new WindowInteropHelper(System.Windows.Application.Current.MainWindow).Handle;
				}
				else if (System.Windows.Forms.Application.OpenForms.Count > 0)
				{
					parentWindow = System.Windows.Forms.Application.OpenForms[0].Handle;
				}
			}
			Guid riid = new Guid("7E9FB0D3-919F-4307-AB2E-9B1860310C93");
			dialog.SetOptions(CalculateNativeDialogOptionFlags());
			if (title != null)
			{
				dialog.SetTitle(title);
			}
			if (initialDirectoryShellContainer != null)
			{
				dialog.SetFolder(initialDirectoryShellContainer.NativeShellItem);
			}
			if (defaultDirectoryShellContainer != null)
			{
				dialog.SetDefaultFolder(defaultDirectoryShellContainer.NativeShellItem);
			}
			if (!string.IsNullOrEmpty(initialDirectory))
			{
				ShellNativeMethods.SHCreateItemFromParsingName(initialDirectory, IntPtr.Zero, ref riid, out IShellItem2 shellItem);
				if (shellItem != null)
				{
					dialog.SetFolder(shellItem);
				}
			}
			if (!string.IsNullOrEmpty(defaultDirectory))
			{
				ShellNativeMethods.SHCreateItemFromParsingName(defaultDirectory, IntPtr.Zero, ref riid, out IShellItem2 shellItem2);
				if (shellItem2 != null)
				{
					dialog.SetDefaultFolder(shellItem2);
				}
			}
			if (filters.Count > 0 && !filterSet)
			{
				dialog.SetFileTypes((uint)filters.Count, filters.GetAllFilterSpecs());
				filterSet = true;
				SyncFileTypeComboToDefaultExtension(dialog);
			}
			if (cookieIdentifier != Guid.Empty)
			{
				dialog.SetClientGuid(ref cookieIdentifier);
			}
			if (!string.IsNullOrEmpty(DefaultExtension))
			{
				dialog.SetDefaultExtension(DefaultExtension);
			}
			dialog.SetFileName(DefaultFileName);
		}

		private ShellNativeMethods.FileOpenOptions CalculateNativeDialogOptionFlags()
		{
			ShellNativeMethods.FileOpenOptions flags = ShellNativeMethods.FileOpenOptions.NoTestFileCreate;
			flags = GetDerivedOptionFlags(flags);
			if (ensureFileExists)
			{
				flags |= ShellNativeMethods.FileOpenOptions.FileMustExist;
			}
			if (ensurePathExists)
			{
				flags |= ShellNativeMethods.FileOpenOptions.PathMustExist;
			}
			if (!ensureValidNames)
			{
				flags |= ShellNativeMethods.FileOpenOptions.NoValidate;
			}
			if (!EnsureReadOnly)
			{
				flags |= ShellNativeMethods.FileOpenOptions.NoReadOnlyReturn;
			}
			if (restoreDirectory)
			{
				flags |= ShellNativeMethods.FileOpenOptions.NoChangeDirectory;
			}
			if (!showPlacesList)
			{
				flags |= ShellNativeMethods.FileOpenOptions.HidePinnedPlaces;
			}
			if (!addToMruList)
			{
				flags |= ShellNativeMethods.FileOpenOptions.DontAddToRecent;
			}
			if (showHiddenItems)
			{
				flags |= ShellNativeMethods.FileOpenOptions.ForceShowHidden;
			}
			if (!navigateToShortcut)
			{
				flags |= ShellNativeMethods.FileOpenOptions.NoDereferenceLinks;
			}
			return flags;
		}

		private static void GenerateNotImplementedException()
		{
			throw new NotImplementedException(LocalizedMessages.NotImplementedException);
		}

		public virtual bool IsCollectionChangeAllowed()
		{
			return true;
		}

		public virtual void ApplyCollectionChanged()
		{
			GetCustomizedFileDialog();
			foreach (CommonFileDialogControl control in controls)
			{
				if (!control.IsAdded)
				{
					control.HostingDialog = this;
					control.Attach(customize);
					control.IsAdded = true;
				}
			}
		}

		public virtual bool IsControlPropertyChangeAllowed(string propertyName, DialogControl control)
		{
			GenerateNotImplementedException();
			return false;
		}

		public virtual void ApplyControlPropertyChange(string propertyName, DialogControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			CommonFileDialogControl commonFileDialogControl = null;
			ShellNativeMethods.ControlState pdwState;
			if (propertyName == "Text")
			{
				CommonFileDialogTextBox commonFileDialogTextBox = control as CommonFileDialogTextBox;
				if (commonFileDialogTextBox != null)
				{
					customize.SetEditBoxText(control.Id, commonFileDialogTextBox.Text);
				}
				else
				{
					customize.SetControlLabel(control.Id, commonFileDialogTextBox.Text);
				}
			}
			else if (propertyName == "Visible" && (commonFileDialogControl = control as CommonFileDialogControl) != null)
			{
				customize.GetControlState(control.Id, out pdwState);
				if (commonFileDialogControl.Visible)
				{
					pdwState |= ShellNativeMethods.ControlState.Visible;
				}
				else if (!commonFileDialogControl.Visible)
				{
					pdwState &= (ShellNativeMethods.ControlState)(-3);
				}
				customize.SetControlState(control.Id, pdwState);
			}
			else if (propertyName == "Enabled" && commonFileDialogControl != null)
			{
				customize.GetControlState(control.Id, out pdwState);
				if (commonFileDialogControl.Enabled)
				{
					pdwState |= ShellNativeMethods.ControlState.Enable;
				}
				else if (!commonFileDialogControl.Enabled)
				{
					pdwState &= (ShellNativeMethods.ControlState)(-2);
				}
				customize.SetControlState(control.Id, pdwState);
			}
			else if (propertyName == "SelectedIndex")
			{
				if (control is CommonFileDialogRadioButtonList commonFileDialogRadioButtonList)
				{
					customize.SetSelectedControlItem(commonFileDialogRadioButtonList.Id, commonFileDialogRadioButtonList.SelectedIndex);
				}
				else if (control is CommonFileDialogComboBox commonFileDialogComboBox)
				{
					customize.SetSelectedControlItem(commonFileDialogComboBox.Id, commonFileDialogComboBox.SelectedIndex);
				}
			}
			else if (propertyName == "IsChecked" && control is CommonFileDialogCheckBox commonFileDialogCheckBox)
			{
				customize.SetCheckButtonState(commonFileDialogCheckBox.Id, commonFileDialogCheckBox.IsChecked);
			}
		}

		protected void CheckFileNamesAvailable()
		{
			if (showState != DialogShowState.Closed)
			{
				throw new InvalidOperationException(LocalizedMessages.CommonFileDialogNotClosed);
			}
			if (canceled.GetValueOrDefault())
			{
				throw new InvalidOperationException(LocalizedMessages.CommonFileDialogCanceled);
			}
			Debug.Assert(filenames.Count != 0, "FileNames empty - shouldn't happen unless dialog canceled or not yet shown.");
		}

		protected void CheckFileItemsAvailable()
		{
			if (showState != DialogShowState.Closed)
			{
				throw new InvalidOperationException(LocalizedMessages.CommonFileDialogNotClosed);
			}
			if (canceled.GetValueOrDefault())
			{
				throw new InvalidOperationException(LocalizedMessages.CommonFileDialogCanceled);
			}
			Debug.Assert(items.Count != 0, "Items list empty - shouldn't happen unless dialog canceled or not yet shown.");
		}

		internal static string GetFileNameFromShellItem(IShellItem item)
		{
			string result = null;
			IntPtr ppszName = IntPtr.Zero;
			if (item.GetDisplayName(ShellNativeMethods.ShellItemDesignNameOptions.DesktopAbsoluteParsing, out ppszName) == HResult.Ok && ppszName != IntPtr.Zero)
			{
				result = Marshal.PtrToStringAuto(ppszName);
				Marshal.FreeCoTaskMem(ppszName);
			}
			return result;
		}

		internal static IShellItem GetShellItemAt(IShellItemArray array, int i)
		{
			array.GetItemAt((uint)i, out var ppsi);
			return ppsi;
		}

		protected void ThrowIfDialogShowing(string message)
		{
			if (NativeDialogShowing)
			{
				throw new InvalidOperationException(message);
			}
		}

		private void GetCustomizedFileDialog()
		{
			if (customize == null)
			{
				if (nativeDialog == null)
				{
					InitializeNativeFileDialog();
					nativeDialog = GetNativeFileDialog();
				}
				customize = (IFileDialogCustomize)nativeDialog;
			}
		}

		protected virtual void OnFileOk(CancelEventArgs e)
		{
			this.FileOk?.Invoke(this, e);
		}

		protected virtual void OnFolderChanging(CommonFileDialogFolderChangeEventArgs e)
		{
			this.FolderChanging?.Invoke(this, e);
		}

		protected virtual void OnFolderChanged(EventArgs e)
		{
			this.FolderChanged?.Invoke(this, e);
		}

		protected virtual void OnSelectionChanged(EventArgs e)
		{
			this.SelectionChanged?.Invoke(this, e);
		}

		protected virtual void OnFileTypeChanged(EventArgs e)
		{
			this.FileTypeChanged?.Invoke(this, e);
		}

		protected virtual void OnOpening(EventArgs e)
		{
			this.DialogOpening?.Invoke(this, e);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				CleanUpNativeFileDialog();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
