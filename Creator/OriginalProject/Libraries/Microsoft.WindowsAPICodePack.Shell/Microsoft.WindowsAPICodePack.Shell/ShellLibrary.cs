#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.WindowsAPICodePack.Shell.Resources;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public sealed class ShellLibrary : ShellContainer, IList<ShellFileSystemFolder>, ICollection<ShellFileSystemFolder>, IEnumerable<ShellFileSystemFolder>, IEnumerable
	{
		internal const string FileExtension = ".library-ms";

		private INativeShellLibrary nativeShellLibrary;

		private IKnownFolder knownFolder;

		private static Guid[] FolderTypesGuids = new Guid[5]
		{
			new Guid("5c4f28b5-f869-4e84-8e60-f11db97c5cc7"),
			new Guid("7d49d726-3c21-4f05-99aa-fdc2c9474656"),
			new Guid("94d6ddcc-4a68-4175-a374-bd584a510b78"),
			new Guid("b3690e58-e961-423b-b687-386ebfd83239"),
			new Guid("5fa96407-7e77-483c-ac93-691d05850de8")
		};

		public override string Name
		{
			get
			{
				if (base.Name == null && NativeShellItem != null)
				{
					base.Name = Path.GetFileNameWithoutExtension(ShellHelper.GetParsingName(NativeShellItem));
				}
				return base.Name;
			}
		}

		public IconReference IconResourceId
		{
			get
			{
				nativeShellLibrary.GetIcon(out var icon);
				return new IconReference(icon);
			}
			set
			{
				nativeShellLibrary.SetIcon(value.ReferencePath);
				nativeShellLibrary.Commit();
			}
		}

		public LibraryFolderType LibraryType
		{
			get
			{
				nativeShellLibrary.GetFolderType(out var ftid);
				return GetFolderTypefromGuid(ftid);
			}
			set
			{
				Guid ftid = FolderTypesGuids[(int)value];
				nativeShellLibrary.SetFolderType(ref ftid);
				nativeShellLibrary.Commit();
			}
		}

		public Guid LibraryTypeId
		{
			get
			{
				nativeShellLibrary.GetFolderType(out var ftid);
				return ftid;
			}
		}

		public string DefaultSaveFolder
		{
			get
			{
				Guid riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
				nativeShellLibrary.GetDefaultSaveFolder(ShellNativeMethods.DefaultSaveFolderType.Detect, ref riid, out var ppv);
				return ShellHelper.GetParsingName(ppv);
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				if (!Directory.Exists(value))
				{
					throw new DirectoryNotFoundException(LocalizedMessages.ShellLibraryDefaultSaveFolderNotFound);
				}
				string fullName = new DirectoryInfo(value).FullName;
				Guid riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
				ShellNativeMethods.SHCreateItemFromParsingName(fullName, IntPtr.Zero, ref riid, out IShellItem shellItem);
				nativeShellLibrary.SetDefaultSaveFolder(ShellNativeMethods.DefaultSaveFolderType.Detect, shellItem);
				nativeShellLibrary.Commit();
			}
		}

		public bool IsPinnedToNavigationPane
		{
			get
			{
				ShellNativeMethods.LibraryOptions lofOptions = ShellNativeMethods.LibraryOptions.PinnedToNavigationPane;
				nativeShellLibrary.GetOptions(out lofOptions);
				return (lofOptions & ShellNativeMethods.LibraryOptions.PinnedToNavigationPane) == ShellNativeMethods.LibraryOptions.PinnedToNavigationPane;
			}
			set
			{
				ShellNativeMethods.LibraryOptions libraryOptions = ShellNativeMethods.LibraryOptions.Default;
				libraryOptions = ((!value) ? (libraryOptions & ~ShellNativeMethods.LibraryOptions.PinnedToNavigationPane) : (libraryOptions | ShellNativeMethods.LibraryOptions.PinnedToNavigationPane));
				nativeShellLibrary.SetOptions(ShellNativeMethods.LibraryOptions.PinnedToNavigationPane, libraryOptions);
				nativeShellLibrary.Commit();
			}
		}

		internal override IShellItem NativeShellItem => NativeShellItem2;

		internal override IShellItem2 NativeShellItem2 => nativeShellItem;

		public static IKnownFolder LibrariesKnownFolder
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return KnownFolderHelper.FromKnownFolderId(new Guid("1B3EA5DC-B587-4786-B4EF-BD1DC332AEAE"));
			}
		}

		private List<ShellFileSystemFolder> ItemsList => GetFolders();

		public ShellFileSystemFolder this[int index]
		{
			get
			{
				return ItemsList[index];
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public int Count => ItemsList.Count;

		public bool IsReadOnly => false;

		public new static bool IsPlatformSupported => CoreHelpers.RunningOnWin7;

		private ShellLibrary()
		{
			CoreHelpers.ThrowIfNotWin7();
		}

		private ShellLibrary(INativeShellLibrary nativeShellLibrary)
			: this()
		{
			this.nativeShellLibrary = nativeShellLibrary;
		}

		private ShellLibrary(IKnownFolder sourceKnownFolder, bool isReadOnly)
			: this()
		{
			Debug.Assert(sourceKnownFolder != null);
			knownFolder = sourceKnownFolder;
			nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
			AccessModes grfMode = ((!isReadOnly) ? AccessModes.ReadWrite : AccessModes.Direct);
			nativeShellItem = ((ShellObject)sourceKnownFolder).NativeShellItem2;
			Guid knownfidLibrary = sourceKnownFolder.FolderId;
			try
			{
				nativeShellLibrary.LoadLibraryFromKnownFolder(ref knownfidLibrary, grfMode);
			}
			catch (InvalidCastException)
			{
				throw new ArgumentException(LocalizedMessages.ShellLibraryInvalidLibrary, "sourceKnownFolder");
			}
			catch (NotImplementedException)
			{
				throw new ArgumentException(LocalizedMessages.ShellLibraryInvalidLibrary, "sourceKnownFolder");
			}
		}

		public ShellLibrary(string libraryName, bool overwrite)
			: this()
		{
			if (string.IsNullOrEmpty(libraryName))
			{
				throw new ArgumentException(LocalizedMessages.ShellLibraryEmptyName, "libraryName");
			}
			Name = libraryName;
			Guid kfidToSaveIn = new Guid("1B3EA5DC-B587-4786-B4EF-BD1DC332AEAE");
			ShellNativeMethods.LibrarySaveOptions lsf = (overwrite ? ShellNativeMethods.LibrarySaveOptions.OverrideExisting : ShellNativeMethods.LibrarySaveOptions.FailIfThere);
			nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
			nativeShellLibrary.SaveInKnownFolder(ref kfidToSaveIn, libraryName, lsf, out nativeShellItem);
		}

		public ShellLibrary(string libraryName, IKnownFolder sourceKnownFolder, bool overwrite)
			: this()
		{
			if (string.IsNullOrEmpty(libraryName))
			{
				throw new ArgumentException(LocalizedMessages.ShellLibraryEmptyName, "libraryName");
			}
			knownFolder = sourceKnownFolder;
			Name = libraryName;
			Guid kfidToSaveIn = knownFolder.FolderId;
			ShellNativeMethods.LibrarySaveOptions lsf = (overwrite ? ShellNativeMethods.LibrarySaveOptions.OverrideExisting : ShellNativeMethods.LibrarySaveOptions.FailIfThere);
			nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
			nativeShellLibrary.SaveInKnownFolder(ref kfidToSaveIn, libraryName, lsf, out nativeShellItem);
		}

		public ShellLibrary(string libraryName, string folderPath, bool overwrite)
			: this()
		{
			if (string.IsNullOrEmpty(libraryName))
			{
				throw new ArgumentException(LocalizedMessages.ShellLibraryEmptyName, "libraryName");
			}
			if (!Directory.Exists(folderPath))
			{
				throw new DirectoryNotFoundException(LocalizedMessages.ShellLibraryFolderNotFound);
			}
			Name = libraryName;
			ShellNativeMethods.LibrarySaveOptions lsf = (overwrite ? ShellNativeMethods.LibrarySaveOptions.OverrideExisting : ShellNativeMethods.LibrarySaveOptions.FailIfThere);
			Guid riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
			ShellNativeMethods.SHCreateItemFromParsingName(folderPath, IntPtr.Zero, ref riid, out IShellItem shellItem);
			nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
			nativeShellLibrary.Save(shellItem, libraryName, lsf, out nativeShellItem);
		}

		private static LibraryFolderType GetFolderTypefromGuid(Guid folderTypeGuid)
		{
			for (int i = 0; i < FolderTypesGuids.Length; i++)
			{
				if (folderTypeGuid.Equals(FolderTypesGuids[i]))
				{
					return (LibraryFolderType)i;
				}
			}
			throw new ArgumentOutOfRangeException("folderTypeGuid", LocalizedMessages.ShellLibraryInvalidFolderType);
		}

		public void Close()
		{
			Dispose();
		}

		public static ShellLibrary Load(string libraryName, bool isReadOnly)
		{
			CoreHelpers.ThrowIfNotWin7();
			IKnownFolder libraries = KnownFolders.Libraries;
			string path = ((libraries != null) ? libraries.Path : string.Empty);
			Guid riid = new Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE");
			string path2 = Path.Combine(path, libraryName + ".library-ms");
			IShellItem shellItem;
			int num = ShellNativeMethods.SHCreateItemFromParsingName(path2, IntPtr.Zero, ref riid, out shellItem);
			if (!CoreErrorHelper.Succeeded(num))
			{
				throw new ShellException(num);
			}
			INativeShellLibrary nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
			AccessModes grfMode = ((!isReadOnly) ? AccessModes.ReadWrite : AccessModes.Direct);
			nativeShellLibrary.LoadLibraryFromItem(shellItem, grfMode);
			ShellLibrary shellLibrary = new ShellLibrary(nativeShellLibrary);
			try
			{
				shellLibrary.nativeShellItem = (IShellItem2)shellItem;
				shellLibrary.Name = libraryName;
				return shellLibrary;
			}
			catch
			{
				shellLibrary.Dispose();
				throw;
			}
		}

		public static ShellLibrary Load(string libraryName, string folderPath, bool isReadOnly)
		{
			CoreHelpers.ThrowIfNotWin7();
			string path = Path.Combine(folderPath, libraryName + ".library-ms");
			ShellFile shellFile = ShellFile.FromFilePath(path);
			IShellItem shellItem = shellFile.NativeShellItem;
			INativeShellLibrary nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
			AccessModes grfMode = ((!isReadOnly) ? AccessModes.ReadWrite : AccessModes.Direct);
			nativeShellLibrary.LoadLibraryFromItem(shellItem, grfMode);
			ShellLibrary shellLibrary = new ShellLibrary(nativeShellLibrary);
			try
			{
				shellLibrary.nativeShellItem = (IShellItem2)shellItem;
				shellLibrary.Name = libraryName;
				return shellLibrary;
			}
			catch
			{
				shellLibrary.Dispose();
				throw;
			}
		}

		internal static ShellLibrary FromShellItem(IShellItem nativeShellItem, bool isReadOnly)
		{
			CoreHelpers.ThrowIfNotWin7();
			INativeShellLibrary nativeShellLibrary = (INativeShellLibrary)new ShellLibraryCoClass();
			AccessModes grfMode = ((!isReadOnly) ? AccessModes.ReadWrite : AccessModes.Direct);
			nativeShellLibrary.LoadLibraryFromItem(nativeShellItem, grfMode);
			ShellLibrary shellLibrary = new ShellLibrary(nativeShellLibrary);
			shellLibrary.nativeShellItem = (IShellItem2)nativeShellItem;
			return shellLibrary;
		}

		public static ShellLibrary Load(IKnownFolder sourceKnownFolder, bool isReadOnly)
		{
			CoreHelpers.ThrowIfNotWin7();
			return new ShellLibrary(sourceKnownFolder, isReadOnly);
		}

		private static void ShowManageLibraryUI(ShellLibrary shellLibrary, IntPtr windowHandle, string title, string instruction, bool allowAllLocations)
		{
			int hr = 0;
			Thread thread = new Thread((ThreadStart)delegate
			{
				hr = ShellNativeMethods.SHShowManageLibraryUI(shellLibrary.NativeShellItem, windowHandle, title, instruction, allowAllLocations ? ShellNativeMethods.LibraryManageDialogOptions.NonIndexableLocationWarning : ShellNativeMethods.LibraryManageDialogOptions.Default);
			});
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
			thread.Join();
			if (!CoreErrorHelper.Succeeded(hr))
			{
				throw new ShellException(hr);
			}
		}

		public static void ShowManageLibraryUI(string libraryName, string folderPath, IntPtr windowHandle, string title, string instruction, bool allowAllLocations)
		{
			using (ShellLibrary shellLibrary = Load(libraryName, folderPath, true))
			{
				ShowManageLibraryUI(shellLibrary, windowHandle, title, instruction, allowAllLocations);
			}
		}

		public static void ShowManageLibraryUI(string libraryName, IntPtr windowHandle, string title, string instruction, bool allowAllLocations)
		{
			using (ShellLibrary shellLibrary = Load(libraryName, true))
			{
				ShowManageLibraryUI(shellLibrary, windowHandle, title, instruction, allowAllLocations);
			}
		}

		public static void ShowManageLibraryUI(IKnownFolder sourceKnownFolder, IntPtr windowHandle, string title, string instruction, bool allowAllLocations)
		{
			using (ShellLibrary shellLibrary = Load(sourceKnownFolder, true))
			{
				ShowManageLibraryUI(shellLibrary, windowHandle, title, instruction, allowAllLocations);
			}
		}

		public void Add(ShellFileSystemFolder item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			nativeShellLibrary.AddFolder(item.NativeShellItem);
			nativeShellLibrary.Commit();
		}

		public void Add(string folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				throw new DirectoryNotFoundException(LocalizedMessages.ShellLibraryFolderNotFound);
			}
			Add(ShellFileSystemFolder.FromFolderPath(folderPath));
		}

		public void Clear()
		{
			List<ShellFileSystemFolder> itemsList = ItemsList;
			foreach (ShellFileSystemFolder item in itemsList)
			{
				nativeShellLibrary.RemoveFolder(item.NativeShellItem);
			}
			nativeShellLibrary.Commit();
		}

		public bool Remove(ShellFileSystemFolder item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			try
			{
				nativeShellLibrary.RemoveFolder(item.NativeShellItem);
				nativeShellLibrary.Commit();
			}
			catch (COMException)
			{
				return false;
			}
			return true;
		}

		public bool Remove(string folderPath)
		{
			ShellFileSystemFolder item = ShellFileSystemFolder.FromFolderPath(folderPath);
			return Remove(item);
		}

		protected override void Dispose(bool disposing)
		{
			if (nativeShellLibrary != null)
			{
				Marshal.ReleaseComObject(nativeShellLibrary);
				nativeShellLibrary = null;
			}
			base.Dispose(disposing);
		}

		~ShellLibrary()
		{
			Dispose(false);
		}

		private List<ShellFileSystemFolder> GetFolders()
		{
			List<ShellFileSystemFolder> list = new List<ShellFileSystemFolder>();
			Guid riid = new Guid("B63EA76D-1F85-456F-A19C-48159EFA858B");
			IShellItemArray ppv;
			HResult folders = nativeShellLibrary.GetFolders(ShellNativeMethods.LibraryFolderFilter.AllItems, ref riid, out ppv);
			if (!CoreErrorHelper.Succeeded(folders))
			{
				return list;
			}
			ppv.GetCount(out var pdwNumItems);
			for (uint num = 0u; num < pdwNumItems; num++)
			{
				ppv.GetItemAt(num, out var ppsi);
				list.Add(new ShellFileSystemFolder(ppsi as IShellItem2));
			}
			if (ppv != null)
			{
				Marshal.ReleaseComObject(ppv);
				ppv = null;
			}
			return list;
		}

		public new IEnumerator<ShellFileSystemFolder> GetEnumerator()
		{
			return ItemsList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ItemsList.GetEnumerator();
		}

		public bool Contains(string fullPath)
		{
			if (string.IsNullOrEmpty(fullPath))
			{
				throw new ArgumentNullException("fullPath");
			}
			return ItemsList.Any((ShellFileSystemFolder folder) => string.Equals(fullPath, folder.Path, StringComparison.OrdinalIgnoreCase));
		}

		public bool Contains(ShellFileSystemFolder item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			return ItemsList.Any((ShellFileSystemFolder folder) => string.Equals(item.Path, folder.Path, StringComparison.OrdinalIgnoreCase));
		}

		public int IndexOf(ShellFileSystemFolder item)
		{
			return ItemsList.IndexOf(item);
		}

		void IList<ShellFileSystemFolder>.Insert(int index, ShellFileSystemFolder item)
		{
			throw new NotImplementedException();
		}

		void IList<ShellFileSystemFolder>.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		void ICollection<ShellFileSystemFolder>.CopyTo(ShellFileSystemFolder[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}
	}
}
