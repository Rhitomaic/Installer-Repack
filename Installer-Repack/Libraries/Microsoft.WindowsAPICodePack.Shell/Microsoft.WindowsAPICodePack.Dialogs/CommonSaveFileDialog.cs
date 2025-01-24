#define DEBUG
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public sealed class CommonSaveFileDialog : CommonFileDialog
	{
		private NativeFileSaveDialog saveDialogCoClass;

		private bool overwritePrompt = true;

		private bool createPrompt;

		private bool isExpandedMode;

		private bool alwaysAppendDefaultExtension;

		public bool OverwritePrompt
		{
			get
			{
				return overwritePrompt;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.OverwritePromptCannotBeChanged);
				overwritePrompt = value;
			}
		}

		public bool CreatePrompt
		{
			get
			{
				return createPrompt;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.CreatePromptCannotBeChanged);
				createPrompt = value;
			}
		}

		public bool IsExpandedMode
		{
			get
			{
				return isExpandedMode;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.IsExpandedModeCannotBeChanged);
				isExpandedMode = value;
			}
		}

		public bool AlwaysAppendDefaultExtension
		{
			get
			{
				return alwaysAppendDefaultExtension;
			}
			set
			{
				ThrowIfDialogShowing(LocalizedMessages.AlwaysAppendDefaultExtensionCannotBeChanged);
				alwaysAppendDefaultExtension = value;
			}
		}

		public ShellPropertyCollection CollectedProperties
		{
			get
			{
				InitializeNativeFileDialog();
				if (GetNativeFileDialog() is IFileSaveDialog fileSaveDialog)
				{
					IPropertyStore ppStore;
					HResult properties = fileSaveDialog.GetProperties(out ppStore);
					if (ppStore != null && CoreErrorHelper.Succeeded(properties))
					{
						return new ShellPropertyCollection(ppStore);
					}
				}
				return null;
			}
		}

		public CommonSaveFileDialog()
		{
		}

		public CommonSaveFileDialog(string name)
			: base(name)
		{
		}

		public void SetSaveAsItem(ShellObject item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			InitializeNativeFileDialog();
			if (GetNativeFileDialog() is IFileSaveDialog fileSaveDialog)
			{
				fileSaveDialog.SetSaveAsItem(item.NativeShellItem);
			}
		}

		public void SetCollectedPropertyKeys(bool appendDefault, params PropertyKey[] propertyList)
		{
			if (propertyList == null || propertyList.Length <= 0)
			{
				return;
			}
			_ = propertyList[0];
			StringBuilder stringBuilder = new StringBuilder("prop:");
			foreach (PropertyKey key in propertyList)
			{
				string canonicalName = ShellPropertyDescriptionsCache.Cache.GetPropertyDescription(key).CanonicalName;
				if (!string.IsNullOrEmpty(canonicalName))
				{
					stringBuilder.AppendFormat("{0};", canonicalName);
				}
			}
			Guid riid = new Guid("1F9FC1D0-C39B-4B26-817F-011967D3440E");
			IPropertyDescriptionList ppv = null;
			try
			{
				int result = PropertySystemNativeMethods.PSGetPropertyDescriptionListFromString(stringBuilder.ToString(), ref riid, out ppv);
				if (!CoreErrorHelper.Succeeded(result))
				{
					return;
				}
				InitializeNativeFileDialog();
				if (GetNativeFileDialog() is IFileSaveDialog fileSaveDialog)
				{
					result = fileSaveDialog.SetCollectedProperties(ppv, appendDefault);
					if (!CoreErrorHelper.Succeeded(result))
					{
						throw new ShellException(result);
					}
				}
			}
			finally
			{
				if (ppv != null)
				{
					Marshal.ReleaseComObject(ppv);
				}
			}
		}

		internal override void InitializeNativeFileDialog()
		{
			if (saveDialogCoClass == null)
			{
				saveDialogCoClass = (NativeFileSaveDialog)new FileSaveDialogRCW();
			}
		}

		internal override IFileDialog GetNativeFileDialog()
		{
			Debug.Assert(saveDialogCoClass != null, "Must call Initialize() before fetching dialog interface");
			return saveDialogCoClass;
		}

		internal override void PopulateWithFileNames(Collection<string> names)
		{
			saveDialogCoClass.GetResult(out var ppsi);
			if (ppsi == null)
			{
				throw new InvalidOperationException(LocalizedMessages.SaveFileNullItem);
			}
			names.Clear();
			names.Add(CommonFileDialog.GetFileNameFromShellItem(ppsi));
		}

		internal override void PopulateWithIShellItems(Collection<IShellItem> items)
		{
			saveDialogCoClass.GetResult(out var ppsi);
			if (ppsi == null)
			{
				throw new InvalidOperationException(LocalizedMessages.SaveFileNullItem);
			}
			items.Clear();
			items.Add(ppsi);
		}

		internal override void CleanUpNativeFileDialog()
		{
			if (saveDialogCoClass != null)
			{
				Marshal.ReleaseComObject(saveDialogCoClass);
			}
		}

		internal override ShellNativeMethods.FileOpenOptions GetDerivedOptionFlags(ShellNativeMethods.FileOpenOptions flags)
		{
			if (overwritePrompt)
			{
				flags |= ShellNativeMethods.FileOpenOptions.OverwritePrompt;
			}
			if (createPrompt)
			{
				flags |= ShellNativeMethods.FileOpenOptions.CreatePrompt;
			}
			if (!isExpandedMode)
			{
				flags |= ShellNativeMethods.FileOpenOptions.DefaultNoMiniMode;
			}
			if (alwaysAppendDefaultExtension)
			{
				flags |= ShellNativeMethods.FileOpenOptions.StrictFileTypes;
			}
			return flags;
		}
	}
}
