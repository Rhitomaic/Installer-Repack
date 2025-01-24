#define DEBUG
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.Shell;

namespace Microsoft.WindowsAPICodePack.Dialogs
{
	public sealed class CommonOpenFileDialog : CommonFileDialog
	{
		private NativeFileOpenDialog openDialogCoClass;

		private bool multiselect;

		private bool isFolderPicker;

		private bool allowNonFileSystem;

		public IEnumerable<string> FileNames
		{
			get
			{
				CheckFileNamesAvailable();
				return base.FileNameCollection;
			}
		}

		public ICollection<ShellObject> FilesAsShellObject
		{
			get
			{
				CheckFileItemsAvailable();
				ICollection<ShellObject> collection = new Collection<ShellObject>();
				foreach (IShellItem item in items)
				{
					collection.Add(ShellObjectFactory.Create(item));
				}
				return collection;
			}
		}

		public bool Multiselect
		{
			get
			{
				return multiselect;
			}
			set
			{
				multiselect = value;
			}
		}

		public bool IsFolderPicker
		{
			get
			{
				return isFolderPicker;
			}
			set
			{
				isFolderPicker = value;
			}
		}

		public bool AllowNonFileSystemItems
		{
			get
			{
				return allowNonFileSystem;
			}
			set
			{
				allowNonFileSystem = value;
			}
		}

		public CommonOpenFileDialog()
		{
			base.EnsureReadOnly = true;
		}

		public CommonOpenFileDialog(string name)
			: base(name)
		{
			base.EnsureReadOnly = true;
		}

		internal override IFileDialog GetNativeFileDialog()
		{
			Debug.Assert(openDialogCoClass != null, "Must call Initialize() before fetching dialog interface");
			return openDialogCoClass;
		}

		internal override void InitializeNativeFileDialog()
		{
			if (openDialogCoClass == null)
			{
				openDialogCoClass = (NativeFileOpenDialog)new FileOpenDialogRCW();
			}
		}

		internal override void CleanUpNativeFileDialog()
		{
			if (openDialogCoClass != null)
			{
				Marshal.ReleaseComObject(openDialogCoClass);
			}
		}

		internal override void PopulateWithFileNames(Collection<string> names)
		{
			openDialogCoClass.GetResults(out var ppenum);
			ppenum.GetCount(out var pdwNumItems);
			names.Clear();
			for (int i = 0; i < pdwNumItems; i++)
			{
				names.Add(CommonFileDialog.GetFileNameFromShellItem(CommonFileDialog.GetShellItemAt(ppenum, i)));
			}
		}

		internal override void PopulateWithIShellItems(Collection<IShellItem> items)
		{
			openDialogCoClass.GetResults(out var ppenum);
			ppenum.GetCount(out var pdwNumItems);
			items.Clear();
			for (int i = 0; i < pdwNumItems; i++)
			{
				items.Add(CommonFileDialog.GetShellItemAt(ppenum, i));
			}
		}

		internal override ShellNativeMethods.FileOpenOptions GetDerivedOptionFlags(ShellNativeMethods.FileOpenOptions flags)
		{
			if (multiselect)
			{
				flags |= ShellNativeMethods.FileOpenOptions.AllowMultiSelect;
			}
			if (isFolderPicker)
			{
				flags |= ShellNativeMethods.FileOpenOptions.PickFolders;
			}
			if (!allowNonFileSystem)
			{
				flags |= ShellNativeMethods.FileOpenOptions.ForceFilesystem;
			}
			else if (allowNonFileSystem)
			{
				flags |= ShellNativeMethods.FileOpenOptions.AllNonStorageItems;
			}
			return flags;
		}
	}
}
