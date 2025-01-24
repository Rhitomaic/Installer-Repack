using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Microsoft.WindowsAPICodePack.Shell
{
	internal static class ShellNativeMethods
	{
		[Flags]
		internal enum FileOpenOptions
		{
			OverwritePrompt = 2,
			StrictFileTypes = 4,
			NoChangeDirectory = 8,
			PickFolders = 0x20,
			ForceFilesystem = 0x40,
			AllNonStorageItems = 0x80,
			NoValidate = 0x100,
			AllowMultiSelect = 0x200,
			PathMustExist = 0x800,
			FileMustExist = 0x1000,
			CreatePrompt = 0x2000,
			ShareAware = 0x4000,
			NoReadOnlyReturn = 0x8000,
			NoTestFileCreate = 0x10000,
			HideMruPlaces = 0x20000,
			HidePinnedPlaces = 0x40000,
			NoDereferenceLinks = 0x100000,
			DontAddToRecent = 0x2000000,
			ForceShowHidden = 0x10000000,
			DefaultNoMiniMode = 0x20000000
		}

		internal enum ControlState
		{
			Inactive,
			Enable,
			Visible
		}

		internal enum ShellItemDesignNameOptions
		{
			Normal = 0,
			ParentRelativeParsing = -2147385343,
			DesktopAbsoluteParsing = -2147319808,
			ParentRelativeEditing = -2147282943,
			DesktopAbsoluteEditing = -2147172352,
			FileSystemPath = -2147123200,
			Url = -2147057664,
			ParentRelativeForAddressBar = -2146975743,
			ParentRelative = -2146959359
		}

		[Flags]
		internal enum GetPropertyStoreOptions
		{
			Default = 0,
			HandlePropertiesOnly = 1,
			ReadWrite = 2,
			Temporary = 4,
			FastPropertiesOnly = 8,
			OpensLowItem = 0x10,
			DelayCreation = 0x20,
			BestEffort = 0x40,
			MaskValid = 0xFF
		}

		internal enum ShellItemAttributeOptions
		{
			And = 1,
			Or = 2,
			AppCompat = 3,
			Mask = 3,
			AllItems = 16384
		}

		internal enum FileDialogEventShareViolationResponse
		{
			Default,
			Accept,
			Refuse
		}

		internal enum FileDialogEventOverwriteResponse
		{
			Default,
			Accept,
			Refuse
		}

		internal enum FileDialogAddPlacement
		{
			Bottom,
			Top
		}

		[Flags]
		internal enum SIIGBF
		{
			ResizeToFit = 0,
			BiggerSizeOk = 1,
			MemoryOnly = 2,
			IconOnly = 4,
			ThumbnailOnly = 8,
			InCacheOnly = 0x10
		}

		[Flags]
		internal enum ThumbnailOptions
		{
			Extract = 0,
			InCacheOnly = 1,
			FastExtract = 2,
			ForceExtraction = 4,
			SlowReclaim = 8,
			ExtractDoNotCache = 0x20
		}

		[Flags]
		internal enum ThumbnailCacheOptions
		{
			Default = 0,
			LowQuality = 1,
			Cached = 2
		}

		[Flags]
		internal enum ShellFileGetAttributesOptions
		{
			CanCopy = 1,
			CanMove = 2,
			CanLink = 4,
			Storage = 8,
			CanRename = 0x10,
			CanDelete = 0x20,
			HasPropertySheet = 0x40,
			DropTarget = 0x100,
			CapabilityMask = 0x177,
			System = 0x1000,
			Encrypted = 0x2000,
			IsSlow = 0x4000,
			Ghosted = 0x8000,
			Link = 0x10000,
			Share = 0x20000,
			ReadOnly = 0x40000,
			Hidden = 0x80000,
			DisplayAttributeMask = 0xFC000,
			FileSystemAncestor = 0x10000000,
			Folder = 0x20000000,
			FileSystem = 0x40000000,
			HasSubFolder = int.MinValue,
			ContentsMask = int.MinValue,
			Validate = 0x1000000,
			Removable = 0x2000000,
			Compressed = 0x4000000,
			Browsable = 0x8000000,
			Nonenumerated = 0x100000,
			NewContent = 0x200000,
			CanMoniker = 0x400000,
			HasStorage = 0x400000,
			Stream = 0x400000,
			StorageAncestor = 0x800000,
			StorageCapabilityMask = 0x70C50008,
			PkeyMask = -2130427904
		}

		[Flags]
		internal enum ShellFolderEnumerationOptions : ushort
		{
			CheckingForChildren = 0x10,
			Folders = 0x20,
			NonFolders = 0x40,
			IncludeHidden = 0x80,
			InitializeOnFirstNext = 0x100,
			NetPrinterSearch = 0x200,
			Shareable = 0x400,
			Storage = 0x800,
			NavigationEnum = 0x1000,
			FastItems = 0x2000,
			FlatList = 0x4000,
			EnableAsync = 0x8000
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct FilterSpec
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			internal string Name;

			[MarshalAs(UnmanagedType.LPWStr)]
			internal string Spec;

			internal FilterSpec(string name, string spec)
			{
				Name = name;
				Spec = spec;
			}
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		internal struct ThumbnailId
		{
			[MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 16)]
			private byte rgbKey;
		}

		internal enum LibraryFolderFilter
		{
			ForceFileSystem = 1,
			StorageItems,
			AllItems
		}

		[Flags]
		internal enum LibraryOptions
		{
			Default = 0,
			PinnedToNavigationPane = 1,
			MaskAll = 1
		}

		internal enum DefaultSaveFolderType
		{
			Detect = 1,
			Private,
			Public
		}

		internal enum LibrarySaveOptions
		{
			FailIfThere,
			OverrideExisting,
			MakeUniqueName
		}

		internal enum LibraryManageDialogOptions
		{
			Default,
			NonIndexableLocationWarning
		}

#pragma warning disable 0649
        internal struct ShellNotifyStruct
		{
			internal IntPtr item1;

			internal IntPtr item2;
        }
#pragma warning restore 0649

        internal struct SHChangeNotifyEntry
		{
			internal IntPtr pIdl;

			[MarshalAs(UnmanagedType.Bool)]
			internal bool recursively;
		}

		[Flags]
		internal enum ShellChangeNotifyEventSource
		{
			InterruptLevel = 1,
			ShellLevel = 2,
			RecursiveInterrupt = 0x1000,
			NewDelivery = 0x8000
		}

		internal const int CommandLink = 14;

		internal const uint SetNote = 5641u;

		internal const uint GetNote = 5642u;

		internal const uint GetNoteLength = 5643u;

		internal const uint SetShield = 5644u;

		internal const int MaxPath = 260;

		internal const int InPlaceStringTruncated = 262560;

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHCreateShellItemArrayFromDataObject(IDataObject pdo, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItemArray iShellItemArray);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string path, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItem2 shellItem);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string path, IntPtr pbc, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItem shellItem);

		[DllImport("shlwapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int PathParseIconLocation([MarshalAs(UnmanagedType.LPWStr)] ref string pszIconFile);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHCreateItemFromIDList(IntPtr pidl, ref Guid riid, [MarshalAs(UnmanagedType.Interface)] out IShellItem2 ppv);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHParseDisplayName([MarshalAs(UnmanagedType.LPWStr)] string pszName, IntPtr pbc, out IntPtr ppidl, ShellFileGetAttributesOptions sfgaoIn, out ShellFileGetAttributesOptions psfgaoOut);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHGetIDListFromObject(IntPtr iUnknown, out IntPtr ppidl);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHGetDesktopFolder([MarshalAs(UnmanagedType.Interface)] out IShellFolder ppshf);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHCreateShellItem(IntPtr pidlParent, [In][MarshalAs(UnmanagedType.Interface)] IShellFolder psfParent, IntPtr pidl, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

		[DllImport("shell32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern uint ILGetSize(IntPtr pidl);

		[DllImport("shell32.dll")]
		public static extern void ILFree(IntPtr pidl);

		[DllImport("gdi32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool DeleteObject(IntPtr hObject);

		[DllImport("Shell32", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SHShowManageLibraryUI([In][MarshalAs(UnmanagedType.Interface)] IShellItem library, [In] IntPtr hwndOwner, [In] string title, [In] string instruction, [In] LibraryManageDialogOptions lmdOptions);

		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SHGetPathFromIDListW(IntPtr pidl, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszPath);

		[DllImport("shell32.dll")]
		internal static extern uint SHChangeNotifyRegister(IntPtr windowHandle, ShellChangeNotifyEventSource sources, ShellObjectChangeTypes events, uint message, int entries, ref SHChangeNotifyEntry changeNotifyEntry);

		[DllImport("shell32.dll")]
		internal static extern IntPtr SHChangeNotification_Lock(IntPtr windowHandle, int processId, out IntPtr pidl, out uint lEvent);

		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SHChangeNotification_Unlock(IntPtr hLock);

		[DllImport("shell32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool SHChangeNotifyDeregister(uint hNotify);
	}
}
