using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	public static class KnownFolders
	{
		public static ICollection<IKnownFolder> All => GetAllFolders();

		public static IKnownFolder Computer => GetKnownFolder(FolderIdentifiers.Computer);

		public static IKnownFolder Conflict => GetKnownFolder(FolderIdentifiers.Conflict);

		public static IKnownFolder ControlPanel => GetKnownFolder(FolderIdentifiers.ControlPanel);

		public static IKnownFolder Desktop => GetKnownFolder(FolderIdentifiers.Desktop);

		public static IKnownFolder Internet => GetKnownFolder(FolderIdentifiers.Internet);

		public static IKnownFolder Network => GetKnownFolder(FolderIdentifiers.Network);

		public static IKnownFolder Printers => GetKnownFolder(FolderIdentifiers.Printers);

		public static IKnownFolder SyncManager => GetKnownFolder(FolderIdentifiers.SyncManager);

		public static IKnownFolder Connections => GetKnownFolder(FolderIdentifiers.Connections);

		public static IKnownFolder SyncSetup => GetKnownFolder(FolderIdentifiers.SyncSetup);

		public static IKnownFolder SyncResults => GetKnownFolder(FolderIdentifiers.SyncResults);

		public static IKnownFolder RecycleBin => GetKnownFolder(FolderIdentifiers.RecycleBin);

		public static IKnownFolder Fonts => GetKnownFolder(FolderIdentifiers.Fonts);

		public static IKnownFolder Startup => GetKnownFolder(FolderIdentifiers.Startup);

		public static IKnownFolder Programs => GetKnownFolder(FolderIdentifiers.Programs);

		public static IKnownFolder StartMenu => GetKnownFolder(FolderIdentifiers.StartMenu);

		public static IKnownFolder Recent => GetKnownFolder(FolderIdentifiers.Recent);

		public static IKnownFolder SendTo => GetKnownFolder(FolderIdentifiers.SendTo);

		public static IKnownFolder Documents => GetKnownFolder(FolderIdentifiers.Documents);

		public static IKnownFolder Favorites => GetKnownFolder(FolderIdentifiers.Favorites);

		public static IKnownFolder NetHood => GetKnownFolder(FolderIdentifiers.NetHood);

		public static IKnownFolder PrintHood => GetKnownFolder(FolderIdentifiers.PrintHood);

		public static IKnownFolder Templates => GetKnownFolder(FolderIdentifiers.Templates);

		public static IKnownFolder CommonStartup => GetKnownFolder(FolderIdentifiers.CommonStartup);

		public static IKnownFolder CommonPrograms => GetKnownFolder(FolderIdentifiers.CommonPrograms);

		public static IKnownFolder CommonStartMenu => GetKnownFolder(FolderIdentifiers.CommonStartMenu);

		public static IKnownFolder PublicDesktop => GetKnownFolder(FolderIdentifiers.PublicDesktop);

		public static IKnownFolder ProgramData => GetKnownFolder(FolderIdentifiers.ProgramData);

		public static IKnownFolder CommonTemplates => GetKnownFolder(FolderIdentifiers.CommonTemplates);

		public static IKnownFolder PublicDocuments => GetKnownFolder(FolderIdentifiers.PublicDocuments);

		public static IKnownFolder RoamingAppData => GetKnownFolder(FolderIdentifiers.RoamingAppData);

		public static IKnownFolder LocalAppData => GetKnownFolder(FolderIdentifiers.LocalAppData);

		public static IKnownFolder LocalAppDataLow => GetKnownFolder(FolderIdentifiers.LocalAppDataLow);

		public static IKnownFolder InternetCache => GetKnownFolder(FolderIdentifiers.InternetCache);

		public static IKnownFolder Cookies => GetKnownFolder(FolderIdentifiers.Cookies);

		public static IKnownFolder History => GetKnownFolder(FolderIdentifiers.History);

		public static IKnownFolder System => GetKnownFolder(FolderIdentifiers.System);

		public static IKnownFolder SystemX86 => GetKnownFolder(FolderIdentifiers.SystemX86);

		public static IKnownFolder Windows => GetKnownFolder(FolderIdentifiers.Windows);

		public static IKnownFolder Profile => GetKnownFolder(FolderIdentifiers.Profile);

		public static IKnownFolder Pictures => GetKnownFolder(FolderIdentifiers.Pictures);

		public static IKnownFolder ProgramFilesX86 => GetKnownFolder(FolderIdentifiers.ProgramFilesX86);

		public static IKnownFolder ProgramFilesCommonX86 => GetKnownFolder(FolderIdentifiers.ProgramFilesCommonX86);

		public static IKnownFolder ProgramFilesX64 => GetKnownFolder(FolderIdentifiers.ProgramFilesX64);

		public static IKnownFolder ProgramFilesCommonX64 => GetKnownFolder(FolderIdentifiers.ProgramFilesCommonX64);

		public static IKnownFolder ProgramFiles => GetKnownFolder(FolderIdentifiers.ProgramFiles);

		public static IKnownFolder ProgramFilesCommon => GetKnownFolder(FolderIdentifiers.ProgramFilesCommon);

		public static IKnownFolder AdminTools => GetKnownFolder(FolderIdentifiers.AdminTools);

		public static IKnownFolder CommonAdminTools => GetKnownFolder(FolderIdentifiers.CommonAdminTools);

		public static IKnownFolder Music => GetKnownFolder(FolderIdentifiers.Music);

		public static IKnownFolder Videos => GetKnownFolder(FolderIdentifiers.Videos);

		public static IKnownFolder PublicPictures => GetKnownFolder(FolderIdentifiers.PublicPictures);

		public static IKnownFolder PublicMusic => GetKnownFolder(FolderIdentifiers.PublicMusic);

		public static IKnownFolder PublicVideos => GetKnownFolder(FolderIdentifiers.PublicVideos);

		public static IKnownFolder ResourceDir => GetKnownFolder(FolderIdentifiers.ResourceDir);

		public static IKnownFolder LocalizedResourcesDir => GetKnownFolder(FolderIdentifiers.LocalizedResourcesDir);

		public static IKnownFolder CommonOemLinks => GetKnownFolder(FolderIdentifiers.CommonOEMLinks);

		public static IKnownFolder CDBurning => GetKnownFolder(FolderIdentifiers.CDBurning);

		public static IKnownFolder UserProfiles => GetKnownFolder(FolderIdentifiers.UserProfiles);

		public static IKnownFolder Playlists => GetKnownFolder(FolderIdentifiers.Playlists);

		public static IKnownFolder SamplePlaylists => GetKnownFolder(FolderIdentifiers.SamplePlaylists);

		public static IKnownFolder SampleMusic => GetKnownFolder(FolderIdentifiers.SampleMusic);

		public static IKnownFolder SamplePictures => GetKnownFolder(FolderIdentifiers.SamplePictures);

		public static IKnownFolder SampleVideos => GetKnownFolder(FolderIdentifiers.SampleVideos);

		public static IKnownFolder PhotoAlbums => GetKnownFolder(FolderIdentifiers.PhotoAlbums);

		public static IKnownFolder Public => GetKnownFolder(FolderIdentifiers.Public);

		public static IKnownFolder ChangeRemovePrograms => GetKnownFolder(FolderIdentifiers.ChangeRemovePrograms);

		public static IKnownFolder AppUpdates => GetKnownFolder(FolderIdentifiers.AppUpdates);

		public static IKnownFolder AddNewPrograms => GetKnownFolder(FolderIdentifiers.AddNewPrograms);

		public static IKnownFolder Downloads => GetKnownFolder(FolderIdentifiers.Downloads);

		public static IKnownFolder PublicDownloads => GetKnownFolder(FolderIdentifiers.PublicDownloads);

		public static IKnownFolder SavedSearches => GetKnownFolder(FolderIdentifiers.SavedSearches);

		public static IKnownFolder QuickLaunch => GetKnownFolder(FolderIdentifiers.QuickLaunch);

		public static IKnownFolder Contacts => GetKnownFolder(FolderIdentifiers.Contacts);

		public static IKnownFolder SidebarParts => GetKnownFolder(FolderIdentifiers.SidebarParts);

		public static IKnownFolder SidebarDefaultParts => GetKnownFolder(FolderIdentifiers.SidebarDefaultParts);

		public static IKnownFolder TreeProperties => GetKnownFolder(FolderIdentifiers.TreeProperties);

		public static IKnownFolder PublicGameTasks => GetKnownFolder(FolderIdentifiers.PublicGameTasks);

		public static IKnownFolder GameTasks => GetKnownFolder(FolderIdentifiers.GameTasks);

		public static IKnownFolder SavedGames => GetKnownFolder(FolderIdentifiers.SavedGames);

		public static IKnownFolder Games => GetKnownFolder(FolderIdentifiers.Games);

		public static IKnownFolder RecordedTV => GetKnownFolder(FolderIdentifiers.RecordedTV);

		public static IKnownFolder SearchMapi => GetKnownFolder(FolderIdentifiers.SearchMapi);

		public static IKnownFolder SearchCsc => GetKnownFolder(FolderIdentifiers.SearchCsc);

		public static IKnownFolder Links => GetKnownFolder(FolderIdentifiers.Links);

		public static IKnownFolder UsersFiles => GetKnownFolder(FolderIdentifiers.UsersFiles);

		public static IKnownFolder SearchHome => GetKnownFolder(FolderIdentifiers.SearchHome);

		public static IKnownFolder OriginalImages => GetKnownFolder(FolderIdentifiers.OriginalImages);

		public static IKnownFolder UserProgramFiles
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.UserProgramFiles);
			}
		}

		public static IKnownFolder UserProgramFilesCommon
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.UserProgramFilesCommon);
			}
		}

		public static IKnownFolder Ringtones
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.Ringtones);
			}
		}

		public static IKnownFolder PublicRingtones
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.PublicRingtones);
			}
		}

		public static IKnownFolder UsersLibraries
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.UsersLibraries);
			}
		}

		public static IKnownFolder DocumentsLibrary
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.DocumentsLibrary);
			}
		}

		public static IKnownFolder MusicLibrary
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.MusicLibrary);
			}
		}

		public static IKnownFolder PicturesLibrary
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.PicturesLibrary);
			}
		}

		public static IKnownFolder VideosLibrary
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.VideosLibrary);
			}
		}

		public static IKnownFolder RecordedTVLibrary
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.RecordedTVLibrary);
			}
		}

		public static IKnownFolder OtherUsers
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.OtherUsers);
			}
		}

		public static IKnownFolder DeviceMetadataStore
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.DeviceMetadataStore);
			}
		}

		public static IKnownFolder Libraries
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.Libraries);
			}
		}

		public static IKnownFolder UserPinned
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.UserPinned);
			}
		}

		public static IKnownFolder ImplicitAppShortcuts
		{
			get
			{
				CoreHelpers.ThrowIfNotWin7();
				return GetKnownFolder(FolderIdentifiers.ImplicitAppShortcuts);
			}
		}

		private static ReadOnlyCollection<IKnownFolder> GetAllFolders()
		{
			IList<IKnownFolder> list = new List<IKnownFolder>();
			IntPtr folders = IntPtr.Zero;
			try
			{
				KnownFolderManagerClass knownFolderManagerClass = new KnownFolderManagerClass();
				knownFolderManagerClass.GetFolderIds(out folders, out var count);
				if (count != 0 && folders != IntPtr.Zero)
				{
					for (int i = 0; i < count; i++)
					{
						IntPtr ptr = new IntPtr(folders.ToInt64() + Marshal.SizeOf(typeof(Guid)) * i);
						Guid knownFolderId = (Guid)Marshal.PtrToStructure(ptr, typeof(Guid));
						IKnownFolder knownFolder = KnownFolderHelper.FromKnownFolderIdInternal(knownFolderId);
						if (knownFolder != null)
						{
							list.Add(knownFolder);
						}
					}
				}
			}
			finally
			{
				if (folders != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(folders);
				}
			}
			return new ReadOnlyCollection<IKnownFolder>(list);
		}

		private static IKnownFolder GetKnownFolder(Guid guid)
		{
			return KnownFolderHelper.FromKnownFolderId(guid);
		}
	}
}
